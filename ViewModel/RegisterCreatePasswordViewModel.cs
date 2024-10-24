using InformacniSystemBanky.View;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.ViewModel
{
    public class RegisterCreatePasswordViewModel : INotifyPropertyChanged
    {
        private string _password;
        private readonly Window _registrationCreatePassword;
        private readonly UserRegistrationViewModel _userRegistrationViewModel;
        public ICommand GoNextCommand { get; }
        public ICommand GoBackCommand { get; }

        public RegisterCreatePasswordViewModel(Window registerCreatePassword, UserRegistrationViewModel userRegistrationViewModel)
        {
            _registrationCreatePassword = registerCreatePassword;
            _userRegistrationViewModel = userRegistrationViewModel;
            GoNextCommand = new RelayCommand(GoNext);
            GoBackCommand = new RelayCommand(GoBack);
        }

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
                CommandManager.InvalidateRequerySuggested();
            }
        }

        private void GoNext()
        {
            var dashboardView = new DashboardKlientView();
            dashboardView.Show();
            _registrationCreatePassword.Close();
        }

        private void GoBack()
        {
            var registerAddressView = new RegisterAddressView(_userRegistrationViewModel);
            registerAddressView.Show();
            _registrationCreatePassword.Close();

         }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
