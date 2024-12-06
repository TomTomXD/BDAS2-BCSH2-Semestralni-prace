namespace FinancniInformacniSystemBanky.Model
{
    public class Loan
    {
        public int LoanId { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime DateOfApproval { get; set; }
        public DateTime DateOfRepayment { get; set; }
        public int ClientId { get; set; }
        //public string ClientFullName => $"{ClientFirstName} {ClientLastName} ({ClientNationaIdNumber})";
        //public string ClientNationaIdNumber { get; set; }
        //public string ClientFirstName { get; set; }
        //public string ClientLastName { get; set; }
        //public string CreditCounselorFullName => $"{CreditCounselorFirstName} {CreditCounselorLastName}";
        //public string CreditCounselorFirstName { get; set; }
        //public string CreditCounselorLastName { get; set; }
        public int CreditCounselorId { get; set; }
        public int LoanType { get; set; }
        public int LoanStatus { get; set; }
    }
}
