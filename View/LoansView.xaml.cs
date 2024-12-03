using FinancniInformacniSystemBanky.ViewModel;
using System.Windows.Controls;

namespace InformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro LoansView.xaml
    /// </summary>
    public partial class LoansView : UserControl
    {
        public LoansView()
        {
            InitializeComponent();
            this.DataContext = new LoansViewModel();
        }
    }
}
