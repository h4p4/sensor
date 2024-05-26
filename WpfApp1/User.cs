namespace WpfApp1
{
    public partial class User
    {
        public bool IsAdmin { get; set; }
        public virtual Sensor Sensor { get; set; }
        public string UserDisplayname { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public string UserPassword { get; set; }
    }
}