﻿using FinancniInformacniSystemBanky.ViewModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.View
{
    /// <summary>
    /// Interakční logika pro ClientEditPersonalDetailsView.xaml
    /// </summary>
    public partial class ClientEditPersonalDetailsView : Window
    {
        public ClientEditPersonalDetailsView()
        {
            InitializeComponent();
            this.DataContext = new ClientEditPersonalDetailsViewModel();
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AddOrEditUserViewModel viewModel)
            {
                PasswordBox pb = sender as PasswordBox;
                if (pb != null)
                {
                    viewModel.Password = pb.Password;
                }
            }
        }

        // Pomocná metoda, která reguluje povolené znaky v komponentě TextBox
        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}