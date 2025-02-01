using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public interface ICardService
    {
        void Add(Card card);
        IEnumerable<Card> GetAllCards();
        Card GetCardByName(string cardName);

        LinkedStack<Card> GetDeck();

        IEnumerable<Card> GetHand();
    }
}
