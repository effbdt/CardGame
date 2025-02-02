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
                .Configure(config =>
                {
                    config.Title = "Main Menu";
                    config.EnableWriteTitle = true;
                });
            return menu;
        }

        private static void OpenSettings(ICardGameService cardGameService)
        {
            var menu = new ConsoleMenu()
                .Add("Add a custom card!", () => CustomCardAdd(cardGameService))
                .Add("Back", ConsoleMenu.Close);
            menu.Show();
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
        }
    }
}
