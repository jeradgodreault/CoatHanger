using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CoatHanger.TestLogger
{
    /// <summary>
    /// Logger for sending output to the console.
    /// </summary>
    [ExtensionUri("logger://Microsoft/TestPlatform/CoatHanger/v1")] /// Uri used to uniquely identify the console logger. 
    [FriendlyName("CoatHanger")] /// Alternate user friendly string to uniquely identify the logger.
    public class CoatHangerLogger : ITestLoggerWithParameters
    {
        public const string LogFilePathKey = "LogFilePath";
        public const string EnvironmentKey = "Environment";
        public const string XUnitVersionKey = "XUnitVersion";
        private string environmentOpt;
        private string xunitVersionOpt;
        private string outputFilePath;

        /// <summary>
        /// Initializes the Test Logger.
        /// </summary>
        /// <param name="events">Events that can be registered for.</param>
        /// <param name="testRunDirectory">Test Run Directory</param>
        public void Initialize(TestLoggerEvents events, string testResultsDirPath)
        {
            if (events == null)
            {
                throw new ArgumentNullException(nameof(events));
            }

            if (testResultsDirPath == null)
            {
                throw new ArgumentNullException(nameof(testResultsDirPath));
            }

            var outputPath = Path.Combine(testResultsDirPath, "TestResults.xml");
            this.InitializeImpl(events, outputPath);
        }

        public void Initialize(TestLoggerEvents events, Dictionary<string, string> parameters)
        {
            if (events == null)
            {
                throw new ArgumentNullException(nameof(events));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (parameters.TryGetValue(LogFilePathKey, out string outputPath))
            {
                this.InitializeImpl(events, outputPath);
            }
            else if (parameters.TryGetValue(DefaultLoggerParameterNames.TestRunDirectory, out string outputDir))
            {
                this.Initialize(events, outputDir);
            }
            else
            {
                throw new ArgumentException($"Expected {LogFilePathKey} or {DefaultLoggerParameterNames.TestRunDirectory} parameter", nameof(parameters));
            }

            parameters.TryGetValue(EnvironmentKey, out this.environmentOpt);
            parameters.TryGetValue(XUnitVersionKey, out this.xunitVersionOpt);
        }

        private void InitializeImpl(TestLoggerEvents events, string outputPath)
        {
            events.TestRunMessage += this.TestMessageHandler;
            events.TestResult += this.TestResultHandler;
            events.TestRunComplete += this.TestRunCompleteHandler;

            this.outputFilePath = Path.GetFullPath(outputPath);
        }


        /// <summary>
        /// Called when a test message is received.
        /// </summary>
        private void TestMessageHandler(object sender, TestRunMessageEventArgs e)
        {
            Console.WriteLine("JERAD IS HERE");
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
            Console.WriteLine("JERAD IS HERE");
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
            Console.WriteLine("JERAD IS HERE");
            Console.WriteLine("Total Executed: {0}", e.TestRunStatistics.ExecutedTests);
            Console.WriteLine("Total Passed: {0}", e.TestRunStatistics[TestOutcome.Passed]);
            Console.WriteLine("Total Failed: {0}", e.TestRunStatistics[TestOutcome.Failed]);
            Console.WriteLine("Total Skipped: {0}", e.TestRunStatistics[TestOutcome.Skipped]);
        }
    }
}
