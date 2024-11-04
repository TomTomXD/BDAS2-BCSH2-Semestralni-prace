using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.ViewModel;
using InformacniSystemBanky.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
