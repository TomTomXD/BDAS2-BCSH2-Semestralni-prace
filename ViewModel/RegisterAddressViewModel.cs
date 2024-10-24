using InformacniSystemBanky.View;
using System.ComponentModel;
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
            GoNextCommand = new RelayCommand(GoNext, CanExecuteGoNext);
        }

        public string AddressStreet
        {
            get => _userRegistrationViewModel.AddressStreet;
            set
            {
                _userRegistrationViewModel.AddressStreet = value;
                OnPropertyChanged(nameof(AddressStreet));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string AddressHouseNumber
        {
            get => _userRegistrationViewModel.AddressHouseNumber;
            set
            {
                _userRegistrationViewModel.AddressHouseNumber = value;
                OnPropertyChanged(nameof(AddressHouseNumber));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string AddressCity
        {
            get => _userRegistrationViewModel.AddressCity;
            set
            {
                _userRegistrationViewModel.AddressCity = value;
                OnPropertyChanged(nameof(AddressCity));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public string AddressZipCode
        {
            get => _userRegistrationViewModel.AddressZipCode;
            set
            {
                _userRegistrationViewModel.AddressZipCode = value;
                OnPropertyChanged(nameof(AddressZipCode));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private bool CanExecuteGoNext()
        {
            return !string.IsNullOrWhiteSpace(AddressStreet) &&
                   !string.IsNullOrWhiteSpace(AddressHouseNumber) &&
                   !string.IsNullOrWhiteSpace(AddressCity) &&
                   !string.IsNullOrWhiteSpace(AddressZipCode);
        }

        private void GoBack()
        {
            var personalDetailsView = new RegisterPersonalDetailsView(_userRegistrationViewModel);
            personalDetailsView.Show();
            _registerAddressView.Close();
        }

        private void GoNext()
        {
            var passwordView = new RegisterCreatePasswordView(_userRegistrationViewModel);
            passwordView.Show();
            _registerAddressView.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
