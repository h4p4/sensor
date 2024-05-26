using System;
using System.Collections.Generic;

namespace WpfApp1
{
    public partial class Sensor
    {
        public int Id { get; set; }
        public string SensorData { get; set; }
        public int UserCreatorId { get; set; }

        public virtual User UserCreator { get; set; }
    }
}
