namespace WpfApp1.ViewModels
{
    using Newtonsoft.Json;

    public class Root : EditableViewModel
    {
        private Hart _hart;
        private int _deviceType;
        private int _deviceVersion;
        private int _manufacturerId;
        private string _description;

        [JsonProperty("description")]
        public string Description
        {
            get => _description;
            set => SetField(ref _description, value);
        }

        [JsonProperty("device_type")]
        public int DeviceType
        {
            get => _deviceType;
            set => SetField(ref _deviceType, value);
        }

        [JsonProperty("device_version")]
        public int DeviceVersion
        {
            get => _deviceVersion;
            set => SetField(ref _deviceVersion, value);
        }

        [JsonProperty("hart")]
        public Hart Hart
        {
            get => _hart;
            set => SetField(ref _hart, value);
        }

        [JsonProperty("manufacturer_id")]
        public int ManufacturerId
        {
            get => _manufacturerId;
            set => SetField(ref _manufacturerId, value);
        }

        public override string GetTitle()
        {
            return "Датчик";
        }
    }
}