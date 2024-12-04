using FinancniInformacniSystemBanky.ViewModel;
using InformacniSystemBanky.ViewModel;
using System.Windows.Controls;

namespace InformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro AccountsView.xaml
    /// </summary>
    public partial class AddressesView : UserControl
    {
        public AddressesView()
        {
            InitializeComponent();
            this.DataContext = new AddressesViewModel();
        }
    }
}
