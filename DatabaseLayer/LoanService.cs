using FinancniInformacniSystemBanky.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class LoanService
    {
        private readonly DatabaseService _databaseService;

        public LoanService()
        {
            _databaseService = new DatabaseService();
        }

        public IEnumerable<Loan> GetLoans()
        {
            throw new NotImplementedException();
        }

        public void AddLoan()
        {
            throw new NotImplementedException();
        }

        public void RemoveLoan()
        {
            throw new NotImplementedException();
        }

        public void EditLoan()
        {
            throw new NotImplementedException();
        }
    }
}
