using Newtonsoft.Json;

namespace HuaweiSolar.Models
{
    public class DeviceInfoResponse : BaseResponse
    {
        public IList<DeviceInfo> data { get; set; }

        [JsonProperty("params")]
        public DeviceInfoParams parameters { get; set;}
    }

    public class DeviceInfo
    {
        public string devName { get; set; }
        public int devTypeId {get;set;}
        public string esnCode {get;set;}
        public long id {get;set;}
        public string invType {get;set;}
        public double latitude {get;set;}
        public double longitude{get;set;}
        public int optimizerNumber {get;set;}
        public string softwareVersion {get;set;}
        public string stationCode {get;set;}
    }

    public class DeviceInfoParams : BaseParams
    {
        public string stationCodes { get; set; }
    }
}