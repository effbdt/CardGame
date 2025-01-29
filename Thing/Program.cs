using Application;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Model;

namespace Thing
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
                 .ConfigureServices((hostContext, services) =>
                 {
                     services.AddSingleton<CardGameDbContext>();
                     services.AddSingleton<IDataProvider, DataProvider>();

                     services.AddSingleton<ICardDataProvider, CardDataProvider>();

                     services.AddSingleton<ICardService, CardService>();
                     services.AddSingleton<IDataImportExportService, DataImportExportService>();
                     services.AddSingleton<ICardGameService, CardGameService>();
                 })
                 .Build();

            host.Start();

            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider serviceProvider = serviceScope.ServiceProvider;

            const string jsonPath = "cards.json";
            var cardGameService = serviceProvider.GetService<ICardGameService>();
            Console.Clear();
            cardGameService.DataImportExportService.JsonReader(jsonPath);

        }
    }
}
