using InformacniSystemBanky.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.View
{
    public partial class RegisterPersonalDetailsView : Window
    {
        private UserRegistrationViewModel _viewModel;

        public RegisterPersonalDetailsView(UserRegistrationViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = new RegisterPersonalDetailsViewModel(this, _viewModel);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
                this.DragMove();
        }

        // Pomocná metoda, která reguluje povolené znaky v komponentě TextBox
        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
