using Newtonsoft.Json;

namespace HuaweiSolar.Models
{
    public class DevRealKpiResponse : BaseResponse
    {
        public IList<DevRealKpiData> data { get; set; }

        [JsonProperty("params")]
        public DevRealKpiParams parameters {get;set;}
    }
}