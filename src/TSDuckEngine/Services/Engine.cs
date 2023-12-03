using Microsoft.Extensions.Options;
using TSDuckEngine.Infrastructure.Metrics;
using TSDuckEngine.Models;
using System.Diagnostics;
using System.Runtime;
using System.Xml.Linq;
using System.Xml.XPath;
using TSDuckHelper;
using TSDuckHelper.Events;
using TSDuckHelper.Models;
using System.IO.Compression;
using System.Diagnostics.Metrics;
using System.Text.Json;

namespace TSDuckEngine.Services
{
    public class Engine : BackgroundService, IDisposable
    {
        private readonly string TSPPidFile = $"tsp_{Program.InstanceName.ToLower()}.pid";

        private readonly ILogger<Engine> _logger;
        private EngineOptions _settings;
        private readonly TSDuckOutputParser _tsduckOutputParser;
        private CancellationTokenSource? _tsduckProcessorTokenSource;

        public Engine(ILogger<Engine> logger, IOptionsMonitor<EngineOptions> settings, TSDuckOutputParser tsduckOutputParser)
        {
            _logger = logger;
            _settings = settings.CurrentValue;
            _tsduckOutputParser = tsduckOutputParser;

            _tsduckOutputParser.OnError += delegate (object? sender, ParserEventArgs<string> e)
            {
                _logger?.LogError($"{e.Data}");
            };

            settings.OnChange(settings =>
            {
                _logger?.LogInformation("Setting changes detected");
                _settings = settings;

                // stop current tsduck instance
                _tsduckProcessorTokenSource?.Cancel();
            });

            _tsduckOutputParser.OnSRTStatistic += delegate (object? sender, ParserEventArgs<TSDuckSrtStatisticReport> e)
            {
                logger?.LogDebug($"* [srt_statistic]: {JsonSerializer.Serialize(e.Data)}");
            };

            _tsduckOutputParser.OnAnalyze += delegate (object? sender, ParserEventArgs<TSDuckAnalyzerReport> e)
            {
                _logger?.LogDebug($"* [analyze]: {JsonSerializer.Serialize(e.Data)}");
            };

            _tsduckOutputParser.OnBitrateMonitor += delegate (object? sender, ParserEventArgs<TSDuckBitrateMonitorReport> e)
            {
                _logger?.LogDebug($"* [bitrate_monitor]: {JsonSerializer.Serialize(e.Data)}");
            };

            _tsduckOutputParser.OnResourceMonitoring += delegate (object? sender, ParserEventArgs<TSDuckResourceMonitoringReport> e)
            {
                _logger?.LogInformation($"* [resource_monitor]: {JsonSerializer.Serialize(e.Data)}");
            };

            _tsduckOutputParser.OnContinuity += delegate (object? sender, ParserEventArgs<TSDuckContinuityReport> e)
            {
                _logger?.LogDebug($"* [continuity]: {JsonSerializer.Serialize(e.Data)}");
            };

            _tsduckOutputParser.OnError += delegate (object? sender, ParserEventArgs<string> e)
            {
                _logger?.LogError($"{e.Data}");
            };
        }


        public override void Dispose()
        {
            _tsduckProcessorTokenSource?.Cancel();
            base.Dispose();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            KillProcessByPidFile(TSPPidFile);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            KillProcessByPidFile(TSPPidFile);
            return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Engine service running at: {time}", DateTimeOffset.Now);

            while (!stoppingToken.IsCancellationRequested)
            {
                _tsduckProcessorTokenSource = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken);
                CancellationToken token = _tsduckProcessorTokenSource.Token;
                try
                {

                    var input = new UriBuilder(_settings.Input ?? throw new Exception("Input is null"));
                    _logger.LogInformation($"Engine.Input: {input}");

                    TSDuckArgumentBuilder tsduckArgumetBuilder = new TSDuckArgumentBuilder(TSDuckArgumentBuilder.Parse(input.Uri))
                    {
                        Realtime = _settings.Tsp.Realtime,
                        SynchronousLoging = _settings.Tsp.SynchronousLoging,
                        ResourceMonitoring = _settings.Tsp.ResourceMonitoring,
                    };

                    if (!String.IsNullOrEmpty(_settings.Output))
                    {
                        var output = new UriBuilder(_settings.Output);
                        _logger.LogInformation($"Engine.Output: {output}");
                        tsduckArgumetBuilder.Output = TSDuckArgumentBuilder.Parse(output.Uri);
                    }
          
                    if (_settings.Tsp.ContinuityMonitoring)
                    {
                        tsduckArgumetBuilder.Processing.Add(new TSDuckArgumentBuilderContinuity());
                    }

                    if (_settings.Tsp.BitrateMonitoring)
                    {
                        tsduckArgumetBuilder.Processing.Add(new TSDuckArgumentBuilderBitrateMonitor());
                    }

                    if (_settings.Tsp.Analyzing)
                    {
                        tsduckArgumetBuilder.Processing.Add(new TSDuckArgumentBuilderAnalyze());
                    }

                    

                    // -P
                    //foreach (string item in _settings.Processing)
                    //{
                    //    tsduckArgumetBuilder.Processing.Add(item);
                    //}


                    // FFMpegOutputParser
                    DataReceivedEventHandler dataReceivedEvent = new DataReceivedEventHandler((sender, e) =>
                    {
                     
                        if (!string.IsNullOrEmpty(e.Data))
                        {
                            this._tsduckOutputParser?.ProcessLine(e.Data);
                        }
                    });

                    _logger.LogInformation($"Engine.TSDuck.RunAsync: {tsduckArgumetBuilder.ToString()}");
                    var exitCode = await TSDuckProcess.Run(tsduckArgumetBuilder.ToString(), dataReceivedEvent, token, (p) => 
                    {
                        var processId = p.Id;
                        var processNmae = p.ProcessName;
                        _logger.LogDebug($"Engine.TSDuck.Process: {processNmae} {processId}");

                        // save tsp process id
                        File.WriteAllText(TSPPidFile, processId.ToString());

                        _logger.LogDebug($"Engine.TSDuck.PID: create file {TSPPidFile}");
                    });
                    _logger.LogDebug($"Engine.TSDuck.ExitCode: {exitCode}");

                    if (exitCode == 0)
                    {
                        File.Delete(TSPPidFile);
                        _logger.LogDebug($"Engine.TSDuck.PID: delete file {TSPPidFile}");
                    }
                }
                catch (TaskCanceledException)
                {
                    _logger.LogWarning($"Engine.TaskCanceled");
                }
                catch (System.OperationCanceledException)
                {
                    _logger.LogWarning($"Engine.OperationCanceled");
                }
                catch (System.TimeoutException)
                {
                    _logger.LogWarning($"Engine.OperationTimeout");
                }
                catch (Exception e)
                {
                    _logger.LogError($"Engine.Exception: {e.Message} {e.GetType()}");
                }
                finally
                {
                    // OpenTelemetry metric 
                    TSDuckEngineMeters.TSPInstanceRestart.Add(1);

                    await Task.Delay(_settings.RestartInstanceDelay, stoppingToken);
                }
            }

            _logger.LogInformation("Engine service stopped at: {time}", DateTimeOffset.Now);
        }

        private void KillProcessByPidFile(string pidFile)
        {
            if (File.Exists(pidFile))
            {
                var textLines = File.ReadAllLines(pidFile);
                if (textLines.Length > 0)
                {
                    var text = textLines[0].Trim();
                    if (Int32.TryParse(text, out var pid))
                    {
                        //System.Diagnostics.Process[] procs = System.Diagnostics.Process.GetProcessesByName("tsp");
                        //Process? tsp = procs.SingleOrDefault(p => p.Id == pid);
                        try
                        {
                            Process? tsp = System.Diagnostics.Process.GetProcessById(pid);
                            if (tsp != null)
                            {
                                tsp.Kill();
                                _logger.LogWarning($"Engine.TSP.Kill: ProcessId={tsp.Id} ProcessName={tsp.ProcessName}");
                            }
                        }
                        catch { }
                    }
                }
                File.Delete(pidFile);
            }
        }


    }
}