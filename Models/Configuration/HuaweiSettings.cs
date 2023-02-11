namespace HuaweiSolar.Models.Configuration
{
    public class HuaweiSettings
    {
        public string BaseURI
        {
            get; set;
        }

        public string Username 
        {
            get; set;
        }

        // this is the systemCode that Huawei provide when requested
        public string Password 
        {
            get; set;
        }

        public string StationName 
        {
            get; set;
        }

        // This is the poll rate in minutes, Huawei ask for this to be 5 minutes
        public int PollRate
        {
            get; set;
        } = 5;
    }
}