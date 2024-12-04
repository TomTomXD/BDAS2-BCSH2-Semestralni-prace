using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.Model;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky
{
    public class AddBankingLicenceViewModel : INotifyPropertyChanged
    {
        private string actionButtonText;
        private string actionLabelText;

        private readonly BankingLicenceService _bankingLicenceServis;
        private Employee _selectedEmployee;
        private BankingLicenceType _selectedLicenceType;

        private int _bankingLicenceId;
        private string _licenceNumber;
        private DateTime _issueDate;
        private DateTime _expirationDate;
        private int _licenceType;
        private int _licenceHolderId;

        public int BankingLicenceId
        {
            get => _bankingLicenceId;
            set
            {
                _bankingLicenceId = value;
                OnPropertyChanged(nameof(BankingLicenceId));
            }
        }
        public string LicenceNumber
        {
            get => _licenceNumber;
            set
            {
                _licenceNumber = value;
                OnPropertyChanged(nameof(LicenceNumber));
            }
        }
        public DateTime IssueDate
        {
            get => _issueDate;
            set
            {
                _issueDate = value;
                OnPropertyChanged(nameof(IssueDate));
            }
        }
        public DateTime ExpirationDate
        {
            get => _expirationDate;
            set
            {
                _expirationDate = value;
                OnPropertyChanged(nameof(ExpirationDate));
            }
        }
        public int LicenceType
        {
            get => _licenceType;
            set
            {
                _licenceType = value;
                OnPropertyChanged(nameof(LicenceType));
            }
        }
        public int LicenceHolderId
        {
            get => _licenceHolderId;
            set
            {
                _licenceHolderId = value;
                OnPropertyChanged(nameof(LicenceHolderId));
            }
        }
        public Employee SelectedEmployee
        {
            get => _selectedEmployee;
            set
            {
                _selectedEmployee = value;
                OnPropertyChanged(nameof(SelectedEmployee));
            }
        }
        public BankingLicenceType SelectedLicenceType
        {
            get => _selectedLicenceType;
            set
            {
                _selectedLicenceType = value;
                OnPropertyChanged(nameof(SelectedLicenceType));
            }
        }
        public string ActionButtonText
        {
            get => actionButtonText;
            set
            {
                actionButtonText = value;
                OnPropertyChanged(nameof(ActionButtonText));
            }
        }
        public string ActionLabelText
        {
            get => actionLabelText;
            set
            {
                actionLabelText = value;
                OnPropertyChanged(nameof(ActionLabelText));
            }
        }

        public ObservableCollection<BankingLicenceType> BankingLicenceTypes { get; set; }
        public ObservableCollection<Employee> Employees { get; set; }

        public ICommand CancelAddingNewLicenceCommand { get; }
        public ICommand AddNewLicenceCommand { get; }

        // Bezparametrický konstruktor pro přidání nové licence
        public AddBankingLicenceViewModel()
        {
            _bankingLicenceServis = new BankingLicenceService();
            BankingLicenceTypes = new ObservableCollection<BankingLicenceType>(_bankingLicenceServis.GetBankingLicenceTypes());
            Employees = new ObservableCollection<Employee>(_bankingLicenceServis.GetEmployees());

            AddNewLicenceCommand = new RelayCommand(AddNewBankingLicence);
            CancelAddingNewLicenceCommand = new RelayCommand(CloseAddingWindow);

            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;

            ActionLabelText = "Přidat licenci";
            ActionButtonText = "Přidat";
        }

        // Konsktruktor s parametr pro úpravu licence, přijímá vybranou licence z datagridu
        public AddBankingLicenceViewModel(BankingLicence selectedLicence)
        {
            _bankingLicenceServis = new BankingLicenceService();
            AddNewLicenceCommand = new RelayCommand(EditBankingLicence);
            CancelAddingNewLicenceCommand = new RelayCommand(CloseAddingWindow);

            BankingLicenceTypes = new ObservableCollection<BankingLicenceType>(_bankingLicenceServis.GetBankingLicenceTypes());
            Employees = new ObservableCollection<Employee>(_bankingLicenceServis.GetEmployees());


            BankingLicenceId = selectedLicence.BankingLicenseId;
            LicenceNumber = selectedLicence.LicenceNumber;
            IssueDate = selectedLicence.IssueDate;
            ExpirationDate = selectedLicence.ExpirationDate;

            SelectedLicenceType = BankingLicenceTypes.First(x => x.BankingLicenceTypeId == selectedLicence.LicenceType);
            SelectedEmployee = Employees.First(x => x.Id == selectedLicence.LicenceHolderId);

            ActionLabelText = "Upravit licenci";
            ActionButtonText = "Upravit";
        }

        private void AddNewBankingLicence()
        {

            _bankingLicenceServis.AddBankingLicence(null, LicenceNumber, IssueDate, ExpirationDate, SelectedLicenceType.BankingLicenceTypeId, SelectedEmployee.Id);
            CloseAddingWindow();
        }

        private void EditBankingLicence()
        {
                _bankingLicenceServis.EditBankingLicence(BankingLicenceId, LicenceNumber, IssueDate, ExpirationDate, SelectedLicenceType.BankingLicenceTypeId, SelectedEmployee.Id);
                CloseAddingWindow();
        }

        private void CloseAddingWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<AddBankingLicenceView>().FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
