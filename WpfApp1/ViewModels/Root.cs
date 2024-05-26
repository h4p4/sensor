namespace WpfApp1.ViewModels
{
    using System.ComponentModel;

    using Newtonsoft.Json;

    public class Root : EditableViewModel
    {
        private Hart _hart;
        private int _deviceType;
        private int _deviceVersion;
        private int _manufacturerId;
        private string _description;

        public Root()
        {
            Hart = new Hart();
        }

        [DisplayName("Описание")]
        [UndoRedo]
        [JsonProperty("description")]
        public string Description
        {
            get => _description;
            set => SetField(ref _description, value);
        }

        [DisplayName("Тип датчика")]
        [UndoRedo]
        [JsonProperty("device_type")]
        public int DeviceType
        {
            get => _deviceType;
            set => SetField(ref _deviceType, value);
        }

        [DisplayName("Версия датчика")]
        [UndoRedo]
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

        [DisplayName("Номер датчика")]
        [JsonIgnore]
        public int Id { get; set; }


        [DisplayName("Производитель датчика")]
        [UndoRedo]
        [JsonProperty("manufacturer_id")]
        public int ManufacturerId
        {
            get => _manufacturerId;
            set => SetField(ref _manufacturerId, value);
        }

        public override string GetTitle()
        {
            return $"Датчик {Id}";
        }
    }
}