using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class NoCardException : Exception
    { }

    public class CardService : ICardService
    {
        private readonly ICardDataProvider _cardDataProvider;

        public CardService(ICardDataProvider cardDataProvider)
        {
            _cardDataProvider = cardDataProvider;
        }

        public void Add(Card card)
        {
            if (HighQualityCardValidator.ValidateCard(card))
            {
                _cardDataProvider.Add(card);
                DataImportExportService.SubFolderCheck(card);
            }

        }

        public IEnumerable<Card> GetAllCards()
        {
            return _cardDataProvider.GetAllCards();
        }

        public Card GetCardByName(string cardName)
        {
            if (_cardDataProvider.GetCardByName(cardName) != null) return _cardDataProvider.GetCardByName(cardName);
            throw new NoCardException();
        }

        public LinkedStack<Card> GetDeck()
        {
            return _cardDataProvider.GetDeck();
        }

        public IEnumerable<Card> GetHand()
        {
            return _cardDataProvider.GetHand();
        }

        public void PlayCard(ref List<Card> Hand, Card playedCard, ref int points, ref int cardsInHand)
        {
            points = points + playedCard.CardPower;
            cardsInHand--;
            Hand.RemoveAll(c => c.CardId == playedCard.CardId);
        }

        public void DrawCards(ref List<Card> Hand, ref LinkedStack<Card> Deck)
        {
            int cardsInHand = Hand.Count;

            while (cardsInHand < 5 && Deck.n > -1)
            {
                Card drawnCard = Deck.GetFromTop();
                Hand.Add(drawnCard);
                cardsInHand++;
            }
        }
    }
}
