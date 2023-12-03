using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSDuckHelper.Models
{
    public class TSDuckArgumentBuilderAnalyze : TSDuckArgument
    {
        /// <summary>
        /// Produce a new output file at regular intervals. After outputting a file, the analysis context is reset, i.e.each output file contains a fully independent analysis.
        /// </summary>
        public TimeSpan? Interval { get; set; } = TimeSpan.FromSeconds(1);

        public bool OutputJson { get; set; } = true;


        public TSDuckArgumentBuilderAnalyze()
        {

        }

        public override string ToString()
        {

            List<string> args = new List<string>();

            args.Add($"analyze");

            //
            if (this.Interval.HasValue)
            {
                args.Add($"--interval {this.Interval.Value.TotalSeconds}");
            }

            //
            if (OutputJson)
            {
                args.Add($"--json-line");
            }


            return String.Join(" ", args);

        }

    }

}
