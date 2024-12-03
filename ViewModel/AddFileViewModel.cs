﻿using FinancniInformacniSystemBanky.DatabaseLayer;
using InformacniSystemBanky.View;
using InformacniSystemBanky.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            }
        }

        private void AddNewFile()
        {
            try
            {
                _fileService.UploadFile(null, FileName, DateOfUpload, FileContent, FileDescription, OwnerId);
                MessageBox.Show("Soubor byl úspěšně nahrán.", "Úspěch", MessageBoxButton.OK, MessageBoxImage.Information);
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
