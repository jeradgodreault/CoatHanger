using CoatHanger.Core.Models;
using CoatHanger.Core.Style;
using System;
using System.Collections.Generic;
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

        public DateTime TestExecutionStartDateTime { get; private set; }

        public bool IsStarted { get; private set; } = false;

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
            AddSharedStep(actions, new List<Evidence>());
        }

        public void AddSharedStep(List<string> actions, Evidence evidence)
        {
            AddSharedStep(actions, new List<Evidence>() { evidence });
        }

        public void AddSharedStep(List<string> actions, List<Evidence> evidences)
        {
            Steps.Add(new TestStep()
            {
                IsSharedStep = true,
                Actions = actions,
                Evidences = evidences,
                StepNumber = CurrentStepNumber
            });

            CurrentStepNumber++;
        }

        /// <summary>
        /// Creates a "Take a screenshot" step that assoicate with a Jpeg screenshot.
        /// </summary>
        public void AddScreenshotStep(string fileName)
        {
            Steps.Add(new TestStep()
            {
                IsSharedStep = false,
                Actions = new List<string> { "*** Take a screenshot" },
                Evidences = new List<Evidence>()
                {
                    new Evidence()
                    {
                        EvidenceType = EvidenceType.JPEG_IMAGE,
                        FileName = fileName,
                        TimeStamp = DateTime.Now
                    }
                },
                StepNumber = CurrentStepNumber
            });

            CurrentStepNumber++;
        }

        public void AddSharedStep(Func<SharedStep> by)
        {
            var sharedStep = by.Invoke();
            AddSharedStep(sharedStep.Actions, sharedStep.Evidences);
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

        public void AddStep(string action)
        {
            Steps.Add(new TestStep()
            {
                StepNumber = CurrentStepNumber++,
                Actions = new List<string> { action },
                Evidences = new List<Evidence>(),
                IsSharedStep = false
            });
        }

        public void AddStep(string action, Evidence evidence)
        {
            Steps.Add(new TestStep()
            {
                StepNumber = CurrentStepNumber++,
                Actions = new List<string> { action },
                Evidences = new List<Evidence>() { evidence },
                IsSharedStep = false
            });
        }

        public void AddStep(string action, Action by)
        {
            by.Invoke();
            AddStep(action);
        }

        public void AddStep<T>(string action, Func<T> by, out T result)
        {
            AddStep(action);
            result = by.Invoke();
        }

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
            Steps.Add(new TestStep()
            {
                StepNumber = CurrentStepNumber,
                Actions = actions,
                IsSharedStep = false,
                ExpectedOutcome = new ExpectedOutcome
                {
                    RequirementID = requirementID,
                    ExpectedResult = expectedResult,
                }
            });

            CurrentStepNumber++;
            CurrentExpectedResultStepNumber++;
        }

        public void AddAttachment(string fileName)
        {
            AddTestAttachment(fileName);
            AddRequirementAttachment(fileName);
        }

        public void AddTestAttachment(string fileName)
        {

        }

        public void AddRequirementAttachment(string fileName)
        {
            
        }
    }
}
