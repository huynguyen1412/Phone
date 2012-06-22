using System;
using System.IO;
using System.IO.IsolatedStorage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WPToolKit;

namespace WPToolKit
{


    [TestClass]
    public class ApplicationSettingsTest
    {
        ApplicationSettings appSettings;

        [TestInitialize]
        public void Setup() {
           appSettings = new ApplicationSettings();
        }

        [TestMethod]
        [Description("Reset Isolated Storage")]
        public void TestReset() {
            const string resetKey = "Resetkey";
            const string data = "ResetData";

            // Add a key
            appSettings[resetKey] = data;
            appSettings.Reset();
            var result = appSettings[resetKey];
            Assert.IsNull(result);

        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        [Description("Add a key that with null parameters")]
        public void TestAddWithNullParametersAssignment() {
            appSettings[null] = "SomeData";
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        [Description("Add a key that with null parameters")]
        public void TestAddWithNullParameters() {
            var result = appSettings[null];
        }

        [TestMethod]
        [Description("Add a key that doesn't exist")]
        public void TestAdd() {

            const string key = "NewKey";
            const string data = "Foo";

            appSettings[key] = data;
            var result = appSettings[key];
            Assert.AreEqual(data, result, "AddKey: Expected Value:" + data + " Actual Value:" + result);
            appSettings.Remove(key);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        [Description("Remove a key that with null parameters")]
        public void TestRemoveWithNullParameters() {
            ApplicationSettings appSettings = new ApplicationSettings();
            appSettings.Remove(null);
        }

        [TestMethod]
        [Description("Attempt to Remove a key that does not exist")]
        public void TestRemove() {

            const string key = "RemoveKey";
            const string bogusKey = "BogusKey";
            const string data = "RemoveKeyData";
            appSettings[key] = data;

            var result = appSettings.Remove(key);
            Assert.IsTrue(result, "Remove: failed to remove the Key:" + key);

            result = appSettings.Remove(bogusKey);
            Assert.IsFalse(result);
        }
    }
}
