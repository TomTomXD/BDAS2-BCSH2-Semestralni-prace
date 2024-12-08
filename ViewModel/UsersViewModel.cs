using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using FinancniInformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class UsersViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Person> People { get; set; }
        public ICollectionView FilteredPeople { get; set; }
        private string _searchText;
        private Person selectedPerson;

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
        public Person SelectedPerson
        {
            get => selectedPerson;
            set
            {
                selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
            }
        }

        private bool _hideSensitiveData;
        public bool HideSensitiveData
        {
            get => _hideSensitiveData;
            set
            {
                _hideSensitiveData = value;
                OnPropertyChanged(nameof(HideSensitiveData));
                PerformActionOnCheckBoxChange();
            }
        }

        public ICommand AddPersonCommand { get; }
        public ICommand ChangePersonalDataCommand { get; }
        public ICommand DeletePersonCommand { get; }

        private readonly PersonService _personDetailsService;

        public UsersViewModel()
        {
            People = new ObservableCollection<Person>();
            FilteredPeople = CollectionViewSource.GetDefaultView(People);
            FilteredPeople.Filter = FilterPeople;

            var databaseService = new DatabaseService();
            _personDetailsService = new PersonService();

            LoadPeopleFromDatabase();

            AddPersonCommand = new RelayCommand(AddPerson);
            ChangePersonalDataCommand = new RelayCommand(EditPerson);
            DeletePersonCommand = new RelayCommand(DeletePerson);
        }

        private void PerformActionOnCheckBoxChange()
        {
            if (HideSensitiveData)
            {
                foreach (var person in People)
                {
                    // Schování jména a příjmení
                    person.Name = $"{person.Name[0]}{new string('*', person.Name.Length - 1)}";
                    person.Surname = $"{person.Surname[0]}{new string('*', person.Surname.Length - 1)}";
                    person.NationalIdNumber = "*********";
                }
            }
            else
            {
                //    LoadPeopleFromDatabase();
                var peopleFromDb = _personDetailsService.GetPersonDetails();

                People.Clear();
                foreach (var person in peopleFromDb)
                {
                    People.Add(person);
                }
            }
        }

        // Metoda pro načtení osob z databáze
        private void LoadPeopleFromDatabase()
        {
            var peopleFromDb = _personDetailsService.GetPersonDetails();

            People.Clear();
            foreach (var person in peopleFromDb)
            {
                People.Add(person);
                if (HideSensitiveData)
                {
                        // Schování jména a příjmení
                        person.Name = $"{person.Name[0]}{new string('*', person.Name.Length - 1)}";
                        person.Surname = $"{person.Surname[0]}{new string('*', person.Surname.Length - 1)}";
                        person.NationalIdNumber = "*********";
                }
            }
        }


        private bool FilterPeople(object obj)
        {
            if (obj is Person person)
            {
                return string.IsNullOrEmpty(SearchText) ||
                       person.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       person.Surname.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       person.DoB.ToString().Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       person.NationalIdNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       person.PhoneNumber.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                       person.Email.Contains(SearchText, StringComparison.OrdinalIgnoreCase); 
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
                if(HideSensitiveData == true)
                {
                    MessageBox.Show("Nelze upravit osobu, pokud jsou skryté citlivé informace. Odkryjte citlivé informace a abyste se ujistili, o kterou osobu se jedná.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var editUserViewModel = new AddOrEditUserViewModel(SelectedPerson);
                var editUserView = new AddPersonView()
                {
                    DataContext = editUserViewModel
                };
                editUserView.ShowDialog();
            }
            LoadPeopleFromDatabase();
        }

        private void DeletePerson()
        {
            if (IsPersonSelected())
            {
                if (HideSensitiveData == true)
                {
                    MessageBox.Show("Nelze odstranit osobu, pokud jsou skryté citlivé informace. Odkryjte citlivé informace a abyste se ujistili, o kterou osobu se jedná.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
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