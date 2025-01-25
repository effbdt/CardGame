using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Thing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
                 .ConfigureServices((hostContext, services) =>
                 {

                 })
                 .Build();

            host.Start();

            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;
        }
    }
}
