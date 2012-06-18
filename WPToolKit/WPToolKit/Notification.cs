using System;
using System.ComponentModel;
using System.Collections.Generic;


namespace WPToolKit
{
   
     public abstract class TMessage : EventArgs {
        public string s;

        public TMessage(string s) {
            this.s = s;
        }
    }

     class Notification
     {
         private Dictionary<Type, Delegate> messageMap;

         public Notification() {
             messageMap = new Dictionary<Type, Delegate>();
         }

         public void Register<T>(T message, Action<object, T> handler) {

             Delegate action;

             if (messageMap.TryGetValue(message.GetType(), out action) == true) {
                 action = Delegate.Combine(action, handler);
                 messageMap[message.GetType()] = action;
             }
             else {
                 messageMap.Add(message.GetType(), handler);
             }
         }

         public void Unregister<T>(T message, Action<object, T> handler) {

             Delegate action;
             if (messageMap.TryGetValue(message.GetType(), out action) != false) {
                 action = MulticastDelegate.Remove((MulticastDelegate)action, handler);

                 messageMap[message.GetType()] = action;
             }
         }

         public void Unregister<T>(T message) {

             Delegate action;
             if (messageMap.TryGetValue(message.GetType(), out action) != false) {
                 // Remove all the delegates first then remove the key from the map
                 Delegate[] dl = ((Action<object, T>)(action)).GetInvocationList();

                 foreach (Action<object, T> d in dl) {
                     Delegate.Remove(d, d);
                 }
                 messageMap.Remove(message.GetType());
             }
         }

         public void Send<T>(object from, T message) {
             Delegate action;
             if (messageMap.TryGetValue(message.GetType(), out action) != false) {

                 try {
                     ((Action<object, T>)action)(from, message);
                 }

                 catch (Exception e) {
                     throw new ArgumentNullException("InvalidHanlderMethod", e);
                 }
             }
             else {
                 throw new ArgumentException("Key: " + message.GetType() + " Not Found");
             }
         }
     }
}
