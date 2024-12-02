using FinancniInformacniSystemBanky.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro AddCardView.xaml
    /// </summary>
    public partial class AddCardView : Window
    {
        public AddCardView()
        {
            InitializeComponent();
            this.DataContext = new AddCardViewModel();
        }

        // Pomocná metoda, která reguluje povolené znaky v komponentě TextBox
        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
