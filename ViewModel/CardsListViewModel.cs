using FinancniInformacniSystemBanky.DatabaseLayer;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class CardsListViewModel : INotifyPropertyChanged
    {
        private readonly CardService _cardService;
        public ICommand AddCardCommand { get; }
        public ICommand EditCardCommand { get; }
        public ICommand DeleteCardCommand { get; }
        public ObservableCollection<CardViewModel> Cards { get; set; }

        public CardsListViewModel()
        {
            AddCardCommand = new RelayCommand(AddNewCard);

            _cardService = new CardService();
            Cards = new ObservableCollection<CardViewModel>();
            LoadCardsFromDatabase();
        }

        private void AddNewCard()
        {
            var addCardView = new AddCardView();
            addCardView.ShowDialog();
            LoadCardsFromDatabase();
        }

        private void LoadCardsFromDatabase()
        {
            // Vyprázdnění stávající kolekce
            Cards.Clear();

            // Načtení karet z databáze
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
