using FinancniInformacniSystemBanky.ViewModel;
using System.Windows;

namespace FinancniInformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro DashboardEmployeeView.xaml
    /// </summary>
    public partial class DashboardEmployeeView : Window
    {
        public DashboardEmployeeView()
        {
            InitializeComponent();
            DataContext = new DashBoardEmployeeViewModel();
        }
    }
}
