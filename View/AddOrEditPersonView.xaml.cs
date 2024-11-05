using FinancniInformacniSystemBanky.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace FinancniInformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro AddPersonView.xaml
    /// </summary>
    public partial class AddPersonView : Window
    {
        public AddPersonView()
        {
            InitializeComponent();
            this.DataContext = new AddOrEditUserViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox pb = sender as PasswordBox;
            if (DataContext is AddOrEditUserViewModel viewModel)
            {
                viewModel.Password = pb.Password;
            }
        }
    }
}