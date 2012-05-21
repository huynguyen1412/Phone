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

namespace MVVM {
    public class Person : INotifyPropertyChanged {
    private String name;
    private int age;

	    public String Name
	    {
		    get { 
                return name;
            }
		    set { 
                if (name != value) {

                    name = value;
                    RaisePropertyChanged("Name");
                }
            }
	    }
        public String Age {
            get {
                return Age;
            }
            set {
                if(Age != value) {

                    name = value;
                    RaisePropertyChanged("Age");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string propertyName) {
            if (PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}
