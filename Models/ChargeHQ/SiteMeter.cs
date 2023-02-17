using Newtonsoft.Json;

namespace HuaweiSolar.Models.ChargeHQ
{
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