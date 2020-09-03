using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core
{
    /// <summary>
    /// Logger for sending output to the console.
    /// </summary>
    [ExtensionUri("logger://CoatHangerLogger/v1")] /// Uri used to uniquely identify the console logger. 
    [FriendlyName("CoatHangerLogger")] /// Alternate user friendly string to uniquely identify the logger.
    internal class CoatHangerLogger : ITestLogger
    {
        /// <summary>
        /// Initializes the Test Logger.
        /// </summary>
        /// <param name="events">Events that can be registered for.</param>
        /// <param name="testRunDirectory">Test Run Directory</param>
        public void Initialize(TestLoggerEvents events, string testRunDirectory)
        {
            // Register for the events.
            events.TestRunMessage += TestMessageHandler;
            events.TestResult += TestResultHandler;
            events.TestRunComplete += TestRunCompleteHandler;
        }

        /// <summary>
        /// Called when a test message is received.
        /// </summary>
        private void TestMessageHandler(object sender, TestRunMessageEventArgs e)
        {
            switch (e.Level)
            {
                case TestMessageLevel.Informational:
                    Console.WriteLine("Information: " + e.Message);
                    break;

                case TestMessageLevel.Warning:
                    Console.WriteLine("Warning: " + e.Message);
                    break;

                case TestMessageLevel.Error:
                    Console.WriteLine("Error: " + e.Message);
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// Called when a test result is received.
        /// </summary>
        private void TestResultHandler(object sender, TestResultEventArgs e)
        {
            string name = !string.IsNullOrEmpty(e.Result.DisplayName) ? e.Result.DisplayName : e.Result.TestCase.FullyQualifiedName;

            if (e.Result.Outcome == TestOutcome.Skipped)
            {
                Console.WriteLine(name + " Skipped");
            }
            else if (e.Result.Outcome == TestOutcome.Failed)
            {
                Console.WriteLine(name + " Failed");
                if (!String.IsNullOrEmpty(e.Result.ErrorStackTrace))
                {
                    Console.WriteLine(e.Result.ErrorStackTrace);
                }
            }
            else if (e.Result.Outcome == TestOutcome.Passed)
            {
                Console.WriteLine(name + " Passed");
            }
        }

        /// <summary>
        /// Called when a test run is completed.
        /// </summary>
        private void TestRunCompleteHandler(object sender, TestRunCompleteEventArgs e)
        {
            Console.WriteLine("Total Executed: {0}", e.TestRunStatistics.ExecutedTests);
            Console.WriteLine("Total Passed: {0}", e.TestRunStatistics[TestOutcome.Passed]);
            Console.WriteLine("Total Failed: {0}", e.TestRunStatistics[TestOutcome.Failed]);
            Console.WriteLine("Total Skipped: {0}", e.TestRunStatistics[TestOutcome.Skipped]);
        }
    }
}
