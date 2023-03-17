using System.Net.Mime;
using System.Text;

using Newtonsoft.Json;

using Serilog;

using HuaweiSolar.Models.Huawei;

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
            Log.Logger.Debug("JSON Request Content: '{0}'", json);
            return GetStringContent(json);
        }

        public static string GetJsonResponse(HttpResponseMessage message, CancellationToken cancellationToken)
        {
            return message.Content.ReadAsStringAsync(cancellationToken).GetAwaiter().GetResult();
        }

        public static bool WasSuccessMessage<T>(HttpResponseMessage message, out string json, out T response, CancellationToken cancellationToken) where T : BaseResponse
        {
            json = string.Empty;
            response = null;
            if (message != null)
            {
                try
                {
                    json = GetJsonResponse(message, cancellationToken);
                    Log.Logger.Debug("JSON Response Content: '{0}'", json);
                    var wasSuccessResponse = JsonConvert.DeserializeObject<BaseResponse>(json);
                    if (wasSuccessResponse != null && !wasSuccessResponse.success) 
                    {
                        response = Activator.CreateInstance<T>();
                        response.failCode = wasSuccessResponse.failCode;
                        response.message = wasSuccessResponse.message;
                        response.buildCode = wasSuccessResponse.buildCode;
                        return false;
                    }
                    response = JsonConvert.DeserializeObject<T>(json);
                    if (response != null)
                    {
                        return response.success;
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.Warning(ex, "Failed to get a valid response from the service. JSON: '{0}'", json);
                    return false;
                }
            }
            return false;
        }
    }
}
