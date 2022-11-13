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
                logger.LogInformation("Successfully sent error data to ChargeHQ Solar Push API.");
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
            bool isShutdown = data.data[0].dataItemMap.inverter_state != 512;
            SiteMeterPush smp;
            if (isShutdown)
            {
                smp = new SiteMeterPush
                {
                    apiKey = ChargeHQSettings.SiteId.ToString(),
                    tsms = data.parameters.currentTime,
                    sitesMeters = new SiteMeter
                    {
                        production_kw = data.data[0].dataItemMap.active_power
                    }
                };
            }
            else 
            {
                // consumption in watts I believe for single phase is Grid_Voltage * Phase_A_Current * Power_Factor
                var consumption_watts = data.data[0].dataItemMap.ab_u * data.data[0].dataItemMap.a_i * data.data[0].dataItemMap.power_factor;
                var consumption_kW = consumption_watts / 1000;
                var net_import_kW = consumption_kW - data.data[0].dataItemMap.active_power;
                var totalYield = data.data[0].dataItemMap.total_cap;
                smp = new SiteMeterPush
                {
                    apiKey = ChargeHQSettings.SiteId.ToString(),
                    tsms = data.parameters.currentTime,
                    sitesMeters = new SiteMeter
                    {
                        production_kw = data.data[0].dataItemMap.active_power,
                        net_import_kw = net_import_kW,
                        consumption_kw = consumption_kW,
                        exported_kwh = totalYield
                    }
                };
            }
            
            logger.LogDebug("ChargeHQ Site Meter Push: {0}", JsonConvert.SerializeObject(smp, Formatting.None));
            
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
            }
            return false;
        }
    }
}