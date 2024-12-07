using FinancniInformacniSystemBanky.ViewModel;
using System.Windows;
using System.Windows.Input;

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
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
