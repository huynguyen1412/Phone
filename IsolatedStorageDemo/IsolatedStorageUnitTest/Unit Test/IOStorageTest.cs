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
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using IsolatedStorageDemo;
using System.IO;

namespace IsolatedStorageDemo {
    [TestClass]
    public class IOStorageTest {

        [TestInitialize]
        public void Setup() {
        }

        [TestMethod]
        [Description("IOStorageTest: Creation of a StorageStream")]
        public void TestConstruction() {
            IOStorage s = new IOStorage();
            Assert.AreSame("", s.IOFilenameString, "IOStorage: IOFilenameString initialize incorrectly to: " + s.IOFilenameString.ToString());
            Uri url = new Uri("http://null/");
            Assert.IsTrue(url == s.IOFilenameUri, "IOStorage: IOFilenameUri initialize incorrectly to: " + s.IOFilenameUri.ToString());
        }
    }
}
