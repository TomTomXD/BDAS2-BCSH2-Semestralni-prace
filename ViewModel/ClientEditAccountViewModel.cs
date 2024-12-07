using FinancniInformacniSystemBanky.DatabaseLayer;
using InformacniSystemBanky.Model;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class ClientEditAccountViewModel : INotifyPropertyChanged
    {
        private readonly AccountService _accountService;
        private int _id;
        private string _accountNumber;
        private decimal _limit;
        public string AccountNumber
        {
            get => _accountNumber;
            set
            {
                _accountNumber = value;
                OnPropertyChanged(nameof(AccountNumber));
            }
        }
        public decimal Limit
        {
            get => _limit;
            set
            {
                _limit = value;
                OnPropertyChanged(nameof(Limit));
            }
        }

        public ICommand ChangeLimitCommand { get; }
        public ICommand CancelChangingLimit { get; }

        public ClientEditAccountViewModel(Account selectedAccount)
        {
            _accountService = new AccountService();
            _id = selectedAccount.AccountId;
            AccountNumber = $"Číslo účtu: {selectedAccount.AccountNumber}";

            ChangeLimitCommand = new RelayCommand(ChangeLimit);
            CancelChangingLimit = new RelayCommand(CloseWindow);
        }

        private void CloseWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<ClientEditAccountView>().FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }

        private void ChangeLimit()
        {
            if(Limit < 0)
            {
                MessageBox.Show("Limit musí být kladné číslo");
                return;
            }
            _accountService.ChangeLimit(_id, Limit);
            CloseWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
