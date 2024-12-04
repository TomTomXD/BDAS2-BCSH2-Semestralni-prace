using FinancniInformacniSystemBanky.ViewModel;
using InformacniSystemBanky.ViewModel;
using System.Windows.Controls;

namespace InformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro AccountsView.xaml
    /// </summary>
    public partial class SystemCatalogView : UserControl
    {
        public SystemCatalogView()
        {
            InitializeComponent();
            this.DataContext = new SystemCatalogViewModel();
        }

    }
}
