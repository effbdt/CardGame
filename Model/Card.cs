using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Card
    {
        public int CardId { get; set; }
        public string CardName { get; set; }
        public int CardPower { get; set; }

        //false is low (bronze or smth)
        //true is high (gold or smth, stronger card)
        public bool HighQuality { get; set; }

        public string Description { get; set; }

        public Card(string cardName, int cardPower, bool highQuality, string description)
        {
            CardName = cardName;
            CardPower = cardPower;
            HighQuality = highQuality;
            Description = description;
        }
    }

    public class Deck
    {
        [MaxLength(25)]
        LinkedStack<Card> Cards { get; set; }
        public Deck(LinkedStack<Card> deck)
        {
            Cards = deck;
        }
    }
}
