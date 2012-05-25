using System;
using System.Windows.Input;

namespace IsolatedStorageDemo {
    public class DelegateCommand : ICommand {

        Func<object, bool> canExecute;
        Action<object> executeAction;

        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecute) {

            if(executeAction == null) {
                throw new ArgumentNullException("executeAction");
            }
            
            this.canExecute = canExecute;
            this.executeAction = executeAction;
        }

        public DelegateCommand(Action<object> executeAction)
            : this(executeAction, null) {
        }

        public bool CanExecute(object param) {
            bool result = true;
            Func<object, bool> canExecuteHandler = this.canExecute;

            if (canExecuteHandler != null) {
                result = canExecuteHandler(param);
            }

            return result;
        }

        public void Execute(object param) {
            this.executeAction(param);
        }


        public event EventHandler CanExecuteChanged;

        public void RaiseCanExecuteChanged() {

            EventHandler handler = this.CanExecuteChanged;

            if(handler != null) {
                handler(this, new EventArgs());
            }
        }
    }
}
