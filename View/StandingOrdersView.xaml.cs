using FinancniInformacniSystemBanky.ViewModel;
using System.Windows.Controls;

namespace InformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro StandingOrdersView.xaml
    /// </summary>
    public partial class StandingOrdersView : UserControl
    {
        public StandingOrdersView()
        {
            InitializeComponent();
            this.DataContext = new StandingOrdersViewModel();
        }
    }
}
