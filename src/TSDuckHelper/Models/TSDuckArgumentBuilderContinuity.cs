using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSDuckHelper.Models
{
    public class TSDuckArgumentBuilderContinuity : TSDuckArgument
    {
        /// <summary>
        /// Fix incorrect continuity counters. By default, only display discontinuities.
        /// </summary>
        public bool Fix { get; set; } = false;


        public TSDuckArgumentBuilderContinuity()
        {

        }

        public override string ToString()
        {

            List<string> args = new List<string>();

            args.Add($"continuity");

            if (this.Fix)
                args.Add($"--fix");

            return String.Join(" ", args);

        }

    }

}
