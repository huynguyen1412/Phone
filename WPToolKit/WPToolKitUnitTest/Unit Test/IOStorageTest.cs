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
        private String filename;
        private String file;

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

        public IOStorageTest() {
            filename = "c:/test.c";
            file = "test.c";
        }
  
        [TestMethod]
        public void TestBasicConstruction() {
            IOStorage s = new IOStorage(new Uri(filename));
            String f = s;
            Assert.IsTrue(file.CompareTo(f) == 0);
        }

        [TestMethod]
        public void TestBasicSetUrl() {
            IOStorage s = new IOStorage();
            s.Url = new IOUrl("c:/test.c");
            Assert.IsTrue("test.c".CompareTo(s) == 0);
        }

// Test all the methods that with no Uri parameter to see if they throw correctly
        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void TestStringCoversionWithoutUrl() {
            IOStorage s = new IOStorage();
            String f = s;
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void TestUriCoversionWithoutUrl() {
            IOStorage s = new IOStorage();
            Uri u = s;
        }

        public void TestBaseConstructor() {
            IOStorage s = new IOStorage();
            String f = s;
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void TestLoadWithoutUrl() {
            IOStorage s = new IOStorage();
            s.Load();
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void TestRemoveWithoutUrl() {
            IOStorage s = new IOStorage();
            s.Remove();
        }
        [TestMethod]
        public void TestConstructorWithUrl() {
            Uri uri2 = new Uri("http://www.foomanchu.com/test.jpg");
            string fn = "test.jpg";

            IOStorage s = new IOStorage(uri2);
            String f = s;
            Uri u = s;

            Assert.IsTrue(u.Equals(uri2));
            Assert.IsTrue(f == fn);
        }

        [TestMethod]
        public void TestPropertyFilename() {

            Uri uri = new Uri("http://www.foomanchu.com");
            Uri uri1 = new Uri("http://www.foomanchu.edu");
            Uri uri2 = new Uri("http://www.foomanchu.com/test.jpg");
            String fn = "test.jpg";

            // verify there isn't a filename within uri
            IOStorage s = new IOStorage(uri);
            Assert.IsTrue(((Uri)s).Equals(uri));

            // verify there is a filename within the uri
            s = new IOStorage(uri2);
            Assert.IsTrue(((Uri)s).Equals(uri2));
            s = new IOStorage(uri2);
            Assert.IsTrue(s == fn);

            // create a uri with a relative path
            TestPropertyFilenameHelper("SplashScreenImage.jpg", "", "SplashScreenImage.jpg", UriKind.Relative);
            TestPropertyFilenameHelper("/SplashScreenImage.jpg", "/", "SplashScreenImage.jpg", UriKind.Relative);

            // verify non-relative path
            TestPropertyFilenameHelper("c:/SplashScreenImage.jpg", "c:/", "SplashScreenImage.jpg", UriKind.Absolute);
            TestPropertyFilenameHelper("file://c:/SplashScreenImage.jpg", "c:/", "SplashScreenImage.jpg", UriKind.Absolute);
        }

        
        private void TestPropertyFilenameHelper(string u, string path, string filename, UriKind k) {
            Uri uri = new Uri(u,k);
            IOStorage s = new IOStorage(uri);

            if (k == UriKind.Absolute) {
                Assert.IsTrue(s.GetPath().CompareTo(path) == 0);
            }
            Assert.IsTrue(((String)s).CompareTo(filename) == 0);
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
            Uri uri = new Uri("TestFile.jpg", UriKind.Relative);
            IOStorage s = new IOStorage(uri);

            // Create a stream with image data
            byte[] buffer = new byte[commonStream.Length];
            commonStream.Read(buffer, 0, (int)commonStream.Length);

            s.Save(buffer, commonStream.Length);
            StorageStream strm = new StorageStream(s.Load());
            Assert.IsTrue(commonStream.Length == strm.Length);

            IOStorage.GetUserFileArea.DeleteFile(uri.OriginalString);
        }
       
        [TestMethod, ExpectedException(typeof(IsolatedStorageException))]
        public void TestRemove() {

            // Make sure the stream is at the beginning of the file
            commonStream.Position = 0;
            Uri uri = new Uri("TestFile.jpg", UriKind.Relative);
            IOStorage s = new IOStorage(uri);

            // Create a stream with image data
            byte[] buffer = new byte[commonStream.Length];
            commonStream.Read(buffer, 0, (int)commonStream.Length);

            // save it then remove it.
            s.Save(buffer, commonStream.Length);
            s.Remove();
            StorageStream strm = new StorageStream(s.Load());
        }
    }
}
