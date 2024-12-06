using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.Model;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class UpdatePasswordViewModel : INotifyPropertyChanged
    {
        private readonly PasswordService _passwordService;
        private int _id;
        private string _password;

        // Vlastnost pro heslo
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public ICommand UpdatePasswordCommand { get; }
        public ICommand CancelUpdatingPasswordCommand { get; }

        public UpdatePasswordViewModel(Password selectedPassword)
        {
            _passwordService = new PasswordService();
            _id = selectedPassword.PasswordId;

            UpdatePasswordCommand = new RelayCommand(UpdatePassword);
            CancelUpdatingPasswordCommand = new RelayCommand(CloseUpdatingWindow);
        }

        private void UpdatePassword()
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Heslo nesmí být prázdné.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var salt = PasswordHasher.GenerateSalt();
            var hashedPassword = PasswordHasher.HashPassword(Password, salt);

            _passwordService.UpdatePassword(_id, hashedPassword, Convert.ToBase64String(salt));
            MessageBox.Show("Heslo bylo úspěšně aktualizováno.", "Informace", MessageBoxButton.OK, MessageBoxImage.Information);
            CloseUpdatingWindow();
        }

        private void CloseUpdatingWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<UpdatePasswordView>().FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}