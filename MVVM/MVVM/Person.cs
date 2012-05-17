using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MVVM {
    public class Person {

        private String name;
        private int age;

        public String Name { 
            get { 
                return name; 
            }

            set {
                if (this.name != value) {
                    this.name = value;
                    this.RaisePropertyChanged("Name");
                }
            }
        }
  
        public int Age {
            get { 
                return age; 
            }
            
            set { 
                this.age = value;
                this.RaisePropertyChanged("Age");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(String propertyName) {

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    
    
    }
}
