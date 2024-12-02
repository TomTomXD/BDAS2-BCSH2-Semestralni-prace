using InformacniSystemBanky.Model;
using System.Collections.ObjectModel;
using Oracle.ManagedDataAccess.Client;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Configuration;
using InformacniSystemBanky.View;
using FinancniInformacniSystemBanky.DatabaseLayer;

namespace InformacniSystemBanky.ViewModel
{
    public class AccountsViewModel : INotifyPropertyChanged
    {
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
        public ICommand EditAccountCommand { get; }
        public ICommand DeleteAccountCommand { get; }
       

        private readonly AccountService _accountService;

        public AccountsViewModel()
        {
            _accountService = new AccountService();
            Accounts = new ObservableCollection<Account>();
            LoadAccountsFromDatabase();
            AddAccountCommand = new RelayCommand(AddAccountToDatabase);
            DeleteAccountCommand = new RelayCommand(DeleteAccountFromDatabase, CanDeleteAccount);
            EditAccountCommand = new RelayCommand(EditAccount);
        }

        private void DeleteAccountFromDatabase()
        {
            if (CanDeleteAccount())
            {
                _accountService.DeleteAccount(SelectedAccount.AccountNumber);
                LoadAccountsFromDatabase();
            }
        }

        private void EditAccount()
        {
            if (SelectedAccount != null)
            {
                var addAccountViewModel = new AddAccountViewModel(SelectedAccount);
                var addAccountView = new AddAccountView
                {
                    DataContext = addAccountViewModel
                };
                addAccountView.ShowDialog();
            }
            LoadAccountsFromDatabase();
        }


        private void LoadAccountsFromDatabase()
        {
            var accountsFromDb = _accountService.GetAccounts();

            Accounts.Clear();
            foreach (var account in accountsFromDb)
            {
                Accounts.Add(account);
            }
        }

        private void AddAccountToDatabase()
        {
            var addAccountView = new AddAccountView();
            if (addAccountView.ShowDialog() == true) 
            {
                LoadAccountsFromDatabase(); 
            }
        }

        

        private bool CanDeleteAccount()
        {
            return SelectedAccount != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}