﻿using FinancniInformacniSystemBanky.DatabaseLayer;
using FinancniInformacniSystemBanky.Model;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        public ICommand EditFileCommand { get; }
        public ICommand DownloadFileCommand { get; }

        public FilesViewModel()
        {
            _fileService = new FileService();
            Files = new ObservableCollection<File>();
            LoadFilesFromDatabase();

            DownloadFileCommand = new RelayCommand(DownloadFileFromDatabase);
            AddFileCommand = new RelayCommand(AddFileToDatabase);
            EditFileCommand = new RelayCommand(EditFile);
            DeleteFileCommand = new RelayCommand(DeleteFileFromDatabase, CanDeleteFile);
        }

        private void EditFile()
        {
            if(SelectedFile != null)
            {
                var AddFileViewModel = new AddFileViewModel(SelectedFile);
                var addFileView = new AddFileView()
                {
                    DataContext = AddFileViewModel
                };
                addFileView.ShowDialog();
                LoadFilesFromDatabase();
            }
        }

        private void DeleteFileFromDatabase()
        {
            if (CanDeleteFile())
            {
                _fileService.DeleteFile(SelectedFile.FileId);
                LoadFilesFromDatabase();
            }
        }

        private void AddFileToDatabase()
        {
            var addFileView = new AddFileView();
            addFileView.ShowDialog();
            LoadFilesFromDatabase();
        }

        private void LoadFilesFromDatabase()
        {
            IEnumerable<File> files;

            // Klienti vidí pouze své soubory
            if (Session.Instance.EmulatedRoleId == 1 || Session.Instance.CurrentRoleId == 1)
            {
                 files = _fileService.GetFilesById(Session.Instance.EmulatedUserId ?? Session.Instance.CurrentUserId);
            }
            else
            {
                // Zaměstnanci a admin vidí všechny soubory
                files = _fileService.GetFiles();
            }

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
                _fileService.SaveFileToDisk(SelectedFile.FileId);
            }
        }
        private bool CanDeleteFile()
        {
            
            return SelectedFile != null;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
