using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;



namespace WPToolKit
{
     public class Notification
     {
         private Dictionary<Type, Delegate> messageMap;

         /// <summary>
         /// Initializes a new instance of the <see cref="Notification"/> class.
         /// </summary>
         /// <remarks></remarks>
         public Notification() {
             messageMap = new Dictionary<Type, Delegate>();
         }
         /// <summary>
         /// Registers the specified handler.
         /// </summary>
         /// <typeparam name="T"></typeparam>
         /// <param name="handler">The handler.</param>
         /// <remarks></remarks>
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
         /// <summary>
         /// Returns then number of messages registerd
         /// </summary>
         /// <returns></returns>
         /// <remarks></remarks>
         public int RegisteredCount() {
             return messageMap.Count;
         }
         /// <summary>
         /// Unregisters the specified handler for the message of type T.
         /// </summary>
         /// <typeparam name="T"></typeparam>
         /// <param name="handler">The handler.</param>
         /// <remarks></remarks>
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
         /// <summary>
         /// Unregisters all specified handlers for the message of type T.
         /// </summary>
         /// <typeparam name="T"></typeparam>
         /// <remarks></remarks>
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
         /// <summary>
         /// Sends a message type T to all the registered handlers.
         /// </summary>
         /// <typeparam name="T"></typeparam>
         /// <param name="from">From.</param>
         /// <param name="message">The message.</param>
         /// <remarks></remarks>
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
         /// <summary>
         /// Sends a message type T to all the registered handlers asynchronously.
         /// </summary>
         /// <typeparam name="T"></typeparam>
         /// <param name="from">From.</param>
         /// <param name="message">The message.</param>
         /// <remarks>This method return immediately</remarks>
         public void SendAsync<T>(object from, T message) {
             Delegate action;

             if (messageMap.TryGetValue(typeof(T), out action) == false) {
                 return;
             }

             Delegate[] invocationList = action.GetInvocationList();

#if WINDOWS_PHONE    
             
             foreach(Action<object, T> d in invocationList) {
                 IAsyncResult r = d.BeginInvoke(from, message, null, null);
             }
#else        
             // not availble on WP
             parallel.foreach(invocationlist, (item) => {
                 action<object, t> a = item as action<object, t>;
                 a(from, message);
             });
#endif
             return;
         }
         /// <summary>
         /// Sends a message type T to all the registered handlers asynchronously. Blocks until all are completed.
         /// </summary>
         /// <typeparam name="T"></typeparam>
         /// <param name="from">From.</param>
         /// <param name="message">The message.</param>
         /// <remarks>Limit of 32 handlers imposed</remarks>
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
     }
}
