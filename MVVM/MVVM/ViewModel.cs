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
            this.loadData = new DelegateCommand(LoadDataAction);
            this.saveData = new DelegateCommand(SaveSelectePerson);
            name = "";
            age = 2;
        }

        public void LoadDataAction(object p) {
            personDataSource.Add(new Person() { Name = "John", Age = 32 });
            personDataSource.Add(new Person() { Name = "Kate", Age = 27 });
            personDataSource.Add(new Person() { Name = "Sam", Age = 30 });
        }
        public void SaveSelectePerson(object p) {
            if (this.SelectedPerson != null) {
                this.SelectedPerson.Name = this.name;
                this.SelectedPerson.Age = this.age;
            }
        }
        public ICommand LoadDataCommand {
            get {
                return this.loadData;
            }
        }
        public ICommand SaveChangesCommand {
            get {
                return this.saveData;
            }
        }
        public ObservableCollection<Person> DataSource {
            get {
                if (personDataSource == null) {
                    this.personDataSource = new ObservableCollection<Person>();
                }
                return this.personDataSource;
            }
        }
        public String SelectedName {
            get {
                if (this.SelectedPerson != null) {
                    return this.name;
                }
                return string.Empty;
            }

            set {
                this.name = value;
            }
        }
        public int SelectedAge {
            get {
                if (this.SelectedPerson != null) {
                    return this.age;
                }
                return 0;
            }

            set {
                this.age = value;
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
                    if (this.selectedPerson != null) {
                        this.selectedPerson.Name = selectedPerson.Name;
                        this.selectedPerson.Age = selectedPerson.Age;
                    }

                    RaisedPropertyChanged("SelectedName");
                    RaisedPropertyChanged("SelectedAge");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisedPropertyChanged(String property) {
            if(PropertyChanged != null) {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
