using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.View;
using InformacniSystemBanky.Model;
using InformacniSystemBanky.ViewModel;
using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class AddOrEditUserViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<string> Roles { get; set; }
        public ICommand AddNewPersonCommand { get; }
        public ICommand CancelAddingNewPersonCommand { get; }

        private string department;
        private string position;
        private string selectedPersonType;
        private string actionLabelText;
        private string actionButtonText;
        private string name;
        private string surname;
        private DateTime doB;
        private string nationalIdNumber;
        private string phoneNumber;
        private string email;
        private string selectedRole;
        private string street;
        private int houseNumber;
        private string city;
        private int postalCode;
        private string password;

        private Visibility departmentVisibility;
        private Visibility positionVisibility;

        public string Department
        {
            get { return department; }
            set
            {
                department = value;
                OnPropertyChanged(nameof(Department));
            }
        }

        public string Position
        {
            get { return position; }
            set
            {
                position = value;
                OnPropertyChanged(nameof(Position));
            }
        }

        public string ActionLabelText
        {
            get => actionLabelText;
            set { actionLabelText = value; OnPropertyChanged(nameof(ActionLabelText)); }
        }

        public string ActionButtonText
        {
            get => actionButtonText;
            set { actionButtonText = value; OnPropertyChanged(nameof(ActionButtonText)); }
        }

        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        public string Surname
        {
            get => surname;
            set { surname = value; OnPropertyChanged(nameof(Surname)); }
        }

        public DateTime DoB
        {
            get => doB;
            set { doB = value; OnPropertyChanged(nameof(DoB)); }
        }

        public string NationalIdNumber
        {
            get => nationalIdNumber;
            set { nationalIdNumber = value; OnPropertyChanged(nameof(NationalIdNumber)); }
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set { phoneNumber = value; OnPropertyChanged(nameof(PhoneNumber)); }
        }

        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(nameof(Email)); }
        }

        public string SelectedRole
        {
            get => selectedRole;
            set
            {
                selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }

        public string Street
        {
            get => street;
            set { street = value; OnPropertyChanged(nameof(Street)); }
        }

        public int HouseNumber
        {
            get => houseNumber;
            set { houseNumber = value; OnPropertyChanged(nameof(HouseNumber)); }
        }

        public string City
        {
            get => city;
            set { city = value; OnPropertyChanged(nameof(City)); }
        }

        public int PostalCode
        {
            get => postalCode;
            set { postalCode = value; OnPropertyChanged(nameof(PostalCode)); }
        }

        public string Password
        {
            get => password;
            set { password = value; OnPropertyChanged(nameof(Password)); }
        }

        public string SelectedPersonType
        {
            get => selectedPersonType;
            set
            {
                selectedPersonType = value;
                OnPropertyChanged(nameof(SelectedPersonType));
                UpdateVisibility();
            }
        }

        public Visibility DepartmentVisibility
        {
            get => departmentVisibility;
            set
            {
                departmentVisibility = value;
                OnPropertyChanged(nameof(DepartmentVisibility));
            }
        }

        public Visibility PositionVisibility
        {
            get => positionVisibility;
            set
            {
                positionVisibility = value;
                OnPropertyChanged(nameof(PositionVisibility));
            }
        }

        public event Action PersonAdded;

        public List<string> PersonTypes { get; } = new List<string> { "K", "Z" };

        // Bez parametrický konstruktor pro přidání nové osoby
        public AddOrEditUserViewModel()
        {
            Roles = new ObservableCollection<string>();
            AddNewPersonCommand = new RelayCommand(AddNewPerson);
            CancelAddingNewPersonCommand = new RelayCommand(CloseAddingWindow);

            actionLabelText = "Přidat osobu";
            actionButtonText = "Přidat";
            LoadRolesFromDatabase();

            DepartmentVisibility = Visibility.Hidden;
            PositionVisibility = Visibility.Hidden;
        }

        public AddOrEditUserViewModel(PersonDetails person) : this()
        {
            Name = person.Name;
            Surname = person.Surname;
            DoB = person.DoB;
            NationalIdNumber = person.NationalIdNumber;
            PhoneNumber = person.PhoneNumber;
            Email = person.Email;
            SelectedRole = person.Role;

            actionLabelText = "Upravit osobu";
            actionButtonText = "Upravit";
            LoadAddressFromDatabase(person.NationalIdNumber);
        }

        private void LoadAddressFromDatabase(string nationalIdNumber)
        {
            string userId = ConfigurationManager.AppSettings["DbUserId"];
            string password = ConfigurationManager.AppSettings["DbPassword"];
            string dataSource = ConfigurationManager.AppSettings["DbDataSource"];

            string connectionString = $"User Id={userId};Password={password};Data Source={dataSource};";
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        string query = "SELECT a.ulice, a.cislo_popisne, a.mesto, a.psc " +
                                       "FROM ADRESA a " +
                                       "JOIN OSOBA o ON a.id_adresa = o.id_adresa " +
                                       "WHERE o.rodne_cislo = :NationalIdNumber";

                        using (var command = new OracleCommand(query, connection))
                        {
                            command.Parameters.Add(new OracleParameter("NationalIdNumber", nationalIdNumber));
                            using (var reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    Street = reader["ulice"].ToString();
                                    HouseNumber = Convert.ToInt32(reader["cislo_popisne"]);
                                    City = reader["mesto"].ToString();
                                    PostalCode = Convert.ToInt32(reader["psc"]);
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nepodařilo se připojit k databázi.");
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show($"Chyba při připojování k databázi: {ex.Message}");
                }
            }
        }

        private void LoadRolesFromDatabase()
        {

            string userId = ConfigurationManager.AppSettings["DbUserId"];
            string password = ConfigurationManager.AppSettings["DbPassword"];
            string dataSource = ConfigurationManager.AppSettings["DbDataSource"];

            string connectionString = $"User Id={userId};Password={password};Data Source={dataSource}";
            string query = "SELECT role FROM ROLE";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                OracleCommand command = new OracleCommand(query, connection);
                try
                {
                    connection.Open();
                    OracleDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Roles.Add(reader["role"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error in fetching roles: {ex.Message}");
                }
            }
        }

        private void AddNewPerson()
        {

            var person = new PersonDetails
            {
                Name = Name,
                Surname = Surname,
                DoB = DoB,
                NationalIdNumber = NationalIdNumber,
                PhoneNumber = PhoneNumber,
                Email = Email,
                Role = SelectedRole
            };

            var address = new Address
            {
                Street = Street,
                HouseNumber = HouseNumber,
                City = City,
                PostalCode = PostalCode
            };

            var password = new Password
            {
                Salt = PasswordHasher.GenerateSalt(),
                HashedPassword = PasswordHasher.HashPassword(Password, PasswordHasher.GenerateSalt())
            };

            var employeeDetails = new EmployeeDetails
            {
                Department = Department,
                Position = Position
            };

            // sestavení connection stringu
            string dbUserId = ConfigurationManager.AppSettings["DbUserId"];
            string dbPassword = ConfigurationManager.AppSettings["DbPassword"];
            string dbDataSource = ConfigurationManager.AppSettings["DbDataSource"];

            string connectionString = $"User Id={dbUserId};Password={dbPassword};Data Source={dbDataSource}";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();
                using (OracleCommand addPersonCommand = new OracleCommand("insert_osoba_adresa_heslo", connection))
                {
                    addPersonCommand.CommandType = CommandType.StoredProcedure;

                    addPersonCommand.Parameters.Add("p_jmeno", OracleDbType.Varchar2).Value = person.Name;
                    addPersonCommand.Parameters.Add("p_prijmeni", OracleDbType.Varchar2).Value = person.Surname;
                    addPersonCommand.Parameters.Add("p_datum_narozeni", OracleDbType.Date).Value = person.DoB;
                    addPersonCommand.Parameters.Add("p_rodne_cislo", OracleDbType.Varchar2).Value = person.NationalIdNumber;
                    addPersonCommand.Parameters.Add("p_telefon", OracleDbType.Char).Value = person.PhoneNumber;
                    addPersonCommand.Parameters.Add("p_email", OracleDbType.Varchar2).Value = person.Email;
                    addPersonCommand.Parameters.Add("p_typ_osoby", OracleDbType.Char).Value = SelectedPersonType;
                    addPersonCommand.Parameters.Add("p_id_role", OracleDbType.Int32).Value = GetRoleId(person.Role);
                    addPersonCommand.Parameters.Add("p_ulice", OracleDbType.Varchar2).Value = address.Street;
                    addPersonCommand.Parameters.Add("p_cislo_popisne", OracleDbType.Char).Value = address.HouseNumber.ToString();
                    addPersonCommand.Parameters.Add("p_mesto", OracleDbType.Varchar2).Value = address.City;
                    addPersonCommand.Parameters.Add("p_psc", OracleDbType.Int32).Value = address.PostalCode;
                    addPersonCommand.Parameters.Add("p_hash", OracleDbType.Varchar2).Value = password.HashedPassword;
                    addPersonCommand.Parameters.Add("p_salt", OracleDbType.Varchar2).Value = password.Salt;
                    addPersonCommand.Parameters.Add("p_oddeleni", OracleDbType.Varchar2).Value = employeeDetails.Department;
                    addPersonCommand.Parameters.Add("p_pozice", OracleDbType.Varchar2).Value = employeeDetails.Position;

                    addPersonCommand.ExecuteNonQuery();
                }
            }

            PersonAdded?.Invoke();
            CloseAddingWindow();
        }

        private int GetRoleId(string roleName)
        {
            string userId = ConfigurationManager.AppSettings["DbUserId"];
            string password = ConfigurationManager.AppSettings["DbPassword"];
            string dataSource = ConfigurationManager.AppSettings["DbDataSource"];

            string connectionString = $"User Id={userId};Password={password};Data Source={dataSource};";
            using (var connection = new OracleConnection(connectionString))
            {
                connection.Open();
                string getRoleIdQuery = "SELECT ID_ROLE FROM ROLE WHERE role = :roleName";
                using (var getRoleIdCommand = new OracleCommand(getRoleIdQuery, connection))
                {
                    getRoleIdCommand.Parameters.Add(new OracleParameter(":roleName", roleName));
                    object result = getRoleIdCommand.ExecuteScalar();
                    if (result == null)
                    {
                        MessageBox.Show("Role nebyla nalezena", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                        return 0;
                    }
                    return Convert.ToInt32(result);
                }
            }
        }


        private void CloseAddingWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<AddPersonView>().FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }

        private void UpdatePersonInDatabase()
        {
            string userId = ConfigurationManager.AppSettings["DbUserId"];
            string password = ConfigurationManager.AppSettings["DbPassword"];
            string dataSource = ConfigurationManager.AppSettings["DbDataSource"];

            string connectionString = $"User Id={userId};Password={password};Data Source={dataSource};";
            using (var connection = new OracleConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        string updatePersonQuery = "UPDATE OSOBA SET jmeno = :Name, prijmeni = :Surname, datum_narozeni = :DoB, telefon = :PhoneNumber, email = :Email, id_role = (SELECT id_role FROM ROLE WHERE role = :Role) WHERE rodne_cislo = :NationalIdNumber";

                        using (var command = new OracleCommand(updatePersonQuery, connection))
                        {
                            command.Parameters.Add(new OracleParameter("Name", Name));
                            command.Parameters.Add(new OracleParameter("Surname", Surname));
                            command.Parameters.Add(new OracleParameter("DoB", DoB));
                            command.Parameters.Add(new OracleParameter("PhoneNumber", PhoneNumber));
                            command.Parameters.Add(new OracleParameter("Email", Email));
                            command.Parameters.Add(new OracleParameter("Role", SelectedRole));
                            command.Parameters.Add(new OracleParameter("NationalIdNumber", NationalIdNumber));

                            command.ExecuteNonQuery();
                        }

                        string updateAddressQuery = "UPDATE ADRESA SET ulice = :Street, cislo_popisne = :HouseNumber, mesto = :City, psc = :PostalCode " +
                                                    "WHERE id_adresa = (SELECT id_adresa FROM OSOBA WHERE rodne_cislo = :NationalIdNumber)";

                        using (var command = new OracleCommand(updateAddressQuery, connection))
                        {
                            command.Parameters.Add(new OracleParameter("Street", Street));
                            command.Parameters.Add(new OracleParameter("HouseNumber", HouseNumber));
                            command.Parameters.Add(new OracleParameter("City", City));
                            command.Parameters.Add(new OracleParameter("PostalCode", PostalCode));
                            command.Parameters.Add(new OracleParameter("NationalIdNumber", NationalIdNumber));

                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nepodařilo se připojit k databázi.");
                    }
                }
                catch (OracleException ex)
                {
                    MessageBox.Show($"Chyba při připojování k databázi: {ex.Message}");
                }
            }
        }

        private void UpdateVisibility()
        {
            if (SelectedPersonType == "Z")
            {
                DepartmentVisibility = Visibility.Visible;
                PositionVisibility = Visibility.Visible;
            }
            else
            {
                DepartmentVisibility = Visibility.Hidden;
                PositionVisibility = Visibility.Hidden;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}