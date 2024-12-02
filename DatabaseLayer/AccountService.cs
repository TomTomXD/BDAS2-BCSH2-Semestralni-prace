using InformacniSystemBanky.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class AccountService
    {
        private readonly DatabaseService _databaseService;

        public AccountService()
        {
            _databaseService = new DatabaseService();
        }   

        public IEnumerable<Account> GetAccounts()
        {
            string query = @"
            SELECT * FROM UCTY";

            return _databaseService.ExecuteSelect(query, reader => new Account
            {
                AccountId = reader.GetInt32(0),
                AccountNumber = reader.GetString(1),
                Balance = reader.GetDecimal(2),
                PaymentLimit = reader.GetDecimal(3),
                PersonId = reader.GetInt32(4),
                AccountType = reader.GetString(5)
            });
        }

        public void AddAccount()
        {

        }

        public void DeleteAccount(string accountNumber)
        {
            try
            {
                string query = @"DELETE FROM UCTY WHERE CISLO_UCTU = :accountNumber";
                _databaseService.ExecuteNonQuery(query, command =>
                {
                    command.Parameters.Add(new OracleParameter("accountNumber", accountNumber));
                });
                MessageBox.Show("Účet odstraněn.", "Odstranění účtu", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Nepodařilo se odstranit účet.", "Odstranění účtu", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateAccount()
        {

        }
    }
}