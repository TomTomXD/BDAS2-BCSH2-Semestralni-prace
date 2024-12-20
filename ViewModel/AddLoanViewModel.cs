﻿using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.Model.Helpers;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class AddLoanViewModel : INotifyPropertyChanged
    {

        private string actionButtonText;
        private string actionLabelText;

        private int _loanId;
        private decimal _amount;
        private decimal _interestRate;
        private DateTime _dateOfApproval;
        private DateTime _dateOfRepayment;
        private int _clientId;
        private int _creditCounselorId;
        private int _loanType;
        private int _loanStatus;
        private bool _clientSetter;
        private bool _creditCounselorSetter;
        private bool _statusSetter;
        private bool _interestRateSetter;

        public bool InterestRateSetter
        {
            get => _interestRateSetter;
            set { _interestRateSetter = value; OnPropertyChanged(nameof(InterestRateSetter)); }
        }

        public bool StatusSetter
        {
            get => _statusSetter;
            set { _statusSetter = value; OnPropertyChanged(nameof(StatusSetter)); }
        }

        public bool CreditCounselorSetter
        {
            get => _creditCounselorSetter;
            set { _creditCounselorSetter = value; OnPropertyChanged(nameof(CreditCounselorSetter)); }
        }

        public bool ClientSetter
        {
            get => _clientSetter;
            set { _clientSetter = value; OnPropertyChanged(nameof(ClientSetter)); }
        }
        public int LoanId
        {
            get => _loanId;
            set
            {
                _loanId = value;
                OnPropertyChanged(nameof(LoanId));
            }
        }

        public decimal Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged(nameof(Amount));
            }
        }

        public decimal InterestRate
        {
            get => _interestRate;
            set
            {
                _interestRate = value;
                OnPropertyChanged(nameof(InterestRate));
            }
        }

        public DateTime DateOfApproval
        {
            get => _dateOfApproval;
            set
            {
                _dateOfApproval = value;
                OnPropertyChanged(nameof(DateOfApproval));
            }
        }

        public DateTime DateOfRepayment
        {
            get => _dateOfRepayment;
            set
            {
                _dateOfRepayment = value;
                OnPropertyChanged(nameof(DateOfRepayment));
            }
        }

        public int ClientId
        {
            get => _clientId;
            set
            {
                _clientId = value;
                OnPropertyChanged(nameof(ClientId));
            }
        }

        public int CreditCounselorId
        {
            get => _creditCounselorId;
            set
            {
                _creditCounselorId = value;
                OnPropertyChanged(nameof(CreditCounselorId));
            }
        }

        public int LoanType
        {
            get => _loanType;
            set
            {
                _loanType = value;
                OnPropertyChanged(nameof(LoanType));
            }
        }

        public int LoanStatus
        {
            get => _loanStatus;
            set
            {
                _loanStatus = value;
                OnPropertyChanged(nameof(LoanStatus));
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

        private readonly LoanService _loanService;

        private LoanStatus _selectedLoanStatus;
        private LoanType _lelectedLoanType;
        private CreditCounselor _selectedCreditCounselor;
        private Client _selectedClient;

        public LoanType SelectedLoanType
        {
            get => _lelectedLoanType;
            set
            {
                _lelectedLoanType = value;
                OnPropertyChanged(nameof(SelectedLoanType));
            }
        }
        public LoanStatus SelectedLoanStatus
        {
            get => _selectedLoanStatus;
            set
            {
                _selectedLoanStatus = value;
                OnPropertyChanged(nameof(SelectedLoanStatus));
            }
        }
        public CreditCounselor SelectedCreditCounselor
        {
            get => _selectedCreditCounselor;
            set
            {
                _selectedCreditCounselor = value;
                OnPropertyChanged(nameof(SelectedCreditCounselor));
            }
        }
        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                OnPropertyChanged(nameof(SelectedClient));
            }
        }

        public ObservableCollection<Client> Clients { get; set; }
        public ObservableCollection<CreditCounselor> CreditCounselors { get; set; }
        public ObservableCollection<LoanType> LoanTypes { get; set; }
        public ObservableCollection<LoanStatus> LoanStatuses { get; set; }

        public ICommand AddNewLoanCommand { get; }
        public ICommand CancelAddingNewLoanCommand { get; }

        // Bezparametrický konstrutkor pro přidání nového úvěru
        public AddLoanViewModel()
        {
            _loanService = new LoanService();
            AddNewLoanCommand = new RelayCommand(AddNewLoad);
            CancelAddingNewLoanCommand = new RelayCommand(CloseAddingWindow);
            
            CreditCounselors = new ObservableCollection<CreditCounselor>(_loanService.GetCreditCounselors());
            Clients = new ObservableCollection<Client>(_loanService.GetEligibleClientsForLoan());
            LoanTypes = new ObservableCollection<LoanType>(_loanService.GetLoanTypes());
            LoanStatuses = new ObservableCollection<LoanStatus>(_loanService.GetLoanStatus());
            ClientSetter = true;
            CreditCounselorSetter = true;
            StatusSetter = true;
            InterestRateSetter = true;

            DateOfApproval = DateTime.Now;
            DateOfRepayment = DateTime.Now;

            if (Session.Instance.EmulatedRoleId == 1 || Session.Instance.CurrentRoleId == 1)
            {
                SelectedClient = Clients.First(x => x.Id == (Session.Instance.EmulatedRoleId ?? Session.Instance.CurrentUserId));
                SelectedLoanStatus = LoanStatuses.First(x => x.Id == 3);

               var random = new Random();
               SelectedCreditCounselor = CreditCounselors[random.Next(CreditCounselors.Count)];

                InterestRate = (decimal)5.2;

                ClientSetter = false;
                CreditCounselorSetter = false;
                StatusSetter = false;
                InterestRateSetter = false;
            }

            ActionLabelText = "Přidat Úvěr";
            ActionButtonText = "Přidat";
        }

        // Parametrický konstruktor pro editaci úvěru, dostává vybraný úvěr z datagridu
        public AddLoanViewModel(Loan selectedLoan)
        {
            _loanService = new LoanService();
            AddNewLoanCommand = new RelayCommand(EditLoan);
            CancelAddingNewLoanCommand = new RelayCommand(CloseAddingWindow);

            CreditCounselors = new ObservableCollection<CreditCounselor>(_loanService.GetCreditCounselors());
            Clients = new ObservableCollection<Client>(_loanService.GetEligibleClientsForLoan());
            LoanTypes = new ObservableCollection<LoanType>(_loanService.GetLoanTypes());
            LoanStatuses = new ObservableCollection<LoanStatus>(_loanService.GetLoanStatus());

            LoanId = selectedLoan.LoanId;
            Amount = selectedLoan.Amount;
            InterestRate = selectedLoan.InterestRate;
            DateOfApproval = selectedLoan.DateOfApproval;
            DateOfRepayment = selectedLoan.DateOfRepayment;
            ClientId = selectedLoan.Client.Id;
            CreditCounselorId = selectedLoan.CreditCounselor.Id;
            LoanStatus = selectedLoan.LoanStatus.Id;
            LoanType = selectedLoan.LoanType.Id;

            CreditCounselorSetter = true;
            StatusSetter = true;
            InterestRateSetter = true;

            // nastavení aktuální hodnoty do comboboxů
            SelectedLoanStatus = LoanStatuses.First(x => x.Id == selectedLoan.LoanStatus.Id);
            SelectedLoanType = LoanTypes.First(x => x.Id == selectedLoan.LoanType.Id);
            SelectedClient = Clients.First(x => x.Id == selectedLoan.Client.Id);
            SelectedCreditCounselor = CreditCounselors.First(x => x.Id == selectedLoan.CreditCounselor.Id);

            ActionLabelText = "Upravit Úvěr";
            ActionButtonText = "Upravit";
        }

        private void EditLoan()
        {
            _loanService.UpdateLoan(LoanId, Amount, InterestRate, DateOfApproval, DateOfRepayment, ClientId, CreditCounselorId, SelectedLoanType.Id, SelectedLoanStatus.Id);
            CloseAddingWindow();
        }

        private void AddNewLoad()
        {
            if (Amount == 0 || InterestRate == 0 || DateOfApproval == null || DateOfRepayment == null || ClientId == 0 || CreditCounselorId == 0 || SelectedLoanType == null || SelectedLoanStatus == null)
            {
                MessageBox.Show("Všechny položky musí být vyplněny", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (DateOfApproval > DateOfRepayment)
            {
                MessageBox.Show("Datum schválení nemůže vyšší než datum splacení", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
                _loanService.AddLoan(Amount, InterestRate, DateOfApproval, DateOfRepayment, ClientId, CreditCounselorId, SelectedLoanType.Id, SelectedLoanStatus.Id);
                CloseAddingWindow();
        }

        private void CloseAddingWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<AddLoanView>().FirstOrDefault();
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
