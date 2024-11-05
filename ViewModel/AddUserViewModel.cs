using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.View;
using InformacniSystemBanky.Model;
using InformacniSystemBanky.ViewModel;
using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class AddUserViewModel : INotifyPropertyChanged 
    {
        public ObservableCollection<string> Roles { get; set; }
        public ICommand AddNewPersonCommand { get; }
        public ICommand CancelAddingNewPersonCommand { get; }

        private string name;
        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }

        private string surname;
        public string Surname
        {
            get => surname;
            set { surname = value; OnPropertyChanged(nameof(Surname)); }
        }

        private DateTime doB;
        public DateTime DoB
        {
            get => doB;
            set { doB = value; OnPropertyChanged(nameof(DoB)); }
        }

        private string nationalIdNumber;
        public string NationalIdNumber
        {
            get => nationalIdNumber;
            set { nationalIdNumber = value; OnPropertyChanged(nameof(NationalIdNumber)); }
        }

        private string phoneNumber;
        public string PhoneNumber
        {
            get => phoneNumber;
            set { phoneNumber = value; OnPropertyChanged(nameof(PhoneNumber)); }
        }

        private string email;
        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(nameof(Email)); }
        }

        private string selectedRole; 
        public string SelectedRole
        {
            get => selectedRole;
            set
            {
                selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }

        private string street;
        public string Street
        {
            get => street;
            set { street = value; OnPropertyChanged(nameof(Street)); }
        }

        private int houseNumber;
        public int HouseNumber
        {
            get => houseNumber;
            set { houseNumber = value; OnPropertyChanged(nameof(HouseNumber)); }
        }

        private string city;
        public string City
        {
            get => city;
            set { city = value; OnPropertyChanged(nameof(City)); }
        }

        private int postalCode;
        public int PostalCode
        {
            get => postalCode;
            set { postalCode = value; OnPropertyChanged(nameof(PostalCode)); }
        }

        private string password;
        public string Password
        {
            get => password;
            set { password = value; OnPropertyChanged(nameof(Password)); }
        }

        private string selectedPersonType;
        public string SelectedPersonType
        {
            get => selectedPersonType;
            set
            {
                selectedPersonType = value;
                OnPropertyChanged(nameof(SelectedPersonType));
            }
        }

        public event Action PersonAdded;

        public List<string> PersonTypes { get; } = new List<string> { "K", "Z" };

        public AddUserViewModel()
        {
            Roles = new ObservableCollection<string>();
            LoadRolesFromDatabase();
            AddNewPersonCommand = new RelayCommand(AddNewPerson);
            CancelAddingNewPersonCommand = new RelayCommand(CloseAddingWindow);
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
           
            if (SelectedRole == "Ověřený klient" || SelectedRole == "Neověřený klient")
            {
                SelectedPersonType = "K";
            }
            else if (SelectedRole == "Admin" || SelectedRole == "Zaměstnanec")
            {
                SelectedPersonType = "Z"; 
            }
            else
            {
                MessageBox.Show("Invalid role selected. Cannot assign person type.");
                return;
            }

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
                HashedPassword = Password,
                Salt = "salt"
            };

            string dbUserId = ConfigurationManager.AppSettings["DbUserId"];
            string dbPassword = ConfigurationManager.AppSettings["DbPassword"];
            string dbDataSource = ConfigurationManager.AppSettings["DbDataSource"];

            string connectionString = $"User Id={dbUserId};Password={dbPassword};Data Source={dbDataSource}";

            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                connection.Open();

                string getNextAddressIdQuery = "SELECT SEQ_ADRESA.NEXTVAL FROM DUAL";
                int addressId;
                using (var getAddressIdCommand = new OracleCommand(getNextAddressIdQuery, connection))
                {
                    addressId = Convert.ToInt32(getAddressIdCommand.ExecuteScalar());
                }

                string insertAddressQuery = "INSERT INTO ADRESA (ID_ADRESA, CISLO_POPISNE, ULICE, MESTO, PSC) " +
                                            "VALUES (:id, :houseNumber, :street, :city, :postalCode)";

                using (var insertAddressCommand = new OracleCommand(insertAddressQuery, connection))
                {
                    insertAddressCommand.Parameters.Add(new OracleParameter(":id", addressId));
                    insertAddressCommand.Parameters.Add(new OracleParameter(":houseNumber", HouseNumber));
                    insertAddressCommand.Parameters.Add(new OracleParameter(":street", Street));
                    insertAddressCommand.Parameters.Add(new OracleParameter(":city", City));
                    insertAddressCommand.Parameters.Add(new OracleParameter(":postalCode", PostalCode));

                    try
                    {
                        insertAddressCommand.ExecuteNonQuery();
                        MessageBox.Show("Address added successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding address: {ex.Message}");
                        return;
                    }
                }

                string getRoleIdQuery = "SELECT ID_ROLE FROM ROLE WHERE role = :roleName";
                int roleId;
                using (var getRoleIdCommand = new OracleCommand(getRoleIdQuery, connection))
                {
                    getRoleIdCommand.Parameters.Add(new OracleParameter(":roleName", SelectedRole));
                    object result = getRoleIdCommand.ExecuteScalar();
                    if (result == null)
                    {
                        MessageBox.Show("Role not found!");
                        return;
                    }
                    roleId = Convert.ToInt32(result);
                }

                string getNextPersonIdQuery = "SELECT SEQ_OSOBA.NEXTVAL FROM DUAL";
                int personId;
                using (var getPersonIdCommand = new OracleCommand(getNextPersonIdQuery, connection))
                {
                    personId = Convert.ToInt32(getPersonIdCommand.ExecuteScalar());
                }

                string insertPersonQuery = "INSERT INTO OSOBA (ID_OSOBA, JMENO, PRIJMENI, DATUM_NAROZENI, RODNE_CISLO, " +
                                           "TELEFON, EMAIL, TYP_OSOBY, ID_ROLE, ID_ADRESA) " +
                                           "VALUES (:id, :name, :surname, :dob, :nationalId, " +
                                           ":phone, :email, :typOsoby, :roleId, :addressId)";

                using (var insertPersonCommand = new OracleCommand(insertPersonQuery, connection))
                {
                    insertPersonCommand.Parameters.Add(new OracleParameter(":id", personId));
                    insertPersonCommand.Parameters.Add(new OracleParameter(":name", Name));
                    insertPersonCommand.Parameters.Add(new OracleParameter(":surname", Surname));
                    insertPersonCommand.Parameters.Add(new OracleParameter(":dob", DoB));
                    insertPersonCommand.Parameters.Add(new OracleParameter(":nationalId", NationalIdNumber));
                    insertPersonCommand.Parameters.Add(new OracleParameter(":phone", PhoneNumber));
                    insertPersonCommand.Parameters.Add(new OracleParameter(":email", Email));
                    insertPersonCommand.Parameters.Add(new OracleParameter(":typOsoby", SelectedPersonType));
                    insertPersonCommand.Parameters.Add(new OracleParameter(":roleId", roleId));
                    insertPersonCommand.Parameters.Add(new OracleParameter(":addressId", addressId));

                    try
                    {
                        insertPersonCommand.ExecuteNonQuery();
                        MessageBox.Show("Person added successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding person: {ex.Message}");
                    }
                }

                string getNextPasswordIdQuery = "SELECT SEQ_HESLO.NEXTVAL FROM DUAL";
                int passwordId;
                using (var getPasswordIdCommand = new OracleCommand(getNextPersonIdQuery, connection))
                {
                    passwordId = Convert.ToInt32(getPasswordIdCommand.ExecuteScalar());
                    MessageBox.Show(passwordId.ToString());
                }

                string insertPasswordQuery = "Insert into HESLO (ID_HESLO, HASH, SALT, ID_OSOBA) " +
                                             "VALUES (:idPassword, :hashedPassword, :salt, :idPerson)";

                using (var insertPasswordCommand = new OracleCommand(insertPasswordQuery, connection))
                {
                    var salt = PasswordHasher.GenerateSalt();
                    insertPasswordCommand.Parameters.Add(new OracleParameter(":idPassword", passwordId));
                    insertPasswordCommand.Parameters.Add(new OracleParameter(":hashedPassword", PasswordHasher.HashPassword(Password, salt)));
                    insertPasswordCommand.Parameters.Add(new OracleParameter(":salt", salt));
                    insertPasswordCommand.Parameters.Add(new OracleParameter(":idPerson", personId));

                    try
                    {
                        insertPasswordCommand.ExecuteNonQuery();
                        MessageBox.Show("Password added successfully!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding password: {ex.Message}");
                    }
                }
            }
            PersonAdded?.Invoke();
            CloseAddingWindow();
        }

        private void CloseAddingWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<AddPersonView>().FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}