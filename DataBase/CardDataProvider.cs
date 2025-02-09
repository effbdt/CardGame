using Infrastructure;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class CardDataProvider : ICardDataProvider
    {
        private readonly CardGameDbContext _context;

        public CardDataProvider(CardGameDbContext context)
        {
            _context = context;
        }

        public void Add(Card card)
        {
            _context.Add(card);
            _context.SaveChanges();
        }

        public IEnumerable<Card> GetAllCards()
        {
            return _context.Cards.ToList();
        }

        public Card GetCardByName(string cardName)
        {
            return _context.Cards.FirstOrDefault(c => c.CardName == cardName);
        }

        public LinkedStack<Card> GetDeck()
        {
            LinkedStack<Card> Deck = new LinkedStack<Card>();
            var cards = _context.Cards.Take(15).ToList();

            foreach (var card in cards)
            {
                Deck.LinkeDStackOnTop(card);
            }

            return Deck;
        }

        public List<Card> GetHand()
        {
            LinkedStack<Card> Deck = GetDeck();
            List<Card> Hand = new List<Card>();
            for (int i = 0; i < 5; i++)
            {
                Hand.Add(Deck.GetFromTop());
            }
            return Hand;
        }
    }
}
