namespace HuaweiSolar.Models
{
    public class StationInfo
    {
        public int aidType { get; set; }
        public string buildState { get; set; }
        public double capacity {get;set;}
        public string combineType {get;set;}
        public string linkmanPho { get;set;}
        public string stationAddr{get;set;}
        public string stationCode {get;set;}
        public string stationLinkman {get;set;}
        public string stationName {get;set;}
    }

    public class StationInfoParams
    {
        public long currentTime {get;set;}
    }
}