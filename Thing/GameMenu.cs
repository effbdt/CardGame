using Model;
using Application;
using ConsoleTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

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

		static void DisplayHand(List<Card> hand)
		{
			for (int i = 0; i < hand.Count; i++)
			{
				Console.WriteLine($"{i + 1}. Play: {hand[i].CardName,-20}		Power: {hand[i].CardPower}");
			}
		}

		static (string Winner, int WinnerPoints) GetWinner(int points1, int points2, ICardGameService cardGameService)
		{
			string winner = points1 >= points2 ? cardGameService.CardService.Name : "opponent";
			int points = points1 >= points2 ? points1 : points2;
			return (winner, points);
		}

		static void OpponentsTurn(LinkedStack<Card> opponentDeck, List<Card> opponentHand, ref int opponentPoints, ICardGameService cardGameService)
		{
			int handCount = opponentHand.Count;
			if (opponentHand.Count > 0)
			{
				Card oppPlayedCard = opponentHand[0];
				opponentHand.RemoveAt(0);
				opponentPoints += oppPlayedCard.CardPower;
				Console.WriteLine($"Opponent played: {oppPlayedCard.CardName}");
				Console.WriteLine($"Opponent's points: {opponentPoints}");
				Console.WriteLine("Enter to proceed!");
				Console.ReadLine();
				Console.Clear();
			}
			else if (!opponentDeck.IsEmpty())
			{
				int amountOfCardsDrawn = Math.Min(5 - handCount, opponentDeck.Length);
				cardGameService.CardService.DrawCards(ref opponentHand, ref opponentDeck);
				Console.WriteLine($"\nOpponent drew {amountOfCardsDrawn} card(s) from the deck!\nCards remaining in opponent's deck: {opponentDeck.Length + 1}");
				Console.WriteLine("Press enter to continue!");
				Console.ReadLine();
				Console.Clear();
			}
		}

		private static void StartGame(ICardGameService cardGameService)
		{
			cardGameService.CardService.Initialize();
			LinkedStack<Card> playerDeck = cardGameService.CardService.GetDeck();
			List<Card> playerHand = cardGameService.CardService.GetHand(ref playerDeck);
			LinkedStack<Card> opponentDeck = cardGameService.CardService.GetDeck();
			List<Card> opponentHand = cardGameService.CardService.GetHand(ref opponentDeck);
			int playerPoints = 0;
			int opponentPoints = 0;
			int turn = 1;
			int choice;
			string playerName = cardGameService.CardService.Name;
			bool canDraw = false;
			Console.Clear();
			do
			{
				Console.WriteLine($"Turn: {turn}\n");
				DisplayHand(playerHand);
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
					Console.Clear();
				}
				else if (canDraw)
				{
					int amountOfCardsDrawn = Math.Min(5 - playerHand.Count(), playerDeck.Length);
					cardGameService.CardService.DrawCards(ref playerHand, ref playerDeck);
					Console.WriteLine($"\n{amountOfCardsDrawn} card(s) were drawn from your deck!\nCards remaining in your deck: {playerDeck.Length + 1}");
					Console.WriteLine("Press enter to continue!");
					Console.ReadLine();
					Console.Clear();
					canDraw = false;
				}
				OpponentsTurn(opponentDeck, opponentHand, ref opponentPoints, cardGameService);

			} while (playerHand.Count() > 0 || !playerDeck.IsEmpty());
			var winner = GetWinner(playerPoints, opponentPoints, cardGameService);

			Console.WriteLine($"The game ended!");
			Console.WriteLine($"The winner is: {winner.Winner}, with {winner.WinnerPoints} points!");
			Console.WriteLine("Enter to proceed!");
			Console.ReadLine();
		}
	}
}
