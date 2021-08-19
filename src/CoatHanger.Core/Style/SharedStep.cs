using CoatHanger.Core.Models;
using System;
using System.Collections.Generic;

namespace CoatHanger.Core.Style
{
    public class SharedStep
    {
        public List<string> Actions { get; private set; } = new List<string>();
        public List<Evidence> Evidences { get; private set; } = new List<Evidence>();

        public SharedStep Action(string action)
        {
            Actions.Add(action);
            return this;
        }

        public SharedStep Action(string action, Action by)
        {
            Actions.Add(action);
            by.Invoke();
            return this;
        }

        public SharedStep Step(string step)
        {
            Actions.Add(step);
            return this;
        }

        public SharedStep Step(string step, Action by)
        {
            Actions.Add(step);
            by.Invoke();
            return this;
        }

        public SharedStep LookFor(string action)
        {
            Actions.Add(action);
            return this;
        }

        public SharedStep LookFor(string action, Action by)
        {
            Actions.Add(action);
            by.Invoke();
            return this;
        }

        public SharedStep Go(string action)
        {
            Actions.Add(action);
            return this;
        }

        public SharedStep Go(string action, Action by)
        {
            Actions.Add(action);
            by.Invoke();
            return this;
        }

        public SharedStep Instruction(string instruction)
        {
            Actions.Add(instruction + "\n");
            return this;
        }

        public SharedStep Instruction(string instruction, Action by)
        {
            Actions.Add(instruction + "\n");
            by.Invoke();

            return this;
        }

        public void AddEvidence(Evidence evidence)
        {
            Actions.Add("*** Collect Evidence" + "\n");
            Evidences.Add(evidence);
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
        }

    }
}
