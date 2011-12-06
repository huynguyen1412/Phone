using System;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Controls;

namespace ICommandSample {

    public class ViewModel : ViewModelBase {

        // Backing fields for properties
        private Model _data;
        private string _inputText;
        private bool _inputTextChanged;

        public ICommand LoadStringCommand { get; set; }
        public event EventHandler CanExecuteChanged;

        #region @property string OutputText
        public string OutputText {
            get { 
                return _inputText as string; 
            }
            set {
                _inputText = value;
                InputTextChanged = false;
                OnPropertyChanged("OutputText");
            }
        }
        #endregion
        #region @property string InputText
        public string InputText {
            get { 
                return _data.Data as string; 
            }
            set {
                _data.Data = value;
                OnPropertyChanged("InputText");
                InputTextChanged = false;
            }
        }
        #endregion
        #region @property bool InputTextChanged
        public bool InputTextChanged {

            get {
                return _inputTextChanged;
            }
            set {
                _inputTextChanged = value;
                OnPropertyChanged("InputTextChanged");
            } 
        }
        #endregion

        public ViewModel() {
            _data = new Model();
            InputTextChanged = false;
           
            CanExecuteChanged += new EventHandler((sender, e) => {
            });

            LoadStringCommand = new DelegateCommand(LoadString, CanLoadString, CanExecuteChanged);
        }

        public void TextChanged() {

            InputTextChanged = true;
        }

        private void LoadString(object param) {
            OutputText = param as string;
        }

        private bool CanLoadString(object param) {
            return true;
        }
    }

#region ViewModelBase
    public abstract class ViewModelBase : INotifyPropertyChanged {

        public ViewModelBase() { }
        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsDesignTime {
            get { return DesignerProperties.IsInDesignTool; }
        }

        protected virtual void OnPropertyChanged(string propertyName) {
            var handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
    #endregion
#region DelegateCommand
    public class DelegateCommand : ICommand {
        Func<object, bool> canExecute;
        Action<object> executeAction;
        public event EventHandler CanExecuteChanged;
        bool canExecuteState;

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecute, EventHandler canExecuteChange) {
            this.canExecute = canExecute;
            this.executeAction = executeAction;
            this.CanExecuteChanged = canExecuteChange;
        }

        public bool CanExecute(object parameter) {

            bool tmp = canExecute(parameter);
            if (canExecuteState != tmp) {
                canExecuteState = tmp;
                if (CanExecuteChanged != null) {
                    CanExecuteChanged(this, new EventArgs());
                }
            }

            return canExecuteState;
        }


        public void Execute(object parameter) {
            executeAction(parameter);
        }
    }
#endregion

}
