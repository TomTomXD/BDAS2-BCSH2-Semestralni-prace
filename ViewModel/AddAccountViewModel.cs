using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.View;
using InformacniSystemBanky.Model;
using InformacniSystemBanky.View;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.ViewModel
{
    public class AddAccountViewModel : INotifyPropertyChanged
    {
        public List<string> AccountTypes { get; } = new List<string> { "B", "S" };

        private readonly AccountService _accountService;

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

        public AddAccountViewModel()
        {
            AddNewAccountCommand = new RelayCommand(AddNewAccount);
            CancelAddingNewAccountCommand = new RelayCommand(CloseAddingWindow);

            _accountService = new AccountService();

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

            actionLabelText = "Upravit účet";
            actionButtonText = "Upravit";

            if(account.AccountType == "B")
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
            SelectedAccountType = account.AccountType;
            IdOwner = account.PersonId;
            AccountId = account.AccountId;

        }

        private void EditAccount()
        {
            string message = $"Upravovaný účet:\n" +
                 $"Číslo účtu: {AccountNumber}\n" +
                 $"Zůstatek: {Balance:C}\n" +
                 $"Limit: {Limit:C}\n" +
                 $"Vlastník (ID): {IdOwner}\n" +
                 $"Typ účtu: {SelectedAccountType}\n" +
                 $"Úroková sazba: {Interest}%\n" +
                 $"Maximální zůstatek: {MaxBalance:C}";

            MessageBox.Show(message, "Potvrzení úpravy účtu", MessageBoxButton.OK, MessageBoxImage.Information);
            _accountService.UpdateAccount(AccountId, AccountNumber, (decimal)Balance, (decimal)Limit, IdOwner, Convert.ToChar(SelectedAccountType), (decimal)Interest, (decimal)MaxBalance);
            CloseAddingWindow();
        }

        private void AddNewAccount()
        {
            string message = $"Přidávaný účet:\n" +
                 $"Číslo účtu: {AccountNumber}\n" +
                 $"Zůstatek: {Balance:C}\n" +
                 $"Limit: {Limit:C}\n" +
                 $"Vlastník (ID): {IdOwner}\n" +
                 $"Typ účtu: {SelectedAccountType}\n" +
                 $"Úroková sazba: {Interest}%\n" +
                 $"Maximální zůstatek: {MaxBalance:C}";

            MessageBox.Show(message, "Potvrzení přidání účtu", MessageBoxButton.OK, MessageBoxImage.Information);

            _accountService.AddAccount(AccountNumber, (decimal)Balance, (decimal)Limit, IdOwner, Convert.ToChar(SelectedAccountType), (decimal)Interest, (decimal)MaxBalance);
            CloseAddingWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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

        private void CloseAddingWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<AddAccountView>().FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }
    }
}