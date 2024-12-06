using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.Model.Helpers;
using InformacniSystemBanky.Model;
using Oracle.ManagedDataAccess.Client;
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
                    IssueDate = reader.GetDateTime(2),
                    ExpirationDate = reader.GetDateTime(3),
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

        public IEnumerable<BankingLicenceType> GetBankingLicenceTypes()
        {
            try
            {
                string query = @"SELECT * FROM TYPY_LICENCI";

                return _databaseService.ExecuteSelect(query, reader => new BankingLicenceType
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return Enumerable.Empty<BankingLicenceType>();
            }
        }

        public IEnumerable<Employee> GetEmployees()
        {
            try
            {
                string query = @"SELECT z.id_osoba, o.jmeno, o.prijmeni 
                                    from zamestnanci z 
                                    join osoby o on z.id_osoba = o.id_osoba";

                return _databaseService.ExecuteSelect(query, reader => new Employee
                {
                    Id = reader.GetInt32(0),
                    FirstName = reader.GetString(1),
                    LastName = reader.GetString(2)
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return Enumerable.Empty<Employee>();
            }
        }

        public void AddBankingLicence(
                 int? licenceId, // Přidáno pro případ aktualizace
                 string licenceNumber,
                 DateTime issueDate,
                 DateTime expirationDate,
                 int licenceType,
                 int licenceHolderId)
        {
            try
            {
                var storedProcedure = "upsert_bakerske_licence";

                _databaseService.ExecuteProcedure(storedProcedure, command =>
                {
                    // Přidáme parametr pro ID licence (NULL pro vložení)
                    var idLicenceParameter = new OracleParameter("p_id_licence", OracleDbType.Int32);
                    idLicenceParameter.Value = licenceId.HasValue ? (object)licenceId.Value : DBNull.Value; // Použití Value

                    command.Parameters.Add(idLicenceParameter);
                    command.Parameters.Add(new OracleParameter("p_cislo_licence", OracleDbType.Int32) { Value = licenceNumber });
                    command.Parameters.Add(new OracleParameter("p_datum_ziskani", OracleDbType.Date) { Value = issueDate });
                    command.Parameters.Add(new OracleParameter("p_datum_platnosti", OracleDbType.Date) { Value = expirationDate });
                    command.Parameters.Add(new OracleParameter("p_id_typu_licence", OracleDbType.Int32) { Value = licenceType });
                    command.Parameters.Add(new OracleParameter("p_zamestnanec_id_osoba", OracleDbType.Int32) { Value = licenceHolderId });
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void DeleteBankingLicence(int licenceId)
        {
            try
            {
                string query = "DELETE FROM BANKERSKE_LICENCE WHERE ID_LICENCE = :id";

                _databaseService.ExecuteNonQuery(query, command =>
                {
                    command.Parameters.Add(new OracleParameter("id", OracleDbType.Varchar2) { Value = licenceId });
                });
                MessageBox.Show("Licence odstraněna.", "Odstranění Licence", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void EditBankingLicence(
            int licenceId, // Přidáno pro případ aktualizace
                 string licenceNumber,
                 DateTime issueDate,
                 DateTime expirationDate,
                 int licenceType,
                 int licenceHolderId)
        {
            try
            {
                var storedProcedure = "upsert_bakerske_licence";

                _databaseService.ExecuteProcedure(storedProcedure, command =>
                {
                    command.Parameters.Add(new OracleParameter("p_id_licence", OracleDbType.Int32) { Value = licenceId });
                    command.Parameters.Add(new OracleParameter("p_cislo_licence", OracleDbType.Varchar2){Value = licenceNumber });
                    command.Parameters.Add(new OracleParameter("p_datum_ziskani", OracleDbType.Date) { Value = issueDate });
                    command.Parameters.Add(new OracleParameter("p_datum_platnosti", OracleDbType.Date) { Value = expirationDate });
                    command.Parameters.Add(new OracleParameter("p_id_typu_licence", OracleDbType.Int32) { Value = licenceType });
                    command.Parameters.Add(new OracleParameter("p_zamestnanec_id_osoba", OracleDbType.Int32) { Value = licenceHolderId });
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
