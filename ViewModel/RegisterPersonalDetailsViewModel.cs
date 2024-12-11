using InformacniSystemBanky.View;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.ViewModel
{
    public class RegisterPersonalDetailsViewModel : INotifyPropertyChanged
    {
        private readonly Window _registerPersonalDetailsView;
        private readonly UserRegistrationViewModel _userRegistrationViewModel;

        public ICommand OpenRegisterAddressViewCommand { get; }

        public RegisterPersonalDetailsViewModel(Window registerPersonalDetailsView, UserRegistrationViewModel userRegistrationViewModel)
        {
            _registerPersonalDetailsView = registerPersonalDetailsView;
            _userRegistrationViewModel = userRegistrationViewModel;
            OpenRegisterAddressViewCommand = new RelayCommand(OpenRegisterAddressView);
            DateOfBirth = DateTime.Now; // nastavení defaultní hodnoty pro datum narození na dnešní datum
        }

        public string FirstName
        {
            get => _userRegistrationViewModel.FirstName;
            set
            {
                _userRegistrationViewModel.FirstName = value;
                OnPropertyChanged(nameof(FirstName));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string LastName
        {
            get => _userRegistrationViewModel.LastName;
            set
            {
                _userRegistrationViewModel.LastName = value;
                OnPropertyChanged(nameof(LastName));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string RodneCislo
        {
            get => _userRegistrationViewModel.RodneCislo;
            set
            {
                _userRegistrationViewModel.RodneCislo = value;
                OnPropertyChanged(nameof(RodneCislo));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public DateTime? DateOfBirth
        {
            get => _userRegistrationViewModel.DateOfBirth;
            set
            {
                if (value.HasValue)
                {
                    _userRegistrationViewModel.DateOfBirth = value.Value;
                }
                OnPropertyChanged(nameof(DateOfBirth));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string PhoneNumber
        {
            get => _userRegistrationViewModel.PhoneNumber;
            set
            {
                _userRegistrationViewModel.PhoneNumber = value;
                OnPropertyChanged(nameof(PhoneNumber));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string Email
        {
            get => _userRegistrationViewModel.Email;
            set
            {
                _userRegistrationViewModel.Email = value;
                OnPropertyChanged(nameof(Email));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private bool ValidateInputs()
        {
            // Kontrola, zda jsou všechny povinné hodnoty vyplněny
            if (string.IsNullOrWhiteSpace(FirstName) ||
                string.IsNullOrWhiteSpace(LastName) ||
                string.IsNullOrWhiteSpace(RodneCislo) ||
                !DateOfBirth.HasValue ||
                string.IsNullOrWhiteSpace(PhoneNumber) ||
                string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Všechny povinné hodnoty musí být vyplněny.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validace rodného čísla (ve tvaru xxxxxx/xxxx)
            if (!Regex.IsMatch(RodneCislo, @"^\d{6}/\d{4}$"))
            {
                MessageBox.Show("Rodné číslo musí mít tvar xxxxxx/xxxx (6 číslic, lomítko, 4 číslice).", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validace jména (začíná velkým písmenem)
            if (!Regex.IsMatch(FirstName, @"^[A-ZÁČĎÉĚÍŇÓŘŠŤÚŮÝŽ][a-záčďéěíňóřšťúůýž]*$"))
            {
                MessageBox.Show("Jméno musí začínat velkým písmenem.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validace příjmení (začíná velkým písmenem)
            if (!Regex.IsMatch(LastName, @"^[A-ZÁČĎÉĚÍŇÓŘŠŤÚŮÝŽ][a-záčďéěíňóřšťúůýž]*$"))
            {
                MessageBox.Show("Příjmení musí začínat velkým písmenem.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validace telefonního čísla (9 číslic)
            if (!Regex.IsMatch(PhoneNumber, @"^\d{9}$"))
            {
                MessageBox.Show("Telefonní číslo musí mít 9 číslic.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void OpenRegisterAddressView()
        {
            // Pokud jsou vstupy platné, otevřete okno
            if (!ValidateInputs())
            {
                return;
            }

            var registerAddressView = new RegisterAddressView(_userRegistrationViewModel);
            registerAddressView.Show();
            _registerPersonalDetailsView.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}