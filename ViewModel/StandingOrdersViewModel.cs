using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class StandingOrdersViewModel : INotifyPropertyChanged
    {
        private readonly StandingOrderService _standingOrderService;

        public ObservableCollection<StandingOrder> StandingOrders { get; set; }

        private StandingOrder _selectedStandingOrder;
        public StandingOrder SelectedStandingOrder
        {
            get => _selectedStandingOrder;
            set
            {
                _selectedStandingOrder = value;
                OnPropertyChanged(nameof(SelectedStandingOrder));
            }
        }

        public ICommand AddStandingOrderCommand { get; }
        public ICommand EditStandingOrderCommand { get; }
        public ICommand DeleteStandingOrderCommand { get; }

        public StandingOrdersViewModel()
        {
            _standingOrderService = new StandingOrderService();
            StandingOrders = new ObservableCollection<StandingOrder>();

            AddStandingOrderCommand = new RelayCommand(AddStandingOrderToDatabase);
            EditStandingOrderCommand = new RelayCommand(EditStandingOrder);
            DeleteStandingOrderCommand = new RelayCommand(DeleteStandingOrderFromDatabase, CanDeleteStandingOrder);

            LoadStandingOrdersFromDatabase();
        }

        private void AddStandingOrderToDatabase()
        {
            var addStandingView = new AddStandingOrderView();
            addStandingView.ShowDialog();
            LoadStandingOrdersFromDatabase();
        }

        private void EditStandingOrder()
        {
            if(SelectedStandingOrder != null)
            {
                var editStandingViewModel = new AddStandingOrderViewModel(SelectedStandingOrder);
                var editStandingView = new AddStandingOrderView()
                {
                    DataContext = editStandingViewModel
                };
                editStandingView.ShowDialog();
                LoadStandingOrdersFromDatabase();
            }
        }

        private void DeleteStandingOrderFromDatabase()
        {
            if (CanDeleteStandingOrder())
            {
                _standingOrderService.DeleteStandingOrder(SelectedStandingOrder.StandingOrderId);
                LoadStandingOrdersFromDatabase();
            }
        }

        private bool CanDeleteStandingOrder()
        {
            return SelectedStandingOrder != null;
        }

        private void LoadStandingOrdersFromDatabase()
        {
            var standingOrders = _standingOrderService.GetStandingOrders();
            StandingOrders.Clear();
            foreach (var standingOrder in standingOrders)
            {
                StandingOrders.Add(standingOrder);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
