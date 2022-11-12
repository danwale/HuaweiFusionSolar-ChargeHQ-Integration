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
    }
}