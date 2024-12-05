using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.Model.Helpers;
using InformacniSystemBanky.Model;
using Oracle.ManagedDataAccess.Client;
using System.Windows;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class StandingOrderService
    {
        private readonly DatabaseService _databaseService;

        public StandingOrderService()
        {
            _databaseService = new DatabaseService();
        }

        public IEnumerable<StandingOrder> GetStandingOrders()
        {
            string query = @"
                        SELECT * FROM TRVALE_PRIKAZY";

            return _databaseService.ExecuteSelect(query, reader => new StandingOrder
            {
                StandingOrderId = reader.GetInt32(0),
                Amount = reader.GetDecimal(1),
                SendersAccountId = reader.GetInt32(2)
            });
        }

        public IEnumerable<NormalAccount> GetAllNormalAccounts()
        {
            string query = "SELECT * FROM V_BEZNE_UCTY";

            return _databaseService.ExecuteSelect(query, reader => new NormalAccount
            {
                AccountId = reader.GetInt32(3),
                FirstName = reader.GetString(0),
                LastName = reader.GetString(1),
                AccountNumber = reader.GetString(2)
            });
        }

        public void AddStandingOrder(decimal amount, int accountId)
        {
            try
            {
                var storedProcedura = "upsert_trvaly_prikaz";

                _databaseService.ExecuteProcedure(storedProcedura, command =>
                {
                    command.Parameters.Add("p_id_trvaly_prikaz", OracleDbType.Int32).Value = DBNull.Value;
                    command.Parameters.Add("p_castka", OracleDbType.Decimal).Value = amount;
                    command.Parameters.Add("p_ucet_id", OracleDbType.Int32).Value = accountId;
                });
                MessageBox.Show("Trvalý příkaz byl úspěšně přidán.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void EditStandingOrder(int standingOrderId, decimal amount, int accountId)
        {
            try
            {
                var storedProcedura = "upsert_trvaly_prikaz";

                _databaseService.ExecuteProcedure(storedProcedura, command =>
                {
                    command.Parameters.Add("p_id_trvaly_prikaz", OracleDbType.Int32).Value = standingOrderId;
                    command.Parameters.Add("p_castka", OracleDbType.Decimal).Value = amount;
                    command.Parameters.Add("p_ucet_id", OracleDbType.Int32).Value = accountId;
                });
                MessageBox.Show("Trvalý příkaz byl úspěšně upraven.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void DeleteStandingOrder(int standingOrderId)
        {
            try
            {
                string query = "DELETE FROM TRVALE_PRIKAZY WHERE ID_TRVALEHO_PRIKAZU = :id";
                _databaseService.ExecuteNonQuery(query, command =>
                {
                    command.Parameters.Add("id", OracleDbType.Int32).Value = standingOrderId;
                });
                MessageBox.Show("Trvalý příkaz byl úspěšně smazán.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}