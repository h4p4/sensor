namespace WpfApp1
{
    using System.Collections;

    public class CollectionHeaderRelator
    {
        public CollectionHeaderRelator(IList collection, RelayCommand editCommand, string header)
        {
            Collection = collection;
            EditCommand = editCommand;
            Header = header;
        }

        public IList Collection { get; }
        public RelayCommand EditCommand { get; }
        public string Header { get; }
    }
}