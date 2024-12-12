using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.Model;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class AddAddressViewModel : INotifyPropertyChanged
    {
        private readonly AddressService _addressService;

        public ICommand AddNewAddressCommand { get; }
        public ICommand CancelAddingNewAddressCommand { get; }

        private string actionButtonText;
        private string actionLabelText;
        
        private int _addressId;
        private string _streetName;
        private string _houseNumber;
        private string _city;
        private string _postalCode;

        public string StreetName
        {
            get => _streetName;
            set { _streetName = value; OnPropertyChanged(nameof(StreetName)); }
        }
        public string HouseNumber
        {
            get => _houseNumber;
            set { _houseNumber = value; OnPropertyChanged(nameof(HouseNumber)); }
        }
        public string City
        {
            get => _city;
            set { _city = value; OnPropertyChanged(nameof(City)); }
        }
        public string PostalCode
        {
            get => _postalCode;
            set { _postalCode = value; OnPropertyChanged(nameof(PostalCode)); }
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

        public AddAddressViewModel()
        {
            _addressService = new AddressService();

            AddNewAddressCommand = new RelayCommand(AddAddressToDatabase);
            CancelAddingNewAddressCommand = new RelayCommand(CloseAddingWindow);

            ActionLabelText = "Přidání nové adresy";
            ActionButtonText = "Přidat";
        }
        public AddAddressViewModel(Address selectedAddress)
        {
            _addressService = new AddressService();

            AddNewAddressCommand = new RelayCommand(UpdateAddress);
            CancelAddingNewAddressCommand = new RelayCommand(CloseAddingWindow);

            _addressId = selectedAddress.Id;
            StreetName = selectedAddress.Street;
            HouseNumber = selectedAddress.HouseNumber;
            City = selectedAddress.City;
            PostalCode = selectedAddress.PostalCode;


            ActionLabelText = "Úprava adresy";
            ActionButtonText = "Upravit";
        }

        private void UpdateAddress()
        {
            _addressService.UpdateAddress(_addressId, StreetName, City, HouseNumber, PostalCode);
            CloseAddingWindow();
        }

        private void AddAddressToDatabase()
        {
            _addressService.AddAddress(StreetName, City, HouseNumber, PostalCode);
            CloseAddingWindow();
        }


        private void CloseAddingWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<AddAddressView>().FirstOrDefault();
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
