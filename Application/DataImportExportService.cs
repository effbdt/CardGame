using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Application
{
    public class DataImportExportService : IDataImportExportService
    {
        private readonly ICardDataProvider _cardDataProvider;

        public DataImportExportService(ICardDataProvider cardDataProvider)
        {
            _cardDataProvider = cardDataProvider;
        }

        private const string MainFolderPath = "CardGame";

        public void JsonReader(string path)
        {
            if (!Directory.Exists(MainFolderPath))
            {
                Directory.CreateDirectory(MainFolderPath);
            }

            string jsonData = File.ReadAllText(path);

            var cardsJson = JsonConvert.DeserializeObject<List<Card>>(jsonData);
            foreach (var card in cardsJson)
            {
                if (HighQualityCardValidator.ValidateCard(card))
                { _cardDataProvider.Add(card); }
            }
        }

        public void ExportGameProcess(string playedCards)
        {
            using (var writer = new StreamWriter(Path.Combine(MainFolderPath, "Game")))
            {
                Console.WriteLine(playedCards);
            }
        }
    }
}
