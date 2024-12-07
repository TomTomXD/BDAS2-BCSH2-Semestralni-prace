using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.Model;
using Oracle.ManagedDataAccess.Client;
using System.Windows;

namespace FinancniInformacniSystemBanky.DatabaseLayer
{
    public class PersonDetailsService
    {
        private readonly DatabaseService _databaseService;

        public PersonDetailsService()
        {
            _databaseService = new DatabaseService();
        }

        /// <summary>
        /// Method for getting the details of a people in database.
        /// </summary>
        /// <returns>
        /// Returns collection of person details.
        /// </returns>
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

        /// <summary>
        /// Method for getting the ID of a person.
        /// </summary>
        /// <param name="nationalIdNumber"></param>
        /// <returns>
        /// Returns the ID of the person.
        /// </returns>
        public int GetPersonId(string nationalIdNumber)
        {
            string query = @"
                    SELECT id_osoba
                    FROM OSOBY
                    WHERE rodne_cislo = :nationalIdNumber";
            return _databaseService.ExecuteSelect(query, reader => reader.GetInt32(0), command =>
            {
                command.Parameters.Add(new OracleParameter("nationalIdNumber", nationalIdNumber));
            }).FirstOrDefault();
        }

        public Address GetAddress(int id)
        {
            string query = @"
        SELECT a.ulice,
               a.cislo_popisne,
               a.mesto,
               a.psc
        FROM ADRESY a
        JOIN OSOBY o ON a.id_adresa = o.id_adresa
        WHERE o.id_osoba = :id";

            try
            {
                var address = _databaseService.ExecuteSelect(query, reader =>
                {
                    return new Address
                    {
                        Street = reader.GetString(reader.GetOrdinal("ulice")),
                        HouseNumber = reader.GetString(reader.GetOrdinal("cislo_popisne")), // Use GetString for CHAR type
                        City = reader.GetString(reader.GetOrdinal("mesto")),
                        PostalCode = reader.GetInt32(reader.GetOrdinal("psc")) // Use GetInt32 for NUMBER type
                    };
                },
                command =>
                {
                    command.Parameters.Add(new OracleParameter("id", OracleDbType.Int32)
                    {
                        Value = id
                    });
                }).FirstOrDefault();

                return address;
            }
            catch (OracleException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return null;
        }


        /// <summary>
        /// Method for getting the type of a person.
        /// </summary>
        /// <param name="nationalIdNumber"></param>
        /// <returns>
        /// Returns char letter of the type of the person.
        /// </returns>
        public char GetTypeOfPerson(string nationalIdNumber)
        {
            string query = @"
                SELECT typ_osoby
                FROM OSOBY
                WHERE rodne_cislo = :nationalIdNumber";

            try
            {
                // Používá ExecuteSelect pro provedení dotazu
                var type = _databaseService.ExecuteSelect(query, reader =>
                {
                    return reader.GetString(reader.GetOrdinal("typ_osoby"))[0];
                },
                command =>
                {
                    command.Parameters.Add(new OracleParameter("nationalIdNumber", OracleDbType.Varchar2)
                    {
                        Value = nationalIdNumber
                    });
                }).FirstOrDefault();

                // Validace hodnoty
                if (type == default(char) || (type != 'K' && type != 'Z'))
                {
                    // Hodnota není platná, buď vyvoláme výjimku, nebo vrátíme výchozí hodnotu
                    MessageBox.Show("Typ osoby nebyl nalezen nebo je neplatný.", "Informace", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return 'K'; // Výchozí hodnota
                }

                return type; // Platná hodnota
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nepředvídaná chyba: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            // Pokud dojde k výjimce, můžeme vrátit výchozí hodnotu
            return 'K';
        }

        /// <summary>
        /// Method for getting the details of an employee.
        /// </summary>
        /// <param name="nationalIdNumber"></param>
        /// <returns>
        /// Returns the details of the employee (department and position).
        /// </returns>
        public EmployeeDetails GetEmployeeDetails(string nationalIdNumber)
        {
            string query = @"
                SELECT o.oddeleni, p.pozice
                FROM ZAMESTNANCI z
                JOIN ODDELENI o ON z.id_oddeleni = o.id_oddeleni
                JOIN POZICE p ON z.id_pozice = p.id_pozice
                JOIN OSOBY os ON z.id_osoba = os.id_osoba
                WHERE os.rodne_cislo = :nationalIdNumber";

            //try
            //{
            //    //var employeeDetails = _databaseService.ExecuteSelect(query, reader =>
            //    //{
            //    //    return new EmployeeDetails
            //    //    {
            //    //        Department = reader.GetString(reader.GetOrdinal("oddeleni")),
            //    //        Position = reader.GetString(reader.GetOrdinal("pozice"))
            //    //    };
            //    //},
            //    //command =>
            //    //{
            //    //    command.Parameters.Add(new OracleParameter("nationalIdNumber", OracleDbType.Varchar2)
            //    //    {
            //    //        Value = nationalIdNumber
            //    //    });
            //    //}).FirstOrDefault();

            //    if (employeeDetails == null)
            //    {
            //        // No data found for the given national ID
            //        MessageBox.Show("Details not found for the provided national ID number.", "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
            //    }

            //    return employeeDetails;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show($"Unexpected error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return null;
            //}

            throw new NotSupportedException();
        }

    }
}