using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.Model;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class ClientAccountsViewModel : INotifyPropertyChanged
    {
        private readonly AccountService _accountService;
        private readonly int _currentUserId;
        public ObservableCollection<Account> Accounts { get; set; }

        private Account _selectedAccount;

        public Account SelectedAccount
        {
            get => _selectedAccount;
            set
            {
                _selectedAccount = value;
                OnPropertyChanged(nameof(SelectedAccount));
            }
        }

        public ICommand AddAccountCommand { get; }
        public ICommand ChangeLimitCommand { get; }
        public ICommand DeleteAccountCommand { get; }
        public ICommand LockAccountCommand { get; }

        public ClientAccountsViewModel()
        {
            _accountService = new AccountService();
            AddAccountCommand = new RelayCommand(AddAccount);
            ChangeLimitCommand = new RelayCommand(ChangeLimit);
            DeleteAccountCommand = new RelayCommand(DeleteAccount, CanDeleteAccount);
            LockAccountCommand = new RelayCommand(LockAccount);

            if (Session.Instance.EmulatedUserId == null )
            {
                _currentUserId = Session.Instance.CurrentUserId;
            }
            else
            {
                _currentUserId = (int) Session.Instance.EmulatedUserId;
            }

            Accounts = new ObservableCollection<Account>();
            LoadAccountsFromDatabase();
        }

        private void LockAccount()
        {
            if (SelectedAccount != null)
            {
                var result = MessageBox.Show(
                    "Pokud si myslíte že Váš účet byl napaden, nebo se někdo zmocnil vaší karty/karet spojených s tímto účtem, MŮŽETE ÚČET ZABLOKOVAT. VŠECHNY KARTY PŘIŘAZENÝ K TOMUTO ÚČTU JSOU IHNED ZRUŠENY A VAŠE LIMITY JSOU NASTAVENY NA 0.",
                    "ZABEZPEČENÍ ÚČTU",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes) 
                {
                    _accountService.LockedAccount(SelectedAccount.AccountNumber);
                    LoadAccountsFromDatabase();
                }
            }
        }

        private bool CanDeleteAccount()
        {
            return SelectedAccount != null;
        }

        private void DeleteAccount()
        {
            if(CanDeleteAccount())
            {
                _accountService.DeleteAccount(SelectedAccount.AccountNumber);
                LoadAccountsFromDatabase();
            }
        }

        private void LoadAccountsFromDatabase()
        {
            var accountsFromDb = _accountService.GetAccountsById(_currentUserId);

            Accounts.Clear();
            foreach (var account in accountsFromDb)
            {
                Accounts.Add(account);
            }
        }

        private void ChangeLimit()
        {
            if(SelectedAccount != null)
            {
                var changeLimitViewModel = new ClientEditAccountViewModel(SelectedAccount);
                var changeLimitView = new ClientEditAccountView()
                {
                    DataContext = changeLimitViewModel
                };
                changeLimitView.ShowDialog();
                LoadAccountsFromDatabase();
            }
        }

        private void AddAccount()
        {
            _accountService.AddClientAccount(_currentUserId);
            LoadAccountsFromDatabase();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
