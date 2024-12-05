using FinancniInformacniSystemBanky.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace FinancniInformacniSystemBanky.View
{
    public partial class CardsListView : UserControl
    {
        public CardsListView()
        {
            InitializeComponent();
            var viewModel = new CardsListViewModel();
            this.DataContext = viewModel;
            LoadCards(viewModel);
        }

        private void LoadCards(CardsListViewModel viewModel)
        {
            foreach (var cardViewModel in viewModel.Cards)
            {
                var cardView = new CardView
                {
                    DataContext = cardViewModel
                };
                cardView.Margin = new Thickness(10);
                CardsWrapPanel.Children.Add(cardView);
            }
        }
    }
}
