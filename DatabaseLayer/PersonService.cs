using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.Model.Helpers;
using InformacniSystemBanky.Model;
using System.Text;
using System.Windows;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class PersonService
    {
        private readonly DatabaseService _databaseService;

        public PersonService()
        {
            _databaseService = new DatabaseService();
        }

        /// <summary>
        /// Method for getting the details of a people in database.
        /// </summary>
        /// <returns>
        /// Returns collection of person details.
        /// </returns>
        public IEnumerable<Person> GetPersonDetails()
        {
            string query = "SELECT * FROM V_VSECHNY_OSOBY";

            return _databaseService.ExecuteSelect(query, reader =>
            {
                var person = new Person
                {
                    Id = reader.GetInt32(0),           // Id z prvního sloupce
                    Name = reader.GetString(1),        // Jméno (index 1)
                    Surname = reader.GetString(2),     // Příjmení (index 2)
                    DoB = reader.GetDateTime(3),       // Datum narození (index 3)
                    NationalIdNumber = reader.GetString(4), // Rodné číslo (index 4)
                    PhoneNumber = reader.GetString(5), // Telefon (index 5)
                    Email = reader.GetString(6),       // Email (index 6)
                    PersonType = reader.GetString(7),  // Typ osoby (index 7)
                    Role = new Role
                    {
                        Id = reader.GetInt32(8),       // Role Id (index 8)
                        Name = reader.GetString(9)     // Role Name (index 9)
                    },
                    Address = new Address
                    {
                        Id = reader.GetInt32(10),       // Adresa Id (index 10)
                        Street = reader.GetString(11),  // Ulice (index 11)
                        HouseNumber = reader.GetString(12), // Číslo popisné (index 12)
                        City = reader.GetString(13),     // Město (index 13)
                        PostalCode = reader.GetString(14) // PSČ (index 14)
                    }
                };

                // Získání údajů o zaměstnanci, pokud existují
                if (!reader.IsDBNull(15) || !reader.IsDBNull(16) || !reader.IsDBNull(17) || !reader.IsDBNull(18) || !reader.IsDBNull(19))
                {
                    person.EmployeeDetails = new EmployeeDetails
                    {
                        Department = new Department
                        {
                            Id = reader.IsDBNull(15) ? 0 : reader.GetInt32(15), // Oddělení Id (index 15)
                            Name = reader.IsDBNull(16) ? string.Empty : reader.GetString(16) // Oddělení Název (index 16)
                        },
                        Position = new Position
                        {
                            Id = reader.IsDBNull(17) ? 0 : reader.GetInt32(17), // Pozice Id (index 17)
                            Name = reader.IsDBNull(18) ? string.Empty : reader.GetString(18) // Pozice Název (index 18)
                        },
                        Manager = reader.IsDBNull(19) ? null : new Employee
                        {
                            Id = reader.GetInt32(19), // Manažer Id (index 19)
                            FirstName = reader.GetString(20), // Manažer Jméno (index 20)
                            LastName = reader.GetString(21) // Manažer Příjmení (index 21)
                        }
                    };
                }

                return person;
            });
        }

    }


}