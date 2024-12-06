using FinancniInformacniSystemBanky.DatabaseLayer;
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

        private ILookupEntry _selectedDepartment;
        private ILookupEntry _selectedPosition;
        private ILookupEntry _selectedRole;
        private ILookupEntry _selectedLoanStatus;
        private ILookupEntry _selectedLicenceType;
        private ILookupEntry _selectedLoanType;
        private ILookupEntry _selectedCardType;

        public ILookupEntry SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                _selectedDepartment = value;
                OnPropertyChanged(nameof(SelectedDepartment));
            }
        }
        public ILookupEntry SelectedPosition
        {
            get => _selectedPosition;
            set
            {
                _selectedPosition = value;
                OnPropertyChanged(nameof(SelectedPosition));
            }
        }
        public ILookupEntry SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged(nameof(SelectedRole));
            }
        }
        public ILookupEntry SelectedLoanStatus
        {
            get => _selectedLoanStatus;
            set
            {
                _selectedLoanStatus = value;
                OnPropertyChanged(nameof(SelectedLoanStatus));
            }
        }
        public ILookupEntry SelectedLicenceType
        {
            get => _selectedLicenceType;
            set
            {
                _selectedLicenceType = value;
                OnPropertyChanged(nameof(SelectedLicenceType));
            }
        }
        public ILookupEntry SelectedLoanType
        {
            get => _selectedLoanType;
            set
            {
                _selectedLoanType = value;
                OnPropertyChanged(nameof(SelectedLoanType));
    }
}
        public ILookupEntry SelectedCardType
        {
            get => _selectedCardType;
            set
            {
                _selectedCardType = value;
                OnPropertyChanged(nameof(SelectedCardType));
            }
        }

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
                        if (SelectedDepartment == null) return;
                        ShowEditRecordInLookupTableView("upsert_oddeleni", SelectedDepartment);
                        break;
                    case "PositionEdit":
                        if (SelectedPosition == null) return;
                        ShowEditRecordInLookupTableView("upsert_pozice", SelectedPosition);
                        break;
                    case "RoelEdit":
                        if (SelectedRole == null) return;
                        ShowEditRecordInLookupTableView("upsert_role", SelectedRole);
                        break;
                    case "LoanStatusEdit":
                        if (SelectedLoanStatus == null) return;
                        ShowEditRecordInLookupTableView("upsert_stavy_uvery", SelectedLoanStatus);
                        break;
                    case "LicenceTypeEdit":
                        if (SelectedLicenceType == null) return;
                        ShowEditRecordInLookupTableView("upsert_typy_licenci", SelectedLicenceType);
                        break;
                    case "LoanTypeEdit":
                        if (SelectedLoanType == null) return;
                        ShowEditRecordInLookupTableView("upsert_typy_uveru", SelectedLoanType);
                        break;
                    case "CardTypeEdit":
                        if (SelectedCardType == null) return;
                        ShowEditRecordInLookupTableView("upsert_typ_karty", SelectedCardType);
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
                        if (SelectedDepartment == null) return;
                        _lookupTablesService.DeleteRecord("ODDELENI", "ID_ODDELENI", SelectedDepartment.Id);
                        break;
                    case "PositionDelete":
                        if (SelectedPosition == null) return;
                        _lookupTablesService.DeleteRecord("POZICE", "ID_POZICE", SelectedPosition.Id);
                        break;
                    case "RoleDelete":
                        if (SelectedRole == null) return;
                        _lookupTablesService.DeleteRecord("ROLE", "ID_ROLE", SelectedRole.Id);
                        break;
                    case "LoanStatusDelete":
                        if (SelectedLoanStatus == null) return;
                        _lookupTablesService.DeleteRecord("STAVY_UVERU", "ID_STAVU_UVERU", SelectedLoanStatus.Id);
                        break;
                    case "LicenceTypeDelete":
                        if (SelectedLicenceType == null) return;
                        _lookupTablesService.DeleteRecord("TYPY_LICENCI", "ID_TYPU_LICENCE", SelectedLicenceType.Id);
                        break;
                    case "LoanTypeDelete":
                        if (SelectedLoanType == null) return;
                        _lookupTablesService.DeleteRecord("TYPY_UVERU", "ID_TYPU_UVERU", SelectedLoanType.Id);
                        break;
                    case "CardTypeDelete":
                        if (SelectedCardType == null) return;
                        _lookupTablesService.DeleteRecord("TYPY_KARET", "ID_TYPU_KARTY", SelectedCardType.Id);
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


        private void ShowEditRecordInLookupTableView(string procedureName, ILookupEntry selectedItem)
        {
            var addViewModel = new AddIntoLookupTableViewModel(procedureName, selectedItem);
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
