using InformacniSystemBanky.View;
using System;
using System.ComponentModel;
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
            OpenRegisterAddressViewCommand = new RelayCommand(OpenRegisterAddressView, CanExecuteOpenRegisterAddressView);
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

        private bool CanExecuteOpenRegisterAddressView()
        {
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   //!string.IsNullOrWhiteSpace(RodneCislo) &&
                   !string.IsNullOrWhiteSpace(PhoneNumber) &&
                   !string.IsNullOrWhiteSpace(Email);
        }

        private void OpenRegisterAddressView()
        {
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
