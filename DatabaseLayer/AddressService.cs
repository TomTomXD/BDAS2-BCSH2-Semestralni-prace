using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.Model;
using Oracle.ManagedDataAccess.Client;
using System.Windows;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class AddressService
    {
        private readonly DatabaseService _databaseService;

        public AddressService()
        {
            _databaseService = new DatabaseService();
        }

        public IEnumerable<Address> GetAddresses()
        {
            string query = "SELECT a.id_adresa, a.cislo_popisne, a.ulice, a. mesto, a.psc FROM ADRESY a";

            return _databaseService.ExecuteSelect(query, reader => new Address
            {
                Id = reader.GetInt32(0),
                HouseNumber = reader.GetString(1),
                Street = reader.GetString(2),
                City = reader.GetString(3),
                PostalCode = reader.GetString(4)
            });
        }

        public void AddAddress(string streetName, string city, string houseNumber, string postalCode)
        {
            try
            {
                var procedureName = "upsert_adresa";
                _databaseService.ExecuteProcedure(procedureName, command =>
                {
                    command.Parameters.Add("p_id_adresa", OracleDbType.Int32).Value = DBNull.Value;
                    command.Parameters.Add("p_cislo_popisne", OracleDbType.Varchar2).Value = houseNumber;
                    command.Parameters.Add("p_ulice", OracleDbType.Varchar2).Value = streetName;
                    command.Parameters.Add("p_mesto", OracleDbType.Varchar2).Value = city;
                    command.Parameters.Add("p_psc", OracleDbType.Int32).Value = postalCode;
                });
                MessageBox.Show("Adresa byla úspěšně přidána.","Úspěch",MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        public void UpdateAddress(int addressId, string streetName, string city, string houseNumber, string postalCode)
        {

            try
            {
                var procedureName = "upsert_adresa";
                _databaseService.ExecuteProcedure(procedureName, command =>
                {
                    command.Parameters.Add("p_id_adresa", OracleDbType.Int32).Value = addressId;
                    command.Parameters.Add("p_cislo_popisne", OracleDbType.Varchar2).Value = houseNumber;
                    command.Parameters.Add("p_ulice", OracleDbType.Varchar2).Value = streetName;
                    command.Parameters.Add("p_mesto", OracleDbType.Varchar2).Value = city;
                    command.Parameters.Add("p_psc", OracleDbType.Int32).Value = postalCode;
                });
                MessageBox.Show("Adresa byla úspěšně upravena.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }   

        public void DeleteAddress(int addressId)
        {
            string query = $"DELETE FROM ADRESY WHERE ID_ADRESA = :id_address";

            try
            {
                _databaseService.ExecuteNonQuery(query, command =>
                {
                    command.Parameters.Add("id_address", OracleDbType.Int32).Value = addressId;
                });
                MessageBox.Show("Adresa byla úspěšně smazána.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
