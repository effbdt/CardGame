using Model;
using Application;
using ConsoleTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Game
{
	public class GameUi
	{
		public static ConsoleMenu MainMenu(ICardGameService cardGameService)
		{

			var menu = new ConsoleMenu()
				.Add("Play!", () => GameUi.StartGame(cardGameService))
				.Add("Settings", () => GameUi.OpenSettings(cardGameService))
				.Add("Exit", () => Environment.Exit(0))
				.Configure((config) =>
				{
					config.Title = "Main menu!\n";
					config.EnableWriteTitle = true;
				});


			return menu;
		}

		private static void OpenSettings(ICardGameService cardGameService)
		{
			var menu = new ConsoleMenu()
				.Add("Add a custom card!", () => CustomCardAdd(cardGameService))
				.Add("Enter a username!", () => PlayerUsername(cardGameService))
				.Add("Back", ConsoleMenu.Close);
			menu.Show();
		}

		private static void PlayerUsername(ICardGameService cardGameService)
		{
			if (cardGameService.CardService.NameEntered)
			{
				Console.WriteLine($"Username has already been entered: {cardGameService.CardService.Name}");
				Console.WriteLine("Press enter to go back!");
				Console.ReadLine();

				return;
			}

			Console.Write("Enter username: ");
			string playerName = Console.ReadLine();
			cardGameService.CardService.playerUsername(playerName);
			Console.Clear();
			Console.WriteLine($"Username set: {playerName}");
			Console.WriteLine("Press enter to go back to settings!");
			Console.ReadLine();
		}

		private static void CustomCardAdd(ICardGameService cardGameService)
		{
			Console.Write("Enter the cards name: ");
			string name = Console.ReadLine();
			Console.Write("\nEnter the cards power(if its going to be high quality it has to be at least 7): ");
			int power;
			while (!int.TryParse(System.Console.ReadLine(), out power))
			{
				Console.Write("Please enter a valid integer: ");
			}
			Console.Write("\nIs the card high quality(true/false): ");
			bool quality;
			while (!bool.TryParse(System.Console.ReadLine(), out quality))
			{
				Console.Write("Please enter \"true\" or \"false\": ");
			}
			Console.Write("\nEnter the cards description(max 150 characters): ");
			string desc = Console.ReadLine();
			Card card = new Card(name, power, quality, desc);
			cardGameService.CardService.Add(card);
			Console.ReadLine();
		}

		private static void StartGame(ICardGameService cardGameService)
		{
			LinkedStack<Card> playerDeck = cardGameService.CardService.GetDeck();
			List<Card> playerHand = cardGameService.CardService.GetHand(ref playerDeck);
			LinkedStack<Card> opponentDeck = cardGameService.CardService.GetDeck();
			var opponentHand = cardGameService.CardService.GetHand(ref opponentDeck);
			int playerPoints = 0;
			int opponentPoints = 0;
			int turn = 1;
			int choice;
			string playerName = cardGameService.CardService.Name;
			bool canDraw = false;
			do
			{
				for (int i = 0; i < playerHand.Count; i++)
				{
					Console.WriteLine($"{i + 1}. Play: {playerHand[i].CardName,-20}		Power: {playerHand[i].CardPower}");
				}
				if (playerHand.Count() < 5 && !playerDeck.IsEmpty())
				{
					canDraw = true;
					Console.WriteLine($"\n{playerHand.Count() + 1}. Draw card(s)");
				}
				while (true)
				{
					Console.WriteLine($"\nCurrent points for {playerName}: {playerPoints}");
					int maxOption = playerHand.Count();
					if (canDraw)
						maxOption++;
					Console.Write("\nEnter the index of the action you want to do: ");
					if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= maxOption)
					{
						break;
					}
					Console.WriteLine("Invalid input. Please enter a valid index.");
				}
				if (choice <= playerHand.Count())
				{
					Card playedCard = playerHand[choice - 1];
					cardGameService.CardService.PlayCard(ref playerHand, playedCard, ref playerPoints);
					Console.Clear();
					Console.WriteLine($"{playerName} played: {playedCard.CardName}");
					Console.WriteLine($"{playerName}'s points: {playerPoints}");
					turn++;
					Console.WriteLine("\nEnter to proceed!");
					Console.ReadLine();
				}
				else if (canDraw)
				{
					int amountOfCardsDrawn = Math.Min(5 - playerHand.Count(), playerDeck.Length);
					cardGameService.CardService.DrawCards(ref playerHand, ref playerDeck);
					Console.WriteLine($"\n{amountOfCardsDrawn} card(s) were drawn from your deck!\nCards remaining in your deck: {playerDeck.n}");
					Console.WriteLine("Press enter to continue!");
					Console.ReadLine();
					Console.Clear();
					canDraw = false;
				}


			} while (playerHand.Count() > 0 || !playerDeck.IsEmpty());

		}
	}
}
