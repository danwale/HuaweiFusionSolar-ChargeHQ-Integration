using Newtonsoft.Json;

namespace HuaweiSolar.Models
{
    public class DeviceInfoResponse : BaseResponse
    {
        public IList<DeviceInfo> data { get; set; }

        [JsonProperty("params")]
        public DeviceInfoParams parameters { get; set;}
    }
}