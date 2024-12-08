using FinancniInformacniSystemBanky.Model.Helpers;

namespace InformacniSystemBanky.Model
{
    public class BankingLicence
    {
        public int BankingLicenseId { get; set; }
        public string LicenceNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public BankingLicenceType LicenceType { get; set; }
        public int LicenceHolderId { get; set; }
        public string LicenceHolderName { get; set; }
        public string LicenceHolderSurname { get; set; }

        public string LicenceHolderFullName => $"{LicenceHolderName} {LicenceHolderSurname}";
        public string FormattedIssueDate => IssueDate.ToString("dd.MM. yyyy");
        public string FormattedExpirationDate => ExpirationDate.ToString("dd.MM. yyyy");
    }
}
