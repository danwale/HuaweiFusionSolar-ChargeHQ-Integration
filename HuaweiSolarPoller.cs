using System.Net;
using System.Timers;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using HuaweiSolar.Models;
using HuaweiSolar.Models.Configuration;

using Timer = System.Timers.Timer;

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

        private static int RetryCount
        {
            get; set;
        } = 0;

        private ChargeHQSender chargeHqSender { get;set;}

        private static CancellationTokenSource CancellationTokenSource;

        public HuaweiSolarPoller(ChargeHQSender chargeHqSender, IConfiguration configuration, ILogger<HuaweiSolarPoller> logger)
        {
            this.chargeHqSender = chargeHqSender;
            this.configuration = configuration;
            this.logger = logger;
        }

        /// <summary>
        /// <c>InitialiseAsync</c> - Sets up the component for polling the Huawei Fusion Solar API.
        /// It will login to the API and read some data objects out that will be used for polling solar production data
        /// </summary>
        /// <returns>The HuaweiSolarPoller object that was initialised</summary>
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

                    var stationListResponse = await PostDataRequestAsync<GetStationListResponse>(GetUri(Constants.STATION_LIST_URI), null, CancellationTokenSource.Token);
                    if (stationListResponse.success)
                    {
                        if (stationListResponse.data != null && stationListResponse.data.Count > 0 && 
                                stationListResponse.data[0].stationName == HuaweiConfig.StationName)
                        {
                            // This validation isn't really needed, verifying because we have the data
                            logger.LogInformation("Station name matches");
                            StationCode = stationListResponse.data[0].stationCode;

                            var gdlr = new GetDeviceListRequest
                            {
                                stationCodes = StationCode
                            };
                            var deviceInfoResponse = await PostDataRequestAsync<DeviceInfoResponse>(GetUri(Constants.DEV_LIST_URI), 
                                            Utility.GetStringContent(gdlr), CancellationTokenSource.Token);
                            if (deviceInfoResponse.success)
                            {
                                if (deviceInfoResponse.data != null && deviceInfoResponse.data.Count > 0)
                                {
                                    logger.LogInformation(JsonConvert.SerializeObject(deviceInfoResponse.data[0]));
                                    DeviceInformation = deviceInfoResponse.data[0];
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
        
        /// <summary>
        /// <c>Start</c> - Starts the poller and does an initial poll immediately.
        /// </summary>
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

        /// <summary>
        /// <c>Stop</c> - Stops the poller
        /// </summary>
        internal void Stop()
        {
            if (isStarted)
            {
                Timer.Stop();
                CancellationTokenSource.Cancel();
                isStarted = false;
            }
        }

        /// <summary>
        /// <c>GetXsrfToken</c> - Sends a Login Request to Huawei's Fusion Solar API, extracts the cookie and puts it in the default headers
        //  for all future requests to the API.
        /// </summary>
        /// <returns>True if it successfully logged in, otherwise False</returns>
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
                var response = await PostDataRequestAsync<BaseResponse>(GetUri(Constants.LOGIN_URI), content, cancellationToken);
                if (response.success)
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
                    logger.LogError("The login request failed, it returned a fail code of {0} and message {1}", response.failCode, response.message);
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// <c>PostDataRequestAsync</c> - Does a HTTP POST to the Huawei Fusion Solar API with automatic re-authentication if required.
        /// If it gets an error response from the API call it will retry every 5 seconds until it gets a valid response 
        /// (such as Fusion Solar being offline for maintenance).
        /// </summary>
        /// <returns>The HTTP response message or null if there was an error with the HTTP POST</returns>
        private async Task<T> PostDataRequestAsync<T>(string uri, StringContent content, CancellationToken cancellationToken) where T: BaseResponse
        {
            try
            {
                var response = await _client.PostAsync(uri, content, cancellationToken);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    bool success = Utility.WasSuccessMessage<T>(response, out string json, out T responseObj, cancellationToken);
                    if (success && responseObj.failCode == 305)
                    {
                        success = await GetXsrfToken(cancellationToken);
                        if (success)
                        {
                            logger.LogInformation("Successfully re-authenticated with the API. Resending original API request.");
                            responseObj = await PostDataRequestAsync<T>(uri, content, cancellationToken);
                        }
                        else 
                        {
                            logger.LogError("Failed to re-authenticate with the API.");
                        }
                    }
                    return responseObj;
                }
                else
                {
                    RetryCount++;
                    if (RetryCount < 5) 
                    {
                        logger.LogWarning("There was a failed request, it is retring after 5 seconds...Retry Count: {0}", RetryCount);
                        Thread.Sleep(5000);
                        return await PostDataRequestAsync<T>(uri, content, cancellationToken);
                    }
                    else
                    {
                        RetryCount = 0;
                        return (T)new BaseResponse
                                {
                                    success = false,
                                    failCode = 1001,
                                    message = string.Format("Failed after the maximum retry count for URI: '{0}'", uri)
                                };
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Request failed to URI: '{0}'", uri);
                return (T)new BaseResponse
                {
                    success = false,
                    failCode = 1001,
                    message = string.Format("Request failed to URI: '{0}'", uri)
                };
            }
        }

        /// <summary>
        /// <c>PollGenerationStatistics_Elapsed</c> - This is the elapsed timer handler, it will poll for solar production data
        /// and if it successfully gets it send it onto the <c>ChargeHQSender</c> for sending to ChargeHQ's Push API.
        /// If it fails to get the data it will send an error string instead.
        /// </summary>
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
                    var powerData = await PostDataRequestAsync<DevRealKpiResponse>(GetUri(Constants.DEV_REAL_KPI_URI), Utility.GetStringContent(req), CancellationTokenSource.Token);
                    if (powerData != null && powerData.success) 
                    { 
                        if (powerData.data != null && powerData.data.Count > 0) 
                        {
                            // Send active power to ChargeHQ
                            logger.LogDebug("Sending power data to ChargeHQ.");
                            bool successfullySent = await this.chargeHqSender.SendData(powerData);
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
                        if (powerData == null)
                        {
                            powerData = new DevRealKpiResponse
                                {
                                    success = false,
                                    failCode = 1002,
                                    message = "The returned power data from Huawei was null"
                                };
                        }
                        logger.LogWarning($"Huawei's FusionSolar API returned a fail code: {powerData.failCode}, message: {powerData.message}");
                        await this.chargeHqSender.SendErrorData($"Huawei's FusionSolar API returned a fail code: {powerData.failCode}, message: {powerData.message}");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An exception was caught while polling for power data to send to ChargeHQ.");
                await this.chargeHqSender.SendErrorData("An error occurred while polling the Huawei FusionSolar API");
            }
        }

        /// <summary>
        /// <c>GetUri</c> - Combines the method URI and base URI for the Huawei services ensuring it handles configuration errors.
        /// </summary>
        /// <param name="methodUri">The relative URI path to the relevant service</param>
        /// <returns>The full URI for the Huawei Fusion Solar API target</returns>
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