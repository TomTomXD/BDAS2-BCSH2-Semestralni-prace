using InformacniSystemBanky.Model;
using System.ComponentModel;

namespace FinancniInformacniSystemBanky.Model
{
    public class Person : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private string _surname;
        private DateTime _doB;
        private string _nationalIdNumber;
        private string _phoneNumber;
        private string _email;
        private string _personType;
        private Role _role;
        private Address _address;
        private EmployeeDetails? _employeeDetails;

        public int Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(nameof(Surname));
            }
        }
        public DateTime DoB
        {
            get => _doB;
            set
            {
                _doB = value;
                OnPropertyChanged(nameof(DoB));
                OnPropertyChanged(nameof(FormattedDoB));
            }
        }
        public string NationalIdNumber
        {
            get => _nationalIdNumber;
            set
            {
                _nationalIdNumber = value;
                OnPropertyChanged(nameof(NationalIdNumber));
            }
        }
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }
        public string PersonType
        {
            get => _personType;
            set
            {
                _personType = value;
                OnPropertyChanged(nameof(PersonType));
            }
        }
        public Role Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
            }
        }
        public Address Address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }
        public EmployeeDetails? EmployeeDetails
        {
            get => _employeeDetails;
            set
            {
                _employeeDetails = value;
                OnPropertyChanged(nameof(EmployeeDetails));
            }
        }

        public string FormattedDoB => DoB.ToString("dd.MM. yyyy");

        public string AccountInfo => $"{Name} {Surname} - {Role.Name}";

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

