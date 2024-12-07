using FinancniInformacniSystemBanky.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace FinancniInformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro ClientMainPage.xaml
    /// </summary>
    public partial class ClientMainPage : UserControl
    {
        public ClientMainPage()
        {
            InitializeComponent();
            this.DataContext = new ClientMainViewModel();
        }
    }
}
