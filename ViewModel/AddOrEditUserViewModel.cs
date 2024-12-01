using FinancniInformacniSystemBanky.DatabaseLayer;
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
        public event Action PersonAdded;
        public List<string> PersonTypes { get; } = new List<string> { "K", "Z" };

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

        private readonly RolesService _rolesService;
        private readonly PersonDetailsService _personDetailsService;
        private readonly UserService _userService;
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


        // Bezparametrický konstruktor pro přidání nové osoby
        public AddOrEditUserViewModel()
        {
            _personDetailsService = new PersonDetailsService();
            _rolesService = new RolesService();
            _userService = new UserService();

            Roles = new ObservableCollection<string>(_rolesService.GetRoles());
            AddNewPersonCommand = new RelayCommand(AddNewPerson);
            CancelAddingNewPersonCommand = new RelayCommand(CloseAddingWindow);

            // Nastavení defaultních hodnot pro dnešní datum v datepickeru
            DoB = DateTime.Now;

            actionLabelText = "Přidat osobu";
            actionButtonText = "Přidat";

            DepartmentVisibility = Visibility.Hidden;
            PositionVisibility = Visibility.Hidden;
        }

        // Konstruktor pro editaci osoby
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

        private void AddNewPerson()
        {
            try
            {
                var salt = PasswordHasher.GenerateSalt();
                var hashedPassword = PasswordHasher.HashPassword(Password, salt);
                var saltBase64 = Convert.ToBase64String(salt);
                var houseNumber = Convert.ToString(HouseNumber);

                _userService.RegisterNewUser(
                    Name, 
                    Surname,
                    DoB,
                    NationalIdNumber, 
                    PhoneNumber, 
                    Email,
                    SelectedPersonType,
                    _rolesService.GetRoleId(SelectedRole),
                    Street,
                    houseNumber,
                    City,
                    PostalCode,
                    hashedPassword,
                    saltBase64
                );
                MessageBox.Show("Osoba byla úspěšně přidána.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo k chybě při registraci: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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