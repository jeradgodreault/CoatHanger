using CoatHanger.Core.Models;
using System;
using System.Collections.Generic;

namespace CoatHanger.Core.Step
{
    public class VerificationStep
    {
        private List<string> Actions = new List<string>();
        private List<Evidence> Evidences = new List<Evidence>();
        private List<BusinessRule> BusinessRules = new List<BusinessRule>();

        public VerificationStep Statement(string statement)
        {
            Actions.Add(statement);
            return this;
        }

        public VerificationStep Instruction(string instruction)
        {
            Actions.Add(instruction);
            return this;
        }

        public VerificationStep Instruction(List<string> instructions)
        {
            Actions.AddRange(instructions);
            return this;
        }

        public VerificationStep Confirm(string that, bool actual, Action<bool> assertionMethod)
        {            
            assertionMethod.Invoke(actual);
            Actions.Add("Confirm that:" + "\r" + that);
            return this;
        }

        public VerificationStep Confirm(BusinessRule businessRule, bool actual, Action<bool> assertionMethod)
        {
            BusinessRules.Add(businessRule);
            return Confirm($"BR.{businessRule.ID} - {businessRule.Title}", actual, assertionMethod);
        }

        public VerificationStep And(string that, bool actual, Action<bool> assertionMethod)
        {
            assertionMethod.Invoke(actual);
            Actions[Actions.Count - 1] = Actions[Actions.Count - 1] + "\r" + that;
            return this;
        }

        public VerificationStep Confirm<T>(BusinessRule businessRule, T actual, Action<T, T> assertionMethod, T expected)
        {
            BusinessRules.Add(businessRule);
            return Confirm($"BR.{businessRule.ID} - {businessRule.Title}", actual, assertionMethod, expected);
        }

        public VerificationStep Confirm<T>(string that, T actual, Action<T, T> assertionMethod, T expected)
        {
            assertionMethod.Invoke(expected, actual);
            Actions.Add("Confirm that:" + "\r" + that);

            return this;
        }

        public VerificationStep And<T>(string that, T actual, Action<T, T> assertionMethod, T expected)
        {
            assertionMethod.Invoke(expected, actual);

            Actions[Actions.Count - 1] = Actions[Actions.Count - 1] + "\r" + that;

            return this;
        }

        public VerificationStep And<T>(BusinessRule businessRule, T actual, Action<T, T> assertionMethod, T expected)
        {
            BusinessRules.Add(businessRule);
            return And($"BR.{businessRule.ID} - {businessRule.Title}", actual, assertionMethod, expected);
        }

        public void AddEvidence(Evidence evidence)
        {
            Evidences.Add(evidence);
        }

        /// <summary>
        /// Assoicates JPEG screenshot as evidence to the current step. 
        /// </summary>
        public void AddScreenshot(string fileName)
        {
            Evidences.Add
            (
                new Evidence()
                {
                    FileName = fileName,
                    TimeStamp = DateTime.Now,
                    EvidenceType = EvidenceType.JPEG_IMAGE
                }
            );
        }

        /// <summary>
        /// Assoicates JPEG screenshot as evidence to the current step at the specified timestamp. 
        /// </summary>
        public void AddScreenshot(string fileName, DateTime timestamp)
        {
            Evidences.Add
            (
                new Evidence()
                {
                    FileName = fileName,
                    TimeStamp = timestamp,
                    EvidenceType = EvidenceType.JPEG_IMAGE
                }
            );
        }

        /// <summary>
        /// Assoicates JPEG screenshot as evidence to the current step at the specified timestamp. 
        /// </summary>
        public void AddEvidence(string fileName, DateTime timestamp, EvidenceType evidenceType)
        {
            Evidences.Add
            (
                new Evidence()
                {
                    FileName = fileName,
                    TimeStamp = timestamp,
                    EvidenceType = evidenceType
                }
            );
        }

        public List<string> GetActionStep()
        {
            return Actions;
        }


        /// <summary>
        /// Observations are not output into the documentation. 
        /// This allow you to have additional Asserts that normally you wouldn't want to include
        /// in the test case. Example: you would want to document the present of a success message , but 
        /// not document that error, warning or other error messages are present. These are "implicit" if you
        /// were run the test case manually. However automated testing might need more clues. 
        public VerificationStep Observe(string that, Action observation)
        {
            try
            {
                observation.Invoke();
            }
            catch (Exception ex)
            {
                new ObservationException(that, ex);
            }

            return this;
        }
    }

    public class ObservationException : Exception
    {
        public ObservationException(string message) : base(message)
        {

        }

        public ObservationException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }

}
