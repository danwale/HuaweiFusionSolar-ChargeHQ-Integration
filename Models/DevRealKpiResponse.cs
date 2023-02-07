using Newtonsoft.Json;

namespace HuaweiSolar.Models
{
    public class DevRealKpiResponse<T> : BaseResponse where T: BaseDevRealKpiDataItemMap
    {
        public IList<DevRealKpiData<T>> data { get; set; }

        [JsonProperty("params")]
        public DevRealKpiParams parameters {get;set;}
    }
}