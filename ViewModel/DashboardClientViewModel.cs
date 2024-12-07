﻿using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.View;
using InformacniSystemBanky.View;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

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

        //public ICommand ShowHomeCommand { get; }
        public ICommand ShowAllAccountCommand { get; }
        public ICommand ShowLoans { get; }
        public ICommand LogOutCommand { get; }

        public DashboardClientViewModel()
        {
            //ShowHomeCommand = new RelayCommand(ShowHome);
            ShowAllAccountCommand = new RelayCommand(ShowAccounts);
            LogOutCommand = new RelayCommand(LogOut);
            ShowLoans = new RelayCommand(ShowLoansView);

            // Výchozí zobrazení
            CurrentView = new AdminMainView();

        }

        private void ShowLoansView()
        {

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
