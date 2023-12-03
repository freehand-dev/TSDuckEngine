using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TSDuckHelper.Models
{
    /// <summary>
    /// ip 239.78.211.1:1234 --ttl 1 --local-address 10.186.0.2 -p 7 -e 
    /// </summary>
    public class TSDuckArgumentBuilderIP : TSDuckArgument
    {
        /// <summary>
        /// The address specifies an IP
        /// address which can be either unicast or multicast.It can be also a host name that translates to an IP
        /// address.
        /// </summary>
        public IPAddress Address { get; set; }

        /// <summary>
        /// The port specifies the destination UDP port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Specifies the TTL (Time-To-Live) socket option. The actual option is either "Unicast TTL" or
        /// "Multicast TTL", depending on the destination address.
        /// Warning: Remember than the default Multicast TTL is 1 on most systems
        /// </summary>
        [JsonPropertyName("ttl")]
        public int? TTL { get; set; }

        /// <summary>
        /// Specifies the TOS (Type-Of-Service) socket option. Depending on the specified value or on the operating system, this option may require privileges or may even have no effect at all.
        /// </summary>
        [JsonPropertyName("tos")]
        public int? TOS { get; set; }

        /// <summary>
        /// When the destination is a multicast address, specify the IP address of the outgoing local interface. It can be also a host name that translates to a local address
        /// </summary>
        [JsonPropertyName("local-address")]
        public string? LocalAddress { get; set; }

        [JsonIgnore]
        public IPAddress? Interface { get => !string.IsNullOrEmpty(LocalAddress) ? IPAddress.Parse(LocalAddress) : null; set => LocalAddress = value?.ToString(); }

        /// <summary>
        /// Enforce that the number of TS packets per UDP packet is exactly what is specified in option --packet-burst. By default, this is only a maximum value.
        /// For instance, without --enforce-burst and the default --packet-burst value (7 packets), if the output plugin receives 16 TS packets, it immediately sends 3 UDP packets containing 7, 7 and 2 TS packets respectively.
        /// With option --enforce-burst, only the first 14 TS packets would be sent, using 2 UDP packets.
        /// The remaining 2 TS packets are buffered, delaying their departure until 5 more TS packets are available.
        /// </summary>
        [JsonPropertyName("enforce-burst")]
        public bool EnforceBurst { get; set; }

        /// <summary>
        /// Specifies the maximum number of TS packets to be grouped into each UDP datagram. The default is 7, the maximum is 128
        /// </summary>
        [JsonPropertyName("packet-burst")]
        public int? PacketBurst { get; set; }

        public TSDuckArgumentBuilderIP()
        {

        }

        public TSDuckArgumentBuilderIP(IPAddress address, int port)
        {
            Address = address;
            Port = port;
        }


        public override string ToString()
        {

            List<string> args = new List<string>();

            args.Add($"ip {Address}:{Port}");


            if (this.TTL.HasValue)
                args.Add($"--ttl {TTL}");

            if (this.TOS.HasValue)
                args.Add($"--tos {TOS}");


            if (this.Interface != null)
                args.Add($"--local-address {Interface}");

            if (this.PacketBurst.HasValue)
                args.Add($"--packet-burst {PacketBurst}");

            if (this.EnforceBurst)
                args.Add($"--enforce-burst");

            return String.Join(" ", args);

        }


        public static TSDuckArgumentBuilderIP BuildDefaultOutput(string address, int port, string? localAddress = null)
        {
            var result = new TSDuckArgumentBuilderIP()
            {
                Address = IPAddress.Parse(address),
                Port = port,
                TTL = 1,
                PacketBurst = 7,
                EnforceBurst = true,
            };

            if (!string.IsNullOrEmpty(localAddress))
            {
                result.Interface = IPAddress.Parse(localAddress);   
            }

            return result;
        }

    }
}
