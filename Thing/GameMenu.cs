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
            List<Card> playerHand = cardGameService.CardService.GetHand();
            LinkedStack<Card> opponentDeck = cardGameService.CardService.GetDeck();
            var opponentHand = cardGameService.CardService.GetHand();
            int playerPoints = 0;
            int opponentPoints = 0;
            int turn = 1;
            int cardsInHand = playerHand.Count();
            int choice;
            string playerName = cardGameService.CardService.Name;

            do
            {
                for (int i = 0; i < playerHand.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {playerHand[i].CardName}    {playerHand[i].CardPower}");
                }
                while (true)
                {
                    Console.Write("Enter the index of the card you want to play: ");
                    if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= playerHand.Count())
                    {
                        break;
                    }
                    Console.WriteLine("Invalid input. Please enter a valid index.");
                }
                Card playedCard = playerHand[choice - 1];
                cardGameService.CardService.PlayCard(ref playerHand, playedCard, ref playerPoints, ref cardsInHand);
                Console.Clear();
                Console.WriteLine($"{playerName} played: {playedCard.CardName}");
                Console.WriteLine($"{playerName}'s points: {playerPoints}");
                turn++;
                Console.WriteLine("\nEnter to proceed!");
                Console.ReadLine();

            } while (cardsInHand > 0);
            Console.ReadLine();
        }
    }
}
