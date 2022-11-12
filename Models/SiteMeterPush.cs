using Newtonsoft.Json;

namespace HuaweiSolar.Models
{
    public class SiteMeterPush
    {
        public string apiKey { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public long? tsms { get; set; } = null;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string error { get; set; } = null;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public SiteMeter sitesMeters { get; set; } = null;
    }

    public class SiteMeter
    {
        public double production_kw { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? net_import_kw {get; set;} = null;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? consumption_kw {get; set;} = null;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? imported_kwh {get; set;} = null;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? exported_kwh {get; set;} = null;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? battery_discharge_kw {get; set;} = null;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? battery_soc {get; set;} = null;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? battery_energy_kwh {get; set;} = null;
    }
}