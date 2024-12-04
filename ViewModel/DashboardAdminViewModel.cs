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
        public ICommand ShowLoansCommand { get; }
        public ICommand ShowBankingLicencesCommand { get; }
        public ICommand LogoClickedCommand { get; }
        public ICommand ShowAllAccountCommand { get; }
        public ICommand LogOutCommand { get; }
        public ICommand ShowAllCardsCommand { get; }
        public ICommand ShowAllPeople { get; }
        public ICommand ShowFilesCommand { get; }
        public ICommand ShowPasswordsCommand { get; }
        public ICommand ShowAddressesCommand { get; }

        public DashboardAdminViewModel()
        {
            ShowAllAccountCommand = new RelayCommand(ShowAccounts);
            ShowLoansCommand = new RelayCommand(ShowLoans);
            ShowBankingLicencesCommand = new RelayCommand(ShowLicences);
            LogoClickedCommand = new RelayCommand(ShowHome);
            LogOutCommand = new RelayCommand(LogOut);
            ShowAllCardsCommand = new RelayCommand(ShowCards);
            ShowAllPeople = new RelayCommand(ShowPeople);
            ShowFilesCommand = new RelayCommand(ShowFiles);
            ShowPasswordsCommand = new RelayCommand(ShowPasswords);
            ShowAddressesCommand = new RelayCommand(ShowAddresses);
        }

        private void ShowAddresses()
        {
           CurrentViewModel = new AddressesView();
        }

        private void ShowPasswords()
        {
            CurrentViewModel = new PasswordsView();
        }

        private void ShowFiles()
        {
            CurrentViewModel = new FilesView();
        }

        private void ShowPeople()
        {
            CurrentViewModel = new UsersView();
        }

        private void ShowAccounts()
        {
            CurrentViewModel = new AccountsView();
        }

        private void ShowHome()
        {
            CurrentViewModel = null;
        }

        private void ShowLoans()
        {
            CurrentViewModel = new LoansView();
        }

        private void ShowLicences()
        {
            CurrentViewModel = new BankingLicencesView();
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
