using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.ViewModel;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky
{
    public class CardsViewModel : INotifyPropertyChanged
    {
        private readonly CardService _cardService;
        public ObservableCollection<Card> Cards { get; set; }

        private Card _selectedCard;
        public Card SelectedCard
        {
            get => _selectedCard;
            set
            {
                _selectedCard = value;
                OnPropertyChanged(nameof(SelectedCard));
            }
        }

        public ICommand AddCardCommand { get; }
        public ICommand EditCardCommand { get; }
        public ICommand DeleteCardCommand { get; }

        public CardsViewModel()
        {
            _cardService = new CardService();
            AddCardCommand = new RelayCommand(AddCardToDatabase);
            EditCardCommand = new RelayCommand(EditCard);
            DeleteCardCommand = new RelayCommand(DeleteCardFromDatabase, CanDeleteCard);

            Cards = new ObservableCollection<Card>();

            LoadCardsFromDatabase();
        }

        private void LoadCardsFromDatabase()
        {
            var loadedCards = _cardService.GetCards();

            Cards.Clear();
            foreach (var card in loadedCards)
            {
                Cards.Add(card);
            }
        }

        private void DeleteCardFromDatabase()
        {
            if (CanDeleteCard())
            {
                _cardService.RemoveCard(SelectedCard.CardId);
                LoadCardsFromDatabase();
            }
        }

        private void EditCard()
        {
            var editCardViewModel = new AddCardViewModel(SelectedCard);
            var editCardView = new AddCardView()
            {
                DataContext = editCardViewModel
            };
            editCardView.ShowDialog();    
            LoadCardsFromDatabase();
        }

        private void AddCardToDatabase()
        {
            var addCardView = new AddCardView();
            addCardView.ShowDialog();
            LoadCardsFromDatabase();
        }

        private bool CanDeleteCard()
        {
            return SelectedCard != null; 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
