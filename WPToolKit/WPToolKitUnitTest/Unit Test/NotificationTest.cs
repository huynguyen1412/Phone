using System;
using WPToolKit.Source;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#if(!CODE_COVERAGE)
    using System.Windows;


#endif

namespace WPToolKit.Unit_Test
{

    [TestClass]
    public class NotificationTest
    {
        Notification _nc;
        public class TestObject
        {
            public bool MsgReceived1;
            private const string Msg1 = "TestProperty";

            public bool MsgReceived2;
            private const string Msg2 = "ImageProperty";

            public bool MsgReceived3;
            private const int Results = 10;

            public TestObject() {
                MsgReceived1 = false;
                MsgReceived2 = false;
                MsgReceived3 = false;
            }
            public void OnMessageReceived(object from, TestMessage e) {
                MsgReceived1 = (e.Msg == Msg1);
                
            }
            public void OnMessageReceived(object from, string s) {
                MsgReceived2 = (s == Msg2);
            }

            public int OnMessageReceived(object from, TestMessageFuncDelegate x) {
                MsgReceived3 = (x.Value == Results);
                return -1;
            }

            public int OnMessageReceived(object from, TestMessageFuncBadDelegate x) {
                MsgReceived3 = (x.Value == Results);
                return -1;
            }

            public string OnMessageReceivedWithReturn(object from, string x) {
                MsgReceived3 = (x == Msg2);
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
            public string Msg = "TestProperty";
        }

        public class TestMessageFuncDelegate {
            public int Value = 10;
        }

        public class TestMessageFuncBadDelegate {
            public int Value = 22;
        }

        public class MtTestMessage {
            static private int _numMessages;

            public void OnMtTestMessage(object from, TestMessage e) {
                Interlocked.Increment(ref _numMessages);
            }
        }
        [TestInitialize]
        public void SetUp() {

#if CODE_COVERAGE
            _nc = new Notification();
#else
            _nc = Application.Current.GetApplicationNotificationObject();
#endif


        }

        [TestCleanup]
        public void CleanUp() {
            _nc = null;
        }

        [TestMethod]
        public void TestNotificationBasicMessage() {

            var msg = new TestObject();
            var m = new TestMessage();
            const string testProperty1 = "ImageProperty";

            _nc.Register<TestMessage>(msg.OnMessageReceived);
            _nc.Send<TestMessage>(this, m);
            Assert.IsTrue(msg.MsgReceived1);

            _nc.Send<TestMessage>(this, m);
            Assert.IsTrue(msg.MsgReceived1);

            _nc.Register<string>(msg.OnMessageReceived);
            _nc.Send<string>(this, testProperty1);
            Assert.IsTrue(msg.MsgReceived2);

            _nc.Unregister<TestMessage>();
            _nc.Unregister<string>();
            Assert.IsTrue(_nc.RegisteredCount == 0);
        }
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestNotificationUnregister() {
            var msg = new TestObject();
            var m = new TestMessage();

            _nc.Register<TestMessage>(msg.OnMessageReceived);
            _nc.Send<TestMessage>(this, m);

            Assert.IsTrue(msg.MsgReceived1);
            _nc.Unregister<TestMessage>(msg.OnMessageReceived);
            Assert.IsTrue(_nc.RegisteredCount == 0);

            _nc.Send<TestMessage>(this, m);
        }
        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestNotificationUnregisterAll() {
            var msg = new TestObject();
            var msg1 = new TestObject();

            var m = new TestMessage();

            // Register multiple receiptients 
            _nc.Register<TestMessage>(msg.OnMessageReceived);
            _nc.Register<TestMessage>(msg1.OnMessageReceived);

            _nc.Send<TestMessage>(this, m);
            Assert.IsTrue(msg.MsgReceived1);
            Assert.IsTrue(msg1.MsgReceived1);

            // Unregister them all and the Send should throw
            _nc.Unregister<TestMessage>();
            Assert.IsTrue(_nc.RegisteredCount == 0);
            _nc.Send<TestMessage>(this, m);

        }
        [TestMethod]
        public void TestSyncMessage() {

            var n = new Notification();
            var mt = new MtTestMessage();

            for(int i = 0; i < 10; i++) {
                n.Register<TestMessage>(mt.OnMtTestMessage);
            }

            // not supported yet.
#if WINDOWS_PHONE
            mt.OnMtTestMessage(null, null);
#else
            n.SendSync<TestMessage>(this, null);
#endif
            n.Unregister<TestMessage>();
        }

        // Test for Func<T>
        [TestMethod]
        public void TestRegisterFunc() {
            var msg = new TestObject();
            var m = new TestMessageFuncDelegate();
            _nc.Register<TestMessageFuncDelegate,int>(msg.OnMessageReceived);
            Assert.IsTrue(_nc.RegisteredCount == 1);
            _nc.Unregister<TestMessageFuncDelegate, int>(msg.OnMessageReceived);
            Assert.IsTrue(_nc.RegisteredCount == 0);
        }

        [TestMethod]
        public void TestSendFunc() {
            var msg = new TestObject();
            var m = new TestMessageFuncDelegate();
            _nc.Register<TestMessageFuncDelegate, int>(msg.OnMessageReceived);
            int x = _nc.Send<TestMessageFuncDelegate, int>(this, m);
            Assert.IsTrue(x == -1);
            Assert.IsTrue(msg.MsgReceived3);
            _nc.Unregister<TestMessageFuncDelegate, int>();

        }

        [TestMethod]
        public void TestSendWithDelegateThrowingException() {
            var msg = new TestObject();

            // explicitly register a bad delegate
            try {
                _nc.Register<int>(msg.OnMessageReceivedException);
                _nc.Send<int>(this, 5);
            }
            catch (ArgumentException e) {
                Assert.IsTrue(e.GetType() == typeof(ArgumentException));
            }
            finally {
                _nc.Unregister<int>();
            }
        }

        [TestMethod]
        public void TestSendWithFuncDelegateThrowingException() {
            var msg = new TestObject();

            try {
                // explicitly register a bad delegate
                _nc.Register<int, int>(msg.OnMessageReceivedWithReturnException);
                int x = _nc.Send<int, int>(this, 5);
            }
            catch (ArgumentException e) {
                Assert.IsTrue(e.GetType() == typeof(ArgumentException));
            }
            finally {
                _nc.Unregister<int, int>();
            }
        }


        [TestMethod]
        public void TestSendFuncInvalidDelegate() {
            var msg = new TestObject();
            var m = new TestMessageFuncDelegate();
            // explicitly register a bad delegate

            try {
                _nc.Register<TestMessageFuncDelegate, int>(null);
                int x = _nc.Send<TestMessageFuncDelegate, int>(this, m);
            } 
            catch (ArgumentException e) {
                Assert.IsTrue(e.GetType() == typeof(ArgumentException));
            } 
            finally {
                _nc.Unregister<TestMessageFuncDelegate, int>();
            }
        }

        [TestMethod]
        public void TestSendFuncInvalidMessage() {
            var msg = new TestObject();
            var m = new TestMessageFuncBadDelegate();

            try {
                // explicity register a bad message type
                _nc.Register<TestMessageFuncDelegate, int>(msg.OnMessageReceived);
                int x = _nc.Send<TestMessageFuncBadDelegate, int>(this, m);
            } 
            catch (ArgumentException e) {
                Assert.IsTrue(e.GetType() == typeof(ArgumentException));
            } 
            finally {
                _nc.Unregister<TestMessageFuncDelegate, int>(msg.OnMessageReceived);
            }
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestRegisterMessageInvalidDelegate() {
            var m = new TestMessageFuncDelegate();
            _nc.Register<TestMessageFuncDelegate, int>(null);
        }

        [TestMethod]
        public void TestUnregisterAllFunc() {
            var msg = new TestObject();
            var m = new TestMessageFuncDelegate();
 
            _nc.Register<TestMessageFuncDelegate, int>(msg.OnMessageReceived);
            _nc.Register<string, string>(msg.OnMessageReceivedWithReturn);
  
            Assert.IsTrue(_nc.RegisteredCount == 2);
            for (int i = 0; i < 2; i++) {
                int x = _nc.Send<TestMessageFuncDelegate, int>(this, m);
                Assert.IsTrue(x == -1);
            }
            // Unregister all of one type and make sure the other type is still there
            _nc.Unregister<TestMessageFuncDelegate, int>();
            Assert.IsTrue(_nc.RegisteredCount == 1);

            string res = _nc.Send<string, string>(this, "This should make the bool set to false");
            Assert.IsTrue(String.Compare(res, "OnMessageReceivedWithReturn", StringComparison.Ordinal) == 0);
            Assert.IsTrue(msg.MsgReceived3 == false);
        }

    }


}
