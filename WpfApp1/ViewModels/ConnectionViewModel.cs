namespace WpfApp1.ViewModels
{
    using System.Windows;
    using System.Windows.Input;

    internal class ConnectionViewModel : ViewModel
    {
        private static ConnectionSettings _settings;
        private bool _isSuccess;

        public ConnectionViewModel()
        {
            _settings = ConnectionSettings.Instance;
            SaveChangesCommand = new RelayCommand(SaveChanges);
        }

        public string DatabaseName
        {
            get => _settings.DatabaseName;
            set
            {
                if (value == _settings.DatabaseName)
                    return;
                _settings.DatabaseName = value;
                OnPropertyChanged();
            }
        }

        public string DatabaseUserName
        {
            get => _settings.DatabaseUserName;
            set
            {
                if (value == _settings.DatabaseUserName)
                    return;
                _settings.DatabaseUserName = value;
                OnPropertyChanged();
            }
        }

        public string Host
        {
            get => _settings.Host;
            set
            {
                if (value == _settings.Host)
                    return;
                _settings.Host = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _settings.Password;
            set
            {
                if (value == _settings.Password)
                    return;
                _settings.Password = value;
                OnPropertyChanged();
            }
        }

        public string Port
        {
            get => _settings.Port;
            set
            {
                if (value == _settings.Port)
                    return;
                _settings.Port = value;
                OnPropertyChanged();
            }
        }


        public ICommand SaveChangesCommand { get; }

        private void SaveChanges(object obj)
        {
            if (obj is not Window window)
                return;
            _settings.SaveCurrent();
            window.DialogResult = true;
            //Application.Current.Windows.OfType<ConnectionWindow>().FirstOrDefault()?.Close();
        }
    }
}