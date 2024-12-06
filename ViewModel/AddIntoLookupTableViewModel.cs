using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model.Helpers;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class AddIntoLookupTableViewModel :INotifyPropertyChanged
    {
        private readonly LookupTablesService _lookupTableService;

        private string actionButtonText;
        private string actionLabelText;

        public ICommand AddIntoLookupTableCommand { get; set; }
        public ICommand CancelAddingIntoLookupTableCommand { get; set; }

        private string _storedProcedure;
        private int _id;
        private string _name;
        public string Name
        {
            get => _name;
            set { _name = value; OnPropertyChanged(nameof(Name)); }
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

        public AddIntoLookupTableViewModel(string storedProcedure)
        {
            _lookupTableService = new LookupTablesService();

            AddIntoLookupTableCommand = new RelayCommand(AddIntoLookupTable);
            CancelAddingIntoLookupTableCommand = new RelayCommand(CloseAddingWindow);

            _storedProcedure = storedProcedure;

            ActionLabelText = "Přidávání do číselníku";
            ActionButtonText = "Přidat";
        }

        public AddIntoLookupTableViewModel(string storedProcedure, ILookupEntry selectedItem)
        {
            _lookupTableService = new LookupTablesService();

            AddIntoLookupTableCommand = new RelayCommand(UpdateRecord);
            CancelAddingIntoLookupTableCommand = new RelayCommand(CloseAddingWindow);

            _storedProcedure = storedProcedure;

            Name = selectedItem.Name;
            _id = selectedItem.Id;

            ActionLabelText = "Úprava hodnoty číselníku";
            ActionButtonText = "Upravit";
        }



        private void AddIntoLookupTable()
        {
            if(string.IsNullOrEmpty(Name))
            {
                MessageBox.Show("Název nesmí být prázdný", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _lookupTableService.InsertIntoLookupTable(_storedProcedure, Name);
            CloseAddingWindow();
        }

        private void UpdateRecord()
        {
            if(string.IsNullOrEmpty(Name))
            {
                MessageBox.Show("Název nesmí být prázdný", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            _lookupTableService.UpdateLookupTable(_storedProcedure, _id, Name);
            CloseAddingWindow();
        }

        private void CloseAddingWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<AddIntoLookupTableView>().FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
