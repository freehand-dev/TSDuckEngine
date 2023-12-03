using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TSDuckHelper
{
    public class TSDuckContinuityReport
    {
        /// <summary>
        /// 
        /// </summary>
        public ulong PacketIndex {get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public int MissingPackets { get; set; } = -1;

        /// <summary>
        /// 
        /// </summary>
        public ushort PID { get; set; } = 0;

        /// <summary>
        /// * continuity: packet index: 6,078, PID: 0x0100, missing 5 packets
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static TSDuckContinuityReport Parse(string line)
        {

            ulong packetIndex = 0;
            int missingPackets = 0;
            ushort pid = 0;

            // packet index
            {
                Match match = Regex.Match(line, @"packet index: ([0-9,]+),");
                if (match.Success)
                {
                    UInt64.TryParse(match.Groups[1].Value.Trim().Replace(",", string.Empty), out packetIndex);
                }
            }

            // missing packets
            {
                Match match = Regex.Match(line, @"missing ([0-9]+) packets$");
                if (match.Success)
                {
                    Int32.TryParse(match.Groups[1].Value.Trim(), out missingPackets);
                }
            }

            // pid
            {
                Match match = Regex.Match(line, @"pid: ([0-9x]+),");
                if (match.Success)
                {
                    try
                    {
                        pid = Convert.ToUInt16(match.Groups[1].Value.Trim(), 16);
                    }
                    catch { }
                }
            }

            return new TSDuckContinuityReport()
            {
                PacketIndex = packetIndex,
                PID = pid,
                MissingPackets = missingPackets
            };
        }
    }
}
