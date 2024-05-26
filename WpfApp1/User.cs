using System;
using System.Collections.Generic;

namespace WpfApp1
{
    public partial class User
    {
        public User()
        {
            Sensors = new HashSet<Sensor>();
        }

        public int UserId { get; set; }
        public bool IsAdmin { get; set; }
        public string UserDisplayname { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }

        public virtual ICollection<Sensor> Sensors { get; set; }
    }
}
