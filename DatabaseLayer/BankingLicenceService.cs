using InformacniSystemBanky.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class BankingLicenceService
    {
        private readonly DatabaseService _databaseService;

        public BankingLicenceService()
        {
            _databaseService = new DatabaseService();
        }

        public IEnumerable<BankingLicence> GetBankingLicences()
        {
            throw new NotImplementedException();
        }

        public void AddBankingLicence()
        {
            throw new NotImplementedException();
        }

        public void RemoveBankingLicence()
        {
            throw new NotImplementedException();
        }

        public void EditBankingLicence()
        {
            throw new NotImplementedException();
        }
    }
}
