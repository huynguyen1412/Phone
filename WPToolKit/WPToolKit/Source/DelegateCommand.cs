using System;
using System.Windows.Input;

namespace WPToolKit.Source
{
    public sealed class DelegateCommand : ICommand
    {
        readonly Func<object, bool> canExecute;
        readonly Action<object> executeAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="executeAction">The execute action.</param>
        /// <param name="canExecute">The can execute.</param>
        /// <remarks></remarks>
        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecute) {

            if (executeAction == null) {
                throw new ArgumentNullException("executeAction");
            }

            this.canExecute = canExecute;
            this.executeAction = executeAction;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
        /// </summary>
        /// <param name="executeAction">The execute action.</param>
        /// <remarks></remarks>
        public DelegateCommand(Action<object> executeAction)
            : this(executeAction, null) {
        }
        /// <summary>
        /// Determines whether this instance can execute the specified param.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <returns><c>true</c> if this instance can execute the specified param; otherwise, <c>false</c>.</returns>
        /// <remarks></remarks>
        public bool CanExecute(object param) {
            bool result = true;
            Func<object, bool> canExecuteHandler = canExecute;

            if (canExecuteHandler != null) {
                result = canExecuteHandler(param);
            }

            return result;
        }
        /// <summary>
        /// Executes the specified param.
        /// </summary>
        /// <param name="param">The param.</param>
        /// <remarks></remarks>
        public void Execute(object param) {
            executeAction(param);
        }
        /// <summary>
        /// Occurs when changes occur that affect whether the command should execute.
        /// </summary>
        /// <remarks></remarks>
        public event EventHandler CanExecuteChanged;
        /// <summary>
        /// Called when [can execute changed].
        /// </summary>
        /// <remarks></remarks>
        public void OnCanExecuteChanged() {

            EventHandler handler = CanExecuteChanged;

            if (handler != null) {
                handler(this, new EventArgs());
            }
        }
    }
}
