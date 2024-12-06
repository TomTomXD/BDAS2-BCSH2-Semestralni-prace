using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.Model.Helpers;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class LookupTablesViewModel : INotifyPropertyChanged
    {
        private readonly LookupTablesService _lookupTablesService;

        public ObservableCollection<Department> Departments { get; set; }
        public ObservableCollection<Position> Positions { get; set; }
        public ObservableCollection<Role> Roles { get; set; }
        public ObservableCollection<LoanStatus> LoanStatuses { get; set; }
        public ObservableCollection<BankingLicenceType> LicenceTypes { get; set; }
        public ObservableCollection<LoanType> LoanTypes { get; set; }
        public ObservableCollection<CardType> CardTypes { get; set; }

        // Pomocí parametru poté budeme určovat, kterou tabulku chceme zobrazit
        public ICommand AddCommand { get; set; }
        public ICommand EditCommand { get; set; }
        public ICommand DeleteCommand { get; set; }


        public LookupTablesViewModel()
        {
            _lookupTablesService = new LookupTablesService();
            AddCommand = new RelayCommand(Add);
            EditCommand = new RelayCommand(Edit);
            DeleteCommand = new RelayCommand(Delete);

            Departments = new ObservableCollection<Department>();

            LoadLookupTablesFromDatabase();
        }

        private void Edit(object parameter)
        {
            if (parameter is string commandParameter)
            {
                switch (commandParameter)
                {
                    case "DepartmentEdit":

                        break;
                    case "PositionEdit":

                        break;
                    case "RoleEdit":

                        break;
                    case "LoanStatusEdit":

                        break;
                    case "LicenceTypeEdit":

                        break;
                    case "LoanTypeEdit":

                        break;
                    case "CardTypeEdit":

                        break;
                    default:
                        break;
                }
                LoadLookupTablesFromDatabase();
            }
        }

        private void Delete(object parameter)
        {
            if (parameter is string commandParameter)
            {
                switch (commandParameter)
                {
                    case "DepartmentDelete":

                        break;
                    case "PositionDelete":

                        break;
                    case "RoleDelete":

                        break;
                    case "LoanStatusDelete":

                        break;
                    case "LicenceTypeDelete":

                        break;
                    case "LoanTypeDelete":

                        break;
                    case "CardTypeDelete":

                        break;
                    default:
                        break;
                }
                LoadLookupTablesFromDatabase();
            }
        }


        private void Add(object parameter)
        {
            if (parameter is string commandParameter)
            {
                switch (commandParameter)
                {
                    case "DepartmentAdd":
                        ShowAddIntoLookupTableView("upsert_oddeleni");
                        break;
                    case "PositionAdd":
                        ShowAddIntoLookupTableView("upsert_pozice");
                        break;
                    case "RoleAdd":
                        ShowAddIntoLookupTableView("upsert_role");
                        break;
                    case "LoanStatusAdd":
                        ShowAddIntoLookupTableView("upsert_stavy_uvery");
                        break;
                    case "LicenceTypeAdd":
                        ShowAddIntoLookupTableView("upsert_typy_licenci");
                        break;
                    case "LoanTypeAdd":
                        ShowAddIntoLookupTableView("upsert_typy_uveru");
                        break;
                    case "CardTypeAdd":
                        ShowAddIntoLookupTableView("upsert_typ_karty");
                        break;
                    default:
                        break;
                }
                LoadLookupTablesFromDatabase();
            }
        }

        private void ShowAddIntoLookupTableView(string procedureName)
        {
            var addViewModel = new AddIntoLookupTableViewModel(procedureName);
            var addView = new AddIntoLookupTableView()
            {
                DataContext = addViewModel
            };
            addView.ShowDialog();
        }

        private void LoadLookupTablesFromDatabase()
        {
            try
            {
                Departments = new ObservableCollection<Department>(_lookupTablesService.GetLookupTableData<Department>("ODDELENI"));
                Positions = new ObservableCollection<Position>(_lookupTablesService.GetLookupTableData<Position>("POZICE"));
                Roles = new ObservableCollection<Role>(_lookupTablesService.GetLookupTableData<Role>("ROLE"));
                LoanStatuses = new ObservableCollection<LoanStatus>(_lookupTablesService.GetLookupTableData<LoanStatus>("STAVY_UVERU"));
                LicenceTypes = new ObservableCollection<BankingLicenceType>(_lookupTablesService.GetLookupTableData<BankingLicenceType>("TYPY_LICENCI"));
                LoanTypes = new ObservableCollection<LoanType>(_lookupTablesService.GetLookupTableData<LoanType>("TYPY_UVERU"));
                CardTypes = new ObservableCollection<CardType>(_lookupTablesService.GetLookupTableData<CardType>("TYPY_KARET"));

                OnPropertyChanged(nameof(Departments));
                OnPropertyChanged(nameof(Positions));
                OnPropertyChanged(nameof(Roles));
                OnPropertyChanged(nameof(LoanStatuses));
                OnPropertyChanged(nameof(LicenceTypes));
                OnPropertyChanged(nameof(LoanTypes));
                OnPropertyChanged(nameof(CardTypes));
            }
            catch (Exception ex)
            {
                // Ošetření výjimek
                MessageBox.Show($"Došlo k chybě při načítání dat: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanDelete()
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
