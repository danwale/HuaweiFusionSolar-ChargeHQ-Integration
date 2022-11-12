using Newtonsoft.Json;

namespace HuaweiSolar.Models.Configuration
{
    public class ChargeHQSettings
    {
        public string PushURI
        {
            get; set;
        }

        public Guid SiteId
        {
            get; set;
        }
    }
}