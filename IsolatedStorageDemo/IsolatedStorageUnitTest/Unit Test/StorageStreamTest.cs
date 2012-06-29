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
using WPToolKit;

namespace IsolatedStorageUnitTest
{
    /// <summary>
    /// Test Cases for StorageStreamTest
    /// </summary>
    /// <remarks></remarks>
    [TestClass]
    public class StorageStreamTest
    {
        StorageStream stream;

        [TestInitialize]
        public void Setup() {
            stream = new StorageStream();
        }

        [TestMethod]
        [Description("StorageStreamTest: Creation of a StorageStream")]
        public void TestConstruction() {
            Assert.IsNotNull(stream);
        }

        [TestMethod, ExpectedException(typeof(System.ObjectDisposedException))]
        [Description("StorageStreamTest: Close should throw exception because of Dispose")]
        public void TestClose() {

            Assert.IsNotNull(stream);
            using (stream) { 
            };

            long position = stream.Position;
        }

        [TestMethod, ExpectedException(typeof(ObjectDisposedException))]
        [Description("StorageStreamTest: Dispose should throw exception because of base:Dispose")]
        public void TestDispose() {

            Assert.IsNotNull(stream);
            using (stream) {
            };

            long position = stream.Position;
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        [Description("StorageStreamTest: Test Position property")]
        public void TestCopyNullArg() {
            Assert.IsNotNull(stream);

            StorageStream streamCopy = null;

            // set to a non zero position to make sure the copy is reset back to 0
            stream.CopyTo(streamCopy);
        }

        [TestMethod]
        [Description("StorageStreamTest: Test copying a stream")]
        public void TestCopy() {

            Assert.IsNotNull(stream);
            StorageStream streamCopy = new StorageStream();
            
            // set to a non zero position to make sure the copy is reset back to 0
            stream.Position = 2;
            stream.CopyTo(streamCopy);

            Assert.IsNotNull(streamCopy);
            Assert.Equals(stream, streamCopy);
            Assert.IsTrue(streamCopy.Position == 0, "StorageStreamTest: Position not set to zero after copy");
        }

        [TestMethod]
        [Description("StorageStreamTest: Test Position property")]
        public void TestPosition() {
            Assert.IsNotNull(stream);

            // set and get position
            stream.Position = 4;
            Assert.IsTrue(stream.Position == 4);
        }
    }
}
