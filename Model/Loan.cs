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
        public int CreditCounselorId { get; set; }
        public int LoanType { get; set; }
        public int LoanStatus { get; set; }
    }

    public class LoanStatus
    {
        public int Id { get; set; }
        public string Status { get; set; }
    }

    public class LoanType
    {
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
