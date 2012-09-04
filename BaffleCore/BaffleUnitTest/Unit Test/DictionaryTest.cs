using System;
using BaffleCore;
using BaffleCore.Source;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BaffleUnitTest.Unit_Test {

    [TestClass]
    public class DictionaryTest {

        [TestMethod]
        public void TestDictionaryCreate() {
            var dict = new Dictionary();
            dict.CreateDictionaryHash();


        }
    }
}
