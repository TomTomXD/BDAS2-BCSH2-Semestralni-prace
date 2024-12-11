using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class LoansViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Loan> Loans { get; set; }
        private readonly LoanService _loanService;
        private Loan _selectedLoan;
        public Loan SelectedLoan
        {
            get => _selectedLoan;
            set
            {
                _selectedLoan = value;
                OnPropertyChanged(nameof(SelectedLoan));
            }
        }

        private Visibility _loanButtons;
        public Visibility LoanButtons
        {
            get => _loanButtons;
            set
            {
                _loanButtons = value;
                OnPropertyChanged(nameof(LoanButtons));
            }
        }

        public ICommand AddLoanCommand { get; }
        public ICommand EditLoanCommand { get; }
        public ICommand DeleteLoanCommand { get; }

        public LoansViewModel()
        {
            _loanService = new LoanService();
            Loans = new ObservableCollection<Loan>();
            LoadLoansFromDatabase();
            
            // Kontrola a upozrnění, zda je kolekce úvěrů prázdná
            if (!Loans.Any())
            {
                MessageBox.Show("Nemáte žádné úvěry", "Informace", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            AddLoanCommand = new RelayCommand(AddNewLoad);
            EditLoanCommand = new RelayCommand(EditLoan);
            DeleteLoanCommand = new RelayCommand(DeleteLoanFromDatabase, CanDeleteLoan);

            if(Session.Instance.EmulatedRoleId == 1 || Session.Instance.CurrentRoleId == 1)
            {
                LoanButtons = Visibility.Hidden;
            }
            else
            {
                LoanButtons = Visibility.Visible;
            }
        }

        private void EditLoan()
        {
            if (SelectedLoan != null)
            {
                var editLoanViewModel = new AddLoanViewModel(SelectedLoan);
                var editLoanView = new AddLoanView()
                {
                    DataContext = editLoanViewModel
                };
                editLoanView.ShowDialog();
                LoadLoansFromDatabase();
            }
        }

        private void AddNewLoad()
        {
            var addLoanView = new AddLoanView();
            addLoanView.ShowDialog();
            LoadLoansFromDatabase();
        }

        private void DeleteLoanFromDatabase()
        {
            if (CanDeleteLoan())
            {
                _loanService.DeleteLoan(SelectedLoan.LoanId);
                LoadLoansFromDatabase();
            }
        }

        private void LoadLoansFromDatabase()
        {
            IEnumerable<Loan> loansFromDb;


            if (Session.Instance.EmulatedRoleId == 1 || Session.Instance.CurrentRoleId == 1)
            {
                loansFromDb = _loanService.GetLoansByClientId(Session.Instance.EmulatedRoleId ?? Session.Instance.CurrentUserId);
            }
            else if (Session.Instance.EmulatedRoleId == 3 || Session.Instance.CurrentRoleId == 3)
            {
                loansFromDb = _loanService.GetLoansByCreditCounselorId(Session.Instance.EmulatedUserId ?? Session.Instance.CurrentUserId);
            }
            else
            {
                loansFromDb = _loanService.GetLoans();
            }

            Loans.Clear();
            foreach (var loan in loansFromDb)
            {
                Loans.Add(loan);
            }
        }

        private bool CanDeleteLoan()
        {
            return SelectedLoan != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
