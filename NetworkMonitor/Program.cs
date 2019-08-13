using System;
using Destructurama;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;
using Topshelf;

namespace NetworkMonitor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Destructure.JsonNetTypes()
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "Network Monitor")
                .Enrich.WithExceptionDetails()
                .CreateLogger();

            Log.Information("Service starting");

            var serviceProvider = BuildServiceProvider(configuration);

            var topShelfExitCode = HostFactory.Run(hostConfigurator =>
            {
                hostConfigurator.UseSerilog(Log.Logger);
                hostConfigurator.Service(() => serviceProvider.GetService<Scheduler>());

                hostConfigurator.RunAsLocalSystem();

                hostConfigurator.SetDescription("Periodically samples and records internet connection quality");
                hostConfigurator.SetDisplayName("Network Monitor");
                hostConfigurator.SetServiceName("NetworkMonitor");
            });

            var exitCode = (int)Convert.ChangeType(topShelfExitCode, topShelfExitCode.GetTypeCode());
            Log.Information("Service exiting with code {ExitCode}", exitCode);
            Environment.ExitCode = exitCode;

            Log.CloseAndFlush();
        }

        public static IConfiguration GetConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static IServiceProvider BuildServiceProvider(IConfiguration configuration)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddOptions();
            serviceCollection.Configure<Settings>(configuration);

            serviceCollection.AddDbContext<StorageContext>(options => options.UseSqlServer(configuration["DatabaseConnectionString"]));

            serviceCollection.AddTransient<Scheduler>();
            serviceCollection.AddTransient<Sampler>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
