using Newtonsoft.Json;

namespace HuaweiSolar.Models
{
    public class StationRealKpiResponse : BaseResponse
    {
        public StationRealKpiDataObject[] data
        {
            get; set;
        }


        [JsonProperty("params")]
        public StationRealKpiParams parameters
        {
            get; set;
        }
    }
}