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
                apiKey = ChargeHQSettings.SiteId.ToString(),
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
        /// <remarks>
        /// These calculations are experimental if they actually correct measurements of net_import and consumption from the grid
        /// it's a little hard to tell from the Huawei documentation and data if this is correct, THIS NEEDS TESTING.
        /// 
        /// The Huawei:SendGridValues setting will leave them out of the payload to ChargeHQ in the case that they are not correct.
        ///
        /// The current assumptions are based on analysis of power usage from the power utility vs Huawei data at those dates and times,
        /// they seem to indicate that the AC energy drawn shown as grid current is measured after the solar energy is converted from
        /// DC to AC and goes into this amount so the net_import would be this consumption_kW minus the active_power.
        /// </remarks>
        public async Task<bool> SendData(DevRealKpiResponse data)
        {
            bool isShutdown = data.data[0].dataItemMap.inverter_state != 512;
            SiteMeterPush smp;

            // If the inverter is shutdown (i.e. there is no sun light) the active power is 0 and no other readings are available
            // this being sent to ChargeHQ should ensure that no charging would occur.
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
                var consumption_kW = consumption_watts / 1000; // convert to kW
                var net_import_kW = consumption_kW - data.data[0].dataItemMap.active_power; // remove the solar input from this amount
                var totalYield = data.data[0].dataItemMap.total_cap; // this is the total lifetime energy produced by the inverter
                smp = new SiteMeterPush
                {
                    apiKey = ChargeHQSettings.SiteId.ToString(),
                    tsms = data.parameters.currentTime,
                    sitesMeters = new SiteMeter
                    {
                        production_kw = data.data[0].dataItemMap.active_power,
                        net_import_kw = HuaweiSettings.SendGridValues ? net_import_kW : null,   // leave out value if toggled off in settings
                        consumption_kw = HuaweiSettings.SendGridValues ? consumption_kW : null, // leave out value if toggled off in settings
                        exported_kwh = totalYield
                    }
                };
            }
            
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