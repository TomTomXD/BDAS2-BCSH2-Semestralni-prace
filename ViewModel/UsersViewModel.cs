using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
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

        public ICommand AddPersonCommand { get; }
        public ICommand ChangePersonalDataCommand { get; }

        public UsersViewModel()
        {
            People = new ObservableCollection<PersonDetails>();
            FilteredPeople = CollectionViewSource.GetDefaultView(People);
            FilteredPeople.Filter = FilterPeople;
            LoadPeopleFromDatabase();
            AddPersonCommand = new RelayCommand(AddPerson);
            ChangePersonalDataCommand = new RelayCommand(ChangePersonalData);
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
            var addPersonView = new AddPersonView();
            addPersonView.ShowDialog();
        }

        // Obsluha tlačítka pro změnu osobních údajů
        private void ChangePersonalData()
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
