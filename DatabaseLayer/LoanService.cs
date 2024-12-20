﻿using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.Model.Helpers;
using Oracle.ManagedDataAccess.Client;
using System.Windows;

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
            string query = @"SELECT * FROM V_VSECHNY_UVERY";

            return _databaseService.ExecuteSelect(query, reader => new Loan
            {
                LoanId = reader.GetInt32(0),
                Amount = reader.GetDecimal(1),
                InterestRate = reader.GetDecimal(2),
                DateOfApproval = reader.GetDateTime(3),
                DateOfRepayment = reader.GetDateTime(4),
                Client = new Client
                {
                    Id = reader.GetInt32(5),
                    FirstName = reader.GetString(6),
                    LastName = reader.GetString(7),
                    NationalIdNumber = null,
                },
                CreditCounselor = new Employee
                {
                    Id = reader.GetInt32(8),
                    FirstName = reader.GetString(9),
                    LastName = reader.GetString(10)
                },
                LoanType = new LoanType
                {
                    Id = reader.GetInt32(11),
                    Name = reader.GetString(12)
                },
                LoanStatus = new LoanStatus
                {
                    Id = reader.GetInt32(13),
                    Name = reader.GetString(14)
                }
            });
        }

        public IEnumerable<Loan> GetLoansByClientId(int id)
        {
            string query = @"SELECT * FROM V_VSECHNY_UVERY WHERE klient_id_osoba = :id";

            return _databaseService.ExecuteSelect(query, reader => new Loan
            {
                LoanId = reader.GetInt32(0),
                Amount = reader.GetDecimal(1),
                InterestRate = reader.GetDecimal(2),
                DateOfApproval = reader.GetDateTime(3),
                DateOfRepayment = reader.GetDateTime(4),
                Client = new Client
                {
                    Id = reader.GetInt32(5),
                    FirstName = reader.GetString(6),
                    LastName = reader.GetString(7),
                    NationalIdNumber = null,
                },
                CreditCounselor = new Employee
                {
                    Id = reader.GetInt32(8),
                    FirstName = reader.GetString(9),
                    LastName = reader.GetString(10)
                },
                LoanType = new LoanType
                {
                    Id = reader.GetInt32(11),
                    Name = reader.GetString(12)
                },
                LoanStatus = new LoanStatus
                {
                    Id = reader.GetInt32(13),
                    Name = reader.GetString(14)
                }
            }, command =>
            {
                command.Parameters.Add(new OracleParameter("klient_id_osoba", id));
            });
        }

        public IEnumerable<Loan> GetLoansByCreditCounselorId(int id)
        {
            string query = @"SELECT * FROM V_VSECHNY_UVERY WHERE zamestnanec_id_osoba = :id";

            return _databaseService.ExecuteSelect(query, reader => new Loan
            {
                LoanId = reader.GetInt32(0),
                Amount = reader.GetDecimal(1),
                InterestRate = reader.GetDecimal(2),
                DateOfApproval = reader.GetDateTime(3),
                DateOfRepayment = reader.GetDateTime(4),
                Client = new Client
                {
                    Id = reader.GetInt32(5),
                    FirstName = reader.GetString(6),
                    LastName = reader.GetString(7),
                    NationalIdNumber = null,
                },
                CreditCounselor = new Employee
                {
                    Id = reader.GetInt32(8),
                    FirstName = reader.GetString(9),
                    LastName = reader.GetString(10)
                },
                LoanType = new LoanType
                {
                    Id = reader.GetInt32(11),
                    Name = reader.GetString(12)
                },
                LoanStatus = new LoanStatus
                {
                    Id = reader.GetInt32(13),
                    Name = reader.GetString(14)
                }
            }, command =>
            {
                command.Parameters.Add(new OracleParameter("zamestnanec_id_osoba", id));
            });
        }

        public void AddLoan(
            decimal ammount,
            decimal interestRate,
            DateTime dateOfApproval,
            DateTime dateOfRepayment,
            int cliendId,
            int creditCounselorId,
            int loanType,
            int loanStatus)
        {
            try
            {
                var procedureName = "upsert_uver";

                _databaseService.ExecuteProcedure(procedureName, configureCommand =>
                {
                    configureCommand.Parameters.Add("p_id_uver", OracleDbType.Int32).Value = null;
                    configureCommand.Parameters.Add("p_castka", OracleDbType.Decimal).Value = ammount;
                    configureCommand.Parameters.Add("p_urokova_sazba", OracleDbType.Decimal).Value = interestRate;
                    configureCommand.Parameters.Add("p_datum_schvaleni", OracleDbType.Date).Value = dateOfApproval;
                    configureCommand.Parameters.Add("p_datum_splatnosti", OracleDbType.Date).Value = dateOfRepayment;
                    configureCommand.Parameters.Add("p_klient_id_osoba", OracleDbType.Int32).Value = cliendId;
                    configureCommand.Parameters.Add("p_schvalovac_id", OracleDbType.Int32).Value = creditCounselorId;
                    configureCommand.Parameters.Add("p_typ_uveru", OracleDbType.Int32).Value = loanType;
                    configureCommand.Parameters.Add("p_stav_uveru", OracleDbType.Int32).Value = loanStatus;
                });
                MessageBox.Show("Úvěr úšpěšně přidán", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void UpdateLoan(
            int id,
            decimal ammount,
            decimal interestRate,
            DateTime dateOfApproval,
            DateTime dateOfRepayment,
            int cliendId,
            int creditCounselorId,
            int loanType,
            int loanStatus)
        {
            try
            {
                var procedureName = "upsert_uver";

                _databaseService.ExecuteProcedure(procedureName, configureCommand =>
                {
                    configureCommand.Parameters.Add("p_id_uver", OracleDbType.Int32).Value = id;
                    configureCommand.Parameters.Add("p_castka", OracleDbType.Decimal).Value = ammount;
                    configureCommand.Parameters.Add("p_urokova_sazba", OracleDbType.Decimal).Value = interestRate;
                    configureCommand.Parameters.Add("p_datum_schvaleni", OracleDbType.Date).Value = dateOfApproval;
                    configureCommand.Parameters.Add("p_datum_splatnosti", OracleDbType.Date).Value = dateOfRepayment;
                    configureCommand.Parameters.Add("p_klient_id_osoba", OracleDbType.Int32).Value = cliendId;
                    configureCommand.Parameters.Add("p_schvalovac_id", OracleDbType.Int32).Value = creditCounselorId;
                    configureCommand.Parameters.Add("p_typ_uveru", OracleDbType.Int32).Value = loanType;
                    configureCommand.Parameters.Add("p_stav_uveru", OracleDbType.Int32).Value = loanStatus;
                });
                MessageBox.Show("Úvěr byl upraven", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void DeleteLoan(int loanId)
        {
            _databaseService.ExecuteNonQuery("DELETE FROM UVERY WHERE ID_UVER = :id", command =>
            {
                command.Parameters.Add("id", OracleDbType.Int32).Value = loanId;
            });
        }

        public IEnumerable<CreditCounselor> GetCreditCounselors()
        {
            string query = @"SELECT * FROM SCHVALOVACI_UVERU";

            return _databaseService.ExecuteSelect(query, reader => new CreditCounselor
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2)
            });
        }

        public IEnumerable<LoanStatus> GetLoanStatus()
        {
            string query = $"SELECT * FROM STAVY_UVERU";

            return _databaseService.ExecuteSelect(query, reader => new LoanStatus
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }

        public IEnumerable<LoanType> GetLoanTypes()
        {
            string query = @"SELECT * FROM TYPY_UVERU";

            return _databaseService.ExecuteSelect(query, reader => new LoanType
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            });
        }

        public IEnumerable<Client> GetEligibleClientsForLoan()
        {
            string query = @"SELECT * FROM DOSPELE_OSOBY";

            return _databaseService.ExecuteSelect(query, reader => new Client
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                NationalIdNumber = reader.GetString(3)
            });
        }

        public int GetRandomCreditCounselorId()
        {
            // Tady předpokládáme, že máte uloženou funkci nebo SQL, která vrací náhodné ID
            string query = "SELECT NAHODNY_SCHVALOVAC_ID FROM DUAL";

            // Zavolejte ExecuteScalar, abyste získali náhodné ID
            object result = _databaseService.ExecuteScalar(query);

            // Zajistěte, aby výsledek nebyl NULL a převeďte na int
            if (result != null && int.TryParse(result.ToString(), out int randomId))
            {
                return randomId;
            }

            // Vratí zaměstnance s ID 1, pokud se nepodaří získat náhodné ID
            return 1;
        }
    }

}
