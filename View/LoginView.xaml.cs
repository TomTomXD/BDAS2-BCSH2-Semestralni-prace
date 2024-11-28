using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InformacniSystemBanky.View
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = new ViewModel.LoginViewModel(this);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModel.LoginViewModel viewModel)
            {
                viewModel.Heslo = ((PasswordBox)sender).Password;
            }
        }
    }
}