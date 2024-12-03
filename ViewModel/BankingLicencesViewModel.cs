using FinancniInformacniSystemBanky.DatabaseLayer;
using InformacniSystemBanky.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class BankingLicencesViewModel : INotifyPropertyChanged
    {
        private readonly BankingLicenceService _bankingLicencesService;

        public ObservableCollection<BankingLicence> Licences { get; set; }

        public BankingLicencesViewModel()
        {
            _bankingLicencesService = new BankingLicenceService();
            Licences = new ObservableCollection<BankingLicence>();
            LoadLicencesFromDatabase();
        }

        private void LoadLicencesFromDatabase()
        {
            var licencesFromDb = _bankingLicencesService.GetBankingLicences();
            Licences.Clear();
            foreach (var licence in licencesFromDb)
            {
                Licences.Add(licence);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
