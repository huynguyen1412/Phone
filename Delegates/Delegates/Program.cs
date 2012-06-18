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
        private Dictionary<Type, Action<object, TMessage>> messageMap;

        public Notification() {
            messageMap = new Dictionary<Type, Action<object, TMessage>>();
        }

        public void Register(TMessage message, Action<object, TMessage> handler) {

            if(messageMap.ContainsKey(message.GetType()) == false) {
                messageMap.Add(message.GetType(), handler);
                return;
            }

            Action<object, TMessage> action;
            if(messageMap.TryGetValue(message.GetType(), out action) != false) {
                action += handler;

                messageMap[message.GetType()] = action;
            }
        }

        public void Unregister(TMessage message, Action<object, TMessage> handler) {
            
            Action<object, TMessage> action;
            if(messageMap.TryGetValue(message.GetType(), out action) != false) {
                action -= handler;

                messageMap[message.GetType()] = action;
            }
        }

        public void Unregister(TMessage message) {

            Action<object, TMessage> action;
            if(messageMap.TryGetValue(message.GetType(), out action) != false) {
                // Remove all the delegates first then remove the key from the map
                Delegate.RemoveAll(action, action);
                messageMap.Remove(message.GetType());
            }
        }

        public void Send(object from, TMessage message) {
            Action<object, TMessage> action;
            if(messageMap.TryGetValue(message.GetType(), out action) != false) {

                try {
                    action(from, message);
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
            nc.Register(f1, c.OnFileChanged);
            nc.Register(f1, d.OnFileChanged);
            nc.Register(m1, d.OnFileChanged);

            // for message type m1, send it to objects a & b
            nc.Register(m1, a.OnImageChanged);
            nc.Register(m1, b.OnImageChanged);
            nc.Send(p, m1);
            nc.Send(p, f1);

            Console.WriteLine("******************* Test Unregister *******************");

            // unregister message f1 from SampleC handler
            Console.WriteLine("Removing " + c.GetType() + "'s OnFileChange()");
            nc.Unregister(f1, c.OnFileChanged);
            nc.Send(p, f1);

            // unregister message f1 from SampleD handler, any subsequent calls to Send should fail
            // because there are no more methods registered for that delegate
            nc.Unregister(f1, d.OnFileChanged);
            Console.WriteLine("Removing " + d.GetType() + "'s OnFileChange()");

            // sending to f1 now will emit an exception since there are no handlers registered
            try {
                nc.Send(p, f1);
            }
            catch(ArgumentNullException e) {
                Console.WriteLine(e.ToString());
            }

            nc.Unregister(m1);

             try {
                nc.Send(p, m1);
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
