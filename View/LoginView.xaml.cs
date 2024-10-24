using System.Windows;
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

        // Ponecháno v modelu pouze umožňuje přesun okna, žádná logika
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        // Ponecháno v modelu pro zavření okna, žádná logika
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}