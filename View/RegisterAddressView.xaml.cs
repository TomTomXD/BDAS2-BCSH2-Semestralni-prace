using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using InformacniSystemBanky.ViewModel;

namespace InformacniSystemBanky.View
{
    public partial class RegisterAddressView : Window
    {
        private UserRegistrationViewModel _viewModel;

        public RegisterAddressView(UserRegistrationViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = new RegisterAddressViewModel(this, _viewModel);
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

        // Pomocná metoda, která reguluje povolené znaky v komponentě TextBox
        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
