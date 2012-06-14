using System;
using System.ComponentModel;

namespace IsolatedStorageDemo
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public bool IsDesignTime {
            get { return DesignerProperties.IsInDesignTool; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(String property) {

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(property, new PropertyChangedEventArgs(property));
            }
        }
    }
}
