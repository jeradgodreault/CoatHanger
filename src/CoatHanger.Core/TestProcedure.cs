﻿using CoatHanger.Core.Models;
using CoatHanger.Core.Style;
using System;
using System.Collections.Generic;
using System.IO;
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

        private bool IsStarted { get; set; } = false;

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
                IsSharedStep = false
            });
        }

        public void AddStep(string action, Action by)
        {
            by.Invoke();
            AddStep(action);
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
    }
}
