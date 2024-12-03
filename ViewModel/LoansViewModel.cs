using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class LoansViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Loan> Loans { get; set; }
        private readonly LoanService _loanService;
        public LoansViewModel()
        {
            _loanService = new LoanService();
            Loans = new ObservableCollection<Loan>();
            LoadLoansFromDatabase();
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
        
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
