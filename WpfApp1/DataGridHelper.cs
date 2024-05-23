namespace WpfApp1
{
    using System.Windows.Controls;

    internal class DataGridHelper
    {
        //public static DataGridColumn GetNewComboBoxColumn(string header,
        //    BindingBase bindingBase,
        //    object itemsSource)
        //{
        //    var comboBoxColumn = new DataGridComboBoxColumn
        //    {
        //        Header = header,
        //        SelectedValueBinding = bindingBase
        //    };

        //    var itemsSourceBinding = new Binding
        //    {
        //        Source = itemsSource
        //    };
        //    BindingOperations.SetBinding(comboBoxColumn, DataGridComboBoxColumn.ItemsSourceProperty,
        //        itemsSourceBinding);

        //    return comboBoxColumn;
        //}

        public static void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs eventArgs)
        {
            var displayName = DisplayNameHelper.GetPropertyDisplayName(eventArgs.PropertyDescriptor, out var type);
            if (string.IsNullOrEmpty(displayName))
            {
                eventArgs.Cancel = true;
                return;
            }

            if (type.IsGenericType)
            {
                eventArgs.Cancel = true;
                return;
            }

            if (type.IsPrimitive)
            {
            }

            eventArgs.Column.Header = displayName;
        }
    }
}