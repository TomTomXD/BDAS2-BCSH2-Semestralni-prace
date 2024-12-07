using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class ClientCardsViewModel : INotifyPropertyChanged
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

        private Visibility _editCardVisibility;
        public Visibility EditCardVisibility
        {
            get => _editCardVisibility;
            set
            {
                _editCardVisibility = value;
                OnPropertyChanged(nameof(EditCardVisibility));
            }
        }

        public ICommand AddCardCommand { get; }
        public ICommand DeleteCardCommand { get; }

        public ClientCardsViewModel()
        {
            _cardService = new CardService();
            AddCardCommand = new RelayCommand(AddCardToDatabase);
            DeleteCardCommand = new RelayCommand(DeleteCardFromDatabase, CanDeleteCard);

            EditCardVisibility = Visibility.Hidden;

            Cards = new ObservableCollection<Card>();

            LoadCardsFromDatabase();
        }

        private void AddCardToDatabase()
        {
            var addClientCardViewModel = new ClientAddCardViewModel();
            var addCardView = new ClientAddCardView()
            {
                DataContext = addClientCardViewModel
            };
            addCardView.ShowDialog();
            LoadCardsFromDatabase();
        }

        private void LoadCardsFromDatabase()
        {
            var loadedCards = _cardService.GetClientsCards(Session.Instance.CurrentUserId);

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
