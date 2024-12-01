using InformacniSystemBanky.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void DeleteAccount()
        {

        }

        public void UpdateAccount()
        {

        }
    }
}