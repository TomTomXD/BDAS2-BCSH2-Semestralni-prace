using InformacniSystemBanky.Model;

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
