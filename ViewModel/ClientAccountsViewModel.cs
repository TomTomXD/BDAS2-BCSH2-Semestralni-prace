using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.Model;
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
    public class ClientAccountsViewModel : INotifyPropertyChanged
    {
        private readonly AccountService _accountService;
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

        public ClientAccountsViewModel()
        {
            _accountService = new AccountService();
            AddAccountCommand = new RelayCommand(AddAccount);
            ChangeLimitCommand = new RelayCommand(ChangeLimit);
            DeleteAccountCommand = new RelayCommand(DeleteAccount, CanDeleteAccount);
            
            Accounts = new ObservableCollection<Account>();
            LoadAccountsFromDatabase();
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
            var accountsFromDb = _accountService.GetAccountsById(Session.Instance.CurrentUserId);

            Accounts.Clear();
            foreach (var account in accountsFromDb)
            {
                Accounts.Add(account);
            }
        }

        private void ChangeLimit()
        {
            var changeLimitViewModel = new ClientEditAccountViewModel(SelectedAccount);
            var changeLimitView = new ClientEditAccountView()
            {
                DataContext = changeLimitViewModel
            };
            changeLimitView.ShowDialog();
        }

        private void AddAccount()
        {
            _accountService.AddClientAccount(Session.Instance.CurrentUserId);
            LoadAccountsFromDatabase();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
