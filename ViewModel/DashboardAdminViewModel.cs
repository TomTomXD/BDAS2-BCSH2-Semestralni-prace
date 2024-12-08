using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.View;
using InformacniSystemBanky.View;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.ViewModel
{
    public class DashboardAdminViewModel : INotifyPropertyChanged
    {
        private object _currentView;


        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
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
        public ICommand ShowDataDictionaryCommand { get; }
        public ICommand ShowStandingOrdersCommand { get; }
        public ICommand ShowLookUpTablesCommand { get; }
        public ICommand ShowHierarchyCommand { get; }
        public ICommand ShowEmulationWindowCommand { get; }

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
            ShowDataDictionaryCommand = new RelayCommand(ShowDataDictionary);
            ShowStandingOrdersCommand = new RelayCommand(ShowStandingOrders);
            ShowLookUpTablesCommand = new RelayCommand(ShowLookUpTables);
            ShowHierarchyCommand = new RelayCommand(DisplayHierarchyTree);
            ShowEmulationWindowCommand = new RelayCommand(ShowEmulationWindow);

            CurrentView = new AdminMainView();
        }

        private void ShowEmulationWindow()
        {
             var emulationView = new EmulationView();
            emulationView.ShowDialog();
        }

        private void DisplayHierarchyTree()
        {
            CurrentView = new EmployeeHierarchyView();
        }

        private void ShowLookUpTables()
        {
            CurrentView = new LookupTablesView();
        }

        private void ShowStandingOrders()
        {
            CurrentView = new StandingOrdersView();
        }

        private void ShowDataDictionary()
        {
            CurrentView = new SystemCatalogView();
        }

        private void ShowAddresses()
        {
            CurrentView = new AddressesView();
        }

        private void ShowPasswords()
        {
            CurrentView = new PasswordsView();
        }

        private void ShowFiles()
        {
            CurrentView = new FilesView();
        }

        private void ShowPeople()
        {
            CurrentView = new UsersView();
        }

        private void ShowAccounts()
        {
            CurrentView = new AccountsView();
        }

        private void ShowHome()
        {
            CurrentView = new AdminMainView();
        }

        private void ShowLoans()
        {
            CurrentView = new LoansView();
        }

        private void ShowLicences()
        {
            CurrentView = new BankingLicencesView();
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
            CurrentView = new CardsView();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
