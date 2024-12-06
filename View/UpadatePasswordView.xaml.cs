using FinancniInformacniSystemBanky;
using FinancniInformacniSystemBanky.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro UpadatePasswordView.xaml
    /// </summary>
    public partial class UpdatePasswordView : Window
    {
        public UpdatePasswordView()
        {
            InitializeComponent();

        }
        private void UpdatePasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                var viewModel = this.DataContext as UpdatePasswordViewModel;
                if (viewModel != null)
                {
                    viewModel.Password = passwordBox.Password;
                }
            }
        }

    }
}
