using System;
using System.Threading;
using BaffleCore.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;



namespace BaffleUnitTest.Unit_Test
{
    [TestClass]
    public class GameTest {

        private Dictionary dictionary;
        [TestInitialize]
        public void SetUp() {
            dictionary = new Dictionary();
            dictionary.CreateDictionaryHash();

        }
        
        [TestMethod]
        public void TestWordLookUp() {
            while (!dictionary.Ready) {
                Thread.Sleep(100);
            }

            Assert.IsTrue(dictionary.Contains("Hello"));

        }

    }
}
