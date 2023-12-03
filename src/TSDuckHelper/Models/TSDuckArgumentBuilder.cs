using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Text.Json.Serialization;
using System.Web;
using TSDuckHelper.Models;

namespace TSDuckHelper
{
    public class TSDuckArgumentBuilder
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
        public TSDuckArgument Input { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<TSDuckArgument> Processing { get; set; } = new List<TSDuckArgument>();

        /// <summary>
        /// 
        /// </summary>
        public TSDuckArgument? Output { get; set; }

        public TSDuckArgumentBuilder(TSDuckArgument input)
        {
            this.Input = input;
        }

        public override string ToString()
        {

            List<string> args = new List<string>();

            // global
            if (this.Realtime)
                args.Add("--realtime");

            if (this.SynchronousLoging)
                args.Add("--synchronous-log");

            if (this.ResourceMonitoring)
                args.Add("--monitor");


            // input
            args.Add($"-I {this.Input.ToString()}");

            // plugins
            foreach (TSDuckArgument processingStep in this.Processing)
            {
                args.Add($"-P {processingStep.ToString()}");
            }



            // output
            args.Add($"-O {this.Output?.ToString() ?? "drop"}");

            return String.Join(" ", args);
        }

        public static TSDuckArgument Parse(Uri uri)
        {
            switch (uri.Scheme)
            {
                case "srt":
                    return TSDuckArgumentBuilder.GetFromQueryString<TSDuckArgumentBuilderSRT>(uri);
                case "udp":
                    return TSDuckArgumentBuilder.GetFromQueryString<TSDuckArgumentBuilderIP>(uri);
                default:
                    throw new Exception($"Protocol not supported: {uri.Scheme} ({uri})");
            }
        }

        public static T GetFromQueryString<T>(Uri uri) where T : new()
        {
            Dictionary<string, string> QueryString = !string.IsNullOrEmpty(uri.Query) ? uri.Query.TrimStart('?').ToLower().Split('&').ToDictionary(x => x.Split('=')[0], x => x.Split('=')[1]) ?? new() : new();

            var obj = new T();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                string propertyName = property.Name.ToLower();

                var hasJsonIgnore = Attribute.IsDefined(property, typeof(JsonIgnoreAttribute));
                if (hasJsonIgnore)
                {
                    continue;
                }


                var attr = (JsonPropertyNameAttribute[])property.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false);
                if (attr.Length > 0)
                {
                    propertyName = attr[0]?.Name.ToLower() ?? propertyName;
                }


                object? value = null; 
                if (propertyName.Equals("address", StringComparison.OrdinalIgnoreCase))
                {
                    value = IPAddress.Parse(uri.Host);
                }
                else if (propertyName.Equals("port", StringComparison.OrdinalIgnoreCase))
                {
                    value = uri.Port;
                } 
                else if (QueryString.ContainsKey(propertyName))
                {
                    var valueAsString = QueryString[propertyName];
                    value = property.PropertyType == typeof(string) ? HttpUtility.UrlDecode(valueAsString) : TypeDescriptor.GetConverter(property.PropertyType).ConvertFromString(null, CultureInfo.InvariantCulture, valueAsString);   
                }

                if (value != null)
                {
                    property.SetValue(obj, value, null);
                }
                
            }
            return obj;
        }
    }





}






