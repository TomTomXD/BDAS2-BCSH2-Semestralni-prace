using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model.Helpers;
using InformacniSystemBanky.Model;
using InformacniSystemBanky.View;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.ViewModel
{
    public class AddAccountViewModel : INotifyPropertyChanged
    {
        private readonly Dictionary<string, char> accountTypesMapping = new Dictionary<string, char>
        {
            { "Běžný", 'B' },
            { "Spořicí", 'S' }
        };

        private readonly AccountService _accountService;

        private ObservableCollection<Client> _possibleOwners;
        private Client _selectedOwner;


        private string actionButtonText;
        private string actionLabelText;
        private string selectedAccountType;

        private int _accountId;
        private string _accountNumber;
        private double _balance;
        private double _limit;
        private double _interest;
        private double _maxBalance;
        private int _idOwner;

        private Visibility _interestVisibility;
        private Visibility _maxBalanceVisibility;

        public int AccountId
        {
            get => _accountId;
            set { _accountId = value; OnPropertyChanged(nameof(AccountId)); }
        }
        public string AccountNumber
        {
            get => _accountNumber;
            set { _accountNumber = value; OnPropertyChanged(nameof(AccountNumber)); }
        }
        public double Balance
        {
            get => _balance;
            set { _balance = value; OnPropertyChanged(nameof(Balance)); }
        }
        public double Limit
        {
            get => _limit;
            set { _limit = value; OnPropertyChanged(nameof(Limit)); }
        }
        public string SelectedAccountType
        {
            get => selectedAccountType;
            set
            {
                selectedAccountType = value;
                OnPropertyChanged(nameof(SelectedAccountType));
                UpdateVisibility();
            }
        }
        public double Interest
        {
            get => _interest;
            set { _interest = value; OnPropertyChanged(nameof(Interest)); }
        }
        public double MaxBalance
        {
            get => _maxBalance;
            set { _maxBalance = value; OnPropertyChanged(nameof(MaxBalance)); }
        }

        public int IdOwner
        {
            get => _idOwner;
            set { _idOwner = value; OnPropertyChanged(nameof(IdOwner)); }
        }
        public Client SelectedOwner
        {
            get => _selectedOwner;
            set { _selectedOwner = value; OnPropertyChanged(nameof(SelectedOwner)); }
        }
        public ObservableCollection<Client> PossibleOwners
        {
            get => _possibleOwners;
            set { _possibleOwners = value; OnPropertyChanged(nameof(PossibleOwners)); }
        }

        public Visibility InterestVisibility
        {
            get => _interestVisibility;
            set
            {
                _interestVisibility = value;
                OnPropertyChanged(nameof(InterestVisibility));
            }
        }
        public Visibility MaxBalanceVisibility
        {
            get => _maxBalanceVisibility;
            set
            {
                _maxBalanceVisibility = value;
                OnPropertyChanged(nameof(MaxBalanceVisibility));
            }
        }
        public string ActionLabelText
        {
            get => actionLabelText;
            set { actionLabelText = value; OnPropertyChanged(nameof(ActionLabelText)); }
        }
        public string ActionButtonText
        {
            get => actionButtonText;
            set { actionButtonText = value; OnPropertyChanged(nameof(ActionButtonText)); }
        }

        public ICommand AddNewAccountCommand { get; }
        public ICommand CancelAddingNewAccountCommand { get; }
        public List<string> AccountTypes { get; } = new List<string> { "Běžný", "Spořicí" };

        public AddAccountViewModel()
        {
            AddNewAccountCommand = new RelayCommand(AddNewAccount);
            CancelAddingNewAccountCommand = new RelayCommand(CloseAddingWindow);

            _accountService = new AccountService();

            PossibleOwners = new ObservableCollection<Client>(_accountService.GetPossibleOwners());

            actionLabelText = "Přidat účet";
            actionButtonText = "Přidat";

            InterestVisibility = Visibility.Hidden;
            MaxBalanceVisibility = Visibility.Hidden;
        }


        public AddAccountViewModel(Account account)
        {
            AddNewAccountCommand = new RelayCommand(EditAccount);
            CancelAddingNewAccountCommand = new RelayCommand(CloseAddingWindow);

            _accountService = new AccountService();

            PossibleOwners = new ObservableCollection<Client>(_accountService.GetPossibleOwners());
            actionLabelText = "Upravit účet";
            actionButtonText = "Upravit";

            if (account.AccountType == "B")
            {
                InterestVisibility = Visibility.Hidden;
                MaxBalanceVisibility = Visibility.Hidden;
            }
            else
            {
                InterestVisibility = Visibility.Visible;
                MaxBalanceVisibility = Visibility.Visible;
            }

            AccountNumber = account.AccountNumber;
            Balance = (double)account.Balance;
            Limit = (double)account.PaymentLimit;
            SelectedAccountType = account.FriendlyAccountType;
            IdOwner = account.PersonId;
            AccountId = account.AccountId;
            SelectedOwner = PossibleOwners.First(x => x.Id == account.PersonId);


        }

        private void EditAccount()
        {
            if(AccountNumber == null || Balance == 0 || Limit == 0 || SelectedOwner == null || SelectedAccountType == null)
            {
                MessageBox.Show("Všechna pole musí být vyplněna", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _accountService.UpdateAccount(AccountId, AccountNumber, (decimal)Balance, (decimal)Limit, SelectedOwner.Id, ConvertAccountTypeToDbFormat(SelectedAccountType), (decimal)Interest, (decimal)MaxBalance);
            CloseAddingWindow();
        }

        private void AddNewAccount()
        {
            if (AccountNumber == null || Balance == 0 || Limit == 0 || SelectedOwner == null || SelectedAccountType == null)
            {
                MessageBox.Show("Všechna pole musí být vyplněna", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _accountService.AddAccount(AccountNumber, (decimal)Balance, (decimal)Limit, SelectedOwner.Id, ConvertAccountTypeToDbFormat(SelectedAccountType), (decimal)Interest, (decimal)MaxBalance);
            CloseAddingWindow();
        }

        private void UpdateVisibility()
        {
            if (SelectedAccountType == "S")
            {
                MaxBalanceVisibility = Visibility.Visible;
                InterestVisibility = Visibility.Visible;
            }
            else
            {
                MaxBalanceVisibility = Visibility.Hidden;
                InterestVisibility = Visibility.Hidden;
            }
        }

        private char ConvertAccountTypeToDbFormat(string accountType)
        {

            return accountTypesMapping.TryGetValue(accountType, out var dbFormat) ? dbFormat : throw new ArgumentException("Neplatný typ osoby");
        }

        private void CloseAddingWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<AddAccountView>().FirstOrDefault();
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