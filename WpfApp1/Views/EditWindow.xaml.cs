namespace WpfApp1.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Media;

    using WpfApp1.Helpers;
    using WpfApp1.ViewModels;

    public partial class EditWindow : Window
    {
        private readonly CollectionHeaderRelator[] _relators;

        public EditWindow(params CollectionHeaderRelator[] relators)
        {
            _relators = relators;
            InitializeComponent();
            InitializeData();

        }

        private void InitializeData()
        {
            if (_relators == null)
                return;

            var columnCount = 0;

            foreach (var relator in _relators)
            {
                EditGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    MinWidth = 100,
                    Width = new GridLength(1, GridUnitType.Auto)
                });

                var dataGrid = new DataGrid
                {
                    ItemsSource = relator.Collection,
                    CanUserAddRows = true,
                    Name = $"DataGrid{columnCount}",
                };
                dataGrid.AutoGeneratingColumn += DataGridHelper.OnAutoGeneratingColumn;

                var textBlock = new TextBlock
                {
                    Text = relator.Header,
                    Padding = new Thickness(5, 0, 0, 0)
                };

                EditGrid.Children.Add(dataGrid);
                EditGrid.Children.Add(textBlock);

                Grid.SetColumn(dataGrid, columnCount);
                Grid.SetColumn(textBlock, columnCount);

                Grid.SetRow(textBlock, 0);
                Grid.SetRow(dataGrid, 2);

                if (relator.EditCommand == null)
                {
                    columnCount++;
                    continue;
                }

                var commandParameterBinding = new Binding
                {
                    Source = dataGrid, // элемент-источник
                    Path = new PropertyPath(Selector.SelectedItemProperty), // свойство элемента-источника
                    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    Mode = BindingMode.TwoWay
                };

                //var isEnabledBinding = new Binding()
                //{
                //    Source = dataGrid, // элемент-источник
                //    Path = new PropertyPath(Selector.IsSelectedProperty),
                //    UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                //    Mode = BindingMode.TwoWay
                //};

                var editButton = new Button
                {
                    Command = relator.EditCommand,
                    Style = (Style)Application.Current.FindResource("TableEditIconButton"),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Background = Brushes.White
                };

                editButton.SetBinding(ButtonBase.CommandParameterProperty, commandParameterBinding); // установка привязки для элемента-приемника
                //editButton.SetBinding(ButtonBase.IsEnabledProperty, isEnabledBinding);

                EditGrid.Children.Add(editButton);
                Grid.SetColumn(editButton, columnCount);
                Grid.SetRow(editButton, 1);

                columnCount++;
            }
        }
    }
}