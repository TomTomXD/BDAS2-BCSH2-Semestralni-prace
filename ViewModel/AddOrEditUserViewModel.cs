using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.View;
using InformacniSystemBanky.Model;
using InformacniSystemBanky.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class AddOrEditUserViewModel : INotifyPropertyChanged
    {
        // Hodnoty řešeny napevno slovníkem, jelikož nemůže nastat jiná situace
        // v databázi je Osoba/Uživatel implementován pomocí super typu Osova a subtypů Klient a Zaměstnanec (samostatné tabulky)
        // tudíž nemůže nastat jiná varianta
        private readonly Dictionary<string, string> personTypeMapping = new Dictionary<string, string>
            {
                { "Klient", "K" },
                { "Zaměstnanec", "Z" }
            };

        public ObservableCollection<string> Roles { get; set; }
        public ICommand AddNewPersonCommand { get; }
        public ICommand CancelAddingNewPersonCommand { get; }
        public ICommand EditPersonCommand { get; }
        public event Action PersonAdded;
        public List<string> PersonTypes { get; } = new List<string> { "Klient", "Zaměstnanec" };

        private string _department;
        private string _position;
        private string selectedPersonType;
        private string actionLabelText;
        private string actionButtonText;
        private string _name;
        private string _surname;
        private DateTime _doB;
        private string _nationalIdNumber;
        private string _phoneNumber;
        private string _email;
        private string _selectedRole;
        private string _street;
        private string _houseNumber;
        private string _city;
        private int _postalCode;
        private string _password;
        private int id;

        private Visibility departmentVisibility;
        private Visibility positionVisibility;

        private readonly RolesService _rolesService;
        private readonly PersonDetailsService _personDetailsService;
        private readonly UserService _userService;

        public string Department
        {
            get { return _department; }
            set
            {
                _department = value;
                OnPropertyChanged(nameof(Department));
            }
        }
        public string Position
        {
            get { return _position; }
            set
            {
                _position = value;
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
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
        }
        public string Surname
        {
            get => _surname;
            set { _surname = value; OnPropertyChanged(nameof(Surname)); }
        }
        public DateTime DoB
        {
            get => _doB;
            set { _doB = value; OnPropertyChanged(nameof(DoB)); }
        }
        public string NationalIdNumber
        {
            get => _nationalIdNumber;
            set { _nationalIdNumber = value; OnPropertyChanged(nameof(NationalIdNumber)); }
        }
        public string PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(nameof(PhoneNumber)); }
        }
        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(nameof(Email)); }
        }
        public string SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }
        public string Street
        {
            get => _street;
            set { _street = value; OnPropertyChanged(nameof(Street)); }
        }
        public string HouseNumber
        {
            get => _houseNumber;
            set { _houseNumber = value; OnPropertyChanged(nameof(HouseNumber)); }
        }
        public string City
        {
            get => _city;
            set { _city = value; OnPropertyChanged(nameof(City)); }
        }
        public int PostalCode
        {
            get => _postalCode;
            set { _postalCode = value; OnPropertyChanged(nameof(PostalCode)); }
        }
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
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
            //DoB = DateOnly.FromDateTime(DateTime.Now);

            actionLabelText = "Přidat osobu";
            actionButtonText = "Přidat";

            DepartmentVisibility = Visibility.Hidden;
            PositionVisibility = Visibility.Hidden;
        }

        // Konstruktor pro editaci osoby
        public AddOrEditUserViewModel(PersonDetails personDetails, EmployeeDetails? employee = null)
        {
            _personDetailsService = new PersonDetailsService();
            _rolesService = new RolesService();
            _userService = new UserService();
            
            id = _personDetailsService.GetPersonId(personDetails.NationalIdNumber);
            Name = personDetails.Name;
            Surname = personDetails.Surname;
            DoB = personDetails.DoB;

            NationalIdNumber = personDetails.NationalIdNumber;
            PhoneNumber = personDetails.PhoneNumber;
            Email = personDetails.Email;
            SelectedRole = personDetails.Role;

            var address = _personDetailsService.GetAddress(id);
            HouseNumber = address.HouseNumber;
            Street = address.Street;
            City = address.City;
            PostalCode = address.PostalCode;

            MessageBox.Show($"House number: {HouseNumber}, Street: {Street}, City: {City}, Postal code: {PostalCode}");

            var personType = _personDetailsService.GetTypeOfPerson(personDetails.NationalIdNumber);
            SelectedPersonType = personType == 'K' ? "Klient" : "Zaměstnanec";

            actionLabelText = "Upravit osobu";
            actionButtonText = "Upravit";
            DepartmentVisibility = Visibility.Hidden;
            PositionVisibility = Visibility.Hidden;

            if (employee != null)
            {
                Department = employee.Department;
                Position = employee.Position;
                actionLabelText = "Upravit osobu";
                actionButtonText = "Upravit";
                DepartmentVisibility = Visibility.Visible;
                PositionVisibility = Visibility.Visible;
            }
            

            Roles = new ObservableCollection<string>(_rolesService.GetRoles());
            AddNewPersonCommand = new RelayCommand(EditPerson);
            CancelAddingNewPersonCommand = new RelayCommand(CloseAddingWindow);

        }

        private string ConvertPersonTypeToDbFormat(string personType)
        {
            return personTypeMapping.TryGetValue(personType, out var dbFormat) ? dbFormat : throw new ArgumentException("Neplatný typ osoby");
        }

        private void EditPerson()
        {
            byte[]? salt = null;
            string passwordHash = null;
            string? saltAsString = null;
           
            if (!string.IsNullOrEmpty(Password))
            {
                salt = PasswordHasher.GenerateSalt();
                passwordHash = PasswordHasher.HashPassword(Password, salt);
                saltAsString = Convert.ToBase64String(salt);
            }

            if (ValidateInputs())
            {
                bool result = _userService.EditUserDetails(
                    id,
                    Name,
                    Surname,
                    DoB,
                    NationalIdNumber,
                    PhoneNumber,
                    Email,
                    ConvertPersonTypeToDbFormat(SelectedPersonType),
                    _rolesService.GetRoleId(SelectedRole),
                    Street,
                    HouseNumber,
                    City,
                    PostalCode,
                    passwordHash,
                    saltAsString,
                    Department,
                    Position
                );

                if (result)
                {
                    MessageBox.Show("Uživatel byl úspěšně aktualizován.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
                    CloseAddingWindow();
                }
                else
                {
                    MessageBox.Show("Aktualizace uživatele selhala.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void AddNewPerson()
        {
            try
            {
                // Kontrola, zda jsou všechny povinné údaje vyplněny
                if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Surname) || string.IsNullOrWhiteSpace(NationalIdNumber) ||
                    string.IsNullOrWhiteSpace(PhoneNumber) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(SelectedPersonType) ||
                    string.IsNullOrWhiteSpace(SelectedRole) || string.IsNullOrWhiteSpace(Street) || string.IsNullOrWhiteSpace(City) ||
                    PostalCode == 0 || string.IsNullOrWhiteSpace(Password))
                {
                    MessageBox.Show("Prosím, vyplňte všechny povinné údaje.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Kontrola, zda jsou všechny údaje ve správném formátu
                if (!ValidateInputs())
                {
                    return;
                }

                var salt = PasswordHasher.GenerateSalt();
                var hashedPassword = PasswordHasher.HashPassword(Password, salt);
                var saltBase64 = Convert.ToBase64String(salt);
                var houseNumber = Convert.ToString(HouseNumber);

                // Převod hodnoty z comboboxu na hodnotu pro databázi
                string personTypeForDb = ConvertPersonTypeToDbFormat(SelectedPersonType);

                _userService.RegisterNewUser(
                    Name,
                    Surname,
                    DoB,
                    NationalIdNumber,
                    PhoneNumber,
                    Email,
                    personTypeForDb,
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
            if (SelectedPersonType == "Zaměstnanec")
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


        private bool ValidateInputs()
        {
            // Validace rodného čísla (11 číslic)
            if (!Regex.IsMatch(NationalIdNumber, @"^\d{11}$"))
            {
                MessageBox.Show("Rodné číslo musí mít 11 číslic.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validace telefonního čísla (9 číslic)
            if (!Regex.IsMatch(PhoneNumber, @"^\d{9}$"))
            {
                MessageBox.Show("Telefonní číslo musí mít 9 číslic.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validace jména (začíná velkým písmenem)
            if (!Regex.IsMatch(Name, @"^[A-ZÁČĎÉĚÍŇÓŘŠŤÚŮÝŽ][a-záčďéěíňóřšťúůýž]*$"))
            {
                MessageBox.Show("Jméno musí začínat velkým písmenem.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validace příjmení (začíná velkým písmenem)
            if (!Regex.IsMatch(Surname, @"^[A-ZÁČĎÉĚÍŇÓŘŠŤÚŮÝŽ][a-záčďéěíňóřšťúůýž]*$"))
            {
                MessageBox.Show("Příjmení musí začínat velkým písmenem.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validace PSČ (5 číslic)
            if (!Regex.IsMatch(PostalCode.ToString(), @"^\d{5}$"))
            {
                MessageBox.Show("PSČ musí mít 5 číslic.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validace emailu (obsahuje @ a minimálně jednu tečku)
            if (!Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Email musí obsahovat @ a minimálně jednu tečku.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}