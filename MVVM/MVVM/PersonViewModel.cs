using System;
using System.Net;
using System.Windows;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MVVM {
    public class PersonViewModel : INotifyPropertyChanged {

        private String name;
        private int age;
        private ObservableCollection<Person> personDataSource;
        private ICommand loadDataCommand;
        private ICommand saveDataCommand;

        PersonViewModel() {
            this.loadDataCommand = new DelegateCommand(this.LoadDataAction);
            this.saveDataCommand = new DelegateCommand(this.SaveDataAction);
        }

        public void LoadDataAction(object p) {
            this.DataSource.Add(new Person() { Name = "John", Age = 32 });
            this.DataSource.Add(new Person() { Name = "Kate", Age = 27 });
            this.DataSource.Add(new Person() { Name = "Sam", Age = 30 });
        }

        public void SaveChangesAction(object p) {
            if (this.personDataSource.SelectedPerson != null) {
                this.SelectedPerson.Name = this.name;
                this.SelectedPerson.Age = this.age;
            }
        }


        public ICommand SaveChangesCommand  {
            get {
                return this.saveDataCommand;
            }
        }

        public ICommand LoadDataCommand {
            get {
                return this.loadDataCommand;
            }
        }

        public ObservableCollection<Person> DataSource {
            get {
                if (this.personDataSource == null) {
                    this.personDataSource = new ObservableCollection<Person>();
                }

                return this.personDataSource;
            }
        }

        public String SelectedName {
            get {
                if (this.SelectedPerson != null) {
                    return this.SelectedPerson.Name;
                }
                return String.Empty;
            }

            set {
                this.name = value;
            }
        }


        public int SelectedAge {
            get {
                if (this.SelectedPerson != null) {
                    return this.SelectedPerson.Age;
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
                return this.selectedPerson;
            }
            set {
                if (this.selectedPerson != value) {
                    this.selectedPerson = value;

                    if (this.selectedPerson == null) {
                        this.name = this.selectedPerson.Name;
                        this.age = this.selectedPerson.Age;
                    }

                    this.RaisePropertyChanged("SelectedName");
                    this.RaisePropertyChanged("SelectedAge");
                }
            }
        }

        public event ProgressChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName) {

            ProgressChangedEventHandler handler = this.PropertyChanged;
            if (handler != null) {
                    handler(this, new ProgressChangedEventArgs(propertyName));
            }
        }
    }
}
