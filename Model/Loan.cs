using FinancniInformacniSystemBanky.Model.Helpers;

namespace FinancniInformacniSystemBanky.Model
{
    public class Loan
    {
        public int LoanId { get; set; }
        public decimal Amount { get; set; }
        public decimal InterestRate { get; set; }
        public DateTime DateOfApproval { get; set; }
        public DateTime DateOfRepayment { get; set; }
        public Client Client { get; set; }
        public Employee CreditCounselor { get; set; }
        public LoanType LoanType { get; set; }
        public LoanStatus LoanStatus { get; set; }

        public string FormattedDateOfApproval => DateOfApproval.ToString("dd.MM. yyyy");
        public string FormattedDateOfRepayment => DateOfRepayment.ToString("dd.MM. yyyy");
    }
}
