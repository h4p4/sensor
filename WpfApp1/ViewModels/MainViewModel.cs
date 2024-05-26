namespace WpfApp1.ViewModels
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Windows;
    using System.Windows.Input;

    using Microsoft.Win32;

    using WpfApp1.Views;

    public class MainViewModel : ViewModel
    {
        private Root _root;

        public MainViewModel()
        {
            ReadFromJsonCommand = new RelayCommand(ReadFromJson);
            WriteToDatabaseCommand = new RelayCommand(WriteToDatabase);
            RemoveFromDatabaseCommand = new RelayCommand(RemoveFromDatabase);
            Roots = new ObservableCollection<Root>();
        }

        private void RemoveFromDatabase(object obj)
        {
        }

        private void WriteToDatabase(object obj)
        {
        }

        private void ReadFromJson(object obj)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Json files (*.json)|*.json"
            };

            ofd.ShowDialog();
            var fileName = ofd.FileName;
            if (fileName == "")
            {
                MessageBox.Show("Файл не выбран", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var fileData = File.ReadAllText(fileName);
                var root = JsonConvert.DeserializeObject<Root>(fileData);
                Logger.Enable(false);
                Roots.Add(root);
                Root = root;
                Logger.Enable();
            }
            catch (Exception)
            {
                MessageBox.Show("Файл не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public Logger Logger { get; set; } = Logger.Instance;

        public Root Root
        {
            get => _root;
            set => SetField(ref _root, value);
        }

        public ObservableCollection<Root> Roots { get; set; }

        public ICommand ReadFromJsonCommand { get; }
        public ICommand WriteToDatabaseCommand { get; }
        public ICommand RemoveFromDatabaseCommand { get; }
    }
}