using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class PasswordsViewModel :INotifyPropertyChanged
    {
        private readonly PasswordService _passwordService;
        private Password _selectedPassword;
        public Password SelectedPassword
        {
            get => _selectedPassword;
            set
            {
                _selectedPassword = value;
                OnPropertyChanged(nameof(SelectedPassword));
            }
        }

        public ObservableCollection<Password> Passwords { get; set; }

        public ICommand EditPasswordCommand { get; }
        public ICommand DeletePasswordCommand { get; }


        public PasswordsViewModel()
        {
            _passwordService = new PasswordService();
            Passwords = new ObservableCollection<Password>();
            LoadPasswordsFromDatabase();

            EditPasswordCommand = new RelayCommand(EditPassword);
            DeletePasswordCommand = new RelayCommand(DeletePasswordFromDatabase, CanDeleteAccount);
        }

        private void DeletePasswordFromDatabase()
        {
            if (CanDeleteAccount())
            {
                _passwordService.DeletePassword(SelectedPassword.PasswordId);
                LoadPasswordsFromDatabase();
            }
        }

        private void EditPassword()
        {
            throw new NotImplementedException();
        }

        private void LoadPasswordsFromDatabase()
        {
            var passwordsFromDb = _passwordService.GetPasswords();
            Passwords.Clear();
            foreach (var password in passwordsFromDb)
            {
                Passwords.Add(password);
            }
        }

        private bool CanDeleteAccount()
        {
            return SelectedPassword != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
