using System.Net;
using System.Timers;

using Timer = System.Timers.Timer;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using HuaweiSolar.Models;
using HuaweiSolar.Models.Configuration;

namespace HuaweiSolar
{
    public class HuaweiSolarPoller
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<HuaweiSolarPoller> logger;

        private HttpClient _client;
        private HttpClientHandler _handler;

        private bool isStarted = false;

        private bool initialised = false;

        private HuaweiSettings HuaweiConfig
        {
            get; set;
        }

        private string StationCode
        {
            get; set;
        }

        private DeviceInfo DeviceInformation
        {
            get; set;
        }

        private Timer Timer
        {
            get; set;
        }

        private ChargeHQSender chargeHqSender { get;set;}

        private static CancellationTokenSource CancellationTokenSource;

        public HuaweiSolarPoller(ChargeHQSender chargeHqSender, IConfiguration configuration, ILogger<HuaweiSolarPoller> logger)
        {
            this.chargeHqSender = chargeHqSender;
            this.configuration = configuration;
            this.logger = logger;
        }

        public async Task<HuaweiSolarPoller> InitialiseAsync(CancellationTokenSource cancellationTokenSource)
        {
            if (!initialised) 
            {
                initialised = true;
                CancellationTokenSource = cancellationTokenSource;
                HuaweiConfig = configuration.GetSection(Constants.HUAWEI_CONFIG_SECTION).Get<HuaweiSettings>();

                Timer = new Timer(HuaweiConfig.PollRate * 60000);
                Timer.Enabled = false;
                Timer.AutoReset = true;
                Timer.Elapsed += PollGenerationStatistics_Elapsed;

                var cookies = new CookieContainer();
                _handler = new HttpClientHandler();
                _handler.CookieContainer = cookies;
                _handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                _handler.ServerCertificateCustomValidationCallback = (httpRequestMessage, cert, certChain, policyErrors) =>
                {
                    return true;
                };
                _client = new HttpClient(_handler);
                var success = await GetXsrfToken(CancellationTokenSource.Token);
                if (success)
                {
                    logger.LogInformation("Successfully authenticated the user during intialisation.");

                    var res = await PostDataRequestAsync(GetUri(Constants.STATION_LIST_URI), null, CancellationTokenSource.Token);
                    var gotStationList = Utility.WasSuccessMessage(res, out string content, out _, CancellationTokenSource.Token);
                    if (gotStationList)
                    {
                        var stationList = JsonConvert.DeserializeObject<GetStationListResponse>(content);

                        if (stationList.data != null && stationList.data.Count > 0 && 
                                stationList.data[0].stationName == HuaweiConfig.StationName)
                        {
                            // This validation isn't really needed, verifying because we have the data
                            logger.LogInformation("Station name matches");
                            StationCode = stationList.data[0].stationCode;

                            var gdlr = new GetDeviceListRequest
                            {
                                stationCodes = StationCode
                            };
                            res = await PostDataRequestAsync(GetUri(Constants.DEV_LIST_URI), 
                                            Utility.GetStringContent(gdlr), CancellationTokenSource.Token);
                            var successDevList = Utility.WasSuccessMessage(res, out content, out _, CancellationTokenSource.Token);
                            if (successDevList)
                            {
                                var devList = JsonConvert.DeserializeObject<DeviceInfoResponse>(content);
                                if (devList.data != null && devList.data.Count > 0)
                                {
                                    logger.LogInformation(JsonConvert.SerializeObject(devList.data[0]));
                                    DeviceInformation = devList.data[0];
                                }
                            }
                        }
                    }
                }
                else 
                {
                    logger.LogError("Failed to authenticate the user during initialisation.");
                }
            }
            return this;
        }
        
        internal void Start()
        {
            if (!isStarted)
            {
                isStarted = true;
                logger.LogInformation("Starting Polling");
                Timer.Start();

                //Do first poll manually
                PollGenerationStatistics_Elapsed(this, null);
            }
        }

        internal void Stop()
        {
            if (isStarted)
            {
                Timer.Stop();
                CancellationTokenSource.Cancel();
            }
        }

        /*
         * Sends a Login Request to Huawei's Fusion Solar API, extracts the cookie and puts it in the default headers
         * for all future requests to the API.
         */
        private async Task<bool> GetXsrfToken(CancellationToken cancellationToken)
        {
            if (initialised)
            {
                LoginCredentialRequest lcr = new LoginCredentialRequest
                {
                    userName = HuaweiConfig.Username,
                    systemCode = HuaweiConfig.Password
                };
                var content = Utility.GetStringContent(lcr);
                var response = await PostDataRequestAsync(GetUri(Constants.LOGIN_URI), content, cancellationToken);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    logger.LogInformation("Successfully did login and got back cookie");
                    bool cookieFound = false;
                    foreach (Cookie cookie in _handler.CookieContainer.GetCookies(new Uri(HuaweiConfig.BaseURI)).Cast<Cookie>())
                    {
                        if (cookie.Name == "XSRF-TOKEN")
                        {
                            _client.DefaultRequestHeaders.Add("XSRF-TOKEN", cookie.Value);
                            logger.LogDebug("XSRF-TOKEN found in coolies and added to default request headers");
                            cookieFound = true;
                            break;
                        }
                    }
                    return cookieFound;
                }
                else
                {
                    logger.LogError("The login request failed, it returned a status code of {0}", response.StatusCode);
                    return false;
                }
            }
            return false;
        }

        private async Task<HttpResponseMessage> PostDataRequestAsync(string uri, StringContent content, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _client.PostAsync(uri, content, cancellationToken);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    bool success = Utility.WasSuccessMessage(response, out string json, out BaseResponse responseObj, cancellationToken);
                    if (success && responseObj.failCode == 305)
                    {
                        success = await GetXsrfToken(cancellationToken);
                        if (success)
                        {
                            logger.LogInformation("Successfully re-authenticated with the API. Resending original API request.");
                            response = await PostDataRequestAsync(uri, content, cancellationToken);
                        }
                        else 
                        {
                            logger.LogError("Failed to re-authenticate with the API.");
                        }
                    }
                    return response;
                }
                else
                {
                    logger.LogWarning("There was a failed request, it is retring after 5 seconds");
                    Thread.Sleep(5000);
                    return await PostDataRequestAsync(uri, content, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Request failed to URI: '{0}'", uri);
                return null;
            }
        }

        private async void PollGenerationStatistics_Elapsed(object sender, ElapsedEventArgs e) 
        {
            try 
            {
                if (!CancellationTokenSource.IsCancellationRequested)
                {
                    var req = new GetDevRealKpiRequest
                    {
                        devIds = DeviceInformation.id.ToString(),
                        devTypeId = DeviceInformation.devTypeId
                    };
                    var powerData = await PostDataRequestAsync(GetUri(Constants.DEV_REAL_KPI_URI), Utility.GetStringContent(req), CancellationTokenSource.Token);
                    var success = Utility.WasSuccessMessage(powerData, out string json, out BaseResponse response, CancellationTokenSource.Token);
                    if (success) 
                    { 
                        var respObj = JsonConvert.DeserializeObject<DevRealKpiResponse>(json);
                        if (respObj.data != null && respObj.data.Count > 0) 
                        {
                            // Send active power to ChargeHQ
                            logger.LogDebug("Sending power data to ChargeHQ.");
                            bool successfullySent = await this.chargeHqSender.SendData(respObj);
                            if (successfullySent) 
                            {
                                logger.LogDebug("Sent the data successfully to ChargeHQ.");
                            }
                            else
                            {
                                logger.LogError("Failed to send the data to ChargeHQ.");
                            }
                        }
                        else 
                        {
                            logger.LogWarning("The power data returned from Huawei's Fusion Solar was not valid.");
                            await this.chargeHqSender.SendErrorData("Huawei's FusionSolar power data was not in an expected format");
                        }
                    }
                    else
                    {
                        logger.LogWarning($"Huawei's FusionSolar API returned a fail code: {response.failCode}, message: {response.message}");
                        await this.chargeHqSender.SendErrorData($"Huawei's FusionSolar API returned a fail code: {response.failCode}, message: {response.message}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An exception was caught while polling for power data to send to ChargeHQ.");
                await this.chargeHqSender.SendErrorData("An error occurred while polling the Huawei FusionSolar API");
            }
        }

        private string GetUri(string methodUri)
        {
            if (methodUri.StartsWith("/"))
            {
                methodUri = methodUri.TrimStart('/');
            }
            if (HuaweiConfig.BaseURI.EndsWith("/"))
            {
                HuaweiConfig.BaseURI = HuaweiConfig.BaseURI.TrimEnd('/');
            }
            return string.Format("{0}/{1}", HuaweiConfig.BaseURI, methodUri);
        }
    }
}