using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.View
{
    public partial class DashboardAdminView : Window
    {
        public DashboardAdminView()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.DashboardAdminViewModel();
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
