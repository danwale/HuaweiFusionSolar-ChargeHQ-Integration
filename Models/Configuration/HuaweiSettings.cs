using Newtonsoft.Json;

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

        public int PollRate
        {
            get; set;
        }

        public bool SendGridValues
        {
            get; set;
        } = true;
    }
}