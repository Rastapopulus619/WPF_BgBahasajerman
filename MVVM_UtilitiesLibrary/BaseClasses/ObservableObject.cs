using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MVVM_UtilitiesLibrary.BaseClasses
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        //protected void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        /*HOW TO USE SET PROPERTY!
         * 
         * public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value); // propertyName is automatically filled as "Name"
        }*/
    }
}