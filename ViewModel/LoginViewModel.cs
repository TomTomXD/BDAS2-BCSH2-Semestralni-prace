using FinancniInformacniSystemBanky.DatabaseLayer;
using InformacniSystemBanky.View;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly Window _loginView;
        private string _email;
        private string _heslo;
        private readonly UserService _userService;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string Heslo
        {
            get => _heslo;
            set
            {
                _heslo = value;
                OnPropertyChanged(nameof(Heslo));
            }
        }

        public ICommand OpenRegisterViewCommand { get; }
        public ICommand OpenDashboardCommand { get; }

        public LoginViewModel(Window loginView)
        {
            _loginView = loginView;
            _userService = new UserService();
            OpenRegisterViewCommand = new RelayCommand(OpenRegisterView);
            OpenDashboardCommand = new RelayCommand(OpenDashboard);
        }

        private void OpenDashboard()
        {
            if (_userService.LoginUser(Email, Heslo))
            {
                // Přihlášení úspěšné, otevřete zatím admin dashboard pro testování a ladění loginů
                var dashboardView = new DashboardAdminView();
                dashboardView.Show();
                _loginView.Close();
            }
            else
            {
                // Přihlášení neúspěšné, zobrazte chybovou zprávu
                MessageBox.Show("Neplatný email nebo heslo.", "Chyba přihlášení", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenRegisterView()
        {
            // Vytvoření instance UserRegistrationViewModel
            var userRegistrationViewModel = new UserRegistrationViewModel();

            // Vytvoření instance nového okna s ViewModel
            var registerView = new RegisterPersonalDetailsView(userRegistrationViewModel);

            // Zobrazení nového okna
            registerView.Show();

            // Zavření původního okna
            _loginView.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}