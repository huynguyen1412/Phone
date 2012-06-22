using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;


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

    public class Notification {
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
                    Delegate.Remove(d,d);
                }
                messageMap.Remove(typeof(T));
            }
        }
        public Action<object, T> GetDelegateList<T>() {

            Delegate action;
            if (messageMap.TryGetValue(typeof(T), out action) == false) {
                return null;
            }

            return action as Action<object, T>;
        }
        public void Send<T>(object from, T message) {
            Delegate action;
            if (messageMap.TryGetValue(typeof(T), out action) == true) {

                try {
                    ((Action<object, T>)action)(from, message);
                }

                catch(Exception e) {
                    throw new ArgumentNullException("InvalidHanlderMethod", e);
                }
            } else {
                throw new ArgumentException("Key: " + typeof(T) + " Not Found");
            }
        }
        public void SendAsync<T>(object from, T message) {
            Delegate action;
  
            if (messageMap.TryGetValue(typeof(T), out action) == false) {
                return;
            }

            Delegate[] invocationList = action.GetInvocationList();
            Parallel.ForEach(invocationList, (item) => { 
                Action<object, T> a = item as Action<object, T>;
                a(from, message);
            });

            return;
        }
        public void SendSync<T>(object from, T message) {
            Delegate action;

            if (messageMap.TryGetValue(typeof(T), out action) == false) {
                return;
            }

            Delegate[] invocationList = action.GetInvocationList();
            if (invocationList.GetLength(0) > 32) {
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
            nc.Register<FileChanged>(c.OnFileChanged);
            nc.Register<FileChanged>(d.OnFileChanged);

            // for message type m1, send it to objects a & b
            nc.Register<ImageChanged>(a.OnImageChanged);
            nc.Register<ImageChanged>(b.OnImageChanged);
            nc.Send<ImageChanged>(p, m1);
            nc.Send<FileChanged>(p, f1);

            Console.WriteLine("******************* Test Unregister *******************");

            // unregister message f1 from SampleC handler
            Console.WriteLine("Removing " + c.GetType() + "'s OnFileChange()");
            nc.Unregister<FileChanged>(c.OnFileChanged);
            nc.Send<FileChanged>(p, f1);

            // unregister message f1 from SampleD handler, any subsequent calls to Send should fail
            // because there are no more methods registered for that delegate
            nc.Unregister<FileChanged>(d.OnFileChanged);
            Console.WriteLine("Removing " + d.GetType() + "'s OnFileChange()");

            // sending to f1 now will emit an exception since there are no handlers registered
            try {
                nc.Send<FileChanged>(p, f1);
            }
            catch (ArgumentException e) {
                Console.WriteLine(e.ToString());
            }

            nc.Unregister<ImageChanged>();

             try {
                 nc.Send<ImageChanged>(p, m1);
            }
            catch(ArgumentException e) {
                Console.WriteLine(e.Message);
            }


            // Test Async 
            // for message type f1, send it to objects c & d
            nc.Register<FileChanged>(c.OnFileChanged);

            // for message type m1, send it to objects a & b
            for (int i = 0; i < 32; i++)
                nc.Register<ImageChanged>(a.OnImageUpdate);

            Console.WriteLine("\n******************* Test Sync *******************");
            nc.SendSync<ImageChanged>(p, m1);

            SampleBase.Slot = 0; SampleBase.Sum = 0;

            Console.WriteLine("\n******************* Test Async *******************");
            nc.SendAsync<ImageChanged>(p, m1);

        }
    }

    public class SampleBase
    {
        private static uint sum;
        private static int slot;
        private static object lockThis = new object();

        public static int Slot { 
            get {
                lock (lockThis) {
                    return slot++;
                }
            }
            set {
                lock (lockThis) {
                    slot = value;
                }
            }
        }
        public static uint Sum { 
            get {
                lock (lockThis) {
                    if (sum == 0) {
                        sum = 1;
                        return sum;
                    };

                    return sum *= 2;
                }
            }
            set {
                lock (lockThis) {
                    sum = value;
                }
            }
        }
    }
    public class SampleClassA : SampleBase
    {
        public SampleClassA() {}
        public void OnImageChanged(object s, TMessage e) {
            Program.Print(s, e);
        }
        public void OnImageUpdate(object s, TMessage e) {
            Console.WriteLine("OnImageUpdate(" + SampleBase.Slot +") \t Sum=" + Sum);
        }
    }
    public class SampleClassB : SampleBase {

        public SampleClassB() { Sum = 0; }
        public void OnImageChanged(object s, TMessage e) {
            Program.Print(s, e);
        }

        public void OnImageUpdate(object s, TMessage e) {
            Console.WriteLine("OnImageUpdate: Sum=" + (Sum *= 2));
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
