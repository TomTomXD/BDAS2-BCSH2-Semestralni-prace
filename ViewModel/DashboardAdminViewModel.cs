using FinancniInformacniSystemBanky.View;
using InformacniSystemBanky.View;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.ViewModel
{
    public class DashboardAdminViewModel : INotifyPropertyChanged
    {
        private object _currentViewModel;

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public ICommand ShowHomeCommand { get; }
        public ICommand ShowSettingsCommand { get; }
        public ICommand ShowProfileCommand { get; }
        public ICommand LogoClickedCommand { get; }
        public ICommand ShowAllAccountCommand { get; }
        public ICommand LogOutCommand { get; }
        public ICommand ShowAllCardsCommand { get; }
        public ICommand ShowAllPeople { get; }

        public DashboardAdminViewModel()
        {
            ShowAllAccountCommand = new RelayCommand(ShowAccounts);
            ShowSettingsCommand = new RelayCommand(ShowSettings);
            ShowProfileCommand = new RelayCommand(ShowProfile);
            LogoClickedCommand = new RelayCommand(ShowHome);
            LogOutCommand = new RelayCommand(LogOut);
            ShowAllCardsCommand = new RelayCommand(ShowCards);
            ShowAllPeople = new RelayCommand(ShowPeople);

        }

        private void ShowPeople()
        {
            CurrentViewModel = new UsersView();
        }

        private void ShowAccounts()
        {
            CurrentViewModel = new AccountsView();
            MessageBox.Show("Accounts clicked");
        }

        private void ShowHome()
        {
            MessageBox.Show("Logo clicked");
        }

        private void ShowSettings()
        {
            CurrentViewModel = new HomeView();
            MessageBox.Show("Settings");
        }

        private void ShowProfile()
        {
            //CurrentViewModel = new ProfileViewModel();
            MessageBox.Show("Profile");
        }

        private void LogOut()
        {
            // Otevření nového přihlašovacího okna (LoginView)
            var loginView = new LoginView();
            loginView.Show();

            // Zavření aktuálního okna (DashboardAdminView)
            var currentWindow = Application.Current.Windows
                .OfType<DashboardAdminView>()
                .FirstOrDefault();

            if (currentWindow != null)
            {
                currentWindow.Close();
            }

        }

        private void ShowCards()
        {
            CurrentViewModel = new CardsListView();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
