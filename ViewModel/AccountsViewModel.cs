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

        private int _newAccountId;
        public int NewAccountId
        {
            get => _newAccountId;
            set
            {
                _newAccountId = value;
                OnPropertyChanged(nameof(NewAccountId));
            }
        }

        private string _newAccountNumber;
        public string NewAccountNumber
        {
            get => _newAccountNumber;
            set
            {
                _newAccountNumber = value;
                OnPropertyChanged(nameof(NewAccountNumber));
            }
        }

        private decimal _newBalance;
        public decimal NewBalance
        {
            get => _newBalance;
            set
            {
                _newBalance = value;
                OnPropertyChanged(nameof(NewBalance));
            }
        }

        private decimal _newPaymentLimit;
        public decimal NewPaymentLimit
        {
            get => _newPaymentLimit;
            set
            {
                _newPaymentLimit = value;
                OnPropertyChanged(nameof(NewPaymentLimit));
            }
        }

        private int _newPersonId;
        public int NewPersonId
        {
            get => _newPersonId;
            set
            {
                _newPersonId = value;
                OnPropertyChanged(nameof(NewPersonId));
            }
        }

        private string _newAccountType;
        public string NewAccountType
        {
            get => _newAccountType;
            set
            {
                _newAccountType = value;
                OnPropertyChanged(nameof(NewAccountType));
            }
        }

        public ICommand AddAccountCommand { get; }
        public ICommand DeleteAccountCommand { get; }

        private readonly AccountService _accountService;

        public AccountsViewModel()
        {
            _accountService = new AccountService();
            Accounts = new ObservableCollection<Account>();
            LoadAccountsFromDatabase();
            AddAccountCommand = new RelayCommand(AddAccountToDatabase);
            //DeleteAccountCommand = new RelayCommand(DeleteAccountFromDatabase, CanDeleteAccount);
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
            if (addAccountView.ShowDialog() == true) // ShowDialog returns true when the window is closed with a "success"
            {
                LoadAccountsFromDatabase(); // Refresh the DataGrid after adding
            }
        }
        //// Retrieve settings from App.config
        //string userId = ConfigurationManager.AppSettings["DbUserId"];
        //string password = ConfigurationManager.AppSettings["DbPassword"];
        //string dataSource = ConfigurationManager.AppSettings["DbDataSource"];

        //string connectionString = $"User Id={userId};Password={password};Data Source={dataSource};";
        //using (var connection = new OracleConnection(connectionString))
        //{
        //    try
        //    {
        //        connection.Open();
        //        if (connection.State == System.Data.ConnectionState.Open)
        //        {
        //            string query = "INSERT INTO UCET (ID_UCET, CISLO_UCTU, ZUSTATEK, LIMIT_PRO_PLATBY, ID_OSOBA, TYP_UCTU) VALUES (:AccountId, :AccountNumber, :Balance, :PaymentLimit, :PersonId, :AccountType)";

        //            using (var command = new OracleCommand(query, connection))
        //            {
        //                command.Parameters.Add(new OracleParameter("AccountId", NewAccountId));
        //                command.Parameters.Add(new OracleParameter("AccountNumber", NewAccountNumber));
        //                command.Parameters.Add(new OracleParameter("Balance", NewBalance));
        //                command.Parameters.Add(new OracleParameter("PaymentLimit", NewPaymentLimit));
        //                command.Parameters.Add(new OracleParameter("PersonId", NewPersonId));
        //                command.Parameters.Add(new OracleParameter("AccountType", NewAccountType));

        //                command.ExecuteNonQuery();
        //            }

        //            // Po úspěšném vložení načtěte znovu data
        //            LoadAccountsFromDatabase();
        //        }
        //        else
        //        {
        //            MessageBox.Show("Nepodařilo se připojit k databázi.");
        //        }
        //    }
        //    catch (OracleException ex)
        //    {
        //        MessageBox.Show($"Chyba při připojování k databázi: {ex.Message}");
        //    }
        //}
        //}

        //private void DeleteAccountFromDatabase()
        //{
        //    if (SelectedAccount == null) return;

        //    using (var connection = new OracleConnection(connectionString))
        //    {
        //        try
        //        {
        //            connection.Open();
        //            if (connection.State == System.Data.ConnectionState.Open)
        //            {
        //                string query = "DELETE FROM UCET WHERE ID_UCET = :AccountId";

        //                using (var command = new OracleCommand(query, connection))
        //                {
        //                    command.Parameters.Add(new OracleParameter("AccountId", SelectedAccount.AccountId));
        //                    command.ExecuteNonQuery();
        //                }

        //                // Po úspěšném smazání načtěte znovu data
        //                LoadAccountsFromDatabase();
        //            }
        //            else
        //            {
        //                MessageBox.Show("Nepodařilo se připojit k databázi.");
        //            }
        //        }
        //        catch (OracleException ex)
        //        {
        //            MessageBox.Show($"Chyba při připojování k databázi: {ex.Message}");
        //        }
        //    }
        //}

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