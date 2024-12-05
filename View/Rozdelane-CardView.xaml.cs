using FinancniInformacniSystemBanky.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace FinancniInformacniSystemBanky.View
{
    public partial class CardView : UserControl
    {
        // Statická instance generátoru náhodných čísel
        private static readonly Random RandomGenerator = new Random();

        public CardView()
        {
            InitializeComponent();
            SetRandomBackgroundColor();
            this.DataContext = new CardViewModel();
        }

        private void SetRandomBackgroundColor()
        {
            // Generování náhodné barvy
            byte r = (byte)RandomGenerator.Next(256);
            byte g = (byte)RandomGenerator.Next(256);
            byte b = (byte)RandomGenerator.Next(256);
            Color randomColor = Color.FromRgb(r, g, b);

            CardBorder.Background = new SolidColorBrush(randomColor);
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Změní barvu okraje na modrou při kliknutí
            CardBorder.BorderBrush = Brushes.Blue;
        }
    }
}
