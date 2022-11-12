using System.Net;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using HuaweiSolar.Models;
using HuaweiSolar.Models.Configuration;

namespace HuaweiSolar
{
    public class ChargeHQSender
    {
        private readonly ILogger<ChargeHQSender> logger;

        private HttpClient _client;

        private ChargeHQSettings ChargeHQSettings { get; set; }

        public ChargeHQSender(ILogger<ChargeHQSender> logger, IConfiguration configuration)
        {
            this.logger = logger;

            ChargeHQSettings = configuration.GetSection(Constants.CHARGE_HQ_CONFIG_SECION).Get<ChargeHQSettings>();

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, certChain, policyErrors) =>
            {
                return true;
            };
            _client = new HttpClient(handler);
        }

        public async Task<bool> SendErrorData(string errorMessage)
        {
            var smp = new SiteMeterPush
            {
                apiKey = ChargeHQSettings.SiteId.ToString(),
                error = errorMessage
            };
            var response = await _client.PostAsync(ChargeHQSettings.PushURI, Utility.GetStringContent(smp));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                logger.LogDebug("Successfully sent error data to ChargeHQ Solar Push API.");
                return true;
            }
            else
            {
                logger.LogWarning("Failed to send error data to ChargeHQ Solar Push API. Response: {0}", response.StatusCode);
                var json = Utility.GetJsonResponse(response, CancellationToken.None);
                logger.LogDebug("Response from ChargeHQ: {0}", json);
            }
            return false;
        }

        public async Task<bool> SendData(DevRealKpiResponse data)
        {
            var smp = new SiteMeterPush
            {
                apiKey = ChargeHQSettings.SiteId.ToString(),
                tsms = data.parameters.currentTime,
                sitesMeters = new SiteMeter
                {
                    production_kw = data.data[0].dataItemMap.active_power
                }
            };
            
            var response = await _client.PostAsync(ChargeHQSettings.PushURI, Utility.GetStringContent(smp));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                logger.LogDebug("Successfully sent data to ChargeHQ Solar Push API.");
                var json = Utility.GetJsonResponse(response, CancellationToken.None);
                logger.LogDebug("Response from ChargeHQ: {0}", json);
                return true;
            }
            else
            {
                logger.LogWarning("Failed to send data to ChargeHQ Solar Push API. Response: {0}", response.StatusCode);
                var json = Utility.GetJsonResponse(response, CancellationToken.None);
                logger.LogDebug("Response from ChargeHQ: {0}", json);
            }
            return false;
        }
    }
}