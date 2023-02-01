using System.Net;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using HuaweiSolar.Models;
using HuaweiSolar.Models.Configuration;

namespace HuaweiSolar
{
    public class ChargeHQSender
    {
        private readonly ILogger<ChargeHQSender> logger;

        private HttpClient _client;

        private ChargeHQSettings ChargeHQSettings { get; set; }

        private HuaweiSettings HuaweiSettings { get; set; }

        public ChargeHQSender(ILogger<ChargeHQSender> logger, IConfiguration configuration)
        {
            this.logger = logger;

            ChargeHQSettings = configuration.GetSection(Constants.CHARGE_HQ_CONFIG_SECION).Get<ChargeHQSettings>();
            HuaweiSettings = configuration.GetSection(Constants.HUAWEI_CONFIG_SECTION).Get<HuaweiSettings>();

            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, certChain, policyErrors) =>
            {
                return true;
            };
            _client = new HttpClient(handler);
        }

        /// <summary>
        /// <c>SendErrorData</c> - Sends just an error message to the ChargeHQ Push API if there was a problem gathering the data
        /// </summary>
        /// <param name="errorMessage">The error message string to send</param>
        /// <returns>True if the error data was successfully pushed the ChargeHQ Push API otherwise False.</returns>
        public async Task<bool> SendErrorData(string errorMessage)
        {
            var smp = new SiteMeterPush
            {
                apiKey = ChargeHQSettings.ApiKey.ToString(),
                error = errorMessage
            };

            // Send the SiteMeterPush data model to ChargeHQ Push API
            var response = await _client.PostAsync(ChargeHQSettings.PushURI, Utility.GetStringContent(smp));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                logger.LogInformation("Successfully sent error data to ChargeHQ Solar Push API.");
                return true;
            }
            else
            {
                logger.LogWarning("Failed to send error data to ChargeHQ Solar Push API. Response: {0}", response.StatusCode);
                var json = Utility.GetJsonResponse(response, CancellationToken.None);
                logger.LogDebug("Response from ChargeHQ: {0}", json);
                return false;
            }
        }

        /// <summary>
        /// <c>SendData</c> - Creates a ChargeHQ SiteMeterPush data model and HTTP POSTs it to the ChargeHQ Push API.
        /// </summary>
        /// <param name="data"><c>DevRealKpiResponse</c> is response object from the Huawei Fusion Solar API</param>
        /// <returns>
        /// True if the data was successfully pushed the ChargeHQ Push API otherwise False.
        /// </returns>
        public async Task<bool> SendData(DevRealKpiResponse data)
        {
            bool isShutdown = data.data[0].dataItemMap.inverter_state != 512;
            if (isShutdown) 
            {
                logger.LogDebug("The inverter is currently shutdown due to no sunlight");
            }

            var totalYield = data.data[0].dataItemMap.total_cap; // this is the total lifetime energy produced by the inverter
            var smp = new SiteMeterPush
            {
                apiKey = ChargeHQSettings.ApiKey.ToString(),
                tsms = data.parameters.currentTime,
                siteMeters = new SiteMeter
                {
                    production_kw = data.data[0].dataItemMap.active_power,
                    exported_kwh = totalYield
                }
            };

            logger.LogDebug("ChargeHQ Site Meter Push: {0}", JsonConvert.SerializeObject(smp, Formatting.None));

            // Send the SiteMeterPush data model to ChargeHQ Push API
            var response = await _client.PostAsync(ChargeHQSettings.PushURI, Utility.GetStringContent(smp));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                logger.LogInformation("Successfully sent data to ChargeHQ Solar Push API.");
                var json = Utility.GetJsonResponse(response, CancellationToken.None);
                logger.LogDebug("Response from ChargeHQ: {0}", json);
                return true;
            }
            else
            {
                logger.LogWarning("Failed to send data to ChargeHQ Solar Push API. Response: {0}", response.StatusCode);
                var json = Utility.GetJsonResponse(response, CancellationToken.None);
                logger.LogDebug("Response from ChargeHQ: {0}", json);
                return false;
            }
        }
    }
}
