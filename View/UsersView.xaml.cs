using FinancniInformacniSystemBanky.ViewModel;
using System.Windows.Controls;

namespace FinancniInformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro UsersView.xaml
    /// </summary>
    public partial class UsersView : UserControl
    {
        public UsersView()
        {
            InitializeComponent();
            this.DataContext = new UsersViewModel();

        }

    }
}
