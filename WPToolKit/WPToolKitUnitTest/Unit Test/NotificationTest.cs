﻿using System;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPToolKit;

namespace WPToolKitUnitTest
{

    [TestClass]
    public class NotificationTest
    {
        public class TestObject
        {
            public bool msgReceived1;
            string msg1 = "TestProperty";

            public bool msgReceived2;
            string msg2 = "ImageProperty";

            public TestObject() {
                msgReceived1 = false;
                msgReceived2 = false;
            }

            public void OnMessageReceived(object from, TestMessage e) {
                msgReceived1 = (e.msg == msg1);
                
            }
            public void OnMessageReceived(object from, string s) {
                msgReceived2 = (s == msg2);
            }
        }

        public class TestMessage  {
            public string msg = "TestProperty";
        }

        Notification nc;

        [TestInitialize]
        public void SetUp() {

            nc = new Notification();
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
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestNotificationUnRegister() {
            TestObject msg = new TestObject();
            TestMessage m = new TestMessage();

            nc.Register<TestMessage>(msg.OnMessageReceived);
            nc.Send<TestMessage>(this, m);

            Assert.IsTrue(msg.msgReceived1);
            nc.Unregister<TestMessage>(msg.OnMessageReceived);
            nc.Send<TestMessage>(this, m);
        }

        [TestMethod, ExpectedException(typeof(ArgumentException))]
        public void TestNotificationUnRegisterAll() {
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
            Assert.IsTrue(nc.RegisteredCount() == 0);
            nc.Send<TestMessage>(this, m);

        }
    }
}
