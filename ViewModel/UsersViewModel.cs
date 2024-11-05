using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class UsersViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PersonDetails> People { get; set; }
        public ICollectionView FilteredPeople { get; set; }
        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilteredPeople.Refresh();
            }
        }

        private PersonDetails selectedPerson;
        public PersonDetails SelectedPerson
        {
            get => selectedPerson;
            set
            {
                selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
            }
        }

        public ICommand AddPersonCommand { get; }
        public ICommand ChangePersonalDataCommand { get; }
        public ICommand DeletePersonCommand { get; }

        public UsersViewModel()
        {
            People = new ObservableCollection<PersonDetails>();
            FilteredPeople = CollectionViewSource.GetDefaultView(People);
            FilteredPeople.Filter = FilterPeople;
            LoadPeopleFromDatabase();
            AddPersonCommand = new RelayCommand(AddPerson);
            ChangePersonalDataCommand = new RelayCommand(ChangePersonalData);
            DeletePersonCommand = new RelayCommand(DeletePerson, CanDeletePerson);
        }

        private bool FilterPeople(object obj)
        {
            if (obj is PersonDetails person)
            {
                return string.IsNullOrEmpty(SearchText) ||
                       person.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       person.Surname.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       person.DoB.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       person.NationalIdNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       person.PhoneNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       person.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       person.Role.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        // Metoda pro načtení osob z databáze
        private void LoadPeopleFromDatabase()
        {
            string userId = ConfigurationManager.AppSettings["DbUserId"];
            string password = ConfigurationManager.AppSettings["DbPassword"];
            string dataSource = ConfigurationManager.AppSettings["DbDataSource"];

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
            var addUserViewModel = new AddOrEditUserViewModel();
            addUserViewModel.PersonAdded += OnPersonAdded;
            var addPersonView = new AddPersonView
            {
                DataContext = addUserViewModel
            };
            addPersonView.ShowDialog();
        }

        // Obsluha tlačítka pro změnu osobních údajů
        private void ChangePersonalData()
        {
            {
                if (SelectedPerson != null)
                {
                    var addUserViewModel = new AddOrEditUserViewModel(SelectedPerson);
                    var addPersonView = new AddPersonView
                    {
                        DataContext = addUserViewModel
                    };
                    addPersonView.ShowDialog();
                }
            }
        }

            private bool CanEditPerson()
            {
                return SelectedPerson != null;
            }

            // Obsluha tlačítka pro smazání osoby
            private void DeletePerson()
            {
            if (SelectedPerson != null)
            {
                string userId = ConfigurationManager.AppSettings["DbUserId"];
                string password = ConfigurationManager.AppSettings["DbPassword"];
                string dataSource = ConfigurationManager.AppSettings["DbDataSource"];
                string connectionString = $"User Id={userId};Password={password};Data Source={dataSource}";

                using (var connection = new OracleConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        var getIdOfDeletedPersonQuery = "SELECT ID_OSOBA FROM OSOBA WHERE RODNE_CISLO = :nationalId";
                        int idOfDeletedPerson;
                        using (var getIdOfDeletedCommand = new OracleCommand(getIdOfDeletedPersonQuery, connection))
                        {
                            getIdOfDeletedCommand.Parameters.Add(new OracleParameter(":nationalId", SelectedPerson.NationalIdNumber));
                            idOfDeletedPerson = Convert.ToInt32(getIdOfDeletedCommand.ExecuteScalar());
                        }

                        // SQL DELETE password command
                        string deletePasswordQuery = "DELETE FROM HESLO WHERE ID_OSOBA = :id";

                        using (var deletePasswordCommand = new OracleCommand(deletePasswordQuery, connection))
                        {
                            deletePasswordCommand.Parameters.Add(new OracleParameter(":id", idOfDeletedPerson));
                            deletePasswordCommand.ExecuteNonQuery();
                        }

                        // SQL DELETE person command
                        string deletePersonQuery = "DELETE FROM OSOBA WHERE RODNE_CISLO = :nationalId";

                        using (var deleteCommand = new OracleCommand(deletePersonQuery, connection))
                        {
                            deleteCommand.Parameters.Add(new OracleParameter(":nationalId", SelectedPerson.NationalIdNumber));
                            int rowsAffected = deleteCommand.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                People.Remove(SelectedPerson);
                                MessageBox.Show("Osoba byla úspěšně odebrána.");
                            }
                            else
                            {
                                MessageBox.Show("Nepodařilo se odebrat osobu z databáze.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Chyba při odstraňování osoby: " + ex.Message);
                    }
                }
            }
            LoadPeopleFromDatabase();
        }


        private bool CanDeletePerson()
        {
            return SelectedPerson != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnPersonAdded()
        {
            LoadPeopleFromDatabase();
        }
    }
}