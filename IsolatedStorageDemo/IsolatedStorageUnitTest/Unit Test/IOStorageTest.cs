using System;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            Assert.IsNull(s.IOFilenameString);
            Assert.IsNull(s.IOFilenameUri);

            // Url constructor
            const string url1 = @"http://www.msn.com/foo/bar/image.png";
            Uri url = new Uri(url1);
            IOStorage s1 = new IOStorage(url);

            Assert.AreEqual("image.png", s1.IOFilenameString, "IOStorage: IOFilenameString initialize incorrectly to: " + s1.IOFilenameString.ToString());
            Assert.AreEqual(url1, s1.IOFilenameUri.ToString(), "IOStorage: IOFilenameUri initialize incorrectly to: " + s1.IOFilenameUri.ToString());

            // Now check to see if the IOFilenameString get's nulled out whem the Url changes to null
            s1.IOFilenameUri = null;
            Assert.IsNull(s1.IOFilenameString);
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

        [TestMethod, ExpectedException(typeof(ArgumentNullException), "Null Parameter for Uri")]
        [Description("IOStorageTest: Test the Load()")]
        public void TestLoadOperationWihoutUrl() {

            // This should throw an exception
            IOStorage s = new IOStorage();
            s.Load();
        }

        [TestMethod, ExpectedException(typeof(IsolatedStorageException), "Valid Uri but does not exist")]
        [Description("IOStorageTest: Test the Load()")]
        public void TestLoadOperationWithUrl() {

            IOStorage s = new IOStorage();
            Stream result = new MemoryStream();

            // Give a non existent web resource, and test to see if it was found in storage.
            Uri sampleUrl = new Uri("http://www.msn.com/test.jpg");
            s.IOFilenameUri = sampleUrl;
            s.Load();
        }
    }
}
