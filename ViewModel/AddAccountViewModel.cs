using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.View;
using InformacniSystemBanky.Model;
using InformacniSystemBanky.View;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel;
using System.Configuration;
using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.ViewModel
{
    public class AddAccountViewModel : INotifyPropertyChanged
    {
        public List<string> AccountTypes { get; } = new List<string> { "B", "S" };

        private readonly AccountService _accountService;

        private string actionButtonText;
        private string actionLabelText;
        private string selectedAccountType;
        private Visibility _interestVisibility;
        private Visibility _maxBalanceVisibility;

        public string SelectedAccountType
        {
            get => selectedAccountType;
            set
            {
                selectedAccountType = value;
                OnPropertyChanged(nameof(SelectedAccountType));
                UpdateVisibility();
            }
        }

        public Visibility InterestVisibility
        {
            get => _interestVisibility;
            set
            {
                _interestVisibility = value;
                OnPropertyChanged(nameof(InterestVisibility));
            }
        }

        public Visibility MaxBalanceVisibility
        {
            get => _maxBalanceVisibility;
            set
            {
                _maxBalanceVisibility = value;
                OnPropertyChanged(nameof(MaxBalanceVisibility));
            }
        }

        public string ActionLabelText
        {
            get => actionLabelText;
            set { actionLabelText = value; OnPropertyChanged(nameof(ActionLabelText)); }
        }
        public string ActionButtonText
        {
            get => actionButtonText;
            set { actionButtonText = value; OnPropertyChanged(nameof(ActionButtonText)); }
        }

        public ICommand AddAccountCommand { get; }
        public ICommand CancelAddingNewAccountCommand { get; }

        public AddAccountViewModel()
        {
            CancelAddingNewAccountCommand = new RelayCommand(CloseAddingWindow);

            actionLabelText = "Přidat účet";
            actionButtonText = "Přidat";

            InterestVisibility = Visibility.Hidden;
            MaxBalanceVisibility = Visibility.Hidden;
        }

        public AddAccountViewModel(Account account)
        {
            CancelAddingNewAccountCommand = new RelayCommand(CloseAddingWindow);

            actionLabelText = "Upravit účet";
            actionButtonText = "Upravit";

            InterestVisibility = Visibility.Hidden;
            MaxBalanceVisibility = Visibility.Hidden;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void UpdateVisibility()
        {
            if (SelectedAccountType == "S")
            {
                MaxBalanceVisibility = Visibility.Visible;
                InterestVisibility = Visibility.Visible;
            }
            else
            {
                MaxBalanceVisibility = Visibility.Hidden;
                InterestVisibility = Visibility.Hidden;
            }
        }

        private void CloseAddingWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<AddAccountView>().FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }
    }
}