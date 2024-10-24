using InformacniSystemBanky.Model;
using System.Collections.ObjectModel;
using Oracle.ManagedDataAccess.Client;
using System.Windows;
using System.Windows.Input;
using System.ComponentModel;
using System.Configuration;
using InformacniSystemBanky.View;

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

        public AccountsViewModel()
        {
            Accounts = new ObservableCollection<Account>();
            LoadAccountsFromDatabase();
            AddAccountCommand = new RelayCommand(AddAccountToDatabase);
            DeleteAccountCommand = new RelayCommand(DeleteAccountFromDatabase, CanDeleteAccount);
        }

        private void LoadAccountsFromDatabase()
        {
            // Retrieve settings from App.config
            string userId = ConfigurationManager.AppSettings["DbUserId"];
            string password = ConfigurationManager.AppSettings["DbPassword"];
            string dataSource = ConfigurationManager.AppSettings["DbDataSource"];

            string connectionString = $"User Id={userId};Password={password};Data Source={dataSource};";


            // Přizpůsobte připojovací řetězec podle vaší databáze
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        //SELECT o.jmeno, o.prijemni, u.cislo_uctu, u.zustatek, u.limit_pro_platby FROM UCET u" +
                        //    " JOIN OSOBA o ON u.id_osoba = o.osoba.id"
                        string query = "SELECT * FROM UCET";

                        using (var command = new OracleCommand(query, connection))
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Accounts.Add(new Account
                                {
                                    AccountId = reader.GetInt32(0),
                                    AccountNumber = reader.GetString(1),
                                    Balance = reader.GetDecimal(2),
                                    PaymentLimit = reader.GetDecimal(3),
                                    PersonId = reader.GetInt32(4),
                                    AccountType = reader.GetString(5)
                                });
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nepodařilo se připojit k databázi.");
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show($"Chyba při připojování k databázi: {ex.Message}");
                }
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

        private void DeleteAccountFromDatabase()
        {
            if (SelectedAccount == null) return;

            string connectionString = "User Id=ST69650;Password=Qn5_89@RoH/e;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=FEI-SQL3.UPCEUCEBNY.CZ)(PORT=1521)))(CONNECT_DATA=(SID=BDAS)));";
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        string query = "DELETE FROM UCET WHERE ID_UCET = :AccountId";

                        using (var command = new OracleCommand(query, connection))
                        {
                            command.Parameters.Add(new OracleParameter("AccountId", SelectedAccount.AccountId));
                            command.ExecuteNonQuery();
                        }

                        // Po úspěšném smazání načtěte znovu data
                        LoadAccountsFromDatabase();
                    }
                    else
                    {
                        MessageBox.Show("Nepodařilo se připojit k databázi.");
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show($"Chyba při připojování k databázi: {ex.Message}");
                }
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