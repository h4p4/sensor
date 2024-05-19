namespace WpfApp1.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    using Newtonsoft.Json;

    using WpfApp1.Views;

    public class EditableViewModel : ViewModel
    {
        private bool _isEditing;

        public EditableViewModel()
        {
            StartEditCommand = new RelayCommand(Edit);
        }

        [JsonIgnore]
        internal bool IsEditing
        {
            get => _isEditing;
            set => SetField(ref _isEditing, value);
        }

        [JsonIgnore]
        public RelayCommand StartEditCommand { get; }

        protected virtual void Edit(object obj)
        {
            IsEditing = true;

            var relators = obj is EditableViewModel viewModel ? viewModel.GetRelators() : GetRelators();

            new EditWindow(relators).ShowDialog();
            IsEditing = false;
        }

        protected virtual CollectionHeaderRelator[] GetRelators()
        {
            return Array.Empty<CollectionHeaderRelator>();
        }
    }

    public class ViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }

    [JsonConverter(typeof(ViewModelToIntConverter))]
    public class IntegerViewModel : ViewModel
    {
        private int _value;

        [DisplayName("Значения")]
        public int Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }
    }
}