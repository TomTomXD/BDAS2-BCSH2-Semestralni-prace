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

        private int _originalUserId;
        private int _originalRoleId;
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

            People.Where(p => p.Id == Session.Instance.CurrentUserId).ToList().ForEach(p => People.Remove(p));
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
            // Uložení aktuálních hodnot
            _originalUserId = Session.Instance.CurrentUserId;
            _originalRoleId = Session.Instance.CurrentRoleId;

            // Nastavení emulovaného uživatele
            Session.Instance.EmulateUser(SelectedPerson.Id, SelectedPerson.Role.Id);

            // Otevírání dashboardu podle role
            switch (Session.Instance.EmulatedRoleId)
            {
                case 1:
                    var dashboardKlientView = new DashboardKlientView();
                    CloseWindow();
                    dashboardKlientView.Show();
                    break;
                case 2:
                    var dashboardNotVerifiedClientView = new DashboardNotVerifiedClientView();
                    CloseWindow();
                    dashboardNotVerifiedClientView.Show();
                    break;
                case 3:
                    var dashboardEmployeeView = new DashboardEmployeeView();
                    CloseWindow();
                    dashboardEmployeeView.Show();
                    break;
                case 4:
                    var dashboardAdminView = new DashboardAdminView();
                    CloseWindow();
                    dashboardAdminView.Show();
                    break;
                default:
                    MessageBox.Show("Nepodařilo se emulovat tuto osobu.", "Chyba přihlášení", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }

            // Obnovení původního uživatele po skončení emulace
            RestoreOriginalUser();
        }

        private void RestoreOriginalUser()
        {
            Session.Instance.SetUser(_originalUserId, _originalRoleId);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
