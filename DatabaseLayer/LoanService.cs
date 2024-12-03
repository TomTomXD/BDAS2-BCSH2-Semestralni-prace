using FinancniInformacniSystemBanky.Model;
using Oracle.ManagedDataAccess.Client;

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
            string query = @"SELECT * FROM UVERY";

            return _databaseService.ExecuteSelect(query, reader => new Loan
            {
                LoanId = reader.GetInt32(0),
                Amount = reader.GetDecimal(1),
                InterestRate = reader.GetDecimal(2),
                DateOfApproval = reader.GetDateTime(3),
                DateOfRepayment = reader.GetDateTime(4),
                ClientId = reader.GetInt32(5),
                CreditCounselorId = reader.GetInt32(6),
                LoanType = reader.GetInt32(7),
                LoanStatus = reader.GetInt32(8)
            });
        }

        public void AddLoan()
        {
            throw new NotImplementedException();
        }

        public void DeleteLoan(int loanId)
        {
            _databaseService.ExecuteNonQuery("DELETE FROM UVERY WHERE ID_UVER = :id", command =>
            {
                command.Parameters.Add("id", OracleDbType.Int32).Value = loanId;
            });
        }

        public void EditLoan()
        {
            throw new NotImplementedException();
        }
    }
}
