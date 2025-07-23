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

		public bool NameEntered { get; set; }
		public string Name { get; set; } = "Player";

		public Action SuccessfullyAddedCardEvent { get; set; }
		public Action FailedValidationEvent { get; set; }

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
				SuccessfullyAddedCardEvent?.Invoke();
			}
			else
			{
				FailedValidationEvent?.Invoke();
			}

		}

		public void playerUsername(string name)
		{
			NameEntered = true;
			Name = name;
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

		public List<Card> GetHand(ref LinkedStack<Card> Deck)
		{
			return _cardDataProvider.GetHand(ref Deck);
		}

		public void PlayCard(ref List<Card> Hand, Card playedCard, ref int points)
		{
			if (Hand.Count() > 0)
			{
				points = points + playedCard.CardPower;
				Hand.RemoveAll(c => c.CardId == playedCard.CardId);
			}
		}

		public void DrawCards(ref List<Card> Hand, ref LinkedStack<Card> Deck)
		{
			while (Hand.Count() < 5 && !Deck.IsEmpty())
			{
				Hand.Add(Deck.GetFromTop());
			}
		}

		public void Initialize()
		{
			_cardDataProvider.Initialize();
		}
	}
}
