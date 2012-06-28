using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPToolKit;
using System.Windows.Resources;
using System.IO;
using System.Windows;
using System.IO.IsolatedStorage;


namespace WPToolKitUnitTest.Unit_Test
{
    [TestClass]
    public class IOStorageTest
    {
        IOStorage commonIOStore;
        Uri commonUri;
        StorageStream commonStream;

        [TestInitialize]
        public void SetUp() {
            commonUri = new Uri("SplashScreenImage.jpg", UriKind.Relative);
            commonIOStore = new IOStorage(commonUri);

            // delete the file from previous test
            IOStorage.GetUserFileArea.DeleteFile(commonUri.OriginalString);

            // Create the file with one stream and load it with a different stream
            commonStream = new StorageStream(Application.GetResourceStream(commonUri).Stream);
            commonIOStore.Save(commonStream);
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

            // verify there isn't a filename within uri
            IOStorage s = new IOStorage(uri);
            Assert.IsTrue(s.IOFilenameUri.Equals(uri));
            Assert.IsTrue(s.IOFilenameString == null);

            // verify there is a filename within the uri
            s = new IOStorage(uri2);
            Assert.IsTrue(s.IOFilenameUri.Equals(uri2));
            s = new IOStorage(uri2);
            Assert.IsTrue(s.IOFilenameString == fn);

            // create a uri with a relative path
            TestPropertyFilenameHelper("SplashScreenImage.jpg");
            TestPropertyFilenameHelper("/SplashScreenImage.jpg");

            // verify non-relative path
            TestPropertyFilenameHelper("c:/SplashScreenImage.jpg", "c:/SplashScreenImage.jpg", "SplashScreenImage.jpg");
            TestPropertyFilenameHelper("file://c:/SplashScreenImage.jpg", "c:/SplashScreenImage.jpg", "SplashScreenImage.jpg");
        }

        private void TestPropertyFilenameHelper(string u, string path, string filename) {
            Uri uri = new Uri(u);
            IOStorage s = new IOStorage(uri);
            Assert.IsTrue(s.IOFilenamePath.CompareTo(path) == 0);
            Assert.IsTrue(s.IOFilenameString.CompareTo(filename) == 0);
            Assert.IsTrue(s.IOFilenameUri.Equals(u));
        }
        private void TestPropertyFilenameHelper(string path) {
            Uri uri = new Uri(path, UriKind.Relative);
            IOStorage s = new IOStorage(uri);
            Assert.IsTrue(s.IOFilenamePath.CompareTo(path) == 0);
            Assert.IsTrue(s.IOFilenameString.CompareTo(path) == 0);
            Assert.IsTrue(s.IOFilenameUri.Equals(uri));
        }

        [TestMethod, ExpectedException(typeof(IsolatedStorageException))]
        public void TestDefaultLoadwithFileNotFound() {

            // delete the file from the setup method
            IOStorage.GetUserFileArea.DeleteFile(commonUri.OriginalString);

            Uri uri = new Uri("SplashScreenImage.jpg", UriKind.Relative);
            IOStorage s = new IOStorage(uri);
            s.Load();
        }

        [TestMethod]
        public void TestDefaultLoad() {

            // test load withOUT Uri argument
            IOStorage ss = new IOStorage(commonUri);
            StorageStream result = new StorageStream(commonIOStore.Load());
            Assert.IsTrue(result.Length == commonStream.Length);

            // delete the file 
            IOStorage.GetUserFileArea.DeleteFile(commonUri.OriginalString);
        }

        [TestMethod]
        public void TestLoadWithUrl() {

            // test load with Uri argument
            IOStorage ss = new IOStorage();
            StorageStream result = new StorageStream(commonIOStore.Load(commonUri));
            Assert.IsTrue(result.Length == commonStream.Length);
        }

        [TestMethod]
        public void TestSaveWithStorageStream() {
            TestLoadWithUrl();
        }
        [TestMethod]
        public void TestSaveRaw() {
            // Make sure the stream is at the beginning of the file
            commonStream.Position = 0;

            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
        }
    }
}
