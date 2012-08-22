using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPToolKit.Source;
using System.Windows.Resources;
using System.IO;
using System.Windows;
using System.IO.IsolatedStorage;

namespace WPToolKitUnitTest.Unit_Test {
    [TestClass]
    public class IOUrlTest {
        String fullpath = "c:/test.cs";
        String path = "c:/";
        String filename = "test.cs";
        IOUrl ioUrl;
        IOUrl ioHttp;

        [TestInitialize]
        public void SetUp() {
            ioUrl = new IOUrl(fullpath);
            fullpath = "c:/test.cs";
            path = "c:/";
            filename = "test.cs";
            ioHttp = new IOUrl("Http://www.foo.com");
        }

        [TestMethod]
        public void TestGetUrl() {
            // check return value is a Uri
            Uri res = ioUrl;
            Assert.IsTrue(res.Equals((Uri)ioUrl));
        }
        [TestMethod]
        public void TestGetUrlString() {
            // check return value is a string;
            String res = ioUrl;
            Assert.IsTrue(res.CompareTo(filename) == 0);
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void TestBasicHttpFilename() {
            String f = ioHttp;
            
        }
        [TestMethod]
        public void TestBasicGetPath() {
            String p;
            p = ioUrl.GetPath();
            Assert.IsTrue(path.CompareTo(p) == 0);
        }

        [TestMethod]
        public void TestAdvancedGetPath() {
            IOUrl pathTest = new IOUrl("/");
            String path = pathTest.GetPath();
            String fn = pathTest;
            Assert.IsTrue("/".CompareTo(path) == 0);
            Assert.IsTrue("".CompareTo(fn) == 0);

            pathTest = new IOUrl("c:/");
            path = pathTest.GetPath();
            Assert.IsTrue("c:/".CompareTo(path) == 0);
            Assert.IsTrue("".CompareTo(fn) == 0);
        }
        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void TestGetPathInvalidArgument() {
            String p = ioHttp.GetPath();
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void TestGetFilenameInvalidArgument() {
            String f = ioHttp; // this is a http Uri, therefore not a valid file name
        }

        [TestMethod]
        public void TestGetFilename() {
            String f = ioUrl;
            Assert.IsTrue(filename.CompareTo(f) == 0);

            IOUrl rawFilename = new IOUrl("/test.c");
            String ff = rawFilename;
            Assert.IsTrue("test.c".CompareTo(ff) == 0);
        }
    }
}
