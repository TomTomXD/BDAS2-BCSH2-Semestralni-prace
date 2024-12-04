using FinancniInformacniSystemBanky;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace InformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro AddBankingLicenceView.xaml
    /// </summary>
    public partial class AddBankingLicenceView : Window
    {
        public AddBankingLicenceView()
        {
            InitializeComponent();
            this.DataContext = new AddBankingLicenceViewModel();
        }

        // Pomocná metoda, která reguluje povolené znaky v komponentě TextBox
        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
