using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class AddCardViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<CardType> _cardTypes;

        private CardType _selectedCardType;

        private readonly CardService _cardService;

        private string actionButtonText;
        private string actionLabelText;

        private string _cardNumber;
        private DateTime _dateOfIssue;
        private DateTime _expirationDate;
        private int _owner;

        public string CardNumber
        {
            get => _cardNumber;
            set { _cardNumber = value; }
        }
        public DateTime DateOfIssue
        {
            get => _dateOfIssue;
            set { _dateOfIssue = value; }
        }
        public DateTime ExpirationDate
        {
            get => _expirationDate;
            set { _expirationDate = value; }
        }
        public int Owner
        {
            get => _owner;
            set { _owner = value; }
        }

        public ICommand CancelAddingNewCardCommand { get; }
        public ICommand AddNewCardCommand { get; }

        public string ActionButtonText
        {
            get => actionButtonText;
            set { actionButtonText = value; }
        }
        public string ActionLabelText
        {
            get => actionLabelText;
            set { actionLabelText = value; }
        }

        public ObservableCollection<CardType> CardTypes
        {
            get => _cardTypes;
            set
            {
                _cardTypes = value;
                OnPropertyChanged(nameof(CardTypes));
            }
        }

        public CardType SelectedCardType
        {
            get => _selectedCardType;
            set
            {
                _selectedCardType = value;
                OnPropertyChanged(nameof(SelectedCardType));
            }
        }

        public AddCardViewModel()
        {
            CancelAddingNewCardCommand = new RelayCommand(CloseAddingWindow);
            AddNewCardCommand = new RelayCommand(AddNewCard);

            _cardService = new CardService();
            ActionButtonText = "Přidat";
            ActionLabelText = "Přidání nové karty";

            LoadCardTypes();
        }

        private void LoadCardTypes()
        {
            var cardTypes = _cardService.GetCardTypes();
            CardTypes = new ObservableCollection<CardType>(cardTypes);
        }

        private void AddNewCard()
        {
            throw new NotImplementedException();
            //// Sestavení zprávy pro MessageBox
            //string message = $"Nová karta bude přidána s následujícími údaji:\n" +
            //                 $"- Číslo karty: {CardNumber}\n" +
            //                 $"- Datum vydání: {DateOfIssue.ToShortDateString()}\n" +
            //                 $"- Datum expirace: {ExpirationDate.ToShortDateString()}\n" +
            //                 $"- Majitel: {Owner}\n" +
            //                 $"- Typ karty (ID): {SelectedCardType.CardTypeId}";

            //// Zobrazení zprávy v MessageBoxu
            //MessageBox.Show(message, "Potvrzení vkládaných údajů", MessageBoxButton.OK, MessageBoxImage.Information);


            //_cardService.AddCard(
            //    DateOfIssue, ExpirationDate,"123", SelectedCardType.CardTypeId, Owner, "53");
            //CloseAddingWindow();

        }

        private void CloseAddingWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<AddCardView>().FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
