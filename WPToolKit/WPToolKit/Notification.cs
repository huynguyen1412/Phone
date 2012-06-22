using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;



namespace WPToolKit
{
     public class Notification
     {
         private Dictionary<Type, Delegate> messageMap;

         public Notification() {
             messageMap = new Dictionary<Type, Delegate>();
         }
         public void Register<T>(Action<object, T> handler) {

             Delegate action;

             if (messageMap.TryGetValue(typeof(T), out action) == true) {
                 action = Delegate.Combine(action, handler);
                 messageMap[typeof(T)] = action;
             }
             else {
                 messageMap.Add(typeof(T), handler);
             }
         }
         public void Unregister<T>(Action<object, T> handler) {

             Delegate action;
             if (messageMap.TryGetValue(typeof(T), out action) == true) {
                 action = MulticastDelegate.Remove((MulticastDelegate)action, handler);

                 // if the delegate list is empty, remove the key
                 // otherwise, set it to the new delegate list
                 if (action == null) {
                     messageMap.Remove(typeof(T));
                 }
                 else {
                     messageMap[typeof(T)] = action;
                 }
             }
         }
         public void Unregister<T>() {

             Delegate action;
             if (messageMap.TryGetValue(typeof(T), out action) == true) {
                 // Remove all the delegates first then remove the key from the map
                 Delegate[] dl = ((Action<object, T>)(action)).GetInvocationList();

                 foreach (Action<object, T> d in dl) {
                     Delegate.Remove(d, d);
                 }
                 messageMap.Remove(typeof(T));
             }
         }
         public void Send<T>(object from, T message) {
             Delegate action;
             if (messageMap.TryGetValue(typeof(T), out action) != false) {

                 try {
                     ((Action<object, T>)action)(from, message);
                 }

                 catch (Exception e) {
                     throw new ArgumentNullException("InvalidHanlderMethod", e);
                 }
             }
             else {
                 throw new ArgumentException("Key: " + typeof(T) + " Not Found");
             }
         }
         public void SendAsync<T>(object from, T message) {
             Delegate action;

             if (messageMap.TryGetValue(typeof(T), out action) == false) {
                 return;
             }

             Delegate[] invocationList = action.GetInvocationList();

             // not availble on WP
             //Parallel.ForEach(invocationList, (item) => {
             //    Action<object, T> a = item as Action<object, T>;
             //    a(from, message);
             //});

             foreach (Action<object, T> d in invocationList) {
                 IAsyncResult r = d.BeginInvoke(from, message, null, null);
             }

             return;
         }

         public void SendSync<T>(object from, T message) {
             Delegate action;
             const int NumberOfWaitsAllowed = 32;

             if (messageMap.TryGetValue(typeof(T), out action) == false) {
                 return;
             }

             Delegate[] invocationList = action.GetInvocationList();
             if (invocationList.GetLength(0) > NumberOfWaitsAllowed) {
                 throw new ArgumentOutOfRangeException("Too many delegates to wait on");
             }

             List<WaitHandle> asyncResult = new List<WaitHandle>();

             foreach (Action<object, T> d in invocationList) {
                 IAsyncResult r = d.BeginInvoke(from, message, null, null);
                 asyncResult.Add(r.AsyncWaitHandle);
             }

             WaitHandle.WaitAll(asyncResult.ToArray());
             return;
         }

         public int RegisteredCount() {
             return messageMap.Count;
         }
     }
}
