using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPToolKit;
using System.IO;

namespace WPToolKitUnitTest.Unit_Test
{
    [TestClass]
    public class StorageStreamObjectTest
    {

        [TestInitialize]
        public void SetUp() {
        }

        [TestMethod]
        public void TestStorageStreamDefaultConstructor() {
            StorageStream s = new StorageStream();
            Assert.IsTrue(s.Position == 0);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void TestStorageStreamCopyConstructor() {

            StorageStream srcStream = new StorageStream();
            srcStream.Position=10;

            StorageStream s = new StorageStream(srcStream);
            Assert.IsTrue(s.Position == 0);
            Assert.IsTrue(srcStream.Position == 10);

            // this should throw ArgumentNullException
            s = new StorageStream((StorageStream)null);
        }
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void TestStorageStreamCopyConstructor1() {

            Stream s = new MemoryStream();
            StorageStream srcStream = new StorageStream(s);
            srcStream.Position = 10;

            Assert.IsTrue(s.Position == 0);
            Assert.IsTrue(srcStream.Position == 10);

            // this should throw ArgumentNullException
            s = new StorageStream((Stream)null);
        }
        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void TestStorageStreamCopyTo() {
            StorageStream srcStream = new StorageStream();
            StorageStream destStream = null;
            srcStream.CopyTo(destStream);
 
        }



    }
}
