using System.Collections.Generic;
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
        public void TestWordLookup() {

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

        [TestMethod]
        public void TestWordPrefixLookup() {

            // incase the PrefixTree is not ready
            while (! MainPage.dictionary.Ready) {
                Thread.Sleep(10);
            }


            // all these test numbers are based on the dictionary.dat file included.  If it changes,
            // these test need to be updated.
            var list = MainPage.dictionary.EnumerateWordsBeginWith("AEO");
            Assert.IsTrue(list.Count == 5);

            list = MainPage.dictionary.EnumerateWordsBeginWith("AOE");
            Assert.IsTrue(list.Count == 0);

            list = MainPage.dictionary.EnumerateWordsBeginWith("LUMINISMS");
            Assert.IsTrue(list.Count == 1);
            Assert.IsTrue(System.String.Compare(list[0], "LUMINISMS", System.StringComparison.Ordinal) == 0);
            list = MainPage.dictionary.EnumerateWordsBeginWith("ZYZZYV");
            Assert.IsTrue(list.Count == 2);
        }

        [TestMethod]
        public void TestWordResolve() {
            var gb = new GameBoard();
            gb.Roll();
            DieFace[] set = gb.GetCurrentSet();

            // incase the PrefixTree is not ready
            while (!MainPage.dictionary.Ready) {
                Thread.Sleep(10);
            }

            Dictionary<string,bool> wordList = gb.ResolveWords(MainPage.dictionary, gb.GetCurrentSet());
        }
    }


}
