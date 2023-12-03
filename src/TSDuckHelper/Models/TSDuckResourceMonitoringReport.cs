using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TSDuckHelper
{
    public class TSDuckResourceMonitoringReport
    {
        /// <summary>
        /// 
        /// </summary>
        public int VM { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public double CPU { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public double Average { get; set; } = 0;



        /// <summary>
        /// * [mon] 2023/06/30 23:15, resource monitoring started
        /// * [mon] 2023/06/30 23:15, vm: 22 mb(+2,612 kb), cpu:0.14% (average:0.14%)
        /// * [mon] 2023/06/30 23:15, vm: 22 mb(stabilizing), cpu:0.00% (average:0.07%)
        /// * [mon] 2023/06/30 23:15, vm: 22 mb(-65,536 b), cpu:0.00% (average:0.04%)
        /// * [mon] 2023/06/30 23:15, vm: 24 mb(+2,004 kb), cpu:0.30% (average:0.11%)
        /// * [mon] 2023/06/30 23:16, vm: 25 mb(+1,196 kb), cpu:5.62% (average:1.21%)
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static TSDuckResourceMonitoringReport Parse(string line)
        {

            int vm = 0;
            double cpu = 0;
            double average = 0;

            // vm
            {
                Match match = Regex.Match(line, @"vm: ([0-9]+) mb");
                if (match.Success)
                {
                    Int32.TryParse(match.Groups[1].Value.Trim().Replace(",", string.Empty), out vm);
                }
            }

            // cpu
            {
                Match match = Regex.Match(line, @"cpu:([0-9.]+)%");
                if (match.Success)
                {
                    Double.TryParse(match.Groups[1].Value.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out cpu);
                }
            }

            // average
            {
                Match match = Regex.Match(line, @"\(average:([0-9.]+)%\)");
                if (match.Success)
                {
                    try
                    {
                        Double.TryParse(match.Groups[1].Value.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out average);
                    }
                    catch { }
                }
            }

            return new TSDuckResourceMonitoringReport()
            {
                VM = vm, 
                CPU = cpu, 
                Average = average 
            };
        }

    }
}
