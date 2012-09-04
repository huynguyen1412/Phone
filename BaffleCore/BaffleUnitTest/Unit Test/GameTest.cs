using System;
using System.Threading;
using BaffleCore.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace BaffleUnitTest.Unit_Test
{
    [TestClass]
    public class GameTest {

        [TestInitialize]
        public void SetUp() {
            //Note, the PrefixTree is created in MainPage to build it before testing
        }
        
        [TestMethod]
        public void TestWordLookUp() {

            // incase the PrefixTree is not ready
            while (! MainPage.dictionary.Ready) {
                Thread.Sleep(10);
            }

            Assert.IsTrue(MainPage.dictionary.Contains("Hello"));
            Assert.IsTrue(MainPage.dictionary.Contains("AAH"));
            Assert.IsTrue(MainPage.dictionary.Contains("LARGEHEARTEDNESS"));
            Assert.IsTrue(MainPage.dictionary.Contains("PETUNIAS"));
            Assert.IsTrue(MainPage.dictionary.Contains("ZYZZYVAS"));
        }

    }
}
