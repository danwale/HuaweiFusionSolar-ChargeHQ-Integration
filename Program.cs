using System.Runtime.Loader;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace HuaweiSolar
{
    public class Program
    {
        private static ServiceProvider ServiceProvider { get; set; }
        private const string XSRF_TOKEN_KEY = "XSRF-TOKEN";
        public static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Share the cancellation token source
            var cts = new CancellationTokenSource();

            // Initialse the poller passing it the Cancellation Token Source
            var huaweiSolarPoller = ServiceProvider.GetRequiredService<HuaweiSolarPoller>()
                .InitialiseAsync(cts).GetAwaiter().GetResult();

            // Start the poller
            huaweiSolarPoller.Start();

            // Wait until the app unloads or is cancelled
            AssemblyLoadContext.Default.Unloading += (ctx) => cts.Cancel();
            Console.CancelKeyPress += (sender, cpe) => cts.Cancel();
            WhenCancelled(cts.Token).Wait();

            // Stop the poller as the service is stopping
            huaweiSolarPoller.Stop();

            Log.Logger.Information("Huawei Solar Poller has exited.");
        }

        /// <summary>
        /// Handles cleanup operations when app is cancelled or unloads
        /// </summary>
        public static Task WhenCancelled(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>();
            cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).SetResult(true), tcs);
            return tcs.Task;
        }

        private static void ConfigureServices(ServiceCollection serviceCollection)
        {
            string configPath = Environment.GetEnvironmentVariable("ConfigPath");
            if (string.IsNullOrWhiteSpace(configPath))
            {
                configPath = "/etc/huaweisolar";
            }
            Console.WriteLine("Reading appsettings.json from: {0}", configPath);

            Console.WriteLine("ConfigureServices called");
            IConfiguration configuration = new ConfigurationBuilder()
                                                    .SetBasePath(configPath)
                                                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
                                                    .AddEnvironmentVariables()
                                                    .Build();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddSingleton<ChargeHQSender>();
            serviceCollection.AddSingleton<HuaweiSolarPoller>();

            // Configure Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            serviceCollection.AddLogging((builder) => {
                builder.AddSerilog();
            });
            Log.Logger.Debug("Configured Logger");
        }
    }
}