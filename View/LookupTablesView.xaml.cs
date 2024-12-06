using FinancniInformacniSystemBanky.ViewModel;
using InformacniSystemBanky.ViewModel;
using System.Windows.Controls;

namespace InformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro AccountsView.xaml
    /// </summary>
    public partial class LookupTablesView : UserControl
    {
        public LookupTablesView()
        {
            InitializeComponent();
            this.DataContext = new LookupTablesViewModel();
        }
    }
}
