using System;
using System.Collections.Generic;
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
    /// Interakční logika pro CardsListView.xaml
    /// </summary>
    public partial class CardsListView : UserControl
    {
        public CardsListView()
        {
            InitializeComponent();
            AddCards();
        }

        private void AddCards()
        {
            for (int i = 0; i < 10; i++) // Například přidání 10 karet
            {
                var cardView = new CardView(); // Vytvoření instance `CardView`
                cardView.Margin = new Thickness(10); // Přidání okrajů mezi kartami
                CardsWrapPanel.Children.Add(cardView); // Přidání do `WrapPanel`
            }
        }
    }

}
