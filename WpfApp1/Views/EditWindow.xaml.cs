namespace WpfApp1.Views
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Media;

    using WpfApp1.ViewModels;

    public partial class EditWindow : Window
    {
        private readonly CollectionHeaderRelator[] _relators;
        private readonly EditableViewModel _viewModel;

        public EditWindow(EditableViewModel viewModel, params CollectionHeaderRelator[] relators)
        {
            _viewModel = viewModel;
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
                var tabItem = new TabItem();
                EditTabControl.Items.Add(tabItem);
                var tabGrid = new Grid();
                tabGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = GridLength.Auto
                });
                tabGrid.RowDefinitions.Add(new RowDefinition());
                tabItem.Content = tabGrid;
                var dataGrid = new DataGrid
                {
                    ItemsSource = relator.Collection,
                    CanUserAddRows = true,
                    ColumnWidth = new DataGridLength(1, DataGridLengthUnitType.Star),
                    Name = $"DataGrid{columnCount}",
                };
                dataGrid.AutoGeneratingColumn += DataGridHelper.OnAutoGeneratingColumn;

                tabItem.Header = relator.Header;
                var buttonsWrapPanel = new WrapPanel
                {
                    Orientation = Orientation.Horizontal
                };
                tabGrid.Children.Add(dataGrid);
                tabGrid.Children.Add(buttonsWrapPanel);

                Grid.SetRow(dataGrid, 1);

                var undoButton = new Button
                {
                    Command = _viewModel.UndoCommand,
                    Style = (Style)Application.Current.FindResource("UndoIconButton"),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Background = Brushes.White
                };
                var redoButton = new Button
                {
                    Command = _viewModel.RedoCommand,
                    Style = (Style)Application.Current.FindResource("RedoIconButton"),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Background = Brushes.White
                };
                var excelButton = new Button
                {
                    Command = _viewModel.GenerateExcelCommand,
                    Style = (Style)Application.Current.FindResource("ExcelIconButton"),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Background = Brushes.White
                };
                var pdfButton = new Button
                {
                    Command = _viewModel.GeneratePdfCommand,
                    Style = (Style)Application.Current.FindResource("PdfIconButton"),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Background = Brushes.White
                };
                var wordButton = new Button
                {
                    Command = _viewModel.GenerateWordCommand,
                    Style = (Style)Application.Current.FindResource("WordIconButton"),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Background = Brushes.White
                };

                var commandParameterDataGridBinding = new Binding
                {
                    Source = dataGrid, // элемент-источник
                };

                excelButton.SetBinding(ButtonBase.CommandParameterProperty, commandParameterDataGridBinding);
                pdfButton.SetBinding(ButtonBase.CommandParameterProperty, commandParameterDataGridBinding);
                wordButton.SetBinding(ButtonBase.CommandParameterProperty, commandParameterDataGridBinding);

                buttonsWrapPanel.Children.Add(undoButton);
                buttonsWrapPanel.Children.Add(redoButton);
                buttonsWrapPanel.Children.Add(excelButton);
                buttonsWrapPanel.Children.Add(pdfButton);
                buttonsWrapPanel.Children.Add(wordButton);

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

                var editButton = new Button
                {
                    Command = relator.EditCommand,
                    Style = (Style)Application.Current.FindResource("TableEditIconButton"),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Background = Brushes.White
                };

                editButton.SetBinding(ButtonBase.CommandParameterProperty,
                    commandParameterBinding); // установка привязки для элемента-приемника

                buttonsWrapPanel.Children.Insert(0, editButton);
                columnCount++;
            }
        }
    }
}