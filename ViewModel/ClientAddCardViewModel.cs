using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.Model.Helpers;
using InformacniSystemBanky.Model;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Channels;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class ClientAddCardViewModel : INotifyPropertyChanged
    {
        private readonly CardService _cardService;
        private readonly AccountService _accountService;

        private string _cardNumber;
        private DateTime _dateOfIssue;
        private DateTime _expirationDate;
        private string _cvv;
        private CardType _selectedCardType;
        private Account _selectedAccount;
        public string CardNumber
        {
            get => _cardNumber;
            set
            {
                _cardNumber = value;
                OnPropertyChanged(nameof(CardNumber));
            }
        }
        public DateTime DateOfIssue
        {
            get => _dateOfIssue;
            set
            {
                _dateOfIssue = value;
                OnPropertyChanged(nameof(DateOfIssue));
            }
        }
        public DateTime ExpirationDate
        {
            get => _expirationDate;
            set
            {
                _expirationDate = value;
                OnPropertyChanged(nameof(ExpirationDate));
            }
        }
        public string CVV
        {
            get => _cvv;
            set
            {
                _cvv = value;
                OnPropertyChanged(nameof(CVV));
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
        public Account SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
            }
        }
        public ObservableCollection<CardType> CardTypes { get; set; }
        public ObservableCollection<Account> ClientAccounts { get; set; }
        public ICommand AddNewCardCommand { get; }
        public ICommand CancelAddingNewCardCommand { get; }
        public ClientAddCardViewModel()
        {
            _cardService = new CardService();
            _accountService = new AccountService();
            AddNewCardCommand = new RelayCommand(AddCardToDatabase);
            CancelAddingNewCardCommand = new RelayCommand(CancelAddingWindow);

            CardNumber = _cardService.GenerateCardNumber();
            DateOfIssue = DateTime.Now;
            ExpirationDate = DateOfIssue.AddYears(4);
            CVV = GenerateCVV();
            CardTypes = new ObservableCollection<CardType>(_cardService.GetCardTypes());
            ClientAccounts = new ObservableCollection<Account>(_accountService.GetAccountsByIdAndType(Session.Instance.CurrentUserId, 'B'));
        }

        private void CancelAddingWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<ClientAddCardView>().FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }

        private void AddCardToDatabase()
        {
            if(SelectedCardType != null && SelectedAccount != null)
            {
                _cardService.AddCard(CardNumber, DateOfIssue, ExpirationDate, CVV, SelectedCardType.Id, SelectedAccount.AccountId);
                CancelAddingWindow();
            }
        }

        private string GenerateCVV()
        {
            Random random = new Random();
            string cvv = "";

            for (int i = 0; i < 3; i++)
            {
                cvv += random.Next(0, 10).ToString(); // Generuje náhodné číslo od 0 do 9
            }

            return cvv;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
