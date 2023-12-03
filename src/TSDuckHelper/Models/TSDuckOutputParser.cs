using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TSDuckHelper.Events;
using TSDuckHelper.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TSDuckHelper
{
    public class TSDuckOutputParser
    {
        private readonly ILogger<TSDuckOutputParser> _logger;


        public event EventHandler<ParserEventArgs<TSDuckSrtStatisticReport>>? OnSRTStatistic;
        public event EventHandler<ParserEventArgs<TSDuckContinuityReport>>? OnContinuity;
        public event EventHandler<ParserEventArgs<TSDuckBitrateMonitorReport>>? OnBitrateMonitor;
        public event EventHandler<ParserEventArgs<TSDuckResourceMonitoringReport>>? OnResourceMonitoring;
        public event EventHandler<ParserEventArgs<TSDuckAnalyzerReport>>? OnAnalyze;
        public event EventHandler<ParserEventArgs<string>>? OnError;

        public TSDuckOutputParser(ILogger<TSDuckOutputParser> logger)
        {
            _logger = logger;      
        }


        public void ProcessLine(string line)
        {
            try
            {
          
                DateTime timestamp = DateTime.UtcNow;
                line = line.Trim().ToLower();

                if (TSDuckOutputParser.IsResourceMonitoringLine(line))
                {
                    this.OnResourceMonitoring?.Invoke(this, 
                        new ParserEventArgs<TSDuckResourceMonitoringReport>(
                            TSDuckResourceMonitoringReport.Parse(line)));
                }
                else if (TSDuckOutputParser.IsSrtLine(line))
                {
                    var start = line.IndexOf('{');
                    var end = line.LastIndexOf('}') + 1;
                    string json = line.Substring(start, end - start).Trim();
                    TSDuckSrtStatisticReport? srtStatistic = TSDuckSrtStatisticReport.Deserialize(json);

                    if (srtStatistic != null)
                    {
                        this.OnSRTStatistic?.Invoke(this,
                            new ParserEventArgs<TSDuckSrtStatisticReport>(srtStatistic));
                    }                   
                }
                else if (TSDuckOutputParser.IsBitrateMonitorLine(line))
                {
                    this.OnBitrateMonitor?.Invoke(this, 
                        new ParserEventArgs<TSDuckBitrateMonitorReport>(
                            TSDuckBitrateMonitorReport.Parse(line)));
                }
                else if (TSDuckOutputParser.IsContinuityLine(line))
                {
                    this.OnContinuity?.Invoke(this,
                        new ParserEventArgs<TSDuckContinuityReport>(
                            TSDuckContinuityReport.Parse(line)));
                }
                else if (TSDuckOutputParser.IsErrorLine(line))
                {
                    string message = line.Length > 9 ? line.Substring(0) : line;
                    this.OnError?.Invoke(this, new ParserEventArgs<string>(message));
                }
                else if (TSDuckOutputParser.IsAnalyzeLine(line))
                {
                    var start = line.IndexOf('{');
                    var end = line.LastIndexOf('}') + 1;
                    string json = line.Substring(start, end - start).Trim();

                    TSDuckAnalyzerReport? analyzeReport = TSDuckAnalyzerReport.Deserialize(json);
                    if (analyzeReport != null)
                    {
                        this.OnAnalyze?.Invoke(this,
                            new ParserEventArgs<TSDuckAnalyzerReport>(analyzeReport));
                    }
                }
                else
                {
                    _logger?.LogDebug(line);
                }

            }
            catch
            {
                _logger?.LogWarning($"Error parse tsduck output: {line}");
            }
        }



        public static bool IsSrtLine(string line)
        {
            Match match = Regex.Match(line, @"\*[^\:]+(srt:)");
            return match.Success;
        }


        public static bool IsResourceMonitoringLine(string line)
        {
            Match match = Regex.Match(line, @"\*[^\:]+(\[mon\])");
            return match.Success;
        }


        public static bool IsBitrateMonitorLine(string line)
        {
            Match match = Regex.Match(line, @"\*[^\:]+(bitrate_monitor:)");
            return match.Success;
        }


        public static bool IsContinuityLine(string line)
        {
            Match match = Regex.Match(line, @"\*[^\:]+(continuity:)");
            return match.Success;
        }

        public static bool IsErrorLine(string line)
        {
            Match match = Regex.Match(line, @"\*[^\:]+(error:)");
            return match.Success;
        }

        public static bool IsAnalyzeLine(string line)
        {
            Match match = Regex.Match(line, @"\*[^\:]+(analyze:)");
            return match.Success;
        }
    }
}
