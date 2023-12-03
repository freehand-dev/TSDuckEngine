using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TSDuckHelper
{
    /// <summary>
    /// https://app.quicktype.io/
    /// </summary>
    public class TSDuckAnalyzerReport
    {
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("pids")]
        public List<TsDuckAnalyzeerPidReport> Pids { get; set; } = new List<TsDuckAnalyzeerPidReport>();

        [JsonPropertyName("services")]
        public List<TsDuckAnalyzeerServiceReport> Services { get; set; } = new List<TsDuckAnalyzeerServiceReport> { };

        [JsonPropertyName("tables")]
        public List<TsDuckAnalyzeerTableReport> Tables { get; set; } = new List<TsDuckAnalyzeerTableReport>();

        [JsonPropertyName("time")]
        public TsDuckAnalyzeerTimeReport Time { get; set; } = new TsDuckAnalyzeerTimeReport();

        [JsonPropertyName("ts")]
        public TsDuckAnalyzeerTsReport Ts { get; set; } = new TsDuckAnalyzeerTsReport();


        public static TSDuckAnalyzerReport? Deserialize(string json)
        {
            return JsonSerializer.Deserialize<TSDuckAnalyzerReport>(json);
        }
    }

    public class TsDuckAnalyzeerPidReport
    {
        [JsonPropertyName("audio")]
        public bool Audio { get; set; }

        [JsonPropertyName("bitrate")]
        public long Bitrate { get; set; }

        [JsonPropertyName("bitrate-204")]
        public long Bitrate204 { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("ecm")]
        public bool Ecm { get; set; }

        [JsonPropertyName("emm")]
        public bool Emm { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("cas")]
        public long? Cas { get; set; }
        
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("crypto-period")]
        public int? CryptoPeriod { get; set; }
        
        [JsonPropertyName("global")]
        public bool Global { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("is-scrambled")]
        public bool IsScrambled { get; set; }

        [JsonPropertyName("packets")]
        public TsDuckAnalyzeerPidPacketsReport Packets { get; set; } = new TsDuckAnalyzeerPidPacketsReport();

        [JsonPropertyName("pmt")]
        public bool Pmt { get; set; }

        [JsonPropertyName("service-count")]
        public int ServiceCount { get; set; }

        [JsonPropertyName("t2mi")]
        public bool T2Mi { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("unit-start")]
        public long? UnitStart { get; set; }

        [JsonPropertyName("unreferenced")]
        public bool Unreferenced { get; set; }

        [JsonPropertyName("video")]
        public bool Video { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("services")]
        public List<long>? Services { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("first-dts")]
        public long? FirstDts { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("first-pcr")]
        public long? FirstPcr { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("first-pts")]
        public long? FirstPts { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("invalid-pes-prefix")]
        public long? InvalidPesPrefix { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("last-dts")]
        public long? LastDts { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("last-pcr")]
        public long? LastPcr { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("last-pts")]
        public long? LastPts { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("pes")]
        public long? Pes { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("pes-stream-id")]
        public long? PesStreamId { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("language")]
        public string? Language { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("languages")]
        public List<string>? Languages { get; set; }
    }


    public partial class TsDuckAnalyzeerPidPacketsReport
    {
        [JsonPropertyName("af")]
        public long Af { get; set; }

        [JsonPropertyName("clear")]
        public long Clear { get; set; }

        [JsonPropertyName("discontinuities")]
        public long Discontinuities { get; set; }

        [JsonPropertyName("dts")]
        public long Dts { get; set; }

        [JsonPropertyName("dts-leap")]
        public long DtsLeap { get; set; }

        [JsonPropertyName("duplicated")]
        public long Duplicated { get; set; }

        [JsonPropertyName("invalid-scrambling")]
        public long InvalidScrambling { get; set; }

        [JsonPropertyName("pcr")]
        public long Pcr { get; set; }

        [JsonPropertyName("pcr-leap")]
        public long PcrLeap { get; set; }

        [JsonPropertyName("pts")]
        public long Pts { get; set; }

        [JsonPropertyName("pts-leap")]
        public long PtsLeap { get; set; }

        [JsonPropertyName("scrambled")]
        public long Scrambled { get; set; }

        [JsonPropertyName("total")]
        public long Total { get; set; }
    }

    public class TsDuckAnalyzeerServiceReport
    {
        [JsonPropertyName("bitrate")]
        public long Bitrate { get; set; }

        [JsonPropertyName("bitrate-204")]
        public long Bitrate204 { get; set; }

        [JsonPropertyName("components")]
        public TsDuckAnalyzeerServicesReport Components { get; set; } = new TsDuckAnalyzeerServicesReport();

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("is-scrambled")]
        public bool IsScrambled { get; set; }

        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("original-network-id")]
        public long OriginalNetworkId { get; set; }

        [JsonPropertyName("packets")]
        public long Packets { get; set; }

        [JsonPropertyName("pcr-pid")]
        public long PcrPid { get; set; }

        [JsonPropertyName("pids")]
        public List<long> Pids { get; set; } = new List<long>();

        [JsonPropertyName("pmt-pid")]
        public long PmtPid { get; set; }

        [JsonPropertyName("provider")]
        public string? Provider { get; set; }

        [JsonPropertyName("ssu")]
        public bool Ssu { get; set; }

        [JsonPropertyName("t2mi")]
        public bool T2Mi { get; set; }

        [JsonPropertyName("tsid")]
        public long Tsid { get; set; }

        [JsonPropertyName("type")]
        public long Type { get; set; }

        [JsonPropertyName("type-name")]
        public string? TypeName { get; set; }
    }

    public partial class TsDuckAnalyzeerServicesReport
    {
        [JsonPropertyName("clear")]
        public long Clear { get; set; }

        [JsonPropertyName("scrambled")]
        public long Scrambled { get; set; }

        [JsonPropertyName("total")]
        public long Total { get; set; }
    }

    public class TsDuckAnalyzeerTableReport
    {
        [JsonPropertyName("first-version")]
        public long FirstVersion { get; set; }

        [JsonPropertyName("last-version")]
        public long LastVersion { get; set; }

        [JsonPropertyName("max-repetition-ms")]
        public long MaxRepetitionMs { get; set; }

        [JsonPropertyName("max-repetition-pkt")]
        public long MaxRepetitionPkt { get; set; }

        [JsonPropertyName("min-repetition-ms")]
        public long MinRepetitionMs { get; set; }

        [JsonPropertyName("min-repetition-pkt")]
        public long MinRepetitionPkt { get; set; }

        [JsonPropertyName("pid")]
        public long Pid { get; set; }

        [JsonPropertyName("repetition-ms")]
        public long RepetitionMs { get; set; }

        [JsonPropertyName("repetition-pkt")]
        public long RepetitionPkt { get; set; }

        [JsonPropertyName("sections")]
        public long Sections { get; set; }

        [JsonPropertyName("tables")]
        public long Tables { get; set; }

        [JsonPropertyName("tid")]
        public long Tid { get; set; }

        [JsonPropertyName("tid-ext")]
        public long TidExt { get; set; }

        [JsonPropertyName("versions")]
        public List<long> Versions { get; set; } = new List<long>();
    }

    public class TsDuckAnalyzeerTimeReport
    {
        [JsonPropertyName("local")]
        public TsDuckAnalyzeerLocalReport Local { get; set; } = new TsDuckAnalyzeerLocalReport();

        [JsonPropertyName("utc")]
        public TsDuckAnalyzeerLocalReport Utc { get; set; } = new TsDuckAnalyzeerLocalReport();
    }

    public class TsDuckAnalyzeerLocalReport
    {
        [JsonPropertyName("system")]
        public TsDuckAnalyzeerSystemClassReport System { get; set; } = new TsDuckAnalyzeerSystemClassReport();
    }

    public class TsDuckAnalyzeerSystemClassReport
    {
        [JsonPropertyName("first")]
        public TsDuckAnalyzeerDataTimeReport First { get; set; } = new TsDuckAnalyzeerDataTimeReport();

        [JsonPropertyName("last")]
        public TsDuckAnalyzeerDataTimeReport Last { get; set; } = new TsDuckAnalyzeerDataTimeReport();
    }

    public class TsDuckAnalyzeerDataTimeReport
    {
        [JsonPropertyName("date")]
        public string? _Date { get; set; }

        [JsonIgnore]
        public DateOnly? Date { get => _Date != null ? DateOnly.Parse(_Date) : null; }

        [JsonPropertyName("seconds-since-2000")]
        public long _SecondsSince2000 { get; set; }

        [JsonIgnore]
        public TimeSpan Since2000 { get => TimeSpan.FromSeconds(_SecondsSince2000);  }

        [JsonPropertyName("time")]
        public string? _Time { get; set; }

        [JsonIgnore]
        public TimeOnly? Time { get => _Time != null ? TimeOnly.Parse(_Time) : null; }
    }

    public class TsDuckAnalyzeerTsReport
    {
        [JsonPropertyName("country")]
        public string? Country { get; set; }

        [JsonPropertyName("bitrate")]
        public int Bitrate { get; set; }

        [JsonPropertyName("bitrate-204")]
        public int Bitrate204 { get; set; }

        [JsonPropertyName("bytes")]
        public long Bytes { get; set; }

        [JsonPropertyName("duration")]
        public long Duration { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("packets")]
        public TsDuckAnalyzeerPacketsReport Packets { get; set; } = new TsDuckAnalyzeerPacketsReport();

        [JsonPropertyName("pcr-bitrate")]
        public int PcrBitrate { get; set; }

        [JsonPropertyName("pcr-bitrate-204")]
        public int PcrBitrate204 { get; set; }

        [JsonPropertyName("pids")]
        public TsDuckAnalyzeerPidsReport Pids { get; set; } = new TsDuckAnalyzeerPidsReport();

        [JsonPropertyName("services")]
        public TsDuckAnalyzeerServicesReport Services { get; set; } = new TsDuckAnalyzeerServicesReport();

        [JsonPropertyName("user-bitrate")]
        public int UserBitrate { get; set; }

        [JsonPropertyName("user-bitrate-204")]
        public int UserBitrate204 { get; set; }
    }

    public class TsDuckAnalyzeerPacketsReport
    {
        [JsonPropertyName("invalid-syncs")]
        public long InvalidSyncs { get; set; }

        [JsonPropertyName("suspect-ignored")]
        public long SuspectIgnored { get; set; }

        [JsonPropertyName("total")]
        public long Total { get; set; }

        [JsonPropertyName("transport-errors")]
        public long TransportErrors { get; set; }
    }

    public class TsDuckAnalyzeerPidsReport
    {
        [JsonPropertyName("clear")]
        public long Clear { get; set; }

        [JsonPropertyName("global")]
        public TsDuckAnalyzeerGlobalReport Global { get; set; } = new TsDuckAnalyzeerGlobalReport();

        [JsonPropertyName("pcr")]
        public long Pcr { get; set; }

        [JsonPropertyName("scrambled")]
        public long Scrambled { get; set; }

        [JsonPropertyName("total")]
        public long Total { get; set; }

        [JsonPropertyName("unreferenced")]
        public long Unreferenced { get; set; }
    }

    public class TsDuckAnalyzeerGlobalReport
    {
        [JsonPropertyName("bitrate")]
        public long Bitrate { get; set; }

        [JsonPropertyName("bitrate-204")]
        public long Bitrate204 { get; set; }

        [JsonPropertyName("clear")]
        public long Clear { get; set; }

        [JsonPropertyName("is-scrambled")]
        public bool IsScrambled { get; set; }

        [JsonPropertyName("packets")]
        public long Packets { get; set; }

        [JsonPropertyName("pids")]
        public List<long> Pids { get; set; } = new List<long>();

        [JsonPropertyName("scrambled")]
        public long Scrambled { get; set; }

        [JsonPropertyName("total")]
        public long Total { get; set; }
    }
}