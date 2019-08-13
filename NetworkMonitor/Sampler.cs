using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Serilog;

namespace NetworkMonitor
{
    public class Sampler
    {
        private readonly Ping ping = new Ping();

        public async Task Execute()
        {
            Log.Information("Ping");
            var reply = await ping.SendPingAsync("1.1.1.1");
            Log.ForContext("Reply", reply, true).Debug("Ping: {Milliseconds}", reply.RoundtripTime);
        }
    }
}