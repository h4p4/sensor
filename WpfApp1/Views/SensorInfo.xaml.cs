namespace WpfApp1.Views
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Forms;

    using Newtonsoft.Json;

    using WpfApp1.ViewModels;

    using MessageBox = System.Windows.MessageBox;

    public partial class SensorInfo
    {
        //private EditCommand _editCommand;
        //private EditForm _editForm;
        //private EditSeque _editSeque;
        //private ViewData _viewData;

        public SensorInfo()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var saveFolder = SaveHelper.GetSaveFolder();
            if (string.IsNullOrWhiteSpace(saveFolder))
            {
                MessageBox.Show("Не выбрана папка сохранения!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var json = JsonConvert.SerializeObject(((MainViewModel)DataContext).Root);

            var saveFile = Path.Combine(saveFolder, $"json_{DateTime.Now:MMddHHmm}.json");
            File.WriteAllText(saveFile, json);
            MessageBox.Show("Файл сохранен успешно по пути:\n" +
                            $"{saveFile}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }



        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataGridHelper.OnAutoGeneratingColumn(sender, e);
        }
    }
}