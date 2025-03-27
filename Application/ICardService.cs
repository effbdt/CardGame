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
        Action SuccessfullyAddedCardEvent { get; set; }
        Action FailedValidationEvent { get; set; }
        void Add(Card card);
        IEnumerable<Card> GetAllCards();
        Card GetCardByName(string cardName);

        LinkedStack<Card> GetDeck();

        List<Card> GetHand();
        void PlayCard(ref List<Card> Hand, Card playedCard, ref int points, ref int cardsInHand);
        void DrawCards(ref List<Card> Hand, ref LinkedStack<Card> Deck);

        string playerUsername(string name);

        bool NameEntered { get; set; }
    }
}
