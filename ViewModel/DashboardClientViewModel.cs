using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.View;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.ViewModel
{
    public class DashboardClientViewModel : INotifyPropertyChanged
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
        //public ICommand ShowSettingsCommand { get; }
        public ICommand ShowProfileCommand { get; }
        public ICommand LogoClickedCommand { get; }
        public ICommand ShowAccountsCommand { get; }
        public ICommand LogOutCommand { get; }

        public DashboardClientViewModel()
        {
            ShowHomeCommand = new RelayCommand(ShowHome);
            //ShowSettingsCommand = new RelayCommand(ShowSettings);
            ShowProfileCommand = new RelayCommand(ShowProfile);
            LogoClickedCommand = new RelayCommand(ShowHome);
            ShowAccountsCommand = new RelayCommand(ShowAccounts);
            LogOutCommand = new RelayCommand(LogOut);

            // Výchozí pohled
            //CurrentViewModel = new HomeViewModel(); // Změněno na HomeViewModel
        }

        private void LogOut()
        {
            // Otevření nového přihlašovacího okna (LoginView)
            var loginView = new LoginView();
            loginView.Show();

            // Zavření aktuálního okna (DashboardView)
            var currentWindow = Application.Current.Windows
                .OfType<DashboardKlientView>()
                .FirstOrDefault();

            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }

        private void ShowHome()
        {
            //CurrentViewModel = new HomeViewModel();
            //CurrentViewModel = new DashboardKlientView();
            //MessageBox.Show("Logo clicked");
        }

        //private void ShowSettings()
        //{
        //    CurrentViewModel = new HomeView();
        //    MessageBox.Show("Settings");
        //}

        private void ShowProfile()
        {
            //CurrentViewModel = new ProfileViewModel();
            MessageBox.Show("Profile");
        }

        private void ShowAccounts()
        {
            CurrentViewModel = new AccountsView()
            {
                DataContext = new AccountsViewModel(Session.Instance.CurrentUserId)
            };
            MessageBox.Show("Accounts clicked");
        }

        // INotifyPropertyChanged implementace
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
