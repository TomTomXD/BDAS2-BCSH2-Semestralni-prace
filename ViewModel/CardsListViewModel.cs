using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using System.Collections.ObjectModel;
using System.Linq;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class CardsListViewModel
    {
        private readonly CardService _cardService;

        public ObservableCollection<CardViewModel> Cards { get; set; }

        public CardsListViewModel()
        {
            _cardService = new CardService();
            Cards = new ObservableCollection<CardViewModel>();
            LoadCardsFromDatabase();
        }

        private void LoadCardsFromDatabase()
        {
            // Fetch the cards from the database
            var cards = _cardService.GetCards().ToList();

            cards.ForEach(card =>
            {
                var cardViewModel = new CardViewModel
                {
                    CardNumber = card.CardNumber,
                    ExpiryDate = card.ExpirationDate.ToString("MM/yy"),
                    CVV = card.CVV
                };

                Cards.Add(cardViewModel);
            });
        }
    }
}
