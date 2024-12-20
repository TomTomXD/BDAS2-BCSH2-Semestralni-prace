﻿using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.Model;
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
    public class AddressesViewModel : INotifyPropertyChanged
    {
        private readonly AddressService _addressService;
        private Address _selectedAddress;
        public Address SelectedAddress
        {
            get => _selectedAddress;
            set
            {
                _selectedAddress = value;
                OnPropertyChanged(nameof(SelectedAddress));
            }
        }

        public ICommand AddAddressCommand { get; }
        public ICommand EditAddressCommand { get; }
        public ICommand DeleteAddressCommand { get; }

        public ObservableCollection<Address> Addresses { get; set; }

        public AddressesViewModel()
        {
            _addressService = new AddressService();

            AddAddressCommand = new RelayCommand(AddAddressToDatabase);
            EditAddressCommand = new RelayCommand(EditAddress);
            DeleteAddressCommand = new RelayCommand(DeleteAddressFromDatabase, CanDeleteAddress);

            Addresses = new ObservableCollection<Address>();
            LoadAddressesFromDatabase();
        }


        private void DeleteAddressFromDatabase()
        {
            if(CanDeleteAddress())
            {
                //MessageBox.Show($"Selected address: {SelectedAddress.Id}, {SelectedAddress.City}");
                _addressService.DeleteAddress(SelectedAddress.Id);
                LoadAddressesFromDatabase();
            }
        }

        private void EditAddress()
        {
            if(SelectedAddress != null)
            {
                var editAddressViewModel = new AddAddressViewModel(SelectedAddress);
                var editAddressView = new AddAddressView()
                {
                    DataContext = editAddressViewModel
                };
                editAddressView.ShowDialog();
                LoadAddressesFromDatabase();
            }
        }

        private void AddAddressToDatabase()
        {
            var addAddressView = new AddAddressView();
            addAddressView.ShowDialog();
            LoadAddressesFromDatabase();
        }

        private void LoadAddressesFromDatabase()
        {
            var loadedAdressesFromDb = _addressService.GetAddresses();
            Addresses.Clear();
            foreach (var address in loadedAdressesFromDb)
            {
                Addresses.Add(address);
            }
        }

        private bool CanDeleteAddress()
        {
            return SelectedAddress != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
