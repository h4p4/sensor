namespace WpfApp1.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Microsoft.Win32;

    using Newtonsoft.Json;

    public class MainViewModel : ViewModel
    {
        private Root _root;

        public MainViewModel()
        {
            ReadFromJsonCommand = new RelayCommand(ReadFromJson);
            WriteToDatabaseCommand = new RelayCommand(WriteToDatabase, CanWriteExecute);
            RemoveFromDatabaseCommand = new RelayCommand(RemoveFromDatabase, CanRemoveExecute);
            Roots = new ObservableCollection<Root>();
        }

        public Logger Logger { get; set; } = Logger.Instance;

        public ICommand ReadFromJsonCommand { get; }
        public ICommand RemoveFromDatabaseCommand { get; }

        public Root Root
        {
            get => _root;
            set => SetField(ref _root, value);
        }

        public ObservableCollection<Root> Roots { get; set; }
        public ICommand WriteToDatabaseCommand { get; }

        private bool CanRemoveExecute(object arg)
        {
            return Root != null &&
                   Root.WasSaved;
        }

        private bool CanWriteExecute(object arg)
        {
            return Root != null;
        }

        private void ReadFromJson(object obj)
        {
            var ofd = new OpenFileDialog
            {
                Filter = "Json files (*.json)|*.json"
            };

            ofd.ShowDialog();
            var fileName = ofd.FileName;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                MessageBox.Show("Файл не выбран", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Logger.Enable(false);
            try
            {
                var fileData = File.ReadAllText(fileName);
                var root = JsonConvert.DeserializeObject<Root>(fileData);
                Roots.Add(root);
                Root = root;
            }
            catch (JsonSerializationException)
            {
                MessageBox.Show("Не удалось преобразовать файл.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Файл не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                Logger.Enable();
            }
        }

        private void RemoveFromDatabase(object obj)
        {
            var context = Connection.Context;
            var sensors = context.Sensors;
            var sensor = sensors.FirstOrDefault(x => x.Id == Root.Id);
            if (sensor != null)
            {
                sensors.Remove(sensor);
                context.SaveChanges(true);
                Root.WasSaved = false;
            }
        }

        private void WriteToDatabase(object obj)
        {
            var root = JsonConvert.SerializeObject(Root);
            var context = Connection.Context;
            context.ChangeTracker.Clear();
            var sensors = context.Sensors;
            var sensor = sensors.FirstOrDefault(x => x.Id == Root.Id);
            if (sensor != null)
            {
                sensor.SensorData = root;
                context.SaveChanges();
                return;
            }

            var newSensor = new Sensor
            {
                SensorData = root,
                UserCreatorId = Connection.CurrentUser.UserId
            };
            sensors.Add(newSensor);
            context.SaveChanges();

            Root.Id = newSensor.Id;
            Root.WasSaved = true;
        }
    }
}