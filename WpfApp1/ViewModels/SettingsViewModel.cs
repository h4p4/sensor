namespace WpfApp1.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    public class SettingsViewModel : ViewModel
    {
        private User _user;
        public SettingsViewModel()
        {
            var currentUser = new[]
            {
                Connection.CurrentUser
            };

            var users = Connection.Context.Users.ToList().Except(currentUser);
            Users = new ObservableCollection<User>(users);
            DeleteUserCommand = new RelayCommand(DeleteUser, CanDeleteUser);
            AddUserCommand = new RelayCommand(AddUsers, CanAddUsers);
        }

        public ICommand AddUserCommand { get; set; }

        public ICommand DeleteUserCommand { get; set; }

        public User User
        {
            get => _user;
            set => SetField(ref _user, value);
        }

        public ObservableCollection<User> Users { get; set; }
        private void AddUsers(object obj)
        {
            var context = Connection.Context;
            context.UpdateRange(Users);
            context.SaveChanges(true);
        }

        private bool CanAddUsers(object arg)
        {
            return Users.Any();
        }

        private bool CanDeleteUser(object arg)
        {
            return User != null;
        }

        private void DeleteUser(object obj)
        {
            var context = Connection.Context;
            Users.Remove(User);
            context.SaveChanges(true);
        }
    }
}