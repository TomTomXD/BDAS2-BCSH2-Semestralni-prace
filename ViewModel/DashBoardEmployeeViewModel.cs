using FinancniInformacniSystemBanky.View;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class DashBoardEmployeeViewModel : INotifyPropertyChanged
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
        public ICommand LogoClickedCommand { get; }
        public ICommand ShowAllAccountCommand { get; }
        public ICommand LogOutCommand { get; }
        public ICommand ShowAllCardsCommand { get; }
        public ICommand ShowAllPeople { get; }
        public ICommand ShowFilesCommand { get; }
        public ICommand ShowStandingOrdersCommand { get; }
        public ICommand ShowHierarchyCommand { get; }

        public DashBoardEmployeeViewModel()
        {
            ShowAllAccountCommand = new RelayCommand(ShowAccounts);
            ShowLoansCommand = new RelayCommand(ShowLoans);
            LogoClickedCommand = new RelayCommand(ShowHome);
            LogOutCommand = new RelayCommand(LogOut);
            ShowAllCardsCommand = new RelayCommand(ShowCards);
            ShowAllPeople = new RelayCommand(ShowPeople);
            ShowFilesCommand = new RelayCommand(ShowFiles);
            ShowStandingOrdersCommand = new RelayCommand(ShowStandingOrders);
            ShowHierarchyCommand = new RelayCommand(DisplayHierarchyTree);

            CurrentView = new AdminMainView();
        }

        private void DisplayHierarchyTree()
        {
            CurrentView = new EmployeeHierarchyView();
        }

        private void ShowStandingOrders()
        {
            CurrentView = new StandingOrdersView();
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

        private void LogOut()
        {
            // Otevření nového přihlašovacího okna (LoginView)
            var loginView = new LoginView();
            loginView.Show();

            // Zavření aktuálního okna (DashboardAdminView)
            var currentWindow = Application.Current.Windows
                .OfType<DashboardEmployeeView>()
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
