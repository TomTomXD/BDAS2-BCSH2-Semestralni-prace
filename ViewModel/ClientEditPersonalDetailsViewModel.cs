using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using System.Windows.Input;
using System.Windows;
using System.ComponentModel;
using System.Text.RegularExpressions;
using FinancniInformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using InformacniSystemBanky.Model;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class ClientEditPersonalDetailsViewModel : INotifyPropertyChanged
    {
        private readonly PersonService _personService;
        private readonly UserService _userService;
        public ICommand CancelEditPersonalDetailsCommand { get; }
        public ICommand ChangePersonalDetailsCommand { get; }

        private int _id;
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
        private string _personType;
        private int _roleId;
        private Person _editedPerson;

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

        public ClientEditPersonalDetailsViewModel()
        {
            _personService = new PersonService();
            _userService = new UserService();

            _editedPerson = LoadPersonData();

            ChangePersonalDetailsCommand = new RelayCommand(EditDetails);
            CancelEditPersonalDetailsCommand = new RelayCommand(CloseEditWindow);

            _id = _editedPerson.Id;
            Name = _editedPerson.Name;
            Surname = _editedPerson.Surname;
            DoB = _editedPerson.DoB;
            NationalIdNumber = _editedPerson.NationalIdNumber;
            PhoneNumber = _editedPerson.PhoneNumber;
            Email = _editedPerson.Email;
            Street = _editedPerson.Address.Street;
            HouseNumber = _editedPerson.Address.HouseNumber;
            City = _editedPerson.Address.City;
            PostalCode = _editedPerson.Address.PostalCode;
            _roleId = _editedPerson.Role.Id;
            _personType = _editedPerson.PersonType;
        }

        private Person LoadPersonData()
        {
            // jednodušší než dělat samostatný select s podmínkou,
            // můžeme si dovolit, protože v databázi je jen velice málo záznamů -> jinak bychom měli použít vlastní sql dotaz
            Person person = _personService.GetPersonDetails().FirstOrDefault(p => p.Id == Session.Instance.CurrentUserId);

            return person;
        }

        private void EditDetails()
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

            _userService.EditUserDetails(_id, Name, Surname, DoB, NationalIdNumber, PhoneNumber, Email, _personType,_roleId, Street, HouseNumber, City, PostalCode, passwordHash, saltAsString);
            CloseEditWindow();
        }

        private void CloseEditWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<ClientEditPersonalDetailsView>().FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Close();
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
