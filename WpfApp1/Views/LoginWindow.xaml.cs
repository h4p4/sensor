namespace WpfApp1.Views
{
    using System.Windows;

    using WpfApp1.ViewModels;

    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            var viewModel = (LoginViewModel)DataContext;
            viewModel.IsPasswordSet = !string.IsNullOrWhiteSpace(PasswordBox.Password);
        }
    }
}