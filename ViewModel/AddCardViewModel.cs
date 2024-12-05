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
    public class AddCardViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<CardType> _cardTypes;
        private readonly StandingOrderService _standingOrderService;

        private CardType _selectedCardType;

        private readonly CardService _cardService;

        private NormalAccount _selectedNormalAccount;
        public ObservableCollection<NormalAccount> NormalAccounts { get; set; }

        private string actionButtonText;
        private string actionLabelText;

        private int _idKarty;
        private string _cardNumber;
        private DateTime _dateOfIssue;
        private DateTime _expirationDate;
        private int _owner;
        private string _cvv;
        private int _cardTypeId;

        public string CardNumber
        {
            get => _cardNumber;
            set { _cardNumber = value; OnPropertyChanged(nameof(CardNumber)); }
        }
        public DateTime DateOfIssue
        {
            get => _dateOfIssue;
            set { _dateOfIssue = value; OnPropertyChanged(nameof(DateOfIssue)); }
        }
        public DateTime ExpirationDate
        {
            get => _expirationDate;
            set { _expirationDate = value; OnPropertyChanged(nameof(ExpirationDate)); }
        }
        public int Owner
        {
            get => _owner;
            set { _owner = value; OnPropertyChanged(nameof(Owner)); }
        }
        public string CVV
        {
            get => _cvv;
            set { _cvv = value; OnPropertyChanged(nameof(CVV)); }
        }
        public int CardTypeId
        {
            get => _cardTypeId;
            set { _cardTypeId = value; OnPropertyChanged(nameof(CardTypeId)); }
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
        public ICommand CancelAddingNewCardCommand { get; }
        public ICommand AddNewCardCommand { get; }
        public ObservableCollection<CardType> CardTypes
        {
            get => _cardTypes;
            set
            {
                _cardTypes = value;
                OnPropertyChanged(nameof(CardTypes));
            }
        }
        public CardType SelectedCardType
        {
            get => _selectedCardType;
            set
            {
                _selectedCardType = value;
                OnPropertyChanged(nameof(SelectedCardType));
            }
        }
        public NormalAccount SelectedNormalAccount
        {
            get => _selectedNormalAccount;
            set { _selectedNormalAccount = value; OnPropertyChanged(nameof(SelectedNormalAccount)); }
        }

        public AddCardViewModel()
        {
            _cardService = new CardService();
            _standingOrderService = new StandingOrderService();
            
            CancelAddingNewCardCommand = new RelayCommand(CloseAddingWindow);
            AddNewCardCommand = new RelayCommand(AddNewCard);
            
            CardNumber = _cardService.GenerateCardNumber(53);
            NormalAccounts = new ObservableCollection<NormalAccount>(_standingOrderService.GetAllNormalAccounts());


            DateOfIssue = DateTime.Now;
            ExpirationDate = DateOfIssue.AddYears(4);

            ActionButtonText = "Přidat";
            ActionLabelText = "Přidání nové karty";

            LoadCardTypes();
        }

        public AddCardViewModel(Card selectedCard)
        {
            _cardService = new CardService();
            _standingOrderService = new StandingOrderService();

            CancelAddingNewCardCommand = new RelayCommand(CloseAddingWindow);
            AddNewCardCommand = new RelayCommand(EditCard);

            LoadCardTypes();
            NormalAccounts = new ObservableCollection<NormalAccount>(_standingOrderService.GetAllNormalAccounts());

            _idKarty = selectedCard.CardId;
            CardNumber = selectedCard.CardNumber;
            CVV = selectedCard.CVV;

            // Najdeme odpovídající typ karty podle ID typu karty z selectedCard
            SelectedCardType = CardTypes.First(x => x.CardTypeId == Convert.ToInt32(selectedCard.CardType));

            DateOfIssue = selectedCard.IssuedDate;
            ExpirationDate = selectedCard.ExpirationDate;

            ActionButtonText = "Upravit";
            ActionLabelText = "Upravit kartu";

            // Najdeme odpovídající účet podle ID účtu z selectedCard
            SelectedNormalAccount = NormalAccounts.First(x => x.AccountId == selectedCard.AccountId);
        }

        private void EditCard()
        {
            _cardService.UpdateCard( 
                _idKarty,
                CardNumber, 
                DateOfIssue, 
                ExpirationDate, 
                CVV, 
                SelectedCardType.CardTypeId, 
                SelectedNormalAccount.AccountId);
            CloseAddingWindow();
        }

        private void LoadCardTypes()
        {
            var cardTypes = _cardService.GetCardTypes();
            CardTypes = new ObservableCollection<CardType>(cardTypes);
        }

        private void AddNewCard()
        {
            _cardService.AddCard(
               CardNumber, DateOfIssue, ExpirationDate, CVV, SelectedCardType.CardTypeId, SelectedNormalAccount.AccountId);
            CloseAddingWindow();
        }

        private void CloseAddingWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<AddCardView>().FirstOrDefault();
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
