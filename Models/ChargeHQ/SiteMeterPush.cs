using Newtonsoft.Json;

namespace HuaweiSolar.Models.ChargeHQ
{
    public class SiteMeterPush
    {
        // obtain your API key from the app: My Equipment -> Solar / Battery Equipment, Push API
        public string apiKey { get; set; }

        // timestamp of meter data (milliseconds since epoch)
        // if the meter data is delayed and has a reliable timestamp then this field should
        // be provided if, otherwise it should be left unset
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? tsms { get; set; } = null;

        // set this field only if there was an error obtaining the meter data
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string error { get; set; } = null;

        // provide the meter data, unless there was an error
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SiteMeter siteMeters { get; set; } = null;
    }
}
