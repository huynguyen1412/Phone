using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPToolKit;

namespace WPToolKitUnitTest
{
    [TestClass]
    public class NotificationTest
    {
        NotificationCenter nc;

        [TestInitialize]
        public void SetUp() {

            nc = new NotificationCenter();
        }

        [TestMethod]
        public void TestNotificationSetup() {
           // nc.Register(this, MessageSink);

            
        }


        
    }
}
