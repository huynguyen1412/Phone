using BaffleCore.Source;
using Microsoft.Phone.Controls;
using Microsoft.Silverlight.Testing;
using BaffleCoreTest.Unit_Test;

namespace BaffleUnitTest {
    public partial class MainPage : PhoneApplicationPage {
        static public PrefixTree dictionary;
        // Constructor
        public MainPage() {
            InitializeComponent();

            // PrefixTree takes a few seconds to create, so do it here
            dictionary = new PrefixTree();
            dictionary.CreateDictionaryHash();

            const bool runUnitTests = true;

            if (runUnitTests) {
                Content = UnitTestSystem.CreateTestPage();
                IMobileTestPage imtp =
                         Content as IMobileTestPage;

                if (imtp != null) {
                    BackKeyPress += (x, xe) => xe.Cancel =
                            imtp.NavigateBack();
                }
            }
        }
    }
}