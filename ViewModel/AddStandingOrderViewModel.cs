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
    public class AddStandingOrderViewModel : INotifyPropertyChanged
    {
        private readonly StandingOrderService _standingOrderService;

        private NormalAccount _selectedNormalAccount;

        public ObservableCollection<NormalAccount> NormalAccounts { get; set; }

        private string actionButtonText;
        private string actionLabelText;

        private StandingOrder _selectedStandingOrder;

        private int _standingOrderId;
        private decimal _amount;
        private int _sendersAccountId;

        public decimal Amount
        {
            get => _amount;
            set { _amount = value; OnPropertyChanged(nameof(Amount)); }
        }
        public int SendersAccountId
        {
            get => _sendersAccountId;
            set { _sendersAccountId = value; OnPropertyChanged(nameof(SendersAccountId)); }
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
        public NormalAccount SelectedNormalAccount
        {
            get => _selectedNormalAccount;
            set { _selectedNormalAccount = value; OnPropertyChanged(nameof(SelectedNormalAccount)); }
        }


        public ICommand CancelAddingNewStandingOrderCommand { get; }
        public ICommand AddNewStandingOrderCommand { get; }

        public AddStandingOrderViewModel()
        {
            _standingOrderService = new StandingOrderService();
            AddNewStandingOrderCommand = new RelayCommand(AddStandingOrderToDatabase);
            CancelAddingNewStandingOrderCommand = new RelayCommand(CloseAddingWindow);

            NormalAccounts = new ObservableCollection<NormalAccount>(_standingOrderService.GetAllNormalAccounts());

            ActionLabelText = "Přidání trvalého příkazu";
            ActionButtonText = "Přidat";
        }


        public AddStandingOrderViewModel(StandingOrder standingOrder)
        {
            _standingOrderService = new StandingOrderService();
            AddNewStandingOrderCommand = new RelayCommand(UpdateStandingOrder);
            CancelAddingNewStandingOrderCommand = new RelayCommand(CloseAddingWindow);

            _standingOrderId = standingOrder.StandingOrderId;
            Amount = standingOrder.Amount;
            SendersAccountId = standingOrder.SendersAccountId;

            NormalAccounts = new ObservableCollection<NormalAccount>(_standingOrderService.GetAllNormalAccounts());

            SelectedNormalAccount = NormalAccounts.First(x => x.AccountId == standingOrder.SendersAccountId);

            ActionLabelText = "Úprava trvalého příkazu";
            ActionButtonText = "Upravit";
        }

        private void UpdateStandingOrder()
        {
            _standingOrderService.EditStandingOrder(_standingOrderId, Amount, SelectedNormalAccount.AccountId);
            CloseAddingWindow();
        }
        private void AddStandingOrderToDatabase()
        {
            _standingOrderService.AddStandingOrder(Amount, SelectedNormalAccount.AccountId);
            CloseAddingWindow();
        }

        private void CloseAddingWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<AddStandingOrderView>().FirstOrDefault();
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
