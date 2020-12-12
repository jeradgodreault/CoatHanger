using CoatHanger.WebDriver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoatHanger.Core.Testing.UnitTest.WebDriverTest
{
    [TestClass]
    [Ignore("Work-in-progress, need to setup pipeline for UI test")]
    public class WebElementDriverTest
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

        [ClassInitialize()] 
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

        [TestMethod]
        public void WhenElement_Click_IsButton_ExpectButtonMessage()
        {
            // arrange
            var samplePage = GetSamplePage();
            _webDriver.Navigate().GoToUrl("data:text/html;charset=utf-8," + samplePage);
            var element = _webDriver.FindElement(By.Id("test_button"), "Test");

            // act
            element.Click();

            // assert
            Assert.AreEqual("Click the `Test` button.", _testProcedure.Steps[1].Action);
        }

        [TestMethod]
        public void WhenElement_Click_IsSubmit_ExpectButtonMessage()
        {
            // arrange
            var samplePage = GetSamplePage();
            _webDriver.Navigate().GoToUrl("data:text/html;charset=utf-8," + samplePage);
            var element = _webDriver.FindElement(By.Id("test_submit"), "Test");

            // act
            element.Click();

            // assert
            Assert.AreEqual("Click the `Test` button.", _testProcedure.Steps[1].Action);
        }

        [TestMethod]
        public void WhenElement_SendKeys_IsTextBox_ExpectTextMessage()
        {
            // arrange
            var samplePage = GetSamplePage();
            _webDriver.Navigate().GoToUrl("data:text/html;charset=utf-8," + samplePage);
            var element = _webDriver.FindElement(By.Id("test_text"), "Test");

            // act
            element.SendKeys("Hello World");

            // assert
            Assert.AreEqual("Enter the value `Hello World` in the Test textbox.", _testProcedure.Steps[1].Action);
        }

        [TestMethod]
        public void WhenElement_SendKeys_IsTextArea_ExpectTextMessage()
        {
            // arrange
            var samplePage = GetSamplePage();
            _webDriver.Navigate().GoToUrl("data:text/html;charset=utf-8," + samplePage);
            var element = _webDriver.FindElement(By.Id("test_textarea"), "Test");

            // act
            element.SendKeys("Send Keys Data");

            // assert
            Assert.AreEqual("Enter the value `Send Keys Data` in the Test textbox.", _testProcedure.Steps[1].Action);
        }


        [TestMethod]
        public void WhenElement_Clear_IsTextBox_ExpectTextMessage()
        {
            // arrange
            var samplePage = GetSamplePage();
            _webDriver.Navigate().GoToUrl("data:text/html;charset=utf-8," + samplePage);
            var element = _webDriver.FindElement(By.Id("test_text"), "Test");

            // act
            element.Clear();

            // assert
            Assert.AreEqual("Clear all the contents in the `Test` textbox.", _testProcedure.Steps[1].Action);
        }

        [TestMethod]
        public void WhenElement_Clear_IsTextArea_ExpectTextMessage()
        {
            // arrange
            var samplePage = GetSamplePage();
            _webDriver.Navigate().GoToUrl("data:text/html;charset=utf-8," + samplePage);
            var element = _webDriver.FindElement(By.Id("test_textarea"), "Test");

            // act
            element.Clear();

            // assert
            Assert.AreEqual("Clear all the contents in the `Test` textbox.", _testProcedure.Steps[1].Action);
        }


        private string GetSamplePage()
        {
            return File.ReadAllText("./WebDriverTest/WebElementTestPage.html");
        }

    }
}
