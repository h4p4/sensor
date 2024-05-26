namespace WpfApp1.ViewModels
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using WpfApp1.Views;

    public class LoginViewModel : ViewModel
    {
        private string _username;

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        public ICommand LoginCommand { get; }

        public User User { get; private set; }

        public string Username
        {
            get => _username;
            set
            {
                if (value == _username)
                    return;
                _username = value;
                OnPropertyChanged();
            }
        }

        public LoginWindow Window { get; set; }

        private void Login(object obj)
        {
            if (obj is not PasswordBox passwordBox)
                return;
            var users = Connection.Context.Users;
            var user = users.FirstOrDefault(x => x.UserPassword == passwordBox.Password &&
                                                 x.Username == Username);

            User = user;
            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Window.DialogResult = true;
        }
    }
}