using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.View;
using FinancniInformacniSystemBanky.ViewModel;
using InformacniSystemBanky.View;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace InformacniSystemBanky.ViewModel
{
    public class DashboardClientViewModel : INotifyPropertyChanged
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

        private Visibility _emulationLabelVisibility = Visibility.Hidden; // Defaultně nastavíme na Hidden

        public Visibility EmulationLabelVisibility
        {
            get { return _emulationLabelVisibility; }
            set
            {
                _emulationLabelVisibility = value;
                OnPropertyChanged(nameof(EmulationLabelVisibility));
            }
        }

        //public ICommand ShowHomeCommand { get; }
        public ICommand ShowAccountCommand { get; }
        public ICommand LogOutCommand { get; }
        public ICommand ShowCardsCommand { get; }
        public ICommand ShowStandingOrdersCommand { get; }
        public ICommand ShowFilesCommand { get; }
        public ICommand ShowLoansCommand { get; }
        public ICommand ShowApplyForLoanCommand { get; }

        public DashboardClientViewModel()
        {
            ShowAccountCommand = new RelayCommand(ShowAccounts);
            LogOutCommand = new RelayCommand(LogOut);
            ShowCardsCommand = new RelayCommand(ShowCards);
            ShowStandingOrdersCommand = new RelayCommand(ShowStandingOrders);
            ShowFilesCommand = new RelayCommand(ShowFiles);
            ShowLoansCommand = new RelayCommand(ShowLoansView);
            ShowApplyForLoanCommand = new RelayCommand(ShowApplyForLoan);

            // Výchozí zobrazení
            CurrentView = new AdminMainView();

            if(Session.Instance.EmulatedUserId != null)
            {
                EmulationLabelVisibility = Visibility.Visible;
            }
        }

        private void ShowApplyForLoan()
        {
            throw new NotImplementedException();
        }

        private void ShowFiles()
        {
            CurrentView = new FilesView();
        }

        private void ShowStandingOrders()
        {

        }

        private void ChangePersonalDetails()
        {
            var clientEditPersonalDetailsView = new ClientEditPersonalDetailsView();
            clientEditPersonalDetailsView.ShowDialog();
        }

        private void ShowCards()
        {
            var clientCardsViewModel = new ClientCardsViewModel();
            var clientCardsView = new CardsView()
            {
                DataContext = clientCardsViewModel
            };
            CurrentView = clientCardsView;
        }

        private void ShowLoansView()
        {
            CurrentView = new LoansView();
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
            CurrentView = new ClientAccountsView();
        }

        // INotifyPropertyChanged implementace
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
