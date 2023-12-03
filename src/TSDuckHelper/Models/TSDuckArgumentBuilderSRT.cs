using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TSDuckHelper.Models
{
    public class TSDuckArgumentBuilderSRT : TSDuckArgument
    {
        /// <summary>
        /// 
        /// </summary>
        public enum SrtMode
        {
            Caller,
            Listener
        }

        public enum SrtApiMode
        {
            MessageApi,
            BufferApi
        }

        public enum SrtKeyLen
        {
            None = 0,
            AES128 = 16,
            AES192 = 24,
            AES256 = 32,
        }

        public enum SrtTransportType
        {
            Live,
            File,
        }



        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("mode")]
        public SrtMode Mode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IPAddress Address { get; set; } = IPAddress.Parse("0.0.0.0");

        /// <summary>
        /// 
        /// </summary>
        public int Port { get; set; } = 9000;


        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("streamid")]
        public string? StreamId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public SrtApiMode API { get; set; } = SrtApiMode.MessageApi;


        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("transtype")]
        public SrtTransportType TransportType { get; set; } = SrtTransportType.Live;

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan? ConnTimeout { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? TOS { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? TTL { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? Latency { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan? NAKReport { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("statistics-interval")]
        public int? StatisticIntervalMs { get; set; } = 1000;

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public TimeSpan? StatisticInterval 
        {
            get
            {
                return (StatisticIntervalMs != null  && StatisticIntervalMs  >= 1000) ? TimeSpan.FromMilliseconds(StatisticIntervalMs.Value) : null;
            }
            set
            {
                StatisticIntervalMs = (int?)(value?.TotalMilliseconds);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("local-interface")]
        public string? LocalInterface { get; set; }

        [JsonIgnore]
        public IPAddress? Interface { get => !string.IsNullOrEmpty(LocalInterface) ? IPAddress.Parse(LocalInterface) : null; set => LocalInterface = value?.ToString(); }

        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("passphrase")]
        public string? Passphrase { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [JsonPropertyName("pbkeylen")]
        public SrtKeyLen PublicKeyLen { get; set; } = SrtKeyLen.AES256;

        public TSDuckArgumentBuilderSRT()
        {

        }

        public TSDuckArgumentBuilderSRT(IPAddress address, int port, SrtMode mode = SrtMode.Listener)
        {
            Address = address;
            Port = port;
            Mode = mode;
        }


        public override string ToString()
        {
            List<string> args = new List<string>();

            args.Add($"srt --{Mode.ToString().ToLower()} {Address}:{Port}");

            if (!string.IsNullOrEmpty(this.StreamId))
                args.Add($"--streamid {this.StreamId}");

            if (this.TTL.HasValue)
                args.Add($"--ttl {TTL}");

            if (this.TOS.HasValue)
                args.Add($"--tos {TOS}");

            if (this.Interface != null)
                args.Add($"--local-address {Interface}");

            args.Add($"--transtype {this.TransportType.ToString().ToLower()}");

            if (!string.IsNullOrEmpty(this.Passphrase))
            {
                args.Add($"--passphrase {this.Passphrase}");
                args.Add($"--pbkeylen {(int)this.PublicKeyLen}");
            }

            if (this.StatisticInterval != null)
            {
                args.Add($"--statistics-interval {StatisticInterval.Value.TotalMilliseconds}");

                args.Add($"--json-line");

            }


            return String.Join(" ", args);
        }

    }
}
