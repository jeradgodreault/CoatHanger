using CoatHanger.Core.Models;
using CoatHanger.Core.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace CoatHanger.Core
{

    public class TestProcedure
    {
        public List<TestStep> Steps { get; private set; } = new List<TestStep>();
        public List<PrerequisiteStep> PrerequisiteSteps { get; private set; } = new List<PrerequisiteStep>();
        private int PrerequisiteStep { get; set; } = 1;
        private int CurrentStepNumber { get; set; } = 1;
        private int CurrentExpectedResultStepNumber { get; set; } = 1;
        public TestCaseAttribute TestCase { get; private set; }
        internal MethodBase TestMethod { get; set; }
        public List<Attachment> TestAttachments { get; private set; } = new List<Attachment>();
        public List<Attachment> RequirementAttachments { get; private set; } = new List<Attachment>();
        public List<BusinessRule> BusinessRules { get; private set; } = new List<BusinessRule>();
        public HashSet<string> References { get; private set; } = new HashSet<string>();

        public DateTime TestExecutionStartDateTime { get; private set; }

        public bool IsStarted { get; private set; } = false;

        public Iteration Iteration { get; private set; } = new Iteration();

        /// <summary>
        /// This boolean is used to ensure you don't add steps within an existing step.
        /// </summary>
        protected bool IsBuilderMode { get; set; } = false;

        public TestProcedure()
        {

        }

        /// <summary>
        /// Start the testing procedure 
        /// </summary>
        public void Start(MethodBase currentMethod
                    , [CallerFilePath] string sourceFilePath = ""
                    , [CallerLineNumber] int sourceLineNumber = 0)
        {
            if (!IsStarted)
            {
                IsStarted = true;

                var testCaseAttribute = (TestCaseAttribute)Attribute.GetCustomAttribute(currentMethod, typeof(TestCaseAttribute));

                if (testCaseAttribute == null)
                {
                    throw new ArgumentException("The current method does not support the " + nameof(TestCase));
                }

                TestCase = testCaseAttribute;
                TestMethod = currentMethod;
                TestExecutionStartDateTime = DateTime.Now;
                Iteration.TestCaseID = TestCase.Identifier;
            }
            else
            {
                throw new InvalidOperationException("A test procedure can only be called once.");
            }
        }

        /// <summary>
        /// The end of the testing procedure
        /// </summary>
        public void Finish([CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
        {

        }

        public void AddSharedStep(List<string> actions)
        {
            AddSharedStep(actions: actions, evidence: null);
        }

        public void AddSharedStep(List<string> actions, Evidence evidence)
        {
            AddSharedStep(actions, new List<Evidence>() { evidence });
        }

        public void AddSharedStep(List<string> actions, List<Evidence> evidences)
        {
            if (IsBuilderMode) throw new InvalidOperationException("You are currently building a step with a builder method. (e.g ToVerify) Complete the builder step first before adding new steps.");

            Steps.Add(new TestStep()
            {
                IsSharedStep = true,
                Actions = actions,
                Evidences = evidences,
                StepNumber = CurrentStepNumber,
                IsSuccessful = true
            });

            CurrentStepNumber++;
        }

        /// <summary>
        /// Creates a "Take a screenshot" step that assoicate with a Jpeg screenshot.
        /// </summary>
        public void AddScreenshotStep(string fileName)
        {
            if (IsBuilderMode) throw new InvalidOperationException("You are currently building a step with a builder method. (e.g ToVerify). Use the builder API method for taking a screenshot.");

            var screenshot = new Evidence()
            {
                EvidenceType = EvidenceType.JPEG_IMAGE,
                FileName = fileName,
                TimeStamp = DateTime.Now
            };

            if (Steps.Count == 0)
            {

                Steps.Add(new TestStep()
                {
                    IsSharedStep = false,
                    Actions = new List<string> { "*** Take a screenshot" },
                    Evidences = new List<Evidence>()
                    {
                       screenshot
                    },
                    StepNumber = CurrentStepNumber,
                    IsSuccessful = true,
                });

                CurrentStepNumber++;
            }
            else
            {
                // Append the previous step with "Take a screenshot"
                var step = Steps[Steps.Count - 1];
                step.Actions.Add("*** Take a screenshot");

                if (step.Evidences != null)
                {
                    step.Evidences.Add(screenshot);
                } else
                {
                    step.Evidences = new List<Evidence>() { screenshot };
                }
            }
        }

        public void AddSharedStep(Func<SharedStep> by)
        {
            SharedStep sharedStep;
            try
            {
                IsBuilderMode = true;
                sharedStep = by.Invoke();
                IsBuilderMode = false;
                AddSharedStep(sharedStep.Actions, sharedStep.Evidences);
            }
            catch
            {
                IsBuilderMode = false;
                throw;
            }
        }

        public void AddPrerequisiteStep(string description)
        {
            if (string.IsNullOrEmpty(description)) throw new ArgumentNullException("Prerequisite step description cannot be null or empty. Please provide a value.");

            PrerequisiteSteps.Add(new PrerequisiteStep()
            {
                StepNumber = PrerequisiteStep++,
                Description = description
            });
        }

        public void AddPrerequisiteStep(string description, Action setup)
        {
            AddPrerequisiteStep(description);
            setup.Invoke();
        }

        public void AddStep(params string[] actions)
        {
            AddStep(string.Join(Environment.NewLine, actions));
        }

        /// <summary>
        /// Adds a step to the test case. 
        /// </summary>
        public void AddStep(string action)
        {
            if (IsBuilderMode) throw new InvalidOperationException("You are currently building a step with a builder method. (e.g ToVerify) Complete the builder step first before adding new steps.");

            Steps.Add(new TestStep()
            {
                StepNumber = CurrentStepNumber++,
                Actions = new List<string> { action },
                Evidences = null,
                IsSharedStep = false,
                IsSuccessful = true,
            });
        }

        /// <summary>
        /// Adds a step to the test case with assoicate evidence of the execution.  
        /// </summary>
        public void AddStep(string action, Evidence evidence)
        {
            if (IsBuilderMode) throw new InvalidOperationException("You are currently building a step with a builder method. (e.g ToVerify) Complete the builder step first before adding new steps.");

            Steps.Add(new TestStep()
            {
                StepNumber = CurrentStepNumber++,
                Actions = new List<string> { action },
                Evidences = new List<Evidence>() { evidence },
                IsSharedStep = false,
                IsSuccessful = true
            });
        }

        /// <summary>
        /// Adds a step to the test case with delegate to do the step. 
        /// </summary>
        public void AddStep(string action, Action by)
        {
            AddStep(action);
            by.Invoke();            
        }

        /// <summary>
        /// Adds a step to the test case with delegate to do the step and return some outcome of step.
        /// This can then be assigned to a out variable. 
        /// <code>
        /// TestProcedure.AddStep
        /// (
        ///     action: "Get the highest price item",
        ///     by : {
        ///        return database.Products.OrderBy(p=> x.Price).Last();
        ///     }
        ///     var out product
        /// )
        /// 
        /// Assert.AreEqual(product.Name, "Most expensive item");
        /// </code>
        /// </summary>
        public void AddStep<T>(string action, Func<T> by, out T result)
        {
            AddStep(action);
            result = by.Invoke();
        }

        /// <summary>
        /// Adds a step to the test case and creates a new variable in the same line.
        /// <code>testProcedure.AddStep(action: "Choose a value from 1 to 10, input: new Random(1,10), out var value);</code>
        /// </summary>
        public void AddStep<T>(string action, T input, out T result)
        {
            AddStep(action);
            result = input;
        }

        public void AddStep(string[] actions, ExpectedOutcome expectedOutcome)
        {
            AddStep(string.Join(Environment.NewLine, actions), expectedOutcome.ExpectedResult, expectedOutcome.RequirementID);
        }

        public void AddStep(string[] actions, string expectedResults)
        {
            AddManualStep(actions: new List<string>(actions)
            , expectedResult: string.Join(Environment.NewLine, expectedResults)
            , requirementID: $"{TestCase.Identifier}-{CurrentExpectedResultStepNumber}");
        }

        /// <summary>
        /// Adds a step to the test case. 
        /// </summary>
        /// <param name="actions">The actions the users performs during the test execution</param>
        /// <param name="expectedResults">The final outcome of the all the steps and what should be observed</param>
        /// <param name="IsSuccessful">Was the the step succefully in its execution</param>
        /// <param name="comment">the comment about the step execution. Should be the assert failure message.</param>
        public void AddStep(string[] actions, string expectedResults, bool IsSuccessful, string comment)
        {
            AddStep(actions, expectedResults, IsSuccessful, comment, new List<BusinessRule>());
        }

        /// <summary>
        /// Adds a step to the test case. 
        /// </summary>
        /// <param name="actions">The actions the users performs during the test execution</param>
        /// <param name="expectedResults">The final outcome of the all the steps and what should be observed</param>
        /// <param name="IsSuccessful">Was the the step succefully in its execution</param>
        /// <param name="comment">the comment about the step execution. Should be the assert failure message.</param>
        /// <param name="businessRules"></param>
        public void AddStep(string[] actions, string expectedResults, bool IsSuccessful, string comment, List<BusinessRule> businessRules)
        {
            AddManualStep(actions: new List<string>(actions)
            , expectedResult: string.Join(Environment.NewLine, expectedResults)
            , requirementID: $"{TestCase.Identifier}-{CurrentExpectedResultStepNumber}"
            , IsSuccessful: IsSuccessful
            , comment: comment
            , businessRules: businessRules
            );
        }

        public void AddStep(string action, params string[] expectedResults)
        {
            AddStep(action, string.Join(Environment.NewLine, expectedResults));
        }

        public void AddStep(string action, string expectedResult)
        {
            AddManualStep(action: action
                , expectedResult: expectedResult
                , requirementID: $"{TestCase.Identifier}-{CurrentExpectedResultStepNumber}");
        }

        public void AddManualStep(string action, string expectedResult, string requirementID)
        {
            AddManualStep(new List<string> { action }, expectedResult, requirementID);
        }

        public void AddManualStep(List<string> actions, string expectedResult, string requirementID)
        {
            AddManualStep(actions, expectedResult, requirementID, true, null);
        }

        public void AddManualStep(List<string> actions, string expectedResult, string requirementID, bool IsSuccessful, string comment)
        {
            AddManualStep(actions, expectedResult, requirementID, IsSuccessful, comment, new List<BusinessRule>());
        }

        public void AddManualStep(List<string> actions
            , string expectedResult
            , string requirementID
            , bool IsSuccessful
            , string comment
            , List<BusinessRule> businessRules
        )
        {
            if (IsBuilderMode) throw new InvalidOperationException("You are currently building a step with a builder method. (e.g ToVerify) Complete the builder step first before adding new steps.");

            Steps.Add(new TestStep()
            {
                StepNumber = CurrentStepNumber,
                Actions = actions,
                IsSharedStep = false,
                ExpectedOutcome = new ExpectedOutcome
                {
                    RequirementID = requirementID,
                    ExpectedResult = expectedResult,
                    
                },
                BusinessRules = businessRules.Select(br=> br.ID).ToList(),
                IsSuccessful = IsSuccessful,
                Comment = comment
            });

            CurrentStepNumber++;
            CurrentExpectedResultStepNumber++;
        }

        /// <summary>
        /// Adds a reference link to the test case summary. These are good for files or website links
        /// that are appear multiple times in many test cases. This avoid bloating up the document with duplicate attachments. 
        /// </summary>
        public void AddTestReference(string reference)
        {
            if (string.IsNullOrEmpty(reference)) throw new ArgumentNullException("Test case references must have a value");
            References.Add(reference);
        }

        /// <summary>
        /// Adds an attachment to both the test case and requirement. 
        /// </summary>
        public void AddAttachment(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("Attachment must have a filename path");

            AddTestAttachment(fileName);
            AddRequirementAttachment(fileName);
        }

        /// <summary>
        /// Adds an attachment to the test case. 
        /// </summary>
        public void AddTestAttachment(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("Test Attachment must have a filename path");

            TestAttachments.Add(new Attachment() { FileName = fileName });
        }

        /// <summary>
        /// Adds an attachment to the requirement.  
        /// </summary>
        public void AddRequirementAttachment(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) throw new ArgumentNullException("Requirement Attachment must have a filename path");

            RequirementAttachments.Add(new Attachment() { FileName = fileName });
        }

        /// <summary>
        /// Register a Parameter for the current Iteration. The idea with Parameter is that you can
        /// substitute them instead of making new test cases. 
        /// </summary>
        public void RegisterParameter(string key, string value)
        {
            Iteration.TestParameters.Add(key, value);
            Iteration.RequirementParameters.Add(key, value);
        }

        /// <summary>
        /// Register a Parameter for the current Iteration. The idea with Parameter is that you can
        /// substitute them instead of making new test cases. 
        /// </summary>
        public void RegisterParameter(string key, int value)
        {
            RegisterParameter(key, value.ToString());
        }

        /// <summary>
        /// Register a Parameter for the current Iteration. The idea with Parameter is that you can
        /// substitute them instead of making new test cases. 
        /// </summary>
        public void RegisterParameter(string key, bool value)
        {
            RegisterParameter(key, value.ToString());
        }

        /// <summary>
        /// Register a Parameter for the current Iteration. The idea with Parameter is that you can
        /// substitute them instead of making new test cases. 
        /// 
        /// </summary>
        public void RegisterParameter(string key, string value, string label)
        {
            Iteration.TestParameters.Add(key, value);
            Iteration.RequirementParameters.Add(key, value);
            Iteration.LabelParameters.Add(key, label);
        }

        public void RegisterParameter(string key, int value, string label)
        {
            RegisterParameter(key, value.ToString(), label);
        }

        public void RegisterParameter(string key, bool value, string label)
        {
            RegisterParameter(key, value.ToString(), label);
        }

        /// <summary>
        /// Register a Parameter for the current Iteration. The idea with Parameter is that you can
        /// substitute them instead of making new test cases. 
        /// </summary>
        public void RegisterTestParameter(string key, string value)
        {
            Iteration.TestParameters.Add(key, value);
        }

        /// <summary>
        /// Register a Parameter for the current Iteration. The idea with Parameter is that you can
        /// substitute them instead of making new test cases. 
        /// </summary>
        public void RegisterTestParameter(string key, int value)
        {
            RegisterTestParameter(key, value.ToString());
        }

        /// <summary>
        /// Register a Parameter for the current Iteration. The idea with Parameter is that you can
        /// substitute them instead of making new test cases. 
        /// </summary>
        public void RegisterTestParameter(string key, bool value)
        {
            RegisterTestParameter(key, value.ToString());
        }

        /// <summary>
        /// Register a Parameter for the current Iteration. The idea with Parameter is that you can
        /// substitute them instead of making new test cases. 
        /// 
        /// Add a custom label to the key for prettier documentation.
        /// </summary>
        public void RegisterTestParameter(string key, string value, string label)
        {
            Iteration.TestParameters.Add(key, value);
            Iteration.LabelParameters.Add(key, label);
        }

        /// <summary>
        /// Register a Parameter for the current Iteration. The idea with Parameter is that you can
        /// substitute them instead of making new test cases. 
        /// 
        /// Add a custom label to the key for prettier documentation.
        /// </summary>
        public void RegisterTestParameter(string key, int value, string label)
        {
            RegisterTestParameter(key, value.ToString(), label);
        }

        /// <summary>
        /// Register a Parameter for the current Iteration. The idea with Parameter is that you can
        /// substitute them instead of making new test cases.
        /// 
        /// Add a custom label to the key for prettier documentation.
        /// </summary>
        public void RegisterTestParameter(string key, bool value, string label)
        {
            RegisterTestParameter(key, value.ToString(), label);
        }


    }
}
