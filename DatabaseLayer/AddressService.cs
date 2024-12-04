using FinancniInformacniSystemBanky.Model;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class AddressService
    {
        private readonly DatabaseService _databaseService;

        public AddressService()
        {
            _databaseService = new DatabaseService();
        }

        public IEnumerable<AddressTable> GetAddresses()
        {
            string query = "SELECT a.id_adresa, a.cislo_popisne, a.ulice, a. mesto, a.psc FROM ADRESY a";

            return _databaseService.ExecuteSelect(query, reader => new AddressTable
            {
                AddressId = reader.GetInt32(0),
                HouseNumber = reader.GetString(1),
                StreetName = reader.GetString(2),
                City = reader.GetString(3),
                PostalCode = reader.GetInt32(4)
            });
        }

        public void AddAddress()
        {
            throw new NotImplementedException();
        }

        public void DeleteAddress(int addressId)
        {
            throw new NotImplementedException();
        }

        public void UpdateAddress(int addressId)
        {
            throw new NotImplementedException();
        }   
    }
}
