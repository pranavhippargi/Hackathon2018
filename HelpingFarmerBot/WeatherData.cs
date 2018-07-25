using System.Collections.Generic;
using Newtonsoft.Json;

namespace HelpingFarmerBot
{
    public class WeatherData
    {
        [JsonProperty("main")]
        public Dictionary<string, string> Main { get; set; }
    }
}
