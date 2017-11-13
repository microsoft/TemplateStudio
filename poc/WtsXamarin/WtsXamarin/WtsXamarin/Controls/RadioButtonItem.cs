using System.ComponentModel;
using System.Windows.Input;

namespace WtsXamarin.Controls
{
    class RadioButtonItem<T> : INotifyPropertyChanged where T : struct
    {
        bool _isSelected;

        public event PropertyChangedEventHandler PropertyChanged;

        internal RadioButtonItem(string name, T value, ICommand command, bool isSelected)
        {
            Name = name;
            Value = value;
            Command = command;
            IsSelected = isSelected;
        }

        public string Name { private set; get; }

        public T Value { private set; get; }

        public ICommand Command { private set; get; }

        public bool IsSelected
        {
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
                }
            }
            get
            {
                return _isSelected;
            }
        }
    }
}
