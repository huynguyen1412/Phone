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
using System.Collections.ObjectModel;

namespace MVVM {
    public class ViewModel : INotifyPropertyChanged {

        private String name;
        private int age;

        private ObservableCollection<Person> personDataSource;
        private ICommand loadData;
        private ICommand saveData;

        public ViewModel() {
            this.loadData= new DelegateCommand(LoadData);
        }

        public void LoadData(object p) {
            personDataSource.Add(new Person() { Name ="John", Age="32"});
            personDataSource.Add(new Person() { Name ="Kate", Age = "27" });
            personDataSource.Add(new Person() { Name ="Sam", Age = "30" });
        }

        public void SaveSelectePerson(object p) {
            if(this.SelectedPerson != null) {
                this.SelectedPerson.Name = this.name;
                this.SelectedPerson.Age = this.age;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisedPropertyChanged(String property) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }

        private String SelectedName {
            get {
                return this.name;
            }

            set {
                this.name = value;
            }
        }
        private int SelectedAge {
            get {
                return this.age;
            }

            set {
                this.age = value;
                }
            }
        }
 
        private Person selectedPerson;
        public Person SelectedPerson {
            get { 
                return selectedPerson; 
            }
            set {
                if(selectedPerson != value) {
                    selectedPerson = value;
                    this.selectedPerson.Name = selectedPerson.Name;
                    this.selectedPerson.Age = selectedPerson.Age;

                    RaisedPropertyChanged("SelectedName");
                    RaisedPropertyChanged("SelectedAge");
                }
            }
        }
        public ObservableCollection<Person> DataSource {
            get {
                if(personDataSource == null) {
                    this.personDataSource = new ObservableCollection<Person>();
                }
                return this.personDataSource;
            }
        }
        public ICommand LoadDataCommand {
            get {
                return this.loadData;
            }
        }

        public ICommand SaveDataCommand {
            get {
                return this.saveData;
            }
        }
    }
}
