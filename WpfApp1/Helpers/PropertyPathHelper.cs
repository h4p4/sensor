namespace WpfApp1.Helpers
{
    using System.Windows;
    using System.Windows.Data;

    public static class PropertyPathHelper
    {
        private static readonly Dummy _dummy = new Dummy();

        public static object GetValue(object obj, string propertyPath)
        {
            var binding = new Binding(propertyPath)
            {
                Mode = BindingMode.OneTime,
                Source = obj
            };
            BindingOperations.SetBinding(_dummy, Dummy.ValueProperty, binding);
            return _dummy.GetValue(Dummy.ValueProperty);
        }

        private class Dummy : DependencyObject
        {
            public static readonly DependencyProperty ValueProperty =
                DependencyProperty.Register("Value", typeof(object), typeof(Dummy), new UIPropertyMetadata(null));
        }
    }
}