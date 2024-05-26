namespace WpfApp1
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;

    using Newtonsoft.Json;

    using WpfApp1.ViewModels;
    using WpfApp1.Views;

    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var sensorWindow = new SensorInfo();
            sensorWindow.Hide();
            var context = GetContext();
            if (context == null)
            {
                sensorWindow.Close();
                return;
            }

            var users = context.Users;

            if (!users.Any())
            {
                var defaultUser = new User
                {
                    IsAdmin = true,
                    UserDisplayname = "Admin",
                    UserPassword = "admin",
                    Username = "admin"
                };

                users.Add(defaultUser);
                context.SaveChanges(true);
            }

            Connection.CurrentUser = GetCurrentUser();
            if (Connection.CurrentUser == null)
            {
                sensorWindow.Close();
                return;
            }

            var dbSensors = Connection.Context.Sensors;
            var sensors = dbSensors;
            var vm = new MainViewModel();
            sensorWindow.DataContext = vm;
            sensorWindow.Show();
            foreach (var sensor in sensors)
            {
                var root = JsonConvert.DeserializeObject<Root>(sensor.SensorData);
                root.Id = sensor.Id;
                vm.Roots.Add(root);
                root.WasSaved = true;
            }

            Logger.Instance.Enable();
        }

        private SensorContext GetContext()
        {
            SensorContext context;
            if (ConnectionSettings.Instance == null)
            {
                var result1 = MessageBox.Show(
                    "Не удалось загрузить данные о подключении.\n" +
                    "Сбросить конфиг подключения?",
                    "Ошибка",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Error);
                if (result1 == MessageBoxResult.OK)
                    ConnectionSettings.CreateConnectionString();
            }

            try
            {
                context = Connection.Context = new SensorContext();
            }
            catch
            {
                MessageBox.Show("Не удалось подключиться к базе данных.");
                var connectionWindow = new ConnectionWindow();
                var result = connectionWindow.ShowDialog();
                if (result == false)
                    return null;
                return Connection.Context = GetContext();
            }

            return context;
        }

        private User GetCurrentUser()
        {
            var logWindow = new LoginWindow();
            var dataContext = (LoginViewModel)logWindow.DataContext;
            dataContext.Window = logWindow;
            if (logWindow.ShowDialog() != true)
                return null;

            var viewModel = (LoginViewModel)logWindow.DataContext;
            if (viewModel.User == null)
                return GetCurrentUser();

            return viewModel.User;
        }
    }
}