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
            string query = @"SELECT * FROM BANKERSKE_LICENCE";

            return _databaseService.ExecuteSelect(query, reader => new BankingLicence
            {
                BankingLicenseId = reader.GetInt32(0),
                LicenceNumber = reader.GetInt32(1),
                IssueDate = reader.GetDateTime(2),
                ExpirationDate = reader.GetDateTime(3),
                LicenceType = reader.GetInt32(4),
                LicenceHolderId = reader.GetInt32(5)
            });
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
