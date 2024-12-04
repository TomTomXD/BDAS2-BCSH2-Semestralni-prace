using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class UsersViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<PersonDetails> People { get; set; }
        public ICollectionView FilteredPeople { get; set; }
        private string _searchText;
        private PersonDetails selectedPerson;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
                FilteredPeople.Refresh();
            }
        }
        public PersonDetails SelectedPerson
        {
            get => selectedPerson;
            set
            {
                selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
            }
        }

        public ICommand AddPersonCommand { get; }
        public ICommand ChangePersonalDataCommand { get; }
        public ICommand DeletePersonCommand { get; }

        private readonly PersonDetailsService _personDetailsService;

        public UsersViewModel()
        {
            People = new ObservableCollection<PersonDetails>();
            FilteredPeople = CollectionViewSource.GetDefaultView(People);
            FilteredPeople.Filter = FilterPeople;

            var databaseService = new DatabaseService();
            _personDetailsService = new PersonDetailsService();

            LoadPeopleFromDatabase();

            AddPersonCommand = new RelayCommand(AddPerson);
            ChangePersonalDataCommand = new RelayCommand(EditPerson);
            DeletePersonCommand = new RelayCommand(DeletePerson);
        }


        // Metoda pro načtení osob z databáze
        private void LoadPeopleFromDatabase()
        {
            var peopleFromDb = _personDetailsService.GetPersonDetails();

            People.Clear();
            foreach (var person in peopleFromDb)
            {
                People.Add(person);
            }
        }

        private bool FilterPeople(object obj)
        {
            if (obj is PersonDetails person)
            {
                return string.IsNullOrEmpty(SearchText) ||
                       person.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       person.Surname.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       person.DoB.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       person.NationalIdNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       person.PhoneNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       person.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       person.Role.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        // Obsluha tlačítka pro přidání osoby
        private void AddPerson()
        {
            var addUserViewModel = new AddOrEditUserViewModel();
            addUserViewModel.PersonAdded += OnPersonAdded;
            var addPersonView = new AddPersonView
            {
                DataContext = addUserViewModel
            };
            addPersonView.ShowDialog();
            LoadPeopleFromDatabase();
        }

        private void EditPerson()
        {
            if (IsPersonSelected())
            {
                var personDetailsService = new PersonDetailsService();
                AddOrEditUserViewModel addOrEditUserViewModel;
               
                if (personDetailsService.GetTypeOfPerson(SelectedPerson.NationalIdNumber) == 'Z')
                {
                    addOrEditUserViewModel = new AddOrEditUserViewModel(SelectedPerson, personDetailsService.GetEmployeeDetails(SelectedPerson.NationalIdNumber));
                }
                else
                {
                    addOrEditUserViewModel = new AddOrEditUserViewModel(SelectedPerson);
                }
               
                var addPersonView = new AddPersonView
                {
                    DataContext = addOrEditUserViewModel
                };
                addPersonView.ShowDialog();
            }
            LoadPeopleFromDatabase();
        }

        private void DeletePerson()
        {
            if (SelectedPerson == null)
            {
                MessageBox.Show("Vyberte osobu, kterou chcete smazat.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var rodneCislo = SelectedPerson.NationalIdNumber;

            var result = MessageBox.Show($"Opravdu chcete smazat osobu s rodným číslem {rodneCislo}?",
                                         "Smazání osoby", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var userService = new UserService();

                if (userService.DeleteUser(rodneCislo))
                {
                    LoadPeopleFromDatabase();
                }
            }
        }

        private bool IsPersonSelected()
        {
            if (SelectedPerson == null)
            {
                MessageBox.Show("Není vybrána žádná osoba.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnPersonAdded()
        {
            LoadPeopleFromDatabase();
        }
    }
}