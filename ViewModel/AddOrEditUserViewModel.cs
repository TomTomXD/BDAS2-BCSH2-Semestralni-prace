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