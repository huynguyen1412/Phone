using System;
using System.Collections.Generic;

namespace Delegates {

    public abstract class TMessage : EventArgs {
        public string s;

        public TMessage(string s) {
            this.s = s;
        }
    }

    public class ImageChanged : TMessage {
        public ImageChanged() : base("Image Changed") { }
    }

    public class FileChanged : TMessage {
        public FileChanged() : base("File Changed") {}
    }

    class Notification {
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
            if(messageMap.TryGetValue(message.GetType(), out action) != false) {
                action = MulticastDelegate.Remove((MulticastDelegate)action, handler);

                messageMap[message.GetType()] = action;
            }
        }

        public void Unregister<T>(T message) {

            Delegate action;
            if(messageMap.TryGetValue(message.GetType(), out action) != false) {
                // Remove all the delegates first then remove the key from the map
                Delegate[] dl = ((Action<object, T>)(action)).GetInvocationList();

                foreach (Action<object, T> d in dl) {
                    Delegate.Remove(d,d);
                }
                messageMap.Remove(message.GetType());
            }
        }

        public void Send<T>(object from, T message) {
            Delegate action;
            if(messageMap.TryGetValue(message.GetType(), out action) != false) {

                try {
                    ((Action<object, T>)action)(from, message);
                }

                catch(Exception e) {
                    throw new ArgumentNullException("InvalidHanlderMethod", e);
                }
            } else {
                throw new ArgumentException("Key: " + message.GetType() + " Not Found");
            }
        }
    }

    class Program {
        public static void Print(object s, TMessage e) {
            Console.WriteLine("Message Recv'ed from:" + s + " with data:" + e.s + " On delegate:" + e.GetType());
        }
        static void Main(string[] args) {


            Notification nc = new Notification();
            Program p = new Program();

            // Four class that have methods we assign delegates too
            SampleClassA a = new SampleClassA();
            SampleClassB b = new SampleClassB();
            SampleClassC c = new SampleClassC();
            SampleClassD d = new SampleClassD();

            // two messages types that we want to send
            ImageChanged m1 = new ImageChanged();
            FileChanged f1 = new FileChanged();
            
            // for message type f1, send it to objects c & d
            nc.Register<TMessage>(f1, c.OnFileChanged);
            nc.Register<TMessage>(f1, d.OnFileChanged);
            nc.Register<TMessage>(m1, d.OnFileChanged);

            // for message type m1, send it to objects a & b
            nc.Register<TMessage>(m1, a.OnImageChanged);
            nc.Register<TMessage>(m1, b.OnImageChanged);
            nc.Send<TMessage>(p, m1);
            nc.Send<TMessage>(p, f1);

            Console.WriteLine("******************* Test Unregister *******************");

            // unregister message f1 from SampleC handler
            Console.WriteLine("Removing " + c.GetType() + "'s OnFileChange()");
            nc.Unregister<TMessage>(f1, c.OnFileChanged);
            nc.Send<TMessage>(p, f1);

            // unregister message f1 from SampleD handler, any subsequent calls to Send should fail
            // because there are no more methods registered for that delegate
            nc.Unregister<TMessage>(f1, d.OnFileChanged);
            Console.WriteLine("Removing " + d.GetType() + "'s OnFileChange()");

            // sending to f1 now will emit an exception since there are no handlers registered
            try {
                nc.Send<TMessage>(p, f1);
            }
            catch(ArgumentNullException e) {
                Console.WriteLine(e.ToString());
            }

            nc.Unregister<TMessage>(m1);

             try {
                 nc.Send<TMessage>(p, m1);
            }
            catch(ArgumentException e) {
                Console.WriteLine(e.Message);
            }
        }
    }

    public class SampleClassA {

        public SampleClassA() {}
        public void OnImageChanged(object s, TMessage e) {
            Program.Print(s, e);
        }
    }

    public class SampleClassB {

        public SampleClassB() { }
        public void OnImageChanged(object s, TMessage e) {
            Program.Print(s, e);
        }
    }

    public class SampleClassC {

        public SampleClassC() { }
        public void OnFileChanged(object s, TMessage e) {
            Program.Print(s, e);    
        }
    }

    public class SampleClassD {

        public SampleClassD() { }
        public void OnFileChanged(object s, TMessage e) {
            Program.Print(s, e);
        }
    }

}
