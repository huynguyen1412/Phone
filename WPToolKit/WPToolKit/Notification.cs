using System;
using System.ComponentModel;
using System.Collections.Generic;


namespace WPToolKit
{
   
    public class TMessage : EventArgs
    {
    }

  
    public interface INotificationCenter
    {
        // The Action<T> method is the message sink
        void Send(TMessage message);
        void Register(Type messageType, Action<TMessage> messageHandler);
    }

    public sealed class NotificationCenter : INotificationCenter {

        private Dictionary<TMessage, Action<TMessage>> messageMaps;

        public void Register<T>(Action<T> action) {
           // map.AddHandler(typeof(T), action);
        }
        public void SendEvent<T>(T args) {
            try {
               // Delegate del = map[typeof(T)];
               // ((Action<T>)del)(args);
            }
            catch { }
        }
        private delegate void GenericMessage(object sender, TMessage t);
        Action<TMessage>[] delegates;
          
        public NotificationCenter() {
            delegates = new Action<TMessage>[] { };       
        }

        public void Send<TMessage>(TMessage message) {
        }

        public void Register<TMessage>(Action<TMessage> messageHandler) {
            messageMaps.Add
        }
    }
}
