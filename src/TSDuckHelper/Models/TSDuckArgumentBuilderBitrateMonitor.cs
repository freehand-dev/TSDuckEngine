using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSDuckHelper.Models
{

    public class TSDuckArgumentBuilderBitrateMonitor : TSDuckArgument
    {
        /// <summary>
        /// Always report the bitrate and net bitrate (without null packets) at the specific intervals in seconds, even if the bitrate is in range.
        /// </summary>
        public TimeSpan? PeriodicBitrate { get; set; } = TimeSpan.FromSeconds(1);

        /// <summary>
        /// Time interval in seconds used to compute the bitrate.
        /// </summary>
        public TimeSpan? TimeInterval { get; set; } = TimeSpan.FromSeconds(1);


        public TSDuckArgumentBuilderBitrateMonitor()
        {

        }

        public override string ToString()
        {

            List<string> args = new List<string>();

            args.Add($"bitrate_monitor");

            if (this.PeriodicBitrate.HasValue)
                args.Add($"--periodic-bitrate {this.PeriodicBitrate.Value.TotalSeconds}");

            if (this.TimeInterval.HasValue)
                args.Add($"--time-interval {this.TimeInterval.Value.TotalSeconds}");

            return String.Join(" ", args);

        }

    }
}
