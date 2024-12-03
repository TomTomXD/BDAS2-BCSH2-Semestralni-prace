using FinancniInformacniSystemBanky.ViewModel;
using InformacniSystemBanky.ViewModel;
using System.Windows.Controls;

namespace InformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro AccountsView.xaml
    /// </summary>
    public partial class FilesView : UserControl
    {
        public FilesView()
        {
            InitializeComponent();
            this.DataContext = new FilesViewModel();
        }
    }
}
