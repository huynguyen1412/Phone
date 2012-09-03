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
        public void TestBasicAdd() {
            const String characters = "LELoHP";
            var table = new Trie(characters);

            table.Add("HELLO");
            table.Add("HELL");
            table.Add("HELP");
            table.Add("PHELP");

            Assert.IsTrue(table.Count == 4);
            var list = table.EnumerateAllWords();

            Assert.IsTrue(list.Count == 4);
            Assert.IsTrue(list.Contains("HELLO"));
            Assert.IsTrue(list.Contains("HELL"));
            Assert.IsTrue(list.Contains("HELP"));
            Assert.IsTrue(list.Contains("PHELP"));
        }

        [TestMethod]
        public void TestEnumerate() {
            const String characters = "DOUTYA";
            var table = new Trie(characters);

            // Test with none in the table
            Assert.IsTrue(table.Count == 0);
            var list = table.EnumerateAllWords();
            Assert.IsTrue(list.Count == 0);

            // Test with one in the table
            table.Add("Duty");
            Assert.IsTrue(table.Count == 1);
            list = table.EnumerateAllWords();
            Assert.IsTrue(list.Count == 1);
            Assert.IsTrue(list.Contains("DUTY"));
            
            // Add some more words and test
            table.Add("Toad");
            table.Add("Dot");
            table.Add("You");
            table.Add("Dat");
            table.Add("Tad");
            Assert.IsTrue(table.Count == 6);
            list = table.EnumerateAllWords();
            Assert.IsTrue(list.Count == 6);

            Assert.IsTrue(table.Contains("Toad"));

        }

        [TestMethod]
        public void TestContains() {
            const String characters = "DOUTYA";
            var table = new Trie(characters);

            // test with no words
            Assert.IsFalse(table.Contains("Bogus"));

            // Add some more words and test
            table.Add("Duty");
            table.Add("Toad");
            table.Add("Dot");
            table.Add("You");
            table.Add("Dat");
            table.Add("Tad");
            Assert.IsTrue(table.Count == 6);
            Assert.IsTrue(table.Contains("Duty"));
            Assert.IsTrue(table.Contains("Tad"));
            Assert.IsTrue(table.Contains("You"));
            Assert.IsTrue(table.Contains("Toad"));
            Assert.IsTrue(table.Contains("Dat"));
            Assert.IsTrue(table.Contains("Dot"));

            // test not found with a mix of letters
            Assert.IsFalse(table.Contains("NoFound"));
            // test not found with know letters
            Assert.IsFalse(table.Contains("DDDDDDDDDDDDDDD"));

            
        }
    }
}
