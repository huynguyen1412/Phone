using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPToolKit;
using System.Windows.Resources;
using System.IO;
using System.Windows;


namespace WPToolKitUnitTest.Unit_Test
{
    [TestClass]
    public class IOStorageTest
    {
        [TestInitialize]
        public void SetUp() {
        }
        [TestMethod]
        public void TestDefaultConstructor() {
            IOStorage s = new IOStorage();
            Assert.IsTrue(s.IOFilenameUri == null);
            Assert.IsTrue(s.IOFilenameString == null);
        }
        [TestMethod]
        public void TestConstructorWithUrl() {
            Uri uri2 = new Uri("http://www.foomanchu.com/test.jpg");
            string fn = "test.jpg";

            IOStorage s = new IOStorage(uri2);
            Assert.IsTrue(s.IOFilenameUri.Equals(uri2));
            Assert.IsTrue(s.IOFilenameString == fn);
        }
        [TestMethod]
        public void TestPropertyFilename() {

            Uri uri = new Uri("http://www.foomanchu.com");
            Uri uri1 = new Uri("http://www.foomanchu.edu");
            Uri uri2 = new Uri("http://www.foomanchu.com/test.jpg");
            string fn = "test.jpg";

            IOStorage s = new IOStorage(uri);

            Assert.IsTrue(s.IOFilenameUri.Equals(uri));
            Assert.IsFalse(s.IOFilenameUri.Equals(uri1));

            // verify there isn't a filename with the uri
            Assert.IsTrue(s.IOFilenameString == null);

            s = new IOStorage(uri2);
            Assert.IsTrue(s.IOFilenameUri.Equals(uri2));

            // verify there IS a filename 
            s = new IOStorage(uri2);
            Assert.IsTrue(s.IOFilenameString == fn);

            string path = "SplashScreenImage.jpg";
            Uri filename = new Uri(path, UriKind.Relative);
            s = new IOStorage(filename);

            Assert.IsTrue(s.IOFilenamePath.CompareTo(path) == 0);
            StorageStream strm = new StorageStream(Application.GetResourceStream(filename).Stream);
        }

        [TestMethod]
        public void TestDefaultLoad() {
            
        }
        [TestMethod]
        public void TestLoadWithUrl() {
        }
        [TestMethod]
        public void TestSaveRaw() {
        }
        [TestMethod]
        public void TestSaveRaw1() {
        }
        [TestMethod]
        public void TestSaveRaw2() {
        }
    }
}
