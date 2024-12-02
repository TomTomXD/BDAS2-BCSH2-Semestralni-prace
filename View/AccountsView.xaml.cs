using InformacniSystemBanky.ViewModel;
using System.Windows.Controls;

namespace InformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro AccountsView.xaml
    /// </summary>
    public partial class AccountsView : UserControl
    {
        public AccountsView()
        {
            InitializeComponent();
            this.DataContext = new AccountsViewModel();
        }
    }
}
