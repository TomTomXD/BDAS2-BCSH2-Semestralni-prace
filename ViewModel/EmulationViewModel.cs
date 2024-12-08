using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.View;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class EmulationViewModel : INotifyPropertyChanged
    {
        private PersonService _personService;

        private Person _selectedPerson;

        public ObservableCollection<Person> People { get; set; }
        public ICommand EmulateCommand { get; }
        public ICommand CloseEmulationWindow { get; }
        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
            }
        }

        public EmulationViewModel()
        {
            _personService = new PersonService();
            People = new ObservableCollection<Person>(_personService.GetPersonDetails());
            EmulateCommand = new RelayCommand(Emulate);
            CloseEmulationWindow = new RelayCommand(CloseWindow);
        }

        private void CloseWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<EmulationView>().FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }

        private void Emulate()
        {
            Session.Instance.EmulateUser(SelectedPerson.Id, SelectedPerson.Role.Id);

            // Přihlášení úspěšné, otevřete zatím admin dashboard pro testování a ladění loginů
            switch (Session.Instance.EmulatedRoleId)
            {
                case 1:
                    var dashboardKlientView = new DashboardKlientView();
                    dashboardKlientView.Show();
                    break;
                case 2:
                    var dashboardNotVerifiedClientView = new DashboardNotVerifiedClientView();
                    dashboardNotVerifiedClientView.Show();
                    break;
                case 3:
                    var dashboardEmployeeView = new DashboardEmployeeView();
                    dashboardEmployeeView.Show();
                    break;
                case 4:
                    var dashboardAdminView = new DashboardAdminView();
                    dashboardAdminView.Show();
                    break;
                default:
                    MessageBox.Show("Nepodařilo se emulovat tuto osobu.", "Chyba přihlášení", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
            CloseWindow();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
