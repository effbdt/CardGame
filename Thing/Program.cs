using Application;
using Game;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Model;

namespace Thing
{
    internal class Program
    {
        private static void EventSubsriber(ICardGameService cardGameService)
        {
            cardGameService.CardService.FailedValidationEvent += () => Console.WriteLine("The card didn't pass the validation." +
                "\nCheck if the power level matches the quality of the card.");
            cardGameService.CardService.SuccessfullyAddedCardEvent += () => Console.WriteLine("Card successfully added! :D");
        }
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
            EventSubsriber(cardGameService);
            var menu = GameUi.MainMenu(cardGameService);
            menu.Show();

        }
    }
}
