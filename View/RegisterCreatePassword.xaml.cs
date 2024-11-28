// View/RegisterCreatePasswordView.xaml.cs
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using InformacniSystemBanky.ViewModel;

namespace InformacniSystemBanky.View
{
    public partial class RegisterCreatePasswordView : Window
    {
        private UserRegistrationViewModel _viewModel;

        public RegisterCreatePasswordView(UserRegistrationViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = new RegisterCreatePasswordViewModel(this, _viewModel);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var addressView = new RegisterAddressView(_viewModel);
            addressView.Show();
            this.Close();
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
            if (DataContext is RegisterCreatePasswordViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }

        private void PasswordBox_PasswordAgainChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is RegisterCreatePasswordViewModel viewModel)
            {
                viewModel.PasswordAgain = ((PasswordBox)sender).Password;
            }
        }

    }
}
