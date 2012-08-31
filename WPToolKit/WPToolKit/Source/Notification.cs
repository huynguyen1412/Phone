using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace WPToolKit.Source
{

    /// <summary>
    /// Extension to Application to expose the Notification singleton
    /// </summary>
    /// <remarks></remarks>
    public static class WptkApplicationExtension {
        static private Notification applicationNotificationObject;
        public static Notification GetApplicationNotificationObject(this Application a) {
            return applicationNotificationObject ?? (applicationNotificationObject = new Notification());
        }
    }

    public class Notification{
        private readonly Dictionary<Type, Delegate> messageMap;

        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class.
        /// </summary>
        /// <remarks></remarks>
        public Notification() {
            messageMap = new Dictionary<Type, Delegate>();
        }

        /// <summary>
        /// Gets the register count.
        /// </summary>
        /// <remarks></remarks>
        public int RegisteredCount {
            get { return messageMap.Count; }
        }

        /// <summary>
        /// Registers the specified handler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler">The handler.</param>
        /// <remarks></remarks>
        public void Register<T>(Action<object, T> handler) {

            Delegate action;

            if (messageMap.TryGetValue(typeof (T), out action)) {
                action = Delegate.Combine(action, handler);
                messageMap[typeof (T)] = action;
            }
            else {
                messageMap.Add(typeof (T), handler);
            }
        }

        /// <summary>
        /// Registers the specified handler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <remarks></remarks>
        public void Register<T, TResult>(Func<object, T, TResult> handler) {

            Delegate func;

            if (handler == null) {
                throw new ArgumentException("Handler can not be null");
            }

            if (messageMap.TryGetValue(typeof (T), out func)) {
                func = Delegate.Combine(func, handler);
                messageMap[typeof (T)] = func;
            }
            else {
                messageMap.Add(typeof (T), handler);
            }
        }

        /// <summary>
        /// Unregisters the specified handler for the message of type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler">The handler.</param>
        /// <remarks></remarks>
        public void Unregister<T>(Action<object, T> handler) {

            Delegate action;
            if (messageMap.TryGetValue(typeof (T), out action)) {
                action = Delegate.Remove(action, handler);

                // if the delegate list is empty, remove the key
                // otherwise, set it to the new delegate list
                if (action == null) {
                    messageMap.Remove(typeof (T));
                }
                else {
                    messageMap[typeof (T)] = action;
                }
            }
        }

        /// <summary>
        /// Unregisters the specified handler.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <remarks></remarks>
        public void Unregister<T, TResult>(Func<object, T, TResult> handler) {

            Delegate func;
            if (messageMap.TryGetValue(typeof (T), out func)) {
                func = Delegate.Remove(func, handler);

                // if the delegate list is empty, remove the key
                // otherwise, set it to the new delegate list
                if (func == null) {
                    messageMap.Remove(typeof (T));
                }
                else {
                    messageMap[typeof (T)] = func;
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
            if (messageMap.TryGetValue(typeof (T), out action)) {
                // Remove all the delegates first then remove the key from the map
                Delegate[] dl = action.GetInvocationList();

                foreach (Action<object, T> d in dl) {
                    Delegate.Remove(d, d);
                }
                messageMap.Remove(typeof (T));
            }
        }

        /// <summary>
        /// Unregisters this instance.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <remarks></remarks>
        public void Unregister<T, TResult>() {
            Delegate func;
            if (messageMap.TryGetValue(typeof (T), out func)) {
                // Remove all the delegates first then remove the key from the map
                Delegate[] dl = func.GetInvocationList();

                foreach (Func<object, T, TResult> d in dl) {
                    Delegate.Remove(d, d);
                }
                messageMap.Remove(typeof (T));
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
            if (messageMap.TryGetValue(typeof (T), out action)) {

                try {
                    ((Action<object, T>) action)(from, message);
                }
                catch (Exception e) {
                    throw new ArgumentException("InvalidHandlerMethod", e);
                }
            }
            else {
                throw new ArgumentException("Key: " + typeof (T) + " Not Found");
            }
        }

        /// <summary>
        /// Sends the specified message and returns a result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="from">From.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public TResult Send<T, TResult>(object from, T message) {

            Delegate func;
            TResult result;

            if (messageMap.TryGetValue(typeof (T), out func)) {

                try {
                    result = ((Func<object, T, TResult>) func)(from, message);
                }
                catch (Exception e) {
                    throw new ArgumentException("InvalidHandlerMethod", e);
                }
            }
            else {
                String s = "Key(" + typeof (T) + ") Not Found";
                throw new ArgumentException(s);
            }

            return result;
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

            if (messageMap.TryGetValue(typeof (T), out action) == false) {
                return;
            }

            Delegate[] invocationList = action.GetInvocationList();

#if WINDOWS_PHONE

            foreach (Action<object, T> d in invocationList) {
                Debug.Assert(d != null);
                IAsyncResult r = d.BeginInvoke(from, message, null, null);
            }
#else        
    // not availble on WP
             parallel.foreach(invocationlist, (item) => {
                 action<object, t> a = item as action<object, t>;
                 a(from, message);
             });
#endif
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
            const int numberOfWaitsAllowed = 32;

            if (messageMap.TryGetValue(typeof (T), out action) == false) {
                return;
            }

            Delegate[] invocationList = action.GetInvocationList();
            if (invocationList.GetLength(0) > numberOfWaitsAllowed) {
                throw new ArgumentOutOfRangeException();
            }

            var asyncResult = new List<WaitHandle>();

            foreach (Action<object, T> d in invocationList) {
                IAsyncResult r = d.BeginInvoke(from, message, null, null);
                asyncResult.Add(r.AsyncWaitHandle);

                WaitHandle.WaitAll(asyncResult.ToArray());
                return;
            }
        }
    }
}
