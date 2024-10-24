using InformacniSystemBanky.View;
using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.ViewModel
{
    public class LoginViewModel
    {
        private readonly Window _loginView;

        public ICommand OpenRegisterViewCommand { get; }
        public ICommand OpenDashboardCommand { get; }

        public LoginViewModel(Window loginView)
        {
            _loginView = loginView;
            OpenRegisterViewCommand = new RelayCommand(OpenRegisterView);
            OpenDashboardCommand = new RelayCommand(OpenDashboard);
        }

        private void OpenDashboard()
        {
            var dashboardView = new DashboardAdminView();
            dashboardView.Show();
            _loginView.Close();
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
    }
}
