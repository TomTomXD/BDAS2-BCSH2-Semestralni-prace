using FinancniInformacniSystemBanky.View;
using FinancniInformacniSystemBanky.ViewModel;
using System.Windows;

namespace InformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro EmulationView.xaml
    /// </summary>
    public partial class EmulationView : Window
    {
        public EmulationView()
        {
            InitializeComponent();
            this.DataContext = new EmulationViewModel();
        }
    }
}
