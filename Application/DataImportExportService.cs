using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Reflection;

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
			string fullPathToCards = Path.Combine(MainFolderPath, "Cards");

			if (!Directory.Exists(fullPathToCards))
			{
				Directory.CreateDirectory(fullPathToCards);
			}

			string jsonData = File.ReadAllText(path);

			var cardsJson = JsonConvert.DeserializeObject<List<Card>>(jsonData);
			foreach (var card in cardsJson)
			{
				if (HighQualityCardValidator.ValidateCard(card))
				{
					_cardDataProvider.Add(card);
					SubFolderCheck(card);
				}
			}
		}

		public void ExportGameProcess(string playedCards)
		{
			using (var writer = new StreamWriter(Path.Combine(MainFolderPath, "Game")))
			{
				writer.WriteLine(playedCards);
			}
		}

		public static void SubFolderCheck(Card card)
		{
			string cardFilePath = Path.Combine(MainFolderPath, "Cards", card.CardName + ".txt");
			if (!File.Exists(cardFilePath))
			{
				StringBuilder sb = new StringBuilder();

				Type type = typeof(Card);
				foreach (PropertyInfo property in type.GetProperties())
				{
					sb.AppendLine(property.Name + ": " + property.GetValue(card));
				}
				File.WriteAllText(cardFilePath, sb.ToString());
			}
		}
	}
}
