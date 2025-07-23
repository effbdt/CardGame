using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
	public interface ICardDataProvider
	{
		void Add(Card card);
		IEnumerable<Card> GetAllCards();
		Card GetCardByName(string cardName);

		LinkedStack<Card> GetDeck();

		List<Card> GetHand(ref LinkedStack<Card> Deck);
	}
}
