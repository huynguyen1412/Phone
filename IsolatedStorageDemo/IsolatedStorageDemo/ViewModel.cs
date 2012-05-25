using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;


namespace IsolatedStorageDemo {
    public class ViewModel : INotifyPropertyChanged {

        public ViewModel() {
            ImageUrl = "http://res1.newagesolution.net/Portals/0/twitter2_icon.jpg";
        }

        private String imageUrl;

        public String ImageUrl {
            get { 
                return imageUrl; 
            }
            set { 
                imageUrl = value;
                RaisedPropertyChanged("ImageUrl");
            }
        }
        

        public event PropertyChangedEventHandler PropertyChanged;

        void RaisedPropertyChanged(String property) {

            PropertyChangedEventHandler handler = PropertyChanged;
            if(handler != null) {
                handler(property, new PropertyChangedEventArgs(property));
            }
        }
    }
}
