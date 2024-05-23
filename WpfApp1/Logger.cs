namespace WpfApp1
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Text.RegularExpressions;

    using WpfApp1.ViewModels;

    public class Logger : ViewModel
    {
        public static readonly Logger Instance = new Logger();

        private string _lastLogMessage = string.Empty;

        private Logger()
        {
            LogCollection = new ObservableCollection<string>();
        }

        public ObservableCollection<string> LogCollection { get; }

        public bool IsEnabled { get; private set; }

        public void Enable()
        {
            IsEnabled = true;
        }

        public void AddViewModel(EditableViewModel viewModel)
        {
            viewModel.PropertyChanging += LogPropertyChanging;
            viewModel.PropertyChanged += LogPropertyChanged;
        }

        private static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName)?.GetValue(src, null);
        }

        private void LogPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!IsEnabled)
                return;

            if (!(sender is EditableViewModel) || !CanBeLogged(sender, e.PropertyName, out _))
                return;

            _lastLogMessage += $"на \"{GetPropValue(sender, e.PropertyName)}\".";
            LogCollection.Add(_lastLogMessage);
            _lastLogMessage = string.Empty;
        }

        private bool CanBeLogged(object sender, string ePropertyName, out string displayName)
        {
            displayName = string.Empty;
            if (!IsEnabled)
                return false;

            var property = sender.GetType().GetProperty(ePropertyName);
            if (property == null)
                return false;
            displayName = DisplayNameHelper.GetPropertyDisplayName(property, out var type);
            if (string.IsNullOrWhiteSpace(displayName))
                return false;

            var t = property.PropertyType;
            if (t.IsPrimitive || t == typeof(decimal) || t == typeof(string))
                return true;

            if (type?.IsGenericType == true)
                return false;

            return false;
        }

        private void LogPropertyChanging(object sender, PropertyChangingEventArgs propertyChangingEventArgs)
        {
            if (!IsEnabled)
                return;

            if (!(sender is EditableViewModel editableViewModel) ||
                !CanBeLogged(sender, propertyChangingEventArgs.PropertyName, out var displayName))
                return;

            _lastLogMessage = $"Строка \"{editableViewModel.GetTitle()}\" " +
                              $"изменила значение поля \"{displayName}\" " +
                              $"с \"{GetPropValue(sender, propertyChangingEventArgs.PropertyName)}\" ";
            Regex.Replace(_lastLogMessage, @"\t|\n|\r", "");
        }
    }
}