using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TSDuckEngine.Infrastructure.Metrics;
using TSDuckEngine.Models;
using TSDuckHelper;
using TSDuckHelper.Events;
using TSDuckHelper.Models;

namespace TSDuckEngine.Services
{
    public class MetricProcessor
    {

        private readonly ILogger<MetricProcessor> _logger;
        private readonly TSDuckOutputParser _tsduckOutputParser;

        private string _input;

        private volatile IEnumerable<Measurement<int>> BitrateMonitorTsBitrate;
        private volatile IEnumerable<Measurement<int>> BitrateMonitorNetBitrate;

        private volatile IEnumerable<Measurement<int>> ResourceMonitoringMem;
        private volatile IEnumerable<Measurement<double>> ResourceMonitoringCpu;
        private volatile IEnumerable<Measurement<double>> ResourceMonitoringCpuAverage;
        

        private volatile IEnumerable<Measurement<int>> TsBitrate;
        private volatile IEnumerable<Measurement<int>> TsPcrBitrate;
        private volatile IEnumerable<Measurement<int>> TsServiceBitrate;
        private volatile IEnumerable<Measurement<int>> TsPidBitrate;
        private volatile IEnumerable<Measurement<int>> TsPidServiceCount;
        private volatile IEnumerable<Measurement<int>> TsPidMinRepititionMs;
        private volatile IEnumerable<Measurement<int>> TsPidMaxRepititionMs;
        private volatile IEnumerable<Measurement<int>> TsPidMinRepititionPkt;
        private volatile IEnumerable<Measurement<int>> TsPidMaxRepititionPkt;
        private volatile IEnumerable<Measurement<int>> TsPidRepititionMs;
        private volatile IEnumerable<Measurement<int>> TsPidRepititionPkt;
        private volatile IEnumerable<Measurement<int>> TsPidCount;
        private volatile IEnumerable<Measurement<int>> TsPcrPidCount;
        private volatile IEnumerable<Measurement<int>> TsPidUnferencedCount;

        private volatile IEnumerable<Measurement<int>> SrtGlobalInstantRttMs;
        private volatile IEnumerable<Measurement<int>> SrtReceiveInstantDeliveryDelayMs;
        private volatile IEnumerable<Measurement<int>> SrtSendInstantDeliveryDelayMs;

        private string GetShortInput(string input)
        {
            string? result = string.Empty;
            try
            {
                var uri = !string.IsNullOrEmpty(input) ? (new UriBuilder(input)) : null;
                result = uri != null ? uri.ToString() : string.Empty;
            }
            catch { }
            return result;
        }

        public MetricProcessor(ILogger<MetricProcessor> logger, IOptionsMonitor<EngineOptions> settings, TSDuckOutputParser tsduckOutputParser)
        {
            _logger = logger;
            _tsduckOutputParser = tsduckOutputParser;
            _input = GetShortInput(settings.CurrentValue.Input);

            settings.OnChange(settings =>
            {
                _logger?.LogInformation("Setting changes detected");
                _input = GetShortInput(settings.Input);
            });

            #region srt-statistic
            _tsduckOutputParser.OnSRTStatistic += delegate (object? sender, ParserEventArgs<TSDuckSrtStatisticReport> e)
            {

                //
                this.SrtGlobalInstantRttMs = new List<Measurement<int>>()
                {
                    new Measurement<int>( 
                        (int)e.Data.Global.Instant.RttMs, 
                        new KeyValuePair<string, object>("source", _input))
                };

                //
                this.SrtSendInstantDeliveryDelayMs = new List<Measurement<int>>()
                {
                    new Measurement<int>(
                        (int)e.Data.Send.Instant.DeliveryDelayMs,
                        new KeyValuePair<string, object>("source", _input))
                };

                //
                this.SrtReceiveInstantDeliveryDelayMs = new List<Measurement<int>>()
                {
                    new Measurement<int>(
                        (int)e.Data.Receive.Instant.DeliveryDelayMs,
                        new KeyValuePair<string, object>("source", _input))
                };


                //
                TSDuckEngineMeters.SrtSendBytes.Add(
                   (long)e.Data.Send.Interval.Bytes,
                   new KeyValuePair<string, object>("source", _input));


                //
                TSDuckEngineMeters.SrtSendDroppedPackets.Add(
                   (long)e.Data.Send.Interval.DroppedPackets,
                   new KeyValuePair<string, object>("source", _input));


                //
                TSDuckEngineMeters.SrtSendLostPackets.Add(
                   (long)e.Data.Send.Interval.LostPackets,
                   new KeyValuePair<string, object>("source", _input));


                //
                TSDuckEngineMeters.SrtSendPackets.Add(
                   (long)e.Data.Send.Interval.Packets,
                   new KeyValuePair<string, object>("source", _input));

                //
                TSDuckEngineMeters.SrtSendRetransmitPackets.Add(
                   (long)e.Data.Send.Interval.RetransmitPackets,
                   new KeyValuePair<string, object>("source", _input));
     

                //
                TSDuckEngineMeters.SrtReceiveBytes.Add(
                   (long)e.Data.Receive.Interval.Bytes,
                   new KeyValuePair<string, object>("source", _input));


                //
                TSDuckEngineMeters.SrtReceiveDroppedPackets.Add(
                   (long)e.Data.Receive.Interval.DroppedPackets,
                   new KeyValuePair<string, object>("source", _input));


                //
                TSDuckEngineMeters.SrtReceiveLostPackets.Add(
                   (long)e.Data.Receive.Interval.LostPackets,
                   new KeyValuePair<string, object>("source", _input));


                //
                TSDuckEngineMeters.SrtReceivePackets.Add(
                   (long)e.Data.Receive.Interval.Packets,
                   new KeyValuePair<string, object>("source", _input));

            };
            #endregion

            #region Analyze
            _tsduckOutputParser.OnAnalyze += delegate (object? sender, ParserEventArgs<TSDuckAnalyzerReport> e)
            {
                // Total TS Bitrate (188byte/pk
                this.TsBitrate = new List<Measurement<int>>()
                {
                    new Measurement<int>( (int)e.Data.Ts.Bitrate, new KeyValuePair<string, object>("service", _input), new KeyValuePair<string, object>("ts_id", e.Data.Ts.Id))
                };

                // PCR TS Bitrate (188byte/pkt)
                this.TsPcrBitrate = new List<Measurement<int>>()
                {
                    new Measurement<int>( (int)e.Data.Ts.PcrBitrate, new KeyValuePair<string, object>("source", _input), new KeyValuePair<string, object>("ts_id", e.Data.Ts.Id))
                };

                // TS Packet Suspect Ignored Count
                if (e.Data.Ts.Packets.InvalidSyncs >= 1)
                {
                    TSDuckEngineMeters.TsPacketInvalidSync.Add(
                        (int)e.Data.Ts.Packets.InvalidSyncs, new KeyValuePair<string, object>("source", _input), new KeyValuePair<string, object>("ts_id", e.Data.Ts.Id));
                }

                // TS Packet Transport Error Count
                if (e.Data.Ts.Packets.TransportErrors >= 1)
                {
                    TSDuckEngineMeters.TsPacketTeiErrors.Add(
                        (int)e.Data.Ts.Packets.TransportErrors, new KeyValuePair<string, object>("source", _input), new KeyValuePair<string, object>("ts_id", e.Data.Ts.Id));
                }

                // TS Total PID Count
                this.TsPidCount = new List<Measurement<int>>()
                {
                    new Measurement<int>( (int)e.Data.Ts.Pids.Total, new KeyValuePair<string, object>("source", _input), new KeyValuePair<string, object>("ts_id", e.Data.Ts.Id))
                };


                // TS PCR PID Count
                this.TsPcrPidCount = new List<Measurement<int>>()
                {
                    new Measurement<int>( (int)e.Data.Ts.Pids.Pcr, new KeyValuePair<string, object>("source", _input), new KeyValuePair<string, object>("ts_id", e.Data.Ts.Id))
                };


                // TS Unreferenced PID Count
                this.TsPidUnferencedCount = new List<Measurement<int>>()
                {
                    new Measurement<int>( (int)e.Data.Ts.Pids.Unreferenced, new KeyValuePair<string, object>("source", _input), new KeyValuePair<string, object>("ts_id", e.Data.Ts.Id))
                };

                // PID Bitrate
                var TsPidBitrateMeasurements = new List<Measurement<int>>();
                foreach (var pid in e.Data.Pids)
                {
                    TsPidBitrateMeasurements.Add(new Measurement<int>(
                        (int)pid.Bitrate,
                        new KeyValuePair<string, object>("source", _input),
                        new KeyValuePair<string, object>("pid", pid.Id),
                        new KeyValuePair<string, object>("pid_hexadecimal", Convert.ToString(pid.Id, 16)),
                        new KeyValuePair<string, object>("description", pid.Description)));
                }
                this.TsPidBitrate = TsPidBitrateMeasurements.ToArray();


                // PID Service Count
                var TsPidServiceCountMeasurements = new List<Measurement<int>>();
                foreach (var pid in e.Data.Pids)
                {
                    TsPidServiceCountMeasurements.Add(new Measurement<int>(
                        (int)pid.ServiceCount,
                        new KeyValuePair<string, object>("source", _input),
                        new KeyValuePair<string, object>("pid", pid.Id),
                        new KeyValuePair<string, object>("pid_hexadecimal", Convert.ToString(pid.Id, 16)),
                        new KeyValuePair<string, object>("description", pid.Description)));
                }
                this.TsPidServiceCount = TsPidServiceCountMeasurements.ToArray();

                // PID Discontinuities Count
                var TsPidDiscontinuityMeasurements = new List<Measurement<int>>();
                foreach (var pid in e.Data.Pids)
                {
                    //if (pid.Packets.Discontinuities >= 1)
                    //{
                    TSDuckEngineMeters.TsPidDiscontinuit.Add(
                        (int)pid.Packets.Discontinuities,
                        new KeyValuePair<string, object>("source", _input),
                        new KeyValuePair<string, object>("pid", pid.Id),
                        new KeyValuePair<string, object>("pid_hexadecimal", Convert.ToString(pid.Id, 16)),
                        new KeyValuePair<string, object>("description", pid.Description));
                    // }
                }

                // Duplicate PID Count
                var TsPidDuplicatedMeasurements = new List<Measurement<int>>();
                foreach (var pid in e.Data.Pids)
                {
                    if (pid.Packets.Duplicated >= 1)
                    {
                        TSDuckEngineMeters.TsPidDuplicated.Add(
                            (int)pid.Packets.Duplicated,
                            new KeyValuePair<string, object>("source", _input),
                            new KeyValuePair<string, object>("pid", pid.Id),
                            new KeyValuePair<string, object>("pid_hexadecimal", Convert.ToString(pid.Id, 16)),
                            new KeyValuePair<string, object>("description", pid.Description));
                    }
                }


                // TS bitrate per service
                var TsServiceBitrateMeasurements = new List<Measurement<int>>();
                foreach (var service in e.Data.Services)
                {
                    TsServiceBitrateMeasurements.Add(new Measurement<int>(
                        (int)service.Bitrate,
                        new KeyValuePair<string, object>("source", _input),
                        new KeyValuePair<string, object>("service_id", service.Id),
                        new KeyValuePair<string, object>("ts_id", service.Tsid),
                        new KeyValuePair<string, object>("name", service.Name),
                        new KeyValuePair<string, object>("provider", service.Provider),
                        new KeyValuePair<string, object>("type_name", service.TypeName),
                        new KeyValuePair<string, object>("pcr_pid", service.PcrPid),
                        new KeyValuePair<string, object>("pmt_pid", service.PmtPid)));
                }
                this.TsServiceBitrate = TsServiceBitrateMeasurements.ToArray();



                // Tables
                // PID Max Repitition MS
                var TsPidMaxRepititionMsMeasurements = new List<Measurement<int>>();
                foreach (var table in e.Data.Tables)
                {
                    TsPidMaxRepititionMsMeasurements.Add(new Measurement<int>(
                            (int)table.MaxRepetitionMs,
                            new KeyValuePair<string, object>("source", _input),
                            new KeyValuePair<string, object>("pid", table.Pid),
                            new KeyValuePair<string, object>("pid_hexadecimal", Convert.ToString(table.Pid, 16))));
                }
                this.TsPidMaxRepititionMs = TsPidMaxRepititionMsMeasurements;


                // PID Max Repitition Pkt
                var TsPidMaxRepititionPktMeasurements = new List<Measurement<int>>();
                foreach (var table in e.Data.Tables)
                {
                    TsPidMaxRepititionPktMeasurements.Add(new Measurement<int>(
                            (int)table.MaxRepetitionPkt,
                            new KeyValuePair<string, object>("source", _input),
                            new KeyValuePair<string, object>("pid", table.Pid),
                            new KeyValuePair<string, object>("pid_hexadecimal", Convert.ToString(table.Pid, 16))));
                }
                this.TsPidMaxRepititionPkt = TsPidMaxRepititionPktMeasurements;

                // PID Min Repitition MS
                var TsPidMinRepititionMsMeasurements = new List<Measurement<int>>();
                foreach (var table in e.Data.Tables)
                {
                    TsPidMinRepititionMsMeasurements.Add(new Measurement<int>(
                            (int)table.MinRepetitionMs,
                            new KeyValuePair<string, object>("source", _input),
                            new KeyValuePair<string, object>("pid", table.Pid),
                            new KeyValuePair<string, object>("pid_hexadecimal", Convert.ToString(table.Pid, 16))));
                }
                this.TsPidMinRepititionMs = TsPidMinRepititionMsMeasurements;

                // PID In Repitition Pkt
                var TsPidMinRepititionPktMeasurements = new List<Measurement<int>>();
                foreach (var table in e.Data.Tables)
                {
                    TsPidMinRepititionPktMeasurements.Add(new Measurement<int>(
                            (int)table.MinRepetitionPkt,
                            new KeyValuePair<string, object>("source", _input),
                            new KeyValuePair<string, object>("pid", table.Pid),
                            new KeyValuePair<string, object>("pid_hexadecimal", Convert.ToString(table.Pid, 16))));
                }
                this.TsPidMinRepititionPkt = TsPidMinRepititionPktMeasurements;

                // PID Repitition MS
                var TsPidRepititionMsMeasurements = new List<Measurement<int>>();
                foreach (var table in e.Data.Tables)
                {
                    TsPidRepititionMsMeasurements.Add(new Measurement<int>(
                            (int)table.RepetitionMs,
                            new KeyValuePair<string, object>("source", _input),
                            new KeyValuePair<string, object>("pid", table.Pid),
                            new KeyValuePair<string, object>("pid_hexadecimal", Convert.ToString(table.Pid, 16))));
                }
                this.TsPidRepititionMs = TsPidRepititionMsMeasurements;

                // PID Repitition Pkt
                var TsPidRepititionPktMeasurements = new List<Measurement<int>>();
                foreach (var table in e.Data.Tables)
                {
                    TsPidRepititionPktMeasurements.Add(new Measurement<int>( 
                            (int)table.RepetitionPkt, 
                            new KeyValuePair<string, object>("source", _input), 
                            new KeyValuePair<string, object>("pid", table.Pid), 
                            new KeyValuePair<string, object>("pid_hexadecimal", Convert.ToString(table.Pid, 16))));
                }
                this.TsPidRepititionPkt = TsPidRepititionPktMeasurements;

            };
            #endregion

            #region BitrateMonitor
            _tsduckOutputParser.OnBitrateMonitor += delegate (object? sender, ParserEventArgs<TSDuckBitrateMonitorReport> e)
            {
                // Total TS Bitrate
                this.BitrateMonitorTsBitrate = new List<Measurement<int>>()
                {
                    new Measurement<int>( (int)e.Data.TSBitrate, new KeyValuePair<string, object>("source", _input))
                };

                // Total Net Bitrate
                this.BitrateMonitorNetBitrate = new List<Measurement<int>>()
                {
                    new Measurement<int>( (int)e.Data.NETBitrate, new KeyValuePair<string, object>("source", _input))
                };
            };
            #endregion

            #region ResourceMonitoring 
            _tsduckOutputParser.OnResourceMonitoring += delegate (object? sender, ParserEventArgs<TSDuckResourceMonitoringReport> e)
            {

                // Virtual Memory
                this.ResourceMonitoringMem = new List<Measurement<int>>()
                    {
                        new Measurement<int>( (int)e.Data.VM, new KeyValuePair<string, object>("source", _input))
                    };

                // CPU
                this.ResourceMonitoringCpu = new List<Measurement<double>>()
                    {
                        new Measurement<double>( e.Data.CPU, new KeyValuePair<string, object>("source", _input))
                    };

                // Average CPU
                this.ResourceMonitoringCpuAverage = new List<Measurement<double>>()
                    {
                        new Measurement<double>( e.Data.Average, new KeyValuePair<string, object>("source", _input))
                    };

            };
            #endregion

            #region Continuity
            _tsduckOutputParser.OnContinuity += delegate (object? sender, ParserEventArgs<TSDuckContinuityReport> e)
            {
                TSDuckEngineMeters.TsContinuityErrorCount.Add(
                    (int)e.Data?.MissingPackets,
                    new KeyValuePair<string, object>("source", _input),
                    new KeyValuePair<string, object>("pid", e.Data?.PID),
                    new KeyValuePair<string, object>("pid_hexadecimal", Convert.ToString((int)e.Data?.PID, 16)));
            };
            #endregion

            #region "Register metrics"
            TSDuckEngineMeters.RegisterTsBitrateObserve(() => this.TsBitrate);
            TSDuckEngineMeters.RegisterTsPcrBitrateObserve(() => this.TsPcrBitrate);
            TSDuckEngineMeters.RegisterTsPidBitrateObserve(() => this.TsPidBitrate);
            TSDuckEngineMeters.RegisterTsPidServiceCountObserve(() => this.TsPidServiceCount);
            TSDuckEngineMeters.RegisterTsServiceBitrateObserve(() => this.TsServiceBitrate);
            TSDuckEngineMeters.RegisterTsPidMinRepititionMsObserve(() => this.TsPidMinRepititionMs);
            TSDuckEngineMeters.RegisterTsPidMaxRepititionMsObserve(() => this.TsPidMaxRepititionMs);
            TSDuckEngineMeters.RegisterTsPidMinRepititionPktObserve(() => this.TsPidMinRepititionPkt);
            TSDuckEngineMeters.RegisterTsPidMaxRepititionPktObserve(() => this.TsPidMaxRepititionPkt);
            TSDuckEngineMeters.RegisterTsPidRepititionMsObserve(() => this.TsPidRepititionMs);
            TSDuckEngineMeters.RegisterTsPidRepititionPktObserve(() => this.TsPidRepititionPkt);
            TSDuckEngineMeters.RegisterTsPidCountObserve(() => this.TsPidCount);
            TSDuckEngineMeters.RegisterTsPcrPidCountObserve(() => this.TsPcrPidCount);
            TSDuckEngineMeters.RegisterTsPidUnferencedCountObserve(() => this.TsPidUnferencedCount);

            TSDuckEngineMeters.RegisterSrtGlobalInstantRttMsObserve(() => this.SrtGlobalInstantRttMs);
            TSDuckEngineMeters.RegisterSrtReceiveInstantDeliveryDelayMsObserve(() => this.SrtReceiveInstantDeliveryDelayMs);
            TSDuckEngineMeters.RegisterSrtSendInstantDeliveryDelayMsObserve(() => this.SrtSendInstantDeliveryDelayMs);

            TSDuckEngineMeters.RegisterBitrateMonitorNetBitrateObserve(() => this.BitrateMonitorNetBitrate);
            TSDuckEngineMeters.RegisterBitrateMonitorTsBitrateObserve(() => this.BitrateMonitorTsBitrate);

            TSDuckEngineMeters.RegisterResourceMonitoringMemObserve(() => this.ResourceMonitoringMem);
            TSDuckEngineMeters.RegisterResourceMonitoringCpuObserve(() => this.ResourceMonitoringCpu);
            TSDuckEngineMeters.RegisterResourceMonitoringCpuAverageObserve(() => this.ResourceMonitoringCpuAverage);
            #endregion
        }

    }
}
