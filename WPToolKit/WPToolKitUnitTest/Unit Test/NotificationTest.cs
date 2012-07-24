using System;

using System.Threading;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPToolKit;
using System.Windows;

namespace WPToolKitUnitTest
{

    [TestClass]
    public class NotificationTest
    {
        Notification nc;
        public class TestObject
        {
            public bool msgReceived1;
            string msg1 = "TestProperty";

            public bool msgReceived2;
            string msg2 = "ImageProperty";

            public bool msgReceived3;
            int results=10;

            public TestObject() {
                msgReceived1 = false;
                msgReceived2 = false;
                msgReceived3 = false;
            }
            public void OnMessageReceived(object from, TestMessage e) {
                msgReceived1 = (e.msg == msg1);
                
            }
            public void OnMessageReceived(object from, string s) {
                msgReceived2 = (s == msg2);
            }

            public int OnMessageReceived(object from, TestMessageFuncDelegate x) {
                msgReceived3 = (x.value == results);
                return -1;
            }

            public int OnMessageReceived(object from, TestMessageFuncBadDelegate x) {
                msgReceived3 = (x.value == results);
                return -1;
            }

            public string OnMessageReceivedWithReturn(object from, string x) {
                msgReceived3 = (x == msg2);
                return "OnMessageReceivedWithReturn";
            }

            public void OnMessageReceivedException(object from, int x) {
                throw new Exception("Exception created in OnMessageReceivedException("+x+")");
            }

            public int OnMessageReceivedWithReturnException(object from, int x) {
                throw new Exception("Exception created in OnMessageReceivedException(" + x + ")");
            }


        }

        public class TestMessage  {
            public string msg = "TestProperty";
        }

        public class TestMessageFuncDelegate {
            public int value = 10;
        }

        public class TestMessageFuncBadDelegate {
            public int value = 22;
        }

        public class MTTestMessage {
            static private int numMessages;

            public void OnMTTestMessage(object from, TestMessage e) {
                Interlocked.Increment(ref numMessages);
            }
        }
        [TestInitialize]
        public void SetUp() {
            nc = Application.Current.GetApplicationNotificationObject();

        }
        [TestMethod]
        public void TestNotificationBasicMessage() {

            TestObject msg = new TestObject();
            TestMessage m = new TestMessage();
            string TestProperty1 = "ImageProperty";

            nc.Register<TestMessage>(msg.OnMessageReceived);
            nc.Send<TestMessage>(this, m);
            Assert.IsTrue(msg.msgReceived1);

            nc.Send<TestMessage>(this, m);
            Assert.IsTrue(msg.msgReceived1);

            nc.Register<string>(msg.OnMessageReceived);
            nc.Send<string>(this, TestProperty1);
            Assert.IsTrue(msg.msgReceived2);

            nc.Unregister<TestMessage>();
            nc.Unregister<string>();
            Assert.IsTrue(nc.RegisteredCount == 0);
        }
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestNotificationUnregister() {
            TestObject msg = new TestObject();
            TestMessage m = new TestMessage();

            nc.Register<TestMessage>(msg.OnMessageReceived);
            nc.Send<TestMessage>(this, m);

            Assert.IsTrue(msg.msgReceived1);
            nc.Unregister<TestMessage>(msg.OnMessageReceived);
            Assert.IsTrue(nc.RegisteredCount == 0);

            nc.Send<TestMessage>(this, m);
        }
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestNotificationUnregisterAll() {
            TestObject msg = new TestObject();
            TestObject msg1 = new TestObject();

            TestMessage m = new TestMessage();

            // Register multiple receiptients 
            nc.Register<TestMessage>(msg.OnMessageReceived);
            nc.Register<TestMessage>(msg1.OnMessageReceived);

            nc.Send<TestMessage>(this, m);
            Assert.IsTrue(msg.msgReceived1);
            Assert.IsTrue(msg1.msgReceived1);

            // Unregister them all and the Send should throw
            nc.Unregister<TestMessage>();
            Assert.IsTrue(nc.RegisteredCount == 0);
            nc.Send<TestMessage>(this, m);

        }
        [TestMethod]
        public void TestSyncMessage() {

            Notification n = new Notification();
            MTTestMessage mt = new MTTestMessage();

            for(int i = 0; i < 10; i++) {
                n.Register<TestMessage>(mt.OnMTTestMessage);
            }

            // not supported yet.
#if WINDOWS_PHONE
            mt.OnMTTestMessage(null, null);
#else
            n.SendSync<TestMessage>(this, null);
#endif
            n.Unregister<TestMessage>();
        }

        // Test for Func<T>
        [TestMethod]
        public void TestRegisterFunc() {
            TestObject msg = new TestObject();
            TestMessageFuncDelegate m = new TestMessageFuncDelegate();
            nc.Register<TestMessageFuncDelegate,int>(msg.OnMessageReceived);
            Assert.IsTrue(nc.RegisteredCount == 1);
            nc.Unregister<TestMessageFuncDelegate, int>(msg.OnMessageReceived);
            Assert.IsTrue(nc.RegisteredCount == 0);
        }

        [TestMethod]
        public void TestSendFunc() {
            TestObject msg = new TestObject();
            TestMessageFuncDelegate m = new TestMessageFuncDelegate();
            nc.Register<TestMessageFuncDelegate, int>(msg.OnMessageReceived);
            int x = nc.Send<TestMessageFuncDelegate, int>(this, m);
            Assert.IsTrue(x == -1);
            Assert.IsTrue(msg.msgReceived3);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestSendWithDelegateThrowingException() {
            TestObject msg = new TestObject();

            // explicitly register a bad delegate
            nc.Register<int>(msg.OnMessageReceivedException);
            nc.Send<int>(this, 5);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestSendWithFuncDelegateThrowingException() {
            TestObject msg = new TestObject();

            // explicitly register a bad delegate
            nc.Register<int, int>(msg.OnMessageReceivedWithReturnException);
            int x = nc.Send<int,int>(this, 5);
        }


        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestSendFuncInvalidDelegate() {
            TestObject msg = new TestObject();
            TestMessageFuncDelegate m = new TestMessageFuncDelegate();
            // explicitly register a bad delegate
            nc.Register<TestMessageFuncDelegate, int>(null);
            int x = nc.Send<TestMessageFuncDelegate, int>(this, m);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestSendFuncInvalidMessage() {
            TestObject msg = new TestObject();
            TestMessageFuncBadDelegate m = new TestMessageFuncBadDelegate();
            // explicity register a bad message type
            nc.Register<TestMessageFuncDelegate, int>(msg.OnMessageReceived);
            int x = nc.Send<TestMessageFuncBadDelegate, int>(this, m);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestRegisterMessageInvalidDelegate() {
            TestMessageFuncDelegate m = new TestMessageFuncDelegate();
            nc.Register<TestMessageFuncDelegate, int>(null);
        }

        [TestMethod]
        public void TestUnregisterAllFunc() {
            TestObject msg = new TestObject();
            TestMessageFuncDelegate m = new TestMessageFuncDelegate();
 
            nc.Register<TestMessageFuncDelegate, int>(msg.OnMessageReceived);
            nc.Register<string, string>(msg.OnMessageReceivedWithReturn);
  
            Assert.IsTrue(nc.RegisteredCount == 2);
            for (int i = 0; i < 2; i++) {
                int x = nc.Send<TestMessageFuncDelegate, int>(this, m);
                Assert.IsTrue(x == -1);
            }
            // Unregister all of one type and make sure the other type is still there
            nc.Unregister<TestMessageFuncDelegate, int>();
            Assert.IsTrue(nc.RegisteredCount == 1);

            string res = nc.Send<string, string>(this, "This should make the bool set to false");
            Assert.IsTrue(res.CompareTo("OnMessageReceivedWithReturn") == 0);
            Assert.IsTrue(msg.msgReceived3 == false);
        }

    }


}
