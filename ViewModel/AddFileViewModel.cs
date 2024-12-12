using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class AddFileViewModel : INotifyPropertyChanged
    {
        private readonly FileService _fileService;
        public ICommand CancelAddingNewFileCommand { get; }
        public ICommand AddNewFileCommand { get; }
        public ICommand AddFileCommand { get; }

        private string _fileDescription;
        private string _filePath;
        private string _fileName;
        private int _ownerId;
        private bool _isDatePickerEnabled;
        private int fileId;
        private string _fileType;

        public bool IsDatePickerEnabled
        {
            get => _isDatePickerEnabled;
            set { _isDatePickerEnabled = value; OnPropertyChanged(nameof(IsDatePickerEnabled)); }
        }
        public string FileDescription
        {
            get => _fileDescription;
            set { _fileDescription = value; OnPropertyChanged(nameof(FileName)); }
        }
        public string FilePath
        {
            get => _filePath;
            set { _filePath = value; OnPropertyChanged(nameof(FilePath)); }
        }
        public string FileName
        {
            get => _fileName;
            set { _fileName = value; OnPropertyChanged(nameof(FileName)); }
        }
        public int OwnerId
        {
            get => _ownerId;
            set { _ownerId = value; OnPropertyChanged(nameof(OwnerId)); }
        }
        public DateTime DateOfUpload { get; set; }

        private string actionButtonText;
        private string actionLabelText;

        public string ActionLabelText
        {
            get => actionLabelText;
            set { actionLabelText = value; OnPropertyChanged(nameof(ActionLabelText)); }
        }
        public string ActionButtonText
        {
            get => actionButtonText;
            set { actionButtonText = value; OnPropertyChanged(nameof(ActionButtonText)); }
        }

        public ObservableCollection<PossibleFileOwner> PossibleFileOwners { get; set; }
        private PossibleFileOwner _SelectedPossibleFileOwner;
        public PossibleFileOwner SelectedPossibleFileOwner
        {
            get => _SelectedPossibleFileOwner;
            set
            {
                _SelectedPossibleFileOwner = value;
                OwnerId = value.Id;
                OnPropertyChanged(nameof(SelectedPossibleFileOwner));
            }
        }

        private bool _changeUser;
        public bool ChangeUser
        {
            get => _changeUser;
            set
            {
                _changeUser = value;
                OnPropertyChanged(nameof(ChangeUser));
            }
        }

        public byte[] FileContent { get; set; }

        public AddFileViewModel()
        {
            _fileService = new FileService();
            CancelAddingNewFileCommand = new RelayCommand(CloseAddingWindow);
            AddNewFileCommand = new RelayCommand(AddNewFile);
            AddFileCommand = new RelayCommand(AddFile);

            IsDatePickerEnabled = false;
            DateOfUpload = DateTime.Now;
            ActionButtonText = "Nahrát";
            ActionLabelText = "Nahrání nového souboru";
            ChangeUser = true;

            PossibleFileOwners = new ObservableCollection<PossibleFileOwner>(_fileService.GetPossibleFileOwners());
            
            if(Session.Instance.EmulatedRoleId == 1 || Session.Instance.CurrentRoleId == 1)
            {
                SelectedPossibleFileOwner = PossibleFileOwners.FirstOrDefault(x => x.Id == (Session.Instance.EmulatedRoleId ?? Session.Instance.CurrentUserId));
                ChangeUser = false;
            }
        }

        public AddFileViewModel(Model.File selectedFile)
        {
            _fileService = new FileService();
            CancelAddingNewFileCommand = new RelayCommand(CloseAddingWindow);
            AddNewFileCommand = new RelayCommand(UpdateFile);
            AddFileCommand = new RelayCommand(AddFile);


            IsDatePickerEnabled = false;
            DateOfUpload = DateTime.Now;

            // nastavení hodnot z vybraného souboru
            fileId = selectedFile.FileId;

            FilePath = selectedFile.FileName;
            FileDescription = selectedFile.Note;
            FileName = selectedFile.FileName;
            DateOfUpload = selectedFile.UploadDate;
            PossibleFileOwners = new ObservableCollection<PossibleFileOwner>(_fileService.GetPossibleFileOwners());
            SelectedPossibleFileOwner = PossibleFileOwners.FirstOrDefault(x => x.Id == selectedFile.Owner.OwnerId);
            ChangeUser = true;

            if (Session.Instance.EmulatedRoleId == 1 || Session.Instance.CurrentRoleId == 1)
            {
                SelectedPossibleFileOwner = PossibleFileOwners.FirstOrDefault(x => x.Id == (Session.Instance.EmulatedRoleId ?? Session.Instance.CurrentUserId));
                ChangeUser = false;
            }

            ActionButtonText = "Upravit";
            ActionLabelText = "Úprava uloženého souboru";
        }

        private void UpdateFile()
        {
            // Kontrola prázdnosti polí
            if (string.IsNullOrEmpty(FileName) ||
                string.IsNullOrEmpty(FileDescription) ||
                SelectedPossibleFileOwner == null ||
                FileContent == null)
            {
                MessageBox.Show("Prosím vyplňte všechny povinné údaje.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                _fileService.UpdateFile(fileId, FileName, DateOfUpload, FileContent, FileDescription, OwnerId);
                CloseAddingWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo k chybě při upravování souboru: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddFile()
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Vyberte soubor k nahrání",
                Filter = "Obrázky a PDF soubory (*.jpg;*.jpeg;*.png;*.pdf)|*.jpg;*.jpeg;*.png;*.pdf"
            };

            if(openFileDialog.ShowDialog() == true)
            {
                FilePath = openFileDialog.FileName;
                FileName = openFileDialog.SafeFileName;
                FileContent = System.IO.File.ReadAllBytes(FilePath);
                _fileType = Path.GetExtension(FilePath).TrimStart('.');
            }
        }

        private void AddNewFile()
        {
            // Kontrola prázdnosti polí
            if (string.IsNullOrEmpty(FileName) ||
                string.IsNullOrEmpty(FileDescription) ||
                SelectedPossibleFileOwner == null ||
                FileContent == null)
            {
                MessageBox.Show("Prosím vyplňte všechny povinné údaje.", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                _fileService.UploadFile(null, FileName, DateOfUpload, FileContent, FileDescription, SelectedPossibleFileOwner.Id, _fileType);
                CloseAddingWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Došlo k chybě při nahrávání souboru: {ex.Message}", "Chyba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelAddingNewFile()
        {
            throw new NotImplementedException();
        }

        private void CloseAddingWindow()
        {
            var currentWindow = Application.Current.Windows.OfType<AddFileView>().FirstOrDefault();
            if (currentWindow != null)
            {
                currentWindow.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}