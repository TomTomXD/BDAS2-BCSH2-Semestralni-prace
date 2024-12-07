using FinancniInformacniSystemBanky.ViewModel;
using System.Windows;

namespace InformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro ClientAddCardView.xaml
    /// </summary>
    public partial class ClientAddCardView : Window
    {
        public ClientAddCardView()
        {
            InitializeComponent();
            this.DataContext = new ClientAddCardViewModel();
        }
    }
}
