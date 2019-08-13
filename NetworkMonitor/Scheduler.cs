using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Topshelf;

namespace NetworkMonitor
{
    public class Scheduler : ServiceControl
    {
        private readonly Sampler sampler;

        private readonly Settings settings;

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        private Task task;

        public Scheduler(Sampler sampler, IOptions<Settings> settings)
        {
            this.sampler = sampler;
            this.settings = settings.Value;
        }

        public bool Start(HostControl hostControl)
        {
            task = ExecuteAsync();
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            cancellationTokenSource.Cancel();
            task.Wait();
            return true;
        }

        private async Task ExecuteAsync()
        {
            while (!cancellationTokenSource.Token.IsCancellationRequested)
            {
                await sampler.Execute();
                try
                {
                    await Task.Delay(settings.ScanInterval, cancellationTokenSource.Token);
                }
                catch (TaskCanceledException)
                {
                }
            }
        }
    }
}