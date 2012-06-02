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
using IsolatedStorageDemo;


namespace IsolatedStorageUnitTest
{
    [TestClass]
    public class ViewModelTest : SilverlightTest
    {
        [TestInitialize]
        public void SetUp()
        {
        }

        [TestCleanup]
        public void Teardown()
        {
        }

        [TestMethod]
        [Description("Sample Test 1")]
        public void TestConstructor()
        {
            Assert.IsNotNull(1);
        }

        [TestMethod]
        [Description("Sample Test 2")]
        public void TestWidget()
        {
            Assert.IsNotNull(1);
        }


        [TestMethod]
        [Description("Test View Model Constructor")]
        public void TestConstuctor()
        {
            ViewModel vm = new ViewModel();
        }

    }

    [TestClass]
    public class ModelTest : SilverlightTest
    {
        [TestInitialize]
        public void SetUp()
        {
        }

        [TestCleanup]
        public void Teardown()
        {
        }

        [TestMethod]
        [Description("Model Test 1")]
        public void TestConstructor()
        {
            Assert.IsNotNull(1);
        }

        [TestMethod]
        [Description("Model Test 2")]
        public void TestWidget()
        {
            Assert.IsTrue(5 == 5, "Five == Five");
        }


        [TestMethod]
        [Description("Test Model Constructor")]
        public void TestConstuctor()
        {
            Model m = new Model();
        
        }
    }
}
