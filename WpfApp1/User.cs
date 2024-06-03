namespace WpfApp1
{
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class User
    {
        public User()
        {
            Sensors = new HashSet<Sensor>();
        }

        public virtual ICollection<Sensor> Sensors { get; set; }

        [DisplayName("Отображаемое имя")]
        public string UserDisplayname { get; set; }

        public int UserId { get; set; }

        [DisplayName("Логин")]
        public string Username { get; set; }

        [DisplayName("Пароль")]
        public string UserPassword { get; set; }

        [DisplayName("Администратор")]
        public bool IsAdmin { get; set; }
    }
}