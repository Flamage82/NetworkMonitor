using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NetworkMonitor.Sampling;

namespace NetworkMonitor.WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> logger;

        private readonly IOptions<Settings> settings;

        private readonly Sampler sampler;

        public Worker(ILogger<Worker> logger, IOptions<Settings> settings, Sampler sampler)
        {
            this.logger = logger;
            this.settings = settings;
            this.sampler = sampler;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await sampler.Execute();
                try
                {
                    await Task.Delay(settings.Value.ScanInterval, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                }
            }
        }
    }
}
