using FinancniInformacniSystemBanky.ViewModel;
using System.Windows.Controls;

namespace InformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro BankingLicenceView.xaml
    /// </summary>
    public partial class BankingLicencesView : UserControl
    {
        public BankingLicencesView()
        {
            InitializeComponent();
            this.DataContext = new BankingLicencesViewModel();
        }
    }
}
