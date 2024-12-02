using System.ComponentModel;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class CardViewModel : INotifyPropertyChanged
    {
        private string _selectedCardNumber;

        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public string CVV { get; set; }

        public string SelectedCardNumber
        {
            get { return _selectedCardNumber; }
            set
            {
                if (_selectedCardNumber != value)
                {
                    _selectedCardNumber = value;
                    OnPropertyChanged(nameof(SelectedCardNumber));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
