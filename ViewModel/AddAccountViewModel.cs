using Oracle.ManagedDataAccess.Client;
using System.Configuration;
using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.ViewModel
{
    public class AddAccountViewModel : Window
    {
        public int NewAccountId { get; set; }
        public string NewAccountNumber { get; set; }
        public decimal NewBalance { get; set; }
        public decimal NewPaymentLimit { get; set; }
        public int NewPersonId { get; set; }
        public string NewAccountType { get; set; }

        public ICommand AddAccountCommand { get; }

        public AddAccountViewModel()
        {
            AddAccountCommand = new RelayCommand(AddAccountToDatabase);
        }

        private void AddAccountToDatabase()
        {
            // Retrieve settings from App.config
            string userId = ConfigurationManager.AppSettings["DbUserId"];
            string password = ConfigurationManager.AppSettings["DbPassword"];
            string dataSource = ConfigurationManager.AppSettings["DbDataSource"];

            string connectionString = $"User Id={userId};Password={password};Data Source={dataSource};";
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        string query = "INSERT INTO UCET (ID_UCET, CISLO_UCTU, ZUSTATEK, LIMIT_PRO_PLATBY, ID_OSOBA, TYP_UCTU) VALUES (:AccountId, :AccountNumber, :Balance, :PaymentLimit, :PersonId, :AccountType)";

                        using (var command = new OracleCommand(query, connection))
                        {
                            command.Parameters.Add(new OracleParameter("AccountId", NewAccountId));
                            command.Parameters.Add(new OracleParameter("AccountNumber", NewAccountNumber));
                            command.Parameters.Add(new OracleParameter("Balance", NewBalance));
                            command.Parameters.Add(new OracleParameter("PaymentLimit", NewPaymentLimit));
                            command.Parameters.Add(new OracleParameter("PersonId", NewPersonId));
                            command.Parameters.Add(new OracleParameter("AccountType", NewAccountType));

                            command.ExecuteNonQuery();
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
    }
}