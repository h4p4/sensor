namespace WpfApp1.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private Root _root;

        public Logger Logger { get; set; } = Logger.Instance;

        public Root Root
        {
            get => _root;
            set => SetField(ref _root, value);
        }
    }
}