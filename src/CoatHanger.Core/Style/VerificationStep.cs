using CoatHanger.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoatHanger.Core.Step
{
    public class VerificationStep
    {
        private List<string> Actions = new List<string>();
        private List<Evidence> Evidences = new List<Evidence>();
        private List<BusinessRule> BusinessRules = new List<BusinessRule>();
        public bool IsVerified { get; private set; } = true;
        public string FailedVerificationErrorMessage { get; private set; }
        public List<string> SqlQueryReferences { get; private set; } = new List<string>();
        private VerficiationStepAction LastAction { get; set; } = VerficiationStepAction.Unknown;

        public int CurrentStepNumber { get; set; } = 1;


        public VerificationStep Statement(string statement)
        {
            Actions.Add(statement);
            LastAction = VerficiationStepAction.Statement;
            return this;
        }

        public VerificationStep Statement(string statement, Action by)
        {
            Actions.Add(statement);
            by.Invoke();
            LastAction = VerficiationStepAction.Statement;
            return this;
        }

        public VerificationStep Statement(BusinessRule businessRule)
        {
            BusinessRules.Add(businessRule);
            return Statement($"{businessRule.ID} - {businessRule.Title}");
        }

        public VerificationStep Statement(BusinessRule businessRule, Action by)
        {
            BusinessRules.Add(businessRule);
            return Statement($"{businessRule.ID} - {businessRule.Title}", by);
        }

        public VerificationStep Instruction(string instruction)
        {
            Actions.Add(instruction);
            LastAction = VerficiationStepAction.Statement;
            return this;
        }

        public VerificationStep Instruction(string instruction, Action by)
        {
            Actions.Add(instruction);
            by.Invoke();
            LastAction = VerficiationStepAction.Statement;
            return this;
        }

        public VerificationStep Instruction(BusinessRule businessRule)
        {
            BusinessRules.Add(businessRule);
            return Instruction($"{businessRule.ID} - {businessRule.Title}");
        }

        public VerificationStep Instruction(List<string> instructions)
        {
            Actions.AddRange(instructions);
            LastAction = VerficiationStepAction.Statement;
            return this;
        }

        public VerificationStep Instruction(List<string> instructions, Action by)
        {
            Actions.AddRange(instructions);
            by.Invoke();
            LastAction = VerficiationStepAction.Statement;
            return this;
        }
        public VerificationStep Confirm(string that, bool actual, Action<bool> assertionMethod)
        {
            return Confirm(that, actual, assertionMethod, false);
        }

        public VerificationStep Confirm(string that, bool actual, Action<bool> assertionMethod, bool isObserveOnly)
        {            
            if (!isObserveOnly)
            {
                if (LastAction == VerficiationStepAction.Statement || LastAction == VerficiationStepAction.Unknown)
                {
                    Actions.Add("Confirm that:" + " • " + that);
                    LastAction = VerficiationStepAction.Confirm;
                }
                else if (LastAction == VerficiationStepAction.Confirm || LastAction == VerficiationStepAction.And)
                {
                    return And(that, actual, assertionMethod, isObserveOnly);
                }
            }
            
            CheckAssertion(assertionMethod, actual, that);
            return this;
        }

        public VerificationStep Confirm(BusinessRule businessRule, bool actual, Action<bool> assertionMethod)
        {
            BusinessRules.Add(businessRule);
            return Confirm($"{businessRule.ID} - {businessRule.Title}", actual, assertionMethod);
        }

        public VerificationStep And(string that, bool actual, Action<bool> assertionMethod)
        {
            return And(that, actual, assertionMethod, false);
        }

        public VerificationStep And(string that, bool actual, Action<bool> assertionMethod, bool isObserveOnly)
        {            
            if (!isObserveOnly)
            {
                if (LastAction == VerficiationStepAction.Statement || LastAction == VerficiationStepAction.Unknown)
                {
                    Actions.Add("Confirm that:" + " • " + that);
                    LastAction = VerficiationStepAction.Confirm;
                }
                else if (LastAction == VerficiationStepAction.Confirm || LastAction == VerficiationStepAction.And)
                {
                    Actions[Actions.Count - 1] = Actions[Actions.Count - 1] + " • " + that;
                    LastAction = VerficiationStepAction.And;
                }
            }
            
            CheckAssertion(assertionMethod, actual, that);
            return this;
        }

        public VerificationStep Confirm<T>(BusinessRule businessRule, T actual, Action<T, T> assertionMethod, T expected)
        {
            BusinessRules.Add(businessRule);
            return Confirm($"{businessRule.Title}", actual, assertionMethod, expected);
        }

        public VerificationStep Confirm<T>(string that, T actual, Action<T, T> assertionMethod, T expected)
        {
            return Confirm(that, actual, assertionMethod, expected, false);
        }

        public VerificationStep Confirm<T>(string that, T actual, Action<T, T> assertionMethod, T expected, bool isObserveOnly)
        {            
            if (!isObserveOnly)
            {
                if (LastAction == VerficiationStepAction.Statement || LastAction == VerficiationStepAction.Unknown)
                {
                    Actions.Add("Confirm that:" + " • " + that);
                    LastAction = VerficiationStepAction.Confirm;
                }
                else if (LastAction == VerficiationStepAction.Confirm || LastAction == VerficiationStepAction.And)
                {
                    return And(that, actual, assertionMethod, expected, isObserveOnly);
                }
            }
            
            CheckAssertion(assertionMethod, expected, actual, that);

            return this;
        }

        public VerificationStep And<T>(string that, T actual, Action<T, T> assertionMethod, T expected)
        {
            return And(that, actual, assertionMethod, expected, false);
        }

        public VerificationStep And<T>(string that, T actual, Action<T, T> assertionMethod, T expected, bool isObserveOnly)
        {
            if (!isObserveOnly)
            {
                if (LastAction == VerficiationStepAction.Statement || LastAction == VerficiationStepAction.Unknown)
                {
                    Actions.Add("Confirm that:" + " • " + that);
                    LastAction = VerficiationStepAction.Confirm;
                }
                else if (LastAction == VerficiationStepAction.Confirm || LastAction == VerficiationStepAction.And)
                {
                    Actions[Actions.Count - 1] = Actions[Actions.Count - 1] + " • " + that;
                    LastAction = VerficiationStepAction.And;
                }
            }        
            
            CheckAssertion(assertionMethod, expected, actual, that);

            return this;
        }

        public VerificationStep And<T>(BusinessRule businessRule, T actual, Action<T, T> assertionMethod, T expected)
        {
            BusinessRules.Add(businessRule);
            return And($"{businessRule.ID} - {businessRule.Title}", actual, assertionMethod, expected);
        }

        public void AddEvidence(Evidence evidence)
        {
            Actions.Add("*** Collect attachment" + "\n");
            Evidences.Add(evidence);
            LastAction = VerficiationStepAction.Statement;
        }

        /// <summary>
        /// Assoicates JPEG screenshot as evidence to the current step. 
        /// </summary>
        public void AddScreenshot(string fileName)
        {
            Actions.Add("*** Take a screenshot" + "\n");

            Evidences.Add
            (
                new Evidence()
                {
                    FileName = fileName,
                    TimeStamp = DateTime.Now,
                    EvidenceType = EvidenceType.JPEG_IMAGE
                }
            );
            LastAction = VerficiationStepAction.Statement;
        }

        public void AddSqlStatement(string rawSqlQuery)
        {
            AddSqlStatement(rawSqlQuery, new List<string>());
        }

        public void AddSqlStatement(string rawSqlQuery, string comment)
        {
            AddSqlStatement(rawSqlQuery, new List<string>() { comment });
        }

        public void AddSqlStatement(string rawSqlQuery, List<string> comments)
        {
            var sqlQueryReferenceName = $"SQL-{CurrentStepNumber:00}.{(SqlQueryReferences.Count + 1):00}";

            foreach (var comment in comments)
            {
                rawSqlQuery = $"-- • {comment}\n" + rawSqlQuery;
            }

            rawSqlQuery = $"-- {sqlQueryReferenceName}\n\n" + rawSqlQuery;

            SqlQueryReferences.Add(rawSqlQuery);

            if (comments.Count > 0)
            {
                Actions.Add($"Execute {sqlQueryReferenceName} script • " + string.Join(" • ", comments));
            }
            else
            {
                Actions.Add($"Execute {sqlQueryReferenceName} script");
            }
            
            LastAction = VerficiationStepAction.Statement;
        }




        public void AddScreenshot(List<string> fileNames)
        {
            Actions.Add("*** Take a screenshot" + "\n");

            foreach(var fileName in fileNames)
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
            LastAction = VerficiationStepAction.Statement;
        }

        /// <summary>
        /// Assoicates JPEG screenshot as evidence to the current step at the specified timestamp. 
        /// </summary>
        public void AddScreenshot(string fileName, DateTime timestamp)
        {
            Actions.Add("*** Take a screenshot" + "\n");

            Evidences.Add
            (
                new Evidence()
                {
                    FileName = fileName,
                    TimeStamp = timestamp,
                    EvidenceType = EvidenceType.JPEG_IMAGE
                }
            );
            LastAction = VerficiationStepAction.Statement;
        }

        /// <summary>
        /// Assoicates JPEG screenshot as evidence to the current step at the specified timestamp. 
        /// </summary>
        public void AddEvidence(string fileName, DateTime timestamp, EvidenceType evidenceType)
        {
            Actions.Add("*** Collect attachment" + "\n");

            Evidences.Add
            (
                new Evidence()
                {
                    FileName = fileName,
                    TimeStamp = timestamp,
                    EvidenceType = evidenceType
                }
            );

            LastAction = VerficiationStepAction.Statement;
        }

        public List<string> GetActionStep()
        {
            return Actions;
        }

        public List<BusinessRule> GetBusinessRules()
        {
            return BusinessRules;
        }

        public List<Evidence> GetEvidences()
        {
            return Evidences;
        }

        private void CheckAssertion(Action<bool> assertionMethod, bool actual, string step)
        {
            try
            {
                assertionMethod.Invoke(actual);
            }
            catch (Exception ex)
            {
                IsVerified = false;
                FailedVerificationErrorMessage = $"Verification failed during the step '{step}'\n\n " + ex.Message;
                throw;
            }
        }

        private void CheckAssertion<T>(Action<T, T> assertionMethod, T expected, T actual, string step)
        {
            try
            {
                assertionMethod.Invoke(expected, actual);
            } 
            catch (Exception ex)
            {
                IsVerified = false;
                FailedVerificationErrorMessage = $"Verification failed during the step '{step}'\n\n " + ex.Message;
                throw;
            }
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
                IsVerified = false;
                FailedVerificationErrorMessage = $"Unexpected observation was detected. Review '{that}'";
                new ObservationException(that, ex);
            }

            return this;
        }

        private enum VerficiationStepAction
        {
            Statement,
            Confirm,
            And,
            Unknown
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
