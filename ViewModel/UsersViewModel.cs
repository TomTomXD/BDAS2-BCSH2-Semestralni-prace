using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.ViewModel;
using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class UsersViewModel
    {
        ObservableCollection<PersonDetails> People { get; set; }

        public ICommand AddPersonCommand { get; }
        public ICommand ChangePersonalDataCommand { get; }

        public UsersViewModel()
        {
            People = new ObservableCollection<PersonDetails>();
            AddPersonCommand = new RelayCommand(AddPerson);
            ChangePersonalDataCommand = new RelayCommand(ChangePersonalData);
        }

        // Metoda pro načtení osob z databáze
        private void LoadPeopleFromDatabase()
        {
            string userId = ConfigurationManager.AppSettings["userId"];
            string password = ConfigurationManager.AppSettings["password"];
            string dataSource = ConfigurationManager.AppSettings["dataSource"];

            string connectionString = $"User Id={userId};Password={password};Data Source={dataSource}";

            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        string query = "SELECT o.jmeno," +
                            "o.prijmeni," +
                            "o.datum_narozeni," +
                            "o.rodne_cislo," +
                            "o.telefon," +
                            "o.email," +
                            "r.role " +
                            "FROM OSOBA O " +
                            "JOIN ROLE r on o.id_role = r.id_role";

                        using (var command = new OracleCommand(query, connection))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    People.Add(new PersonDetails
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
                            }
                        }   
                    }
                    else
                    {
                        MessageBox.Show("Nepodařilo se připojit k databázi");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Nepodařilo se připojit k databázi: " + ex.Message);
                }
            }
        }

        // Obsluha tlačítka pro přidání osoby
        private void AddPerson()
        {
            throw new NotImplementedException();
        }

        // Obsluha tlačítka pro změnu osobních údajů
        private void ChangePersonalData()
        {
            throw new NotImplementedException();
        }

    }
}
