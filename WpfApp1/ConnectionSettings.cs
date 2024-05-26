namespace WpfApp1
{
    using System.IO;
    using System.Windows;
    using System.Xml.Serialization;

    public class ConnectionSettings
    {
        private const string DATA_PATH = "..\\..\\..\\..\\data";
        private static ConnectionSettings _instance;
        private static string _connectionFilePath = Path.Combine(DATA_PATH, "connection.xml");

        private ConnectionSettings()
        {
        }

        public string DatabaseName { get; set; }
        public string DatabaseUserName { get; set; }

        public string Host { get; set; }

        [XmlIgnore]
        public static ConnectionSettings Instance => _instance ??= GetOrCreateConnectionConfig();

        public string Password { get; set; }
        public string Port { get; set; }

        public void SaveCurrent()
        {
            using var fsSer = new FileStream(_connectionFilePath, FileMode.OpenOrCreate);
            var xmlSerializer = new XmlSerializer(typeof(ConnectionSettings));
            xmlSerializer.Serialize(fsSer, this);
        }

        public static void CreateConnectionString()
        {
            var xmlSerializer = new XmlSerializer(typeof(ConnectionSettings));
            File.Delete(_connectionFilePath);
            using var fsSer = new FileStream(_connectionFilePath, FileMode.OpenOrCreate);

            var localSettings = new ConnectionSettings
            {
                Host = "localhost",
                Port = "5432",
                DatabaseName = "Sensor",
                DatabaseUserName = "postgres",
                Password = "postgres"
            };

            xmlSerializer.Serialize(fsSer, localSettings);
            _instance =  localSettings;
        }

        private static ConnectionSettings GetOrCreateConnectionConfig()
        {
            var xmlSerializer = new XmlSerializer(typeof(ConnectionSettings));
            try
            {
                if (!File.Exists(_connectionFilePath))
                    CreateConnectionString();

                using var fsDeser = new FileStream(_connectionFilePath, FileMode.OpenOrCreate);
                var settings = (ConnectionSettings)xmlSerializer.Deserialize(fsDeser);
                return settings;
            }
             catch
            {
                return null;
            }
        }
    }
}