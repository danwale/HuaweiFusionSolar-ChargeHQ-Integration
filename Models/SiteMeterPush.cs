using Newtonsoft.Json;

namespace HuaweiSolar.Models
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

    public class SiteMeter
    {
        // if solar is present, provide the following field
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? production_kw { get; set; } = null;

        // if a consumption meter is present, the following fields should be set
        //grid import, negative means export
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? net_import_kw {get; set;} = null;

        //total site consumption
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? consumption_kw {get; set;} = null;

        // if accumulated import/export energy is available, set the following fields
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? imported_kwh {get; set;} = null;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? exported_kwh {get; set;} = null;

        // if a battery is present, provide the following fields
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? battery_discharge_kw {get; set;} = null;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? battery_soc {get; set;} = null;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public double? battery_energy_kwh {get; set;} = null;
    }
}
