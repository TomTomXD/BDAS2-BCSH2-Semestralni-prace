using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.Model.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class LookupTablesViewModel
    {
        private readonly LookupTablesService _lookupTablesService;

        public ObservableCollection<Department> Departments { get; set; }
        public ObservableCollection<Position> Positions { get; set; }
        public ObservableCollection<Role> Roles { get; set; }
        public ObservableCollection<LoanStatus> LoanStatuses { get; set; }
        public ObservableCollection<BankingLicenceType> LicenceTypes { get; set; }
        public ObservableCollection<LoanType> LoanTypes { get; set; }
        public ObservableCollection<CardType> CardTypes{ get; set; }

        // Pomocí parametru poté budeme určovat, kterou tabulku chceme zobrazit
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }


        public LookupTablesViewModel()
        {
            _lookupTablesService = new LookupTablesService();
        }

        private void LoadLookupTablesFromDatabase()
        {
            throw new NotImplementedException();
        }

    }
}
