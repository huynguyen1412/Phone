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

namespace WPToolKit
{
    public interface ITMessage
    {
    }

  
    public interface INotificationCenter
    {
        // The Action<T> method is the message sink
        void Send(object sendTo, Action<ITMessage> messageHandler);
        void Register(object receiver, Action<ITMessage> messageHandler);
    }

    public sealed class NotificationCenter : INotificationCenter {

        private delegate void GenericMessage(object sender, ITMessage t);

        public NotificationCenter() {
        }

        public void Send(object sendTo, Action<ITMessage> messageHandler) {
        }

        public void Register(object receiver, Action<ITMessage> messageHandler) {

        }
    }
}
