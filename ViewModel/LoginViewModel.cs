using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.View;
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
        public ICommand LoginCommand { get; }

        public LoginViewModel(Window loginView)
        {
            _loginView = loginView;
            _userService = new UserService();
            OpenRegisterViewCommand = new RelayCommand(OpenRegisterView);
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            if (_userService.LoginUser(Email, Heslo))
            {
                // Přihlášení úspěšné, otevřete zatím admin dashboard pro testování a ladění loginů
                switch (Session.Instance.LoggedInRoleId)
                {
                    case 1:
                        var dashboardKlientView = new DashboardKlientView();
                        dashboardKlientView.Show();
                        break;
                    case 3:
                        var dashboardEmployeeView = new DashboardEmployeeView();
                        dashboardEmployeeView.Show();
                        break;
                    case 4:
                        var dashboardAdminView = new DashboardAdminView();
                        dashboardAdminView.Show();
                        break;
                    default:
                        MessageBox.Show("Nepodporovaná role uživatele.", "Chyba přihlášení", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
                               
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

            // Vytvoření instance nového okna s ViewModel
            var registerView = new RegisterPersonalDetailsView(new UserRegistrationViewModel());

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