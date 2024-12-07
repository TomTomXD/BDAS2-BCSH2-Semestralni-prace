using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.View
{
    public partial class DashboardKlientView : Window
    {
        public DashboardKlientView()
        {
            InitializeComponent();
            this.DataContext = new ViewModel.DashboardClientViewModel();
        }

        // Allow window dragging functionality
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}
