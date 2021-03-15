using CoatHanger.WebDriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace CoatHanger.Core.Testing.UnitTest
{
    [TestClass]
    [Ignore("Work-in-progress, need to setup pipeline")]
    public class NavigationDriverTest
    {
        private TestContext testContextInstance;
        private CoatHangerDriver _webDriver;
        private TestProcedure _testProcedure;
        private static ChromeDriver _chromeDriver;

        /// <summary>
        ///  Gets or sets the test context which provides
        ///  information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            _chromeDriver = new ChromeDriver();
        }

        [ClassCleanup()]
        public static void Cleanup()
        {
            _chromeDriver.Quit();
        }

        [TestInitialize()]
        public void BeforeTestExecution()
        {
            _testProcedure = new TestProcedure();
            _webDriver = new CoatHangerDriver(_chromeDriver, ref _testProcedure);
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
            Assert.AreEqual("Use the browser to navigate to `https://en.wikipedia.org/wiki/Unit_testing`", _testProcedure.Steps[0].Actions);
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
            Assert.AreEqual("Use the browser *back* button.", _testProcedure.Steps[2].Actions);
            Assert.AreEqual(pageTitle, _webDriver.Title);
        }

        [TestMethod]
        public void WhenNavigating_Back_WithActionOverrideMessage_ExpectCustomAction()
        {
            // arrange
            WebNavigation navigation = _webDriver.NavigateStep();
            navigation.GoToUrl("https://en.wikipedia.org/wiki/Unit_testing");
            var pageTitle = _webDriver.Title;

            // act
            navigation.GoToUrl("https://en.wikipedia.org/wiki/Requirements_analysis");
            navigation.Back(skipCoatHangerStep: false, stepActionOverride: "Go back now!!");

            // assert
            Assert.AreEqual("Go back now!!", _testProcedure.Steps[2].Actions);
            Assert.AreEqual(pageTitle, _webDriver.Title);
        }

        [TestMethod]
        public void WhenNavigating_Back_WithSkipEnabled_ExpectNoAction()
        {
            // arrange
            WebNavigation navigation = _webDriver.NavigateStep();
            navigation.GoToUrl("https://en.wikipedia.org/wiki/Unit_testing"); // step 1
            var pageTitle = _webDriver.Title;

            // act
            navigation.GoToUrl("https://en.wikipedia.org/wiki/Requirements_analysis"); // step 2
            navigation.Back(skipCoatHangerStep: true); // step 3            

            // assert
            Assert.AreEqual(2, _testProcedure.Steps.Count);
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
            Assert.AreEqual("Use the browser *forward* button.", _testProcedure.Steps[2].Actions);
            Assert.AreEqual(pageTitle, _webDriver.Title);

        }

        [TestMethod]
        public void WhenNavigating_Forward_WithSkipEnabled_ExpectNoAction()
        {
            // arrange
            WebNavigation navigation = _webDriver.NavigateStep();
            navigation.GoToUrl("https://en.wikipedia.org/wiki/Unit_testing"); // step 1
            var pageTitle = _webDriver.Title; 
            navigation.Back(); // step 2

            // act
            navigation.Forward(skipCoatHangerStep: true); // step 3

            // assert
            Assert.AreEqual(2, _testProcedure.Steps.Count);
            Assert.AreEqual(pageTitle, _webDriver.Title);

        }

        [TestMethod]
        public void WhenNavigating_Forward_WithActionOverrideMessage_ExpectCustomAction()
        {
            // arrange
            WebNavigation navigation = _webDriver.NavigateStep();
            navigation.GoToUrl("https://en.wikipedia.org/wiki/Unit_testing"); // step 1
            var pageTitle = _webDriver.Title;
            navigation.Back(); // step 2

            // act
            navigation.Forward(skipCoatHangerStep: false
                , stepActionOverride: "Quickly hit the forward button!"); // step 3

            // assert
            Assert.AreEqual("Quickly hit the forward button!", _testProcedure.Steps[2].Actions);
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
            Assert.AreEqual("Use the browser *refresh* button.", _testProcedure.Steps[1].Actions);
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
            Assert.AreEqual("Use the browser to navigate to `/wiki/Unit_testing`", _testProcedure.Steps[0].Actions);
        }

        [TestMethod]
        public void WhenNavigatingStep_GoToUrl_IncludeDomainEnabled_ExpectFullPath()
        {
            // arrange
            var url = "https://en.wikipedia.org/wiki/Unit_testing";

            // act
            _webDriver.NavigateStep().GoToUrl(url: url, testCaseIncludeDomain: true);

            // assert
            Assert.AreEqual("Use the browser to navigate to `https://en.wikipedia.org/wiki/Unit_testing`", _testProcedure.Steps[0].Actions);
        }

        #endregion

    }
}
