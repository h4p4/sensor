namespace WpfApp1.Views
{
    using System.Windows;
    using System.Windows.Controls;

    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            DataGridHelper.OnAutoGeneratingColumn(sender, e);
        }
    }
}