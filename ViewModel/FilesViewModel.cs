using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FinancniInformacniSystemBanky.ViewModel
{
    public class FilesViewModel : INotifyPropertyChanged
    {
        private readonly FileService _fileService;
        public ObservableCollection<File> Files { get; set; }
        private File _selecteFile;
        public File SelectedFile
        {
            get => _selecteFile;
            set
            {
                _selecteFile = value;
                OnPropertyChanged(nameof(SelectedFile));
            }
        }

        public ICommand AddFileCommand { get; }
        public ICommand DeleteFileCommand { get; }
        public ICommand EditFileCommnad { get; }
        public ICommand DownloadFileCommand { get; }

        public FilesViewModel()
        {
            _fileService = new FileService();
            Files = new ObservableCollection<File>();
            LoadFilesFromDatabase();

            DownloadFileCommand = new RelayCommand(DownloadFileFromDatabase);
        }

        private void LoadFilesFromDatabase()
        {
            var files = _fileService.GetFiles();
            Files.Clear();
            foreach (var file in files)
            {
                Files.Add(file);
            }
        }

        private void DownloadFileFromDatabase()
        {
            if (SelectedFile != null)
            {
                // kontrolní výpis pro ladění
                //var id = SelectedFile.FileId;
                //MessageBox.Show($"Downloading file with id: {id}");
                _fileService.SaveFileToDisk(SelectedFile.FileId);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
