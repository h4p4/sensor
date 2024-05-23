namespace WpfApp1
{
    using System;
    using System.IO;
    using System.Windows;

    using Microsoft.Win32;

    using Newtonsoft.Json;

    using WpfApp1.ViewModels;
    using WpfApp1.Views;

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public static string fileName;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Json files (*.json)|*.json";
            ofd.ShowDialog();
            fileName = ofd.FileName;
            if (fileName == "")
            {
                MessageBox.Show("Файл не выбран", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var fileData = File.ReadAllText(fileName);
                var root = JsonConvert.DeserializeObject<Root>(fileData);
                var sensorInfo = new SensorInfo
                {
                    DataContext = new MainViewModel()
                    {
                        Root = root
                    }
                };

                Logger.Instance.Enable();
                sensorInfo.Show();
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Файл не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}