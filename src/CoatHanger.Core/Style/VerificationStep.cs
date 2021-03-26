using CoatHanger.Core.Models;
using System;
using System.Collections.Generic;

namespace CoatHanger.Core.Step
{
    public class VerificationStep
    {
        private List<string> Actions = new List<string>();
        private List<Evidence> Evidences = new List<Evidence>();

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

        public List<string> GetActionStep()
        {
            return Actions;
        }

    }
}
