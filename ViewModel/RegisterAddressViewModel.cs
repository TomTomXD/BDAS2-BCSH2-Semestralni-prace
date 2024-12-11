using InformacniSystemBanky.View;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.ViewModel
{
    public class RegisterAddressViewModel : INotifyPropertyChanged
    {
        private readonly Window _registerAddressView;
        private readonly UserRegistrationViewModel _userRegistrationViewModel;

        public ICommand GoBackCommand { get; }
        public ICommand GoNextCommand { get; }

        public RegisterAddressViewModel(Window registerAddressView, UserRegistrationViewModel userRegistrationViewModel)
        {
            _registerAddressView = registerAddressView;
            _userRegistrationViewModel = userRegistrationViewModel;
            GoBackCommand = new RelayCommand(GoBack);
            GoNextCommand = new RelayCommand(GoNext);
        }

        public string AddressStreet
        {
            get => _userRegistrationViewModel.AddressStreet;
            set
            {
                _userRegistrationViewModel.AddressStreet = value;
                OnPropertyChanged(nameof(AddressStreet));
            }
        }

        public string AddressHouseNumber
        {
            get => _userRegistrationViewModel.AddressHouseNumber;
            set
            {
                _userRegistrationViewModel.AddressHouseNumber = value;
                OnPropertyChanged(nameof(AddressHouseNumber));
            }
        }

        public string AddressCity
        {
            get => _userRegistrationViewModel.AddressCity;
            set
            {
                _userRegistrationViewModel.AddressCity = value;
                OnPropertyChanged(nameof(AddressCity));
            }
        }

        public string AddressZipCode
        {
            get => _userRegistrationViewModel.AddressZipCode;
            set
            {
                _userRegistrationViewModel.AddressZipCode = value;
                OnPropertyChanged(nameof(AddressZipCode));
            }
        }

        private void GoBack()
        {
            var personalDetailsView = new RegisterPersonalDetailsView(_userRegistrationViewModel);
            personalDetailsView.Show();
            _registerAddressView.Close();
        }

        private void GoNext()
        {
            // Validace vstupů
            ValidateInputs(); // Zde zavoláme validaci, která nyní pouze zobrazuje chyby pro každý vstup zvlášť

            // Pokud je všechno v pořádku, pokračujeme
            if (!string.IsNullOrWhiteSpace(AddressStreet) &&
                !string.IsNullOrWhiteSpace(AddressHouseNumber) &&
                !string.IsNullOrWhiteSpace(AddressCity) &&
                !string.IsNullOrWhiteSpace(AddressZipCode) &&
                Regex.IsMatch(AddressHouseNumber, @"^\d+$") &&
                Regex.IsMatch(AddressZipCode, @"^\d{5}$") &&
                char.IsUpper(AddressStreet[0]) &&
                char.IsUpper(AddressCity[0]))
            {
                var passwordView = new RegisterCreatePasswordView(_userRegistrationViewModel);
                passwordView.Show();
                _registerAddressView.Close();
            }
        }

       // Kontrola, zda jsou povinné hodnoty vyplněny a platné
        private bool ValidateInputs()
        {
            if(string.IsNullOrWhiteSpace(AddressStreet) ||
                string.IsNullOrWhiteSpace(AddressHouseNumber) ||
                string.IsNullOrWhiteSpace(AddressCity) ||
                string.IsNullOrWhiteSpace(AddressZipCode))
            {
                MessageBox.Show("Všechny hodnoty musí být vyplněny.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(AddressStreet) || !char.IsUpper(AddressStreet[0]))
            {
                MessageBox.Show("Ulice musí být vyplněna a začínat velkým písmenem.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(AddressHouseNumber) || !Regex.IsMatch(AddressHouseNumber, @"^\d+$"))
            {
                MessageBox.Show("Číslo domu musí být vyplněno a obsahovat pouze čísla.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false; 
            }

            if (string.IsNullOrWhiteSpace(AddressCity) || !char.IsUpper(AddressCity[0]))
            {
                MessageBox.Show("Město musí být vyplněno a začínat velkým písmenem.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false; 
            }

            if (string.IsNullOrWhiteSpace(AddressZipCode) || !Regex.IsMatch(AddressZipCode, @"^\d{5}$"))
            {
                MessageBox.Show("Poštovní směrovací číslo musí být vyplněno a mít tvar 5 číslic.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false; 
            }
            return true; // Pokud všechno prošlo, vrátíme true
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}