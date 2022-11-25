using System.Net.Mime;
using System.Text;

using Newtonsoft.Json;

using Serilog;

using HuaweiSolar.Models;

namespace HuaweiSolar
{
    public static class Utility
    {
        public static StringContent GetStringContent(string json)
        {
            return new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        public static StringContent GetStringContent(object obj, Formatting formatOption = Formatting.None)
        {
            string json = JsonConvert.SerializeObject(obj, formatOption);
            Log.Logger.Debug("JSON Content: '{0}", json);
            return GetStringContent(json);
        }

        public static string GetJsonResponse(HttpResponseMessage message, CancellationToken cancellationToken)
        {
            return message.Content.ReadAsStringAsync(cancellationToken).GetAwaiter().GetResult();
        }

        public static bool WasSuccessMessage<T>(HttpResponseMessage message, out string json, out T response, CancellationToken cancellationToken) where T : BaseResponse
        {
            json = null;
            response = null;
            if (message != null)
            {
                json = GetJsonResponse(message, cancellationToken);
                response = JsonConvert.DeserializeObject<T>(json);
                if (response != null) 
                {
                    return response.success;
                }
            }
            return false;
        }
    }
}