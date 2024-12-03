using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

        public ICommand AddLoanCommand { get; }
        public ICommand EditLoanCommand { get; }
        public ICommand DeleteLoanCommand { get; }

        public LoansViewModel()
        {
            _loanService = new LoanService();
            Loans = new ObservableCollection<Loan>();
            LoadLoansFromDatabase();

            DeleteLoanCommand = new RelayCommand(DeleteLoanFromDatabase, CanDeleteLoan);
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
            var loansFromDb = _loanService.GetLoans();

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
