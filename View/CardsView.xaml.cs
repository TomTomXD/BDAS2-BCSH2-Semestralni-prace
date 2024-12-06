using FinancniInformacniSystemBanky;
using FinancniInformacniSystemBanky.ViewModel;
using System.Windows.Controls;

namespace InformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro CardsView.xaml
    /// </summary>
    public partial class CardsView : UserControl
    {
        public CardsView()
        {
            InitializeComponent();
            this.DataContext = new CardsViewModel();
        }
    }
}
