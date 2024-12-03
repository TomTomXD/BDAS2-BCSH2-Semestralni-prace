using InformacniSystemBanky.Model;
using System.Windows;

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
            try
            {
                string query = @"SELECT * FROM BANKERSKE_LICENCE";

                return _databaseService.ExecuteSelect(query, reader => new BankingLicence
                {
                    BankingLicenseId = reader.GetInt32(0),
                    LicenceNumber = reader.GetString(1),
                     IssueDate = DateOnly.FromDateTime(reader.GetDateTime(2)),
                     ExpirationDate = DateOnly.FromDateTime(reader.GetDateTime(3)),
                    LicenceType = reader.GetInt32(4),
                    LicenceHolderId = reader.GetInt32(5)
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return Enumerable.Empty<BankingLicence>();
            }
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
