using CoatHanger.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Testing.Web.UnitTest.UITests
{

    [TestClass]
    public class PrivacyPolicyPageTest
    {
        private TestContext testContextInstance;
        private IWebDriver _webDriver;

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
            _webDriver = new ChromeDriver();
        }

        [TestMethod]
        [Ignore("Work in progress")]
        public void WhereMenuNavigationIncludeLinkToPage()
        {
            _webDriver.Navigate().GoToUrl("http://localhost:51306");

            var menuElement = _webDriver.FindElement(By.LinkText("Privacy"));

            menuElement.Click();

            var titleElement = _webDriver.FindElement(By.TagName("h1"));
            Assert.AreEqual(titleElement.Text, "Privacy Policy");
        }
    }
}
