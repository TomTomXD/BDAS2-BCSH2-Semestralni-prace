// ViewModel/UserRegistrationViewModel.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace InformacniSystemBanky.ViewModel
{
    public class UserRegistrationViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _addressStreet;
        private string _addressHouseNumber;
        private string _addressCity;
        private string _addressZipCode;
        private string _rodneCislo;
        private string _phoneNumber;
        private string _email;
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _passwordAgain;
        public string PasswordAgain
        {
            get => _passwordAgain;
            set
            {
                _passwordAgain = value;
                OnPropertyChanged();
            }
        }


        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string AddressStreet
        {
            get => _addressStreet;
            set
            {
                _addressStreet = value;
                OnPropertyChanged();
            }
        }

        public string AddressHouseNumber
        {
            get => _addressHouseNumber;
            set
            {
                _addressHouseNumber = value;
                OnPropertyChanged();
            }
        }

        public string AddressCity
        {
            get => _addressCity;
            set
            {
                _addressCity = value;
                OnPropertyChanged();
            }
        }

        public string AddressZipCode
        {
            get => _addressZipCode;
            set
            {
                _addressZipCode = value;
                OnPropertyChanged();
            }
        }

        public string RodneCislo
        {
            get => _rodneCislo;
            set
            {
                _rodneCislo = value;
                OnPropertyChanged();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public ICommand GoBackCommand { get; set; }
        public ICommand GoNextCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
