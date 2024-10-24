using InformacniSystemBanky.View;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.ViewModel
{
    public class DashboardViewModel : INotifyPropertyChanged
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

        public DashboardViewModel()
        {
            ShowHomeCommand = new RelayCommand(ShowHome);
            ShowSettingsCommand = new RelayCommand(ShowSettings);
            ShowProfileCommand = new RelayCommand(ShowProfile);
            LogoClickedCommand = new RelayCommand(ShowHome);

            // Výchozí pohled
            //CurrentViewModel = new HomeViewModel(); // Změněno na HomeViewModel
        }

        private void ShowHome()
        {
            //CurrentViewModel = new HomeViewModel();
            //CurrentViewModel = new DashboardKlientView();
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

        // INotifyPropertyChanged implementace
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
