using CoatHanger.Core.Style;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core.Step
{
    public class GivenWhenThen
    {
        private TestProcedure TestProcedure { get; }
        public List<string> Givens { get; private set; } = new List<string>();
        public List<string> Whens { get; private set; } = new List<string>();
        public List<string> Thens { get; private set; } = new List<string>();
        
        public GivenWhenThen(TestProcedure testProcedure)
        {
            TestProcedure = testProcedure;
        }

        public void Background(string that)
        {
            TestProcedure.AddPrerequisiteStep(that);
        }

        public void Background(string that, Action setup)
        {
            TestProcedure.AddPrerequisiteStep(that);
            setup.Invoke();
        }

        /// <summary>
        /// A short cut of writing Given in a single line 
        /// leveraging using String.Format() and same line out parameter.
        /// 
        /// Example
        /// Given(that: "I have entered {0} into the calculator", value: 50, out var input)
        /// </summary>
        public GivenWhenThen Given<T>(string thatFormat, T value, out T input)
        {
            Given(string.Format(thatFormat, value));
            input = value;
            return this;
        }

        /// <summary>
        /// A short cut of writing Given in a single line 
        /// leveraging using String.Format() and same line out parameter.
        /// 
        /// Example
        /// AndGiven(that: "I have entered {0} into the calculator", value: 50, out var input)
        /// </summary>
        public GivenWhenThen AndGiven<T>(string thatFormat, T value, out T input)
        {
            AndGiven(string.Format(thatFormat, value));
            input = value;
            return this;
        }

        public GivenWhenThen AndGivenTemplate<T>(string template, T value, out T input)
        {
            if (Givens.Count == 0) throw new InvalidOperationException("You need to call Given first before chaining it with 'AndGiven'");
            var result = Nustache.Core.Render.StringToString(template.ToGivenFormat(), value);
            Givens.Add(result);

            TestProcedure.AddStep($"And {result}");

            input = value;
            return this;
        }

        public GivenWhenThen Given(string that)
        {
            that = that.ToGivenFormat();
            TestProcedure.AddStep((Givens.Count == 0)
               ? $"Given {that}"
               : $"And {that}"
            );

            Givens.Add(that);

            return this;
        }

        public GivenWhenThen AndGiven(string that)
        {
            if (Givens.Count == 0) throw new InvalidOperationException("You need to call Given first before chaining it with 'AndGiven'");
            return Given(that);
        }

        public GivenWhenThen Given(string that, Action by)
        {
            Given(that);
            by.Invoke();
            return this;
        }

        public GivenWhenThen AndGiven(string that, Action by)
        {
            if (Givens.Count == 0) throw new InvalidOperationException("You need to call Given() first before chaining it with 'AndGiven'");
            return Given(that.ToAndFormat(), by);
        }

        public GivenWhenThen When(string that)
        {
            that = that.ToWhenFormat();

            TestProcedure.AddStep((Whens.Count == 0)
               ? $"When {that}"
               : $"And {that}"
            );

            Whens.Add(that);

            return this;
        }

        public GivenWhenThen AndWhen(string that)
        {
            if (Whens.Count == 0) throw new InvalidOperationException("You need to call When() first before chaining it with 'AndWhen'");
            return When(that);
        }

        public GivenWhenThen AndWhen(string that, Action by)
        {
            if (Whens.Count == 0) throw new InvalidOperationException("You need to call When() first before chaining it with 'AndWhen'");
            return When(that, by);
        }

        public GivenWhenThen When(string that, Action by)
        {
            When(that);
            by.Invoke();
            return this;
        }

        public GivenWhenThen When<T>(string that, Func<T> by, out T result)
        {
            When(that);
            result = by.Invoke();
            return this;
        }

        public GivenWhenThen AndWhen<T>(string that, Func<T> by, out T result)
        {
            AndWhen(that);
            result = by.Invoke();
            return this;
        }

        public GivenWhenThen Then(string that, string by)
        {
            that = that.ToThenFormat();

            string expectOutcome = (Thens.Count == 0) ? $"Then {that}" : $"And {that}";
            TestProcedure.AddStep(by, expectOutcome);

            return this;
        }

        public GivenWhenThen Then(string that)
        {
            that = that.ToThenFormat();

            string expectOutcome = (Thens.Count == 0) ? $"Then {that}" : $"And {that}";
            TestProcedure.AddStep("Confirm the expected result.", expectOutcome);
            return this;
        }

        public GivenWhenThen Then(string that, Action by)
        {
            Then(that);
            by.Invoke();
            return this;
        }

        public GivenWhenThen Then<T>(string thatFormat, T expectedResult, Action<T> by)
        {
            Then(string.Format(thatFormat, expectedResult));
            by.Invoke(expectedResult);
            return this;
        }

        public GivenWhenThen ThenTemplate<T>(string template, T expectedResult, Action<T> by)
        {
            var thatStatement = Nustache.Core.Render.StringToString(template.ToGivenFormat(), expectedResult);
            Thens.Add(template);

            string expectOutcome = (Thens.Count == 0) ? $"Then {thatStatement}" : $"And {thatStatement}";
            TestProcedure.AddStep("Confirm the expected result.", expectOutcome);
            
            by.Invoke(expectedResult);
            return this;
        }

        public GivenWhenThen Then(string that, string byActionStep, Action byActionMethod)
        {
            Then(that, byActionStep);
            byActionMethod.Invoke();
            return this;
        }

    }
    
    
}
