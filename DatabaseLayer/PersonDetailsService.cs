using FinancniInformacniSystemBanky.Model;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class PersonDetailsService
    {
        private readonly DatabaseService _databaseService;

        public PersonDetailsService()
        {
            _databaseService = new DatabaseService();
        }

        public IEnumerable<PersonDetails> GetPersonDetails()
        {
            string query = @"
                    SELECT o.jmeno,
                           o.prijmeni,
                           o.datum_narozeni,
                           o.rodne_cislo,
                           o.telefon,
                           o.email,
                           r.role
                    FROM OSOBY o
                    JOIN ADRESY a ON o.id_adresa = a.id_adresa
                    JOIN ROLE r ON o.id_role = r.id_role";

            return _databaseService.ExecuteSelect(query, reader => new PersonDetails
            {
                Name = reader.GetString(0),
                Surname = reader.GetString(1),
                DoB = reader.GetDateTime(2),
                NationalIdNumber = reader.GetString(3),
                PhoneNumber = reader.GetString(4),
                Email = reader.GetString(5),
                Role = reader.GetString(6)
            });
        }

        public void AddPerson()
        {

        }
    }
}
