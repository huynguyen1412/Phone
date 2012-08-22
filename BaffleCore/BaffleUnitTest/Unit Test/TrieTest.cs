using System;
using BaffleCore.Source;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BaffleUnitTest.Unit_Test
{
    [TestClass]
    public class TrieTest {
        [TestInitialize]
        public void SetUp() {}

        [TestMethod, ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void TestTriesCreateWithEmptyString() {
            var table = new Trie("");
        }

        [TestMethod, ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void TestTriesCreateWithOversizeString() {
            // Sixteen is the maximum size, make sure it throws if greater than 16.
            var table = new Trie("AAAAAAAAAAAAAAAAA");
        }

        [TestMethod]
        public void TestMapping() {
            const String characters = "AbXZCA";
            var table = new Trie(characters);

            // dupilcate letters are ignored
            Assert.IsTrue(condition: table.NumberOfUniqueCharacters == 5);

            // Check the indexs
            // A=0, b=1, X=2, .etc
            int x = 0;
            foreach (var c in characters) {
                int index = table.MapCharacter(c);
                Assert.IsTrue(x == index);
                ++x;
                // the tail in the sting, A, is really index 0 again
                if (x == 5) {
                    x = 0;
                }
            }
        }

        [TestMethod]
        public void TestBadMapping() {
            const String characters = "AbXZCA";
            var table = new Trie(characters);

            // access a character that is not in the map
            int index = table.MapCharacter('W');
            Assert.IsTrue(index == -1);
        }

        [TestMethod]
        public void TestAdd() {
            const String characters = "LELoHP";
            var table = new Trie(characters);

            table.Add("Hello");
            table.Add("Hell");
            table.Add("Help");
            table.Add("Phelp");




        }
    }
}
