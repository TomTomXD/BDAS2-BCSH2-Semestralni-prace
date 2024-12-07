using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.Model.Helpers;
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

        public ICommand AddNewPersonCommand { get; }
        public ICommand CancelAddingNewPersonCommand { get; }
        public ICommand EditPersonCommand { get; }
        public event Action PersonAdded;
        public List<string> PersonTypes { get; } = new List<string> { "Klient", "Zaměstnanec" };

        private string selectedPersonType;
        private string actionLabelText;
        private string actionButtonText;
        private int id;
        private string _name;
        private string _surname;
        private DateTime _doB;
        private string _nationalIdNumber;
        private string _phoneNumber;
        private string _email;
        private string _street;
        private string _houseNumber;
        private string _city;
        private string _postalCode;
        private string _password;
        private Role _selectedRole;
        private Department _selectedDepartment;
        private Position _selectedPosition;
        private Employee _selectedManager;

        private Visibility _departmentVisibility;
        private Visibility _positionVisibility;
        private Visibility _managerVisibility;

        private readonly RolesService _rolesService;
        private readonly PersonService _personDetailsService;
        private readonly UserService _userService;
        private readonly LookupTablesService _lookupTables;
        private readonly EmployeesService _employeesService;

        public ObservableCollection<Role> Roles { get; set; }
        public ObservableCollection<Employee> Managers { get; set; }
        public ObservableCollection<Department> Departments { get; set; }
        public ObservableCollection<Position> Positions { get; set; }


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
        public string PostalCode
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
        public Role SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }
        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                _selectedDepartment = value;
                OnPropertyChanged(nameof(SelectedDepartment));
            }
        }
        public Position SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                _selectedPosition = value;
                OnPropertyChanged(nameof(SelectedPosition));
            }
        }
        public Employee SelectedManager
        {
            get => _selectedManager;
            set
            {
                _selectedManager = value;
                OnPropertyChanged(nameof(SelectedManager));
            }
        }
        public Visibility DepartmentVisibility
        {
            get => _departmentVisibility;
            set
            {
                _departmentVisibility = value;
                OnPropertyChanged(nameof(DepartmentVisibility));
            }
        }
        public Visibility PositionVisibility
        {
            get => _positionVisibility;
            set
            {
                _positionVisibility = value;
                OnPropertyChanged(nameof(PositionVisibility));
            }
        }
        public Visibility ManagerVisibility
        {
            get => _managerVisibility;
            set
            {
                _managerVisibility = value;
                OnPropertyChanged(nameof(ManagerVisibility));
            }
        }

        // Bezparametrický konstruktor pro přidání nové osoby
        public AddOrEditUserViewModel()
        {
            _personDetailsService = new PersonService();
            _rolesService = new RolesService();
            _userService = new UserService();
            _lookupTables = new LookupTablesService();
            _employeesService = new EmployeesService();

            AddNewPersonCommand = new RelayCommand(AddNewPerson);
            CancelAddingNewPersonCommand = new RelayCommand(CloseAddingWindow);

            // Nastavení defaultních hodnot pro dnešní datum v datepickeru
            DoB = DateTime.Now;

            // Získání hodnot pro comboboxy
            Roles = new ObservableCollection<Role>(_lookupTables.GetLookupTableData<Role>("ROLE"));
            Departments = new ObservableCollection<Department>(_lookupTables.GetLookupTableData<Department>("ODDELENI"));
            Positions = new ObservableCollection<Position>(_lookupTables.GetLookupTableData<Position>("POZICE"));
            Managers = new ObservableCollection<Employee>(_employeesService.GetPossibleManagers());

            actionLabelText = "Přidat osobu";
            actionButtonText = "Přidat";

            DepartmentVisibility = Visibility.Hidden;
            PositionVisibility = Visibility.Hidden;
            ManagerVisibility = Visibility.Hidden;
        }

        // Konstruktor pro editaci osoby
        public AddOrEditUserViewModel(Person selectedPerson)
        {
            _personDetailsService = new PersonService();
            _rolesService = new RolesService();
            _userService = new UserService();
            _lookupTables = new LookupTablesService();
            _employeesService = new EmployeesService();

            // Získání hodnot pro comboboxy
            Roles = new ObservableCollection<Role>(_lookupTables.GetLookupTableData<Role>("ROLE"));
            Departments = new ObservableCollection<Department>(_lookupTables.GetLookupTableData<Department>("ODDELENI"));
            Positions = new ObservableCollection<Position>(_lookupTables.GetLookupTableData<Position>("POZICE"));
            Managers = new ObservableCollection<Employee>(_employeesService.GetPossibleManagers());

            AddNewPersonCommand = new RelayCommand(EditPerson);
            CancelAddingNewPersonCommand = new RelayCommand(CloseAddingWindow);

            actionLabelText = "Upravit osobu";
            actionButtonText = "Upravit";

            // načtení hodnot vybrané osoby do formuláře
            id = selectedPerson.Id;
            Name = selectedPerson.Name;
            Surname = selectedPerson.Surname;
            DoB = selectedPerson.DoB;
            NationalIdNumber = selectedPerson.NationalIdNumber;
            PhoneNumber = selectedPerson.PhoneNumber;
            Email = selectedPerson.Email;
            Street = selectedPerson.Address.Street;
            HouseNumber = selectedPerson.Address.HouseNumber;
            City = selectedPerson.Address.City;
            PostalCode = selectedPerson.Address.PostalCode;
            SelectedRole = Roles.First(r => r.Id == selectedPerson.Role.Id);

            SelectedPersonType = selectedPerson.PersonType == "K" ? "Klient" : "Zaměstnanec";
            if(SelectedPersonType == "Klient")
            {
                DepartmentVisibility = Visibility.Hidden;
                PositionVisibility = Visibility.Hidden;
                ManagerVisibility = Visibility.Hidden;
            }
            else
            {
                DepartmentVisibility = Visibility.Visible;
                PositionVisibility = Visibility.Visible;
                ManagerVisibility = Visibility.Visible;
                SelectedDepartment = Departments.First(d => d.Id == selectedPerson.EmployeeDetails.Department.Id);
                SelectedPosition = Positions.First(p => p.Id == selectedPerson.EmployeeDetails.Position.Id);
                SelectedManager = Managers.FirstOrDefault(m => m.Id == selectedPerson.EmployeeDetails.Manager?.Id);
            }
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
                    SelectedRole.Id,
                    Street,
                    HouseNumber,
                    City,
                    PostalCode,
                    passwordHash,
                    saltAsString,
                    SelectedDepartment.Id,
                    SelectedPosition.Id,
                    SelectedManager.Id
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
                    string.IsNullOrWhiteSpace(SelectedRole.Name) || string.IsNullOrWhiteSpace(Street) || string.IsNullOrWhiteSpace(City))
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

                MessageBox.Show($"id oddeleni: {SelectedDepartment.Id}, id pozice: {SelectedPosition.Id}, manazer: {SelectedManager.Id}");

                _userService.RegisterNewUser(
                        Name,
                        Surname,
                        DoB,
                        NationalIdNumber,
                        PhoneNumber,
                        Email,
                        personTypeForDb,
                        SelectedRole.Id,
                        Street,
                        houseNumber,
                        City,
                        PostalCode,
                        hashedPassword,
                        saltBase64,
                        SelectedDepartment != null ? SelectedDepartment.Id : (int?)null, // Ošetření null
                        SelectedPosition != null ? SelectedPosition.Id : (int?)null,   // Ošetření null
                        SelectedManager != null ? SelectedManager.Id : (int?)null      // Ošetření null
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
                ManagerVisibility = Visibility.Visible;
            }
            else
            {
                DepartmentVisibility = Visibility.Hidden;
                PositionVisibility = Visibility.Hidden;
                ManagerVisibility = Visibility.Hidden;
            }
        }


        private bool ValidateInputs()
        {
            // Validace rodného čísla (ve tvaru xxxxxx/xxxx)
            if (!Regex.IsMatch(NationalIdNumber, @"^\d{6}/\d{4}$"))
            {
                MessageBox.Show("Rodné číslo musí mít tvar xxxxxx/xxxx (6 číslic, lomítko, 4 číslice).", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
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