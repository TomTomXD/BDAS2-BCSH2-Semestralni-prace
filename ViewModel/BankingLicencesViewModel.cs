using FinancniInformacniSystemBanky.DatabaseLayer;
using InformacniSystemBanky.Model;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class BankingLicencesViewModel : INotifyPropertyChanged
    {
        private readonly BankingLicenceService _bankingLicencesService;

        private BankingLicence _selectedLicence;
        public BankingLicence SelectedLicence
        {
            get => _selectedLicence;
            set
            {
                _selectedLicence = value;
                OnPropertyChanged(nameof(SelectedLicence));
            }
        }
        public ObservableCollection<BankingLicence> Licences { get; set; }

        public ICommand AddLicenceCommand { get; }
        public ICommand EditLicenceCommand { get; }
        public ICommand DeleteLicenceCommand { get; }


        public BankingLicencesViewModel()
        {
            _bankingLicencesService = new BankingLicenceService();
            Licences = new ObservableCollection<BankingLicence>();

            AddLicenceCommand = new RelayCommand(AddLicenceToDatabase);
            EditLicenceCommand = new RelayCommand(EditLicence);
            DeleteLicenceCommand = new RelayCommand(DeleteLicenceFromDatabase, CanDeleteLicence);

            LoadLicencesFromDatabase();
        }

        private bool CanDeleteLicence()
        {
            return SelectedLicence != null;
        }

        private void DeleteLicenceFromDatabase()
        {
            if (CanDeleteLicence())
            {
                _bankingLicencesService.DeleteBankingLicence(SelectedLicence.BankingLicenseId);
                LoadLicencesFromDatabase();
            }
        }

        private void EditLicence()
        {
            if(SelectedLicence != null)
            {
                var addBankingLicenceViewModel = new AddBankingLicenceViewModel(SelectedLicence);
                var addBankingLicenceView = new AddBankingLicenceView()
                {
                    DataContext = addBankingLicenceViewModel
                };
                addBankingLicenceView.ShowDialog();
                LoadLicencesFromDatabase();
            }
        }

        private void AddLicenceToDatabase()
        {
            var addBankingLicenceView = new AddBankingLicenceView();
            addBankingLicenceView.ShowDialog();
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
