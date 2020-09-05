using CoatHanger.Core;
using CoatHanger.Core.Enums;
using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Net.Http;

namespace CoatHanger.WebDriver
{

    /// <summary>
    /// A wrapper class for the amazing selenium driver. This 
    /// </summary>
    public class CoatHangerDriver : IWebDriver
    {
        private IWebDriver _seleniumDriver { get; set; }
        private TestProcedure _testProcedure;
        private INavigation _navigation;

        public CoatHangerDriver(IWebDriver seleniumDriver, ref TestProcedure testProcedure)
        {
            _seleniumDriver = seleniumDriver;
            _testProcedure = testProcedure;

            var seleniumNavigation = seleniumDriver.Navigate();
            _navigation = new WebNavigation(ref seleniumNavigation, ref testProcedure);
        }

        public string Url { get => _seleniumDriver.Url; set => _seleniumDriver.Url = value; }

        public string Title => _seleniumDriver.Title;

        public string PageSource => _seleniumDriver.PageSource;

        public string CurrentWindowHandle => _seleniumDriver.CurrentWindowHandle;

        public ReadOnlyCollection<string> WindowHandles => _seleniumDriver.WindowHandles;


        /// <summary>
        /// For example, if your on a login page. You might use "username" to represent the element friendly name. 
        /// So that when you call .SendKeys("yourusername"), the test step will be "Enter the value "yourusername" in the username textbox."
        /// </summary>
        /// <inheritdoc cref="ISearchContext.FindElement"/>
        public WebElement FindElement(By by, string friendlyName)
        {
            var element = _seleniumDriver.FindElement(by);

            return new WebElement(ref element, ref _testProcedure, friendlyName);
        }

        /// <inheritdoc cref="ISearchContext.FindElement"/>
        public IWebElement FindElement(By by)
        {
            var element = _seleniumDriver.FindElement(by);
            return new WebElement(ref element, ref _testProcedure, "");
        }

        /// <inheritdoc cref="ISearchContext.FindElements"/>
        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return _seleniumDriver.FindElements(by);
        }

        /// <inheritdoc cref="IWebDriver.Manage"/>
        public IOptions Manage()
        {
            return _seleniumDriver.Manage();
        }

        /// <inheritdoc cref="IWebDriver.Navigate"/>
        public INavigation Navigate()
        {
            return _navigation;
        }

        /// <inheritdoc cref="IWebDriver.Quit"/>
        public void Quit()
        {
            _seleniumDriver.Quit();
        }

        /// <inheritdoc cref="IWebDriver.SwitchTo"/>
        public ITargetLocator SwitchTo()
        {
            return _seleniumDriver.SwitchTo();
        }

        /// <inheritdoc cref="IWebDriver.Close"/>
        public void Close()
        {
            _seleniumDriver.Close();
        }

        /// <inheritdoc cref="IWebDriver.Dispose"/>
        public void Dispose()
        {
            _seleniumDriver.Dispose();
        }
    }

    public class WebNavigation : INavigation
    {
        private INavigation _seleniumNavigation;
        private TestProcedure _testProcedure;

        public WebNavigation(ref INavigation navigation, ref TestProcedure testProcedure)
        {
            _seleniumNavigation = navigation;
            _testProcedure = testProcedure;
        }

        /// <inheritdoc cref="INavigation.Back"/>
        public void Back()
        {
            _testProcedure.AddManualStep("Use the browser *back* button.");
            _seleniumNavigation.Back();
        }

        /// <inheritdoc cref="INavigation.Forward"/>
        public void Forward()
        {
            _testProcedure.AddManualStep("Use the browser *forward* button.");
            _seleniumNavigation.Forward();
        }

        /// <inheritdoc cref="INavigation.Refresh"/>
        public void Refresh()
        {
            _testProcedure.AddManualStep("Use the browser *refresh* button.");
            _seleniumNavigation.Refresh();
        }

        /// <summary>
        ///  Load a new web page in the current browser window.
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="testCaseIncludeDomain">Determines if you want the coat hanger test case to output "https://www.domain/Action?parms" vs "/Action?Parms" </param>
        /// <inheritdoc cref="INavigation.GoToUrl(Uri)"/>
        public void GoToUrl(Uri url, bool testCaseIncludeDomain)
        {
            _testProcedure.AddManualStep
            (
                testCaseIncludeDomain
                ? $"Use the browser to navigate to `{url}`"
                : $"Use the browser to navigate to `{url.PathAndQuery}`"
            );
            _seleniumNavigation.GoToUrl(url);
        }

        #region  Alternative Go To Url methods  
        
        /// <inheritdoc cref="INavigation.GoToUrl(string)"/>
        public void GoToUrl(string url)
        {
            GoToUrl(url: new Uri(url), testCaseIncludeDomain: true);
        }

        /// <inheritdoc cref="INavigation.GoToUrl(string)"/>
        public void GoToUrl(string url, bool testCaseIncludeDomain)
        {
            GoToUrl(url: new Uri(url), testCaseIncludeDomain: testCaseIncludeDomain);
        }

        /// <inheritdoc cref="INavigation.GoToUrl(Uri)"/>
        public void GoToUrl(Uri url)
        {
            GoToUrl(url: url, testCaseIncludeDomain: true);
        }

        #endregion
    }


    public class WebElement : IWebElement
    {
        private IWebElement _seleniumElement;
        private TestProcedure _testProcedure;
        private string _testFriendlyName;

        public WebElement(ref IWebElement seleniumWebElement, ref TestProcedure testProcedure, string testFriendlyName)
        {
            _seleniumElement = seleniumWebElement;
            _testProcedure = testProcedure;
            _testFriendlyName = testFriendlyName;
        }

        /// <inheritdoc cref="IWebElement.TagName"/>
        public string TagName => _seleniumElement.TagName;

        /// <inheritdoc cref="IWebElement.Text"/>
        public string Text => _seleniumElement.Text;

        /// <inheritdoc cref="IWebElement.Enabled"/>
        public bool Enabled => _seleniumElement.Enabled;

        /// <inheritdoc cref="IWebElement.Selected"/>
        public bool Selected => _seleniumElement.Selected;

        /// <inheritdoc cref="IWebElement.Location"/>
        public Point Location => _seleniumElement.Location;

        /// <inheritdoc cref="IWebElement.Size"/>
        public Size Size => _seleniumElement.Size;

        /// <inheritdoc cref="IWebElement.Displayed"/>
        public bool Displayed => _seleniumElement.Displayed;

        /// <inheritdoc cref="IWebElement.Clear"/>
        public void Clear()
        {
            _testProcedure.AddManualStep($"Clear all the contents in the `{_testFriendlyName}` {GetTestCaseElementType()}.");
            _seleniumElement.Clear();
        }

        /// <inheritdoc cref="IWebElement.Click"/>
        public void Click()
        {
            _testProcedure.AddManualStep($"Click the `{_testFriendlyName}` {GetTestCaseElementType()}.");
            _seleniumElement.Click();
        }

        /// <inheritdoc cref="IWebElement.FindElement"/>
        public IWebElement FindElement(By by)
        {
            return _seleniumElement.FindElement(by);
        }

        /// <inheritdoc cref="IWebElement.FindElements(By)"/>
        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return _seleniumElement.FindElements(by);
        }

        /// <inheritdoc cref="IWebElement.GetAttribute(string)"/>
        public string GetAttribute(string attributeName)
        {
            return _seleniumElement.GetAttribute(attributeName);
        }


        /// <inheritdoc cref="IWebElement.GetCssValue(string)"/>
        public string GetCssValue(string propertyName)
        {
            return _seleniumElement.GetCssValue(propertyName);
        }

        /// <inheritdoc cref="IWebElement.GetProperty(string)"/>
        public string GetProperty(string propertyName)
        {
            return _seleniumElement.GetProperty(propertyName);
        }


        /// <inheritdoc cref="IWebElement.SendKeys"/>
        public void SendKeys(string text)
        {
            _testProcedure.AddManualStep($"Enter the value `{text}` in the {_testFriendlyName} {GetTestCaseElementType()}.");            
            _seleniumElement.SendKeys(text);
        }

        /// <inheritdoc cref="IWebElement.Submit"/>
        public void Submit()
        {
            _testProcedure.AddManualStep("Submit the form.");
            _seleniumElement.Submit();
        }

        /// <summary>
        /// Figure out what kind of the element this is so the test cases can be more descritpive.
        /// e.g "Click the `male` element vs Select the `male` radio option
        /// </summary>
        private string GetTestCaseElementType()
        {
            var elementType = "element"; // when in doubt... use generic term like element.
            var attribute = GetAttribute("type");

            if (TagName == "option")
            {
                elementType = "dropdown option";
            }
            else if (attribute == "radio")
            {
                elementType = "radio option";
            }
            else if (attribute == "checkbox")
            {
                elementType = "checkbox";
            }
            else if (TagName == "button" || attribute == "button" || attribute == "submit" || attribute == "reset")
            {
                elementType = "button";
            }
            else if (attribute == "password" 
                    || attribute == "text" 
                    || attribute == "number" 
                    || attribute == "email" 
                    || TagName == "textarea"
                    )
            {
                elementType = "textbox";
            }
                        
            return elementType;
        }
    }
}
