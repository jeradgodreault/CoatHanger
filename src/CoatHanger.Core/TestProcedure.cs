using CoatHanger.Core.Models;
using CoatHanger.Core.Step;
using CoatHanger.Core.Style;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq.Expressions;
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
        private TestCaseAttribute TestCaseAttribute { get; set; }
        internal MethodBase TestMethod { get; set; }

        public DateTime TestExecutionStartDateTime { get; private set; }

        /// <summary>
        /// Determine if all the steps will be consider as shared step. 
        /// </summary>
        public bool IsSharedStepMode { get; set; }

        private bool IsStarted { get; set; } = false;

        public TestProcedure()
        {

        }

        /// <summary>
        /// Start the testing procedure 
        /// </summary>
        public void Start(MethodBase currentMethod)
        {
            if (!IsStarted)
            {               
                IsStarted = true;

                var testCaseAttribute = (TestCaseAttribute)Attribute.GetCustomAttribute(currentMethod, typeof(TestCaseAttribute));

                if (testCaseAttribute == null)
                {
                    throw new ArgumentException("The current method does not support the " + nameof(TestCaseAttribute));
                }

                TestCaseAttribute = testCaseAttribute;
                TestMethod = currentMethod;
                TestExecutionStartDateTime = DateTime.Now;
            } 
            else
            {
                throw new InvalidOperationException("A test procedure can only be called once.");
            }
        }

        public GivenWhenThen StartGivenWhenThen(MethodBase currentMethod)
        {
            Start(currentMethod);
            return new GivenWhenThen(this);
        }

        public ArrangeActAssert StartArrangeActAssert(MethodBase currentMethod)
        {
            Start(currentMethod);
            return new ArrangeActAssert(this);
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

        public void AddPrerequisiteStep(string description, Action setupPrerequisiteAction)
        {
            AddPrerequisiteStep(description);

            try
            {
                setupPrerequisiteAction.Invoke();
            }
            catch (Exception ex)
            {
                new Exception($"Unable to setup the Prerequisite Step - `${description}`", ex);
            }
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
                IsSharedStep = IsSharedStepMode
            });
        }

        public void AddStep(string[] actions, ExpectedOutcome expectedOutcome)
        {
            AddStep(string.Join(Environment.NewLine, actions), expectedOutcome.ExpectedResult, expectedOutcome.RequirementID);
        }

        public void AddStep(string[] actions, string expectedResults)
        {
            AddManualStep(actions: new List<string>(actions)
            , expectedResult: string.Join(Environment.NewLine, expectedResults)
            , requirementID: $"{TestCaseAttribute.Identifier}-{CurrentExpectedResultStepNumber}");
        }

        public void AddStep(string action, params string[] expectedResults)
        {
            AddStep(action, string.Join(Environment.NewLine, expectedResults));
        }

        public void AddStep(string action, string expectedResult)
        {
            AddManualStep(action: action
                , expectedResult: expectedResult
                , requirementID: $"{TestCaseAttribute.Identifier}-{CurrentExpectedResultStepNumber}");
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
                IsSharedStep = IsSharedStepMode,
                ExpectedOutcome = new ExpectedOutcome
                {
                    RequirementID = requirementID,
                    ExpectedResult = expectedResult,
                }
            });

            CurrentStepNumber++;
            CurrentExpectedResultStepNumber++;
        }

        /// <summary>
        /// Verify **that** the variable is **asserted** **to** an expected value.
        /// </summary>
        /// <param name="that">The variable **name** that we are Verify</param>
        /// <param name="value">The variable **value** that we are Verify</param>
        /// <param name="assertionMethod">The Assertion method for verfication.</param>
        /// <param name="to">Expected value of verfication</param>
        public void ThenVerify<T>(string that, T value, Action<T, T> assertionMethod, T to)
        {
            AddStep
            (
                action: $"Examine the {that} variable.", 
                expectedResult: $"The system shall output the value {to}"
            );

            assertionMethod.Invoke(to, value);
        }

        /// <summary>
        /// Verify **that** the variable is **asserted** **to** an expected value. 
        /// </summary>
        /// <param name="that">The variable **name** that we are Verify</param>
        /// <param name="value">The variable **value** that we are Verify</param>
        /// <param name="assertionMethod">The Assertion method for verfication.</param>
        /// <param name="to">Expected value of verfication</param>
        /// <param name="expectedResultNotMetMessage">The override failure comment for not meeting the expected result </param>
        public void ThenVerify<T>(string that, T value, Action<T, T, string> assertionMethod, T to, string expectedResultNotMetMessage) 
        {
            AddStep
            (
                action: $"Examine the {that} variable.",
                expectedResult: $"The system shall output the value {to}"
            );

            assertionMethod.Invoke(to, value, expectedResultNotMetMessage);
        }

        /// <summary>
        /// Verify **that** the variable is **asserted** **to** an expected value. 
        /// </summary>
        /// <param name="value">The variable **value** that we are Verify</param>
        /// <param name="assertionMethod">The Assertion method for verfication.</param>
        /// <param name="to">Expected value of verfication</param>
        /// <param name="expectedResultNotMetMessage">The override failure comment for not meeting the expected result </param>
        public void ThenVerify<T>(string action, string expectedResult, T value, Action<T, T, string> assertionMethod, T to, string expectedResultNotMetMessage)
        {
            AddStep
            (
                action: action,
                expectedResult: expectedResult
            );

            assertionMethod.Invoke(to, value, expectedResultNotMetMessage);
        }
    }
}
