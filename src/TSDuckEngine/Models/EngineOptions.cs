using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSDuckEngine.Models
{
    public class EngineOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public TspInstanceOptions Tsp { get; set; } = new TspInstanceOptions();

        /// <summary>
        /// 
        /// </summary>
        public string Input { get; set; } = "udp://239.45.45.45:1234";

        /// <summary>
        /// 
        /// </summary>
        public string? Output { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<string> Processing { get; set; } = new List<string>(); 
        
        /// <summary>
        /// 
        /// </summary>
        public TimeSpan RestartInstanceDelay { get; set; } = TimeSpan.FromSeconds(5);
    }



    public class TspInstanceOptions
    {
        /// <summary>
        /// 
        /// </summary>
        public bool Realtime { get; set; } = true;


        /// <summary>
        /// 
        /// </summary>
        public bool SynchronousLoging { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public bool ResourceMonitoring { get; set; } = true;


        /// <summary>
        /// 
        /// </summary>
        public bool ContinuityMonitoring { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public bool BitrateMonitoring { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        public bool Analyzing { get; set; } = true;

    }
}
