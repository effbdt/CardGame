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

		public Card? GetCardByName(string cardName)
		{
			return _context.Cards.FirstOrDefault(c => c.CardName == cardName);
		}

		List<Card> availableCards = new List<Card>();


		public void Initialize()
		{
			availableCards = _context.Cards.ToList();
		}

		private static readonly Random rand = new Random();

		public LinkedStack<Card> GetDeck()
		{

			if (availableCards.Count < 15)
				throw new InvalidOperationException("Not enough cards left to create a full deck.");

			var shuffled = availableCards.OrderBy(c => rand.Next()).ToList();

			var selected = shuffled.Take(15).ToList();

			foreach (var card in selected)
				availableCards.Remove(card);

			LinkedStack<Card> deck = new LinkedStack<Card>();
			foreach (var card in selected)
				deck.LinkeDStackOnTop(card);

			return deck;
		}

		public List<Card> GetHand(ref LinkedStack<Card> Deck)
		{

			List<Card> Hand = new List<Card>();
			for (int i = 0; i < 5 && !Deck.IsEmpty(); i++)
			{
				Hand.Add(Deck.GetFromTop());
			}
			return Hand;
		}
	}
}
