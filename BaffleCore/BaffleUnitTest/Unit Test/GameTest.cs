using System.Threading;
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
            list = MainPage.dictionary.EnumerateWordsBeginWith("AOE");
            list = MainPage.dictionary.EnumerateWordsBeginWith("OEA");
            list = MainPage.dictionary.EnumerateWordsBeginWith("EOA");

          //  Assert.IsTrue(list.Count == 10320);
           // Assert.IsTrue(System.String.Compare(list[0], "AAH", System.StringComparison.Ordinal) == 0);
   //         Assert.IsTrue(System.String.Compare(list[list.Count - 1], "AZYGOUS", System.StringComparison.Ordinal) == 0);

     //       list = MainPage.dictionary.EnumerateWordsBeginWith("AEO");

        }
    }
}
