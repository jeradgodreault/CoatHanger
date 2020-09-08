using CoatHanger.WebDriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoatHanger.Core.Testing.UnitTest
{
    [TestClass]
    [Ignore("Work-in-progress, need to setup pipeline")]
    public class NavigationDriverTest
    {
        private TestContext testContextInstance;
        private CoatHangerDriver _webDriver;
        private TestProcedure _testProcedure;

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestInitialize()]
        public void BeforeTestExecution()
        {
            _testProcedure = new TestProcedure();
            _webDriver = new CoatHangerDriver(new ChromeDriver(), ref _testProcedure);
        }

        /// <summary>
        /// Most project already have selenium   
        /// </summary>
        #region Backwards compitability with existing automated Selenium test cases  

        [TestMethod]
        public void WhenNavigating_GoToUrl_WithFullUrl_ExpectFullLink()
        {
            // arrange
            var url = "https://en.wikipedia.org/wiki/Unit_testing";

            // act
            _webDriver.Navigate().GoToUrl(url);

            // assert
            Assert.AreEqual("Use the browser to navigate to `https://en.wikipedia.org/wiki/Unit_testing`", _testProcedure.Steps[0].Action);
        }

        [TestMethod]
        public void WhenNavigating_Back_ExpectAction()
        {
            // arrange
            INavigation navigation = _webDriver.Navigate();
            navigation.GoToUrl("https://en.wikipedia.org/wiki/Unit_testing");
            var pageTitle = _webDriver.Title;

            // act
            navigation.GoToUrl("https://en.wikipedia.org/wiki/Requirements_analysis");
            navigation.Back();

            // assert
            Assert.AreEqual("Use the browser *back* button.", _testProcedure.Steps[2].Action);
            Assert.AreEqual(pageTitle, _webDriver.Title);
        }

        [TestMethod]
        public void WhenNavigating_Forward_ExpectAction()
        {
            // arrange
            INavigation navigation = _webDriver.Navigate();
            navigation.GoToUrl("https://en.wikipedia.org/wiki/Unit_testing");
            var pageTitle = _webDriver.Title;
            navigation.Back();

            // act
            navigation.Forward();

            // assert
            Assert.AreEqual("Use the browser *forward* button.", _testProcedure.Steps[2].Action);
            Assert.AreEqual(pageTitle, _webDriver.Title);

        }

        [TestMethod]
        public void WhenNavigating_Refresh_ExpectAction()
        {
            // arrange
            INavigation navigation = _webDriver.Navigate();
            navigation.GoToUrl("https://en.wikipedia.org/wiki/Unit_testing");
            var pageTitle = _webDriver.Title;

            // act
            navigation.Refresh();

            // assert
            Assert.AreEqual("Use the browser *refresh* button.", _testProcedure.Steps[1].Action);
            Assert.AreEqual(pageTitle, _webDriver.Title);
        }

        #endregion

        #region These are new API methods to control test cases outputs

        [TestMethod]
        public void WhenNavigatingStep_GoToUrl_IncludeDomainDisabled_ExpectPartialPath()
        {
            // arrange
            var url = "https://en.wikipedia.org/wiki/Unit_testing";

            // act
            _webDriver.NavigateStep().GoToUrl(url: url, testCaseIncludeDomain: false);
            
            // assert
            Assert.AreEqual("Use the browser to navigate to `/wiki/Unit_testing`", _testProcedure.Steps[0].Action);
        }

        [TestMethod]
        public void WhenNavigatingStep_GoToUrl_IncludeDomainEnabled_ExpectFullPath()
        {
            // arrange
            var url = "https://en.wikipedia.org/wiki/Unit_testing";

            // act
            _webDriver.NavigateStep().GoToUrl(url: url, testCaseIncludeDomain: true);

            // assert
            Assert.AreEqual("Use the browser to navigate to `https://en.wikipedia.org/wiki/Unit_testing`", _testProcedure.Steps[0].Action);
        }

        #endregion


        [TestCleanup]
        public void AfterTestExecution()
        {
            _webDriver.Quit();
        }

    }
}
