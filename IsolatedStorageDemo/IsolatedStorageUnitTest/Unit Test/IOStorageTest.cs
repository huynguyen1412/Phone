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
            
            // Default constructor
            IOStorage s = new IOStorage();
            Assert.AreEqual("", s.IOFilenameString, "IOStorage: IOFilenameString initialize incorrectly to: " + s.IOFilenameString.ToString());
            Uri url = new Uri("http://null/");
            Assert.IsTrue(url == s.IOFilenameUri, "IOStorage: IOFilenameUri initialize incorrectly to: " + s.IOFilenameUri.ToString());

            // Url constructor
            const string url1 = @"http://www.msn.com/foo/bar/image.png";
            IOStorage s1 = new IOStorage(new Uri(url1));
            Assert.AreEqual("image.png", s1.IOFilenameString, "IOStorage: IOFilenameString initialize incorrectly to: " + s1.IOFilenameString.ToString());
        }

        [TestMethod]
        [Description("IOStorageTest: Base the filename from the Url passed in")]
        public void TestSettingUrl() {

            IOStorage s = new IOStorage();

            //
            // Verify when setting the Uri, IOFilenameString is updated
            //
            const string sampleUrl = "https://www.msn.com/foo.jpeg";
            string fileName = "foo.jpeg";
            s.IOFilenameUri = new Uri(sampleUrl);
            Assert.AreEqual(fileName, s.IOFilenameString, "IOStorage: IOFilenameString initialize incorrectly to: " + s.IOFilenameString.ToString());

            // add trailing slash, which means no filename
            const string sampleUrl1 = "https://www.msn.com/foo.jpeg/";
            s.IOFilenameUri = new Uri(sampleUrl1);
            Assert.AreEqual("", s.IOFilenameString, "IOStorage: IOFilenameString initialize incorrectly to: " + s.IOFilenameString.ToString());

            // add trailing +, which means foo.jpeg+
            const string sampleUrl2 = "https://www.msn.com/foo.jpeg+";
            s.IOFilenameUri = new Uri(sampleUrl2);
            Assert.AreEqual("foo.jpeg+", s.IOFilenameString, "IOStorage: IOFilenameString initialize incorrectly to: " + s.IOFilenameString.ToString());
        }
    }
}
