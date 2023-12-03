using TSDuckEngine.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSDuckEngine.Infrastructure.Metrics
{
    public static class TSDuckEngineMeters
    {

        internal static readonly Meter Meter = new Meter("TSDuckEngine", "1.0.0");


        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterSrtGlobalInstantRttMsObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_srt_statistic_global_instant_rtt_ms",
                observeValues: observeValue,
                unit: "Round Trip Time",
                description: "");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterSrtReceiveInstantDeliveryDelayMsObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_srt_statistic_receive_instant_delivery_delay_ms",
                observeValues: observeValue,
                unit: "ms",
                description: "Delivery delay");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterSrtSendInstantDeliveryDelayMsObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_srt_statistic_send_instant_delivery_delay_ms",
                observeValues: observeValue,
                unit: "ms",
                description: "Delivery delay");
        }


        /// <summary>
        /// 
        /// </summary>
        public static Counter<long> SrtSendBytes = TSDuckEngineMeters.Meter.CreateCounter<long>(
            name: "tsp_srt_statistic_send_bytes",
            description: "");


        /// <summary>
        /// 
        /// </summary>
        public static Counter<long> SrtSendDroppedPackets = TSDuckEngineMeters.Meter.CreateCounter<long>(
            name: "tsp_srt_statistic_send_dropped_packets",
            description: "");


        /// <summary>
        /// 
        /// </summary>
        public static Counter<long> SrtSendLostPackets = TSDuckEngineMeters.Meter.CreateCounter<long>(
            name: "tsp_srt_statistic_send_lost_packets",
            description: "");

        /// <summary>
        /// 
        /// </summary>
        public static Counter<long> SrtSendPackets = TSDuckEngineMeters.Meter.CreateCounter<long>(
            name: "tsp_srt_statistic_send_packets",
            description: "");



        /// <summary>
        /// 
        /// </summary>
        public static Counter<long> SrtSendRetransmitPackets = TSDuckEngineMeters.Meter.CreateCounter<long>(
            name: "tsp_srt_statistic_send_retransmit_packets",
            description: "");


        /// <summary>
        /// 
        /// </summary>
        public static Counter<long> SrtReceiveBytes = TSDuckEngineMeters.Meter.CreateCounter<long>(
            name: "tsp_srt_statistic_receive_bytes",
            description: "");


        /// <summary>
        /// 
        /// </summary>
        public static Counter<long> SrtReceiveDroppedPackets = TSDuckEngineMeters.Meter.CreateCounter<long>(
            name: "tsp_srt_statistic_receive_dropped_packets",
            description: "");


        /// <summary>
        /// 
        /// </summary>
        public static Counter<long> SrtReceiveLostPackets = TSDuckEngineMeters.Meter.CreateCounter<long>(
            name: "tsp_srt_statistic_receive_lost_packets",
            description: "");

        /// <summary>
        /// 
        /// </summary>
        public static Counter<long> SrtReceivePackets = TSDuckEngineMeters.Meter.CreateCounter<long>(
            name: "tsp_srt_statistic_receive_packets",
            description: "");






        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterResourceMonitoringMemObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_resource_monitoring_mem",
                observeValues: observeValue,
                unit: "Mb",
                description: "The TSP process virtual memory usage.");
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterResourceMonitoringCpuObserve(Func<IEnumerable<Measurement<double>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<double>(
                name: "tsp_resource_monitoring_cpu",
                observeValues: observeValue,
                unit: "%",
                description: "The TSP process CPU usage.");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterResourceMonitoringCpuAverageObserve(Func<IEnumerable<Measurement<double>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<double>(
                name: "tsp_resource_monitoring_cpu_avg",
                observeValues: observeValue,
                unit: "%",
                description: "The TSP process average CPU usage.");
        }






        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterBitrateMonitorTsBitrateObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_bitrate_monitor_ts_bitrate",
                observeValues: observeValue,
                unit: "bps",
                description: "The overall TS bitrate.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterBitrateMonitorNetBitrateObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_bitrate_monitor_net_bitrate",
                observeValues: observeValue,
                unit: "bps",
                description: "The overall Net bitrate, without null packets.");
        }



        /// <summary>
        /// 
        /// </summary>
        public static Counter<long> TsContinuityErrorCount = TSDuckEngineMeters.Meter.CreateCounter<long>(
            name: "tsp_continuity_error_count",
            description: "The number of continuity errors detected.");


        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterTsBitrateObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_analyze_ts_bitrate", 
                observeValues: observeValue, 
                unit: "bps",
                description: "The overall TS bitrate based upon 188 byte TS packet.");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterTsPcrBitrateObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_analyze_ts_pcr_bitrate", 
                observeValues: observeValue,
                unit: "bps",
                description: "The PCR bitrate of the TS based upon 188 byte TS packet.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterTsServiceBitrateObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_analyze_ts_service_bitrate",
                observeValues: observeValue,
                description: "The bitrate of each service carried in the TS based upon a 188 byte TS packet.");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterTsPidBitrateObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_analyze_ts_pid_bitrate", 
                observeValues: observeValue, 
                unit: "bps",
                description: "The bitrate for an individual PID based upon a 188 byte TS packet.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterTsPidServiceCountObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_analyze_ts_pid_service_count", 
                observeValues: observeValue,
                description: "The total number of services within the a given PID in the TS.");
        }

        /// <summary>
        /// 
        /// </summary>
        public static Counter<int> TsPidDiscontinuit = TSDuckEngineMeters.Meter.CreateCounter<int>(
            name: "tsp_analyze_ts_pid_discontinuities",
            description: "The discontinuities per PID.");


        /// <summary>
        /// 
        /// </summary>
        public static Counter<int> TsPidDuplicated = TSDuckEngineMeters.Meter.CreateCounter<int>(
            name: "tsp_analyze_ts_pid_duplicated",
            description: "The number of duplicated PIDs seen for a given PID since the start of monitoring.");


        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterTsPidMinRepititionMsObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_analyze_ts_pid_min_repitition_ms",
                observeValues: observeValue,
                unit: "ms");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterTsPidMaxRepititionMsObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_analyze_ts_pid_max_repitition_ms",
                observeValues: observeValue,
                unit: "ms");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterTsPidMinRepititionPktObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_analyze_ts_pid_min_repitition_pkt",
                observeValues: observeValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterTsPidMaxRepititionPktObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_analyze_ts_pid_max_repitition_pkt",
                observeValues: observeValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterTsPidRepititionMsObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_analyze_ts_pid_repitition_ms",
                observeValues: observeValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterTsPidRepititionPktObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_analyze_ts_pid_repitition_pkt",
                observeValues: observeValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static Counter<long> TsPacketInvalidSync = TSDuckEngineMeters.Meter.CreateCounter<long>(
            name: "tsp_analyze_ts_packet_invalid_sync",
            description: "The number of invalid sync packets detected.");

        /// <summary>
        /// 
        /// </summary>
        public static Counter<long> TsPacketSuspectIgnored = TSDuckEngineMeters.Meter.CreateCounter<long>(
            name: "tsp_analyze_ts_packet_suspect_ignored",
            description: "The number of suspect packets ignored.");


        /// <summary>
        /// 
        /// </summary>
        public static Counter<long> TsPacketTeiErrors = TSDuckEngineMeters.Meter.CreateCounter<long>(
            name: "tsp_analyze_ts_packet_tei_count",
            description: "The number of transport errors detected.");




        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterTsPidCountObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_analyze_ts_pid_count",
                observeValues: observeValue, 
                description: "The total number of pids detected in the TS.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterTsPcrPidCountObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_analyze_ts_pid_pcr_count",
                observeValues: observeValue,
                description: "The total number of PCR pids detected in the TS.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="observeValue"></param>
        public static void RegisterTsPidUnferencedCountObserve(Func<IEnumerable<Measurement<int>>> observeValue)
        {
            TSDuckEngineMeters.Meter.CreateObservableGauge<int>(
                name: "tsp_analyze_ts_pid_unreferenced_count",
                observeValues: observeValue,
                description: "The total number of unreferenced pids detected in the TS.");
        }

        /// <summary>
        /// 
        /// </summary>
        public static Counter<int> TSPInstanceRestart = TSDuckEngineMeters.Meter.CreateCounter<int>(
            name: "tsp_instance_restart_count",
            description: "The number of transport errors detected.");


        /// <summary>
        /// 
        /// </summary>
        public static ObservableCounter<double> ProcessorTimes = TSDuckEngineMeters.Meter.CreateObservableCounter(
            name: "process_cpu_time", 
            observeValues: TSDuckEngineMeters.GetProcessorTimes, 
            unit: "s", 
            description: "Processor time of this process");

        private static IEnumerable<Measurement<double>> GetProcessorTimes()
        {
            var process = Process.GetCurrentProcess();
            return new[]
            {
                new Measurement<double>(process.UserProcessorTime.TotalSeconds, new KeyValuePair<string, object?>("state", "user")),
                new Measurement<double>(process.PrivilegedProcessorTime.TotalSeconds, new KeyValuePair<string, object?>("state", "system")),
            };
        }


    }
}
