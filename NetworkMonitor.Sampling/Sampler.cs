using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NetworkMonitor.Sampling
{
    public class Sampler
    {
        private readonly ILogger<Sampler> logger;

        private readonly Ping ping = new Ping();

        private readonly byte[] sendBuffer;

        public Sampler(ILogger<Sampler> logger)
        {
            this.logger = logger;
            sendBuffer = new byte[32];
            for (var index = 0; index < 32; ++index)
            {
                sendBuffer[index] = (byte) (97 + index % 23);
            }
        }

        public async Task Execute()
        {
            logger.LogDebug("Ping");
            var ipAddress = IPAddress.Parse("10.171.150.100");
            var options = new PingOptions
            {
                Ttl = 2
            };
            var reply = await ping.SendPingAsync(ipAddress, 3000, sendBuffer, options);
            logger.LogInformation("Ping: {Milliseconds}", reply.RoundtripTime);
        }
    }
}
