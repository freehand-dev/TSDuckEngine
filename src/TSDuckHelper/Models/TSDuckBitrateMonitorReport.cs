using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TSDuckHelper
{
    public class TSDuckBitrateMonitorReport
    {
        /// <summary>
        /// 
        /// </summary>
        public int TSBitrate { get; set; } = -1; 

        /// <summary>
        /// 
        /// </summary>
        public int NETBitrate { get; set; } = -1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static TSDuckBitrateMonitorReport Parse(string line)
        {

            int tsBitrate = -1;
            int netBitrate = -1;

            // TS bitrate
            {
                Match match = Regex.Match(line, @"ts bitrate: ([0-9,]+) bits\/s");
                if (match.Success)
                {
                    Int32.TryParse(match.Groups[1].Value.Trim().Replace(",", string.Empty), out tsBitrate);
                }
            }

            // TS bitrate
            {
                Match match = Regex.Match(line, @"net bitrate: ([0-9,]+) bits\/s");
                if (match.Success)
                {
                    Int32.TryParse(match.Groups[1].Value.Trim().Replace(",", string.Empty), out netBitrate);
                }
            }

            return new TSDuckBitrateMonitorReport()
            {
                TSBitrate = tsBitrate,
                NETBitrate = netBitrate
            };
        }
    }
}
