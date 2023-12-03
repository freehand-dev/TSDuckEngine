using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TSDuckHelper.Models
{
    public class TSDuckSrtStatisticReport
    {
        [JsonPropertyName("global")]
        public TSDuckSrtStatisticGlobalReport Global { get; set; }= new TSDuckSrtStatisticGlobalReport();

        [JsonPropertyName("receive")]
        public TSDuckSrtStatisticReceiveReport Receive { get; set; } = new TSDuckSrtStatisticReceiveReport();

        [JsonPropertyName("send")]
        public TSDuckSrtStatisticSendReport Send { get; set; } = new TSDuckSrtStatisticSendReport();


        public static TSDuckSrtStatisticReport? Deserialize(string line)
        {
            return JsonSerializer.Deserialize<TSDuckSrtStatisticReport>(line);
        }
    }


    public class TSDuckSrtStatisticGlobalReport
    {
        [JsonPropertyName("instant")]
        public TSDuckSrtStatisticGlobalInstantReport Instant { get; set; } = new TSDuckSrtStatisticGlobalInstantReport();
    }

    public class TSDuckSrtStatisticGlobalInstantReport
    {
        [JsonPropertyName("rtt-ms")]
        public int RttMs { get; set; }
    }

    public class TSDuckSrtStatisticReceiveStatisticReport
    {
        [JsonPropertyName("bytes")]
        public ulong Bytes { get; set; } 

        [JsonPropertyName("dropped-packets")]
        public ulong DroppedPackets { get; set; }

        [JsonPropertyName("lost-packets")]
        public ulong LostPackets { get; set; }

        [JsonPropertyName("packets")]
        public ulong Packets { get; set; } 

        [JsonPropertyName("retransmit-packets")]
        public ulong RetransmitPackets { get; set; }
    }

    public class TSDuckSrtStatisticReceiveReport
    {
        [JsonPropertyName("instant")]
        public TSDuckSrtStatisticInstantReport Instant { get; set; } = new TSDuckSrtStatisticInstantReport();

        [JsonPropertyName("interval")]
        public TSDuckSrtStatisticReceiveStatisticReport Interval { get; set; } = new TSDuckSrtStatisticReceiveStatisticReport();

        [JsonPropertyName("total")]
        public TSDuckSrtStatisticReceiveStatisticReport Total { get; set; } = new TSDuckSrtStatisticReceiveStatisticReport();
    }

    public class TSDuckSrtStatisticInstantReport
    {
        [JsonPropertyName("delivery-delay-ms")]
        public long DeliveryDelayMs { get; set; }
    }


    public class TSDuckSrtStatisticSendReport
    {
        [JsonPropertyName("instant")]
        public TSDuckSrtStatisticInstantReport Instant { get; set; } = new TSDuckSrtStatisticInstantReport();

        [JsonPropertyName("interval")]
        public TSDuckSrtStatisticReceiveStatisticReport Interval { get; set; } = new TSDuckSrtStatisticReceiveStatisticReport();

        [JsonPropertyName("total")]
        public TSDuckSrtStatisticReceiveStatisticReport Total { get; set; } = new TSDuckSrtStatisticReceiveStatisticReport();
    }

}
