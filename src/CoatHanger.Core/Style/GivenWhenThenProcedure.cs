using CoatHanger.Core.Style;
using System;
using System.Collections.Generic;

namespace CoatHanger.Core.Step
{
    public class GivenWhenThenProcedure : TestProcedure
    {
        public List<string> Givens { get; private set; } = new List<string>();
        public List<string> Whens { get; private set; } = new List<string>();
        public List<string> Thens { get; private set; } = new List<string>();
        private GivenWhenThenAction LastAction { get; set; } = GivenWhenThenAction.Unknown;

        private enum GivenWhenThenAction
        {
            Given,
            When,
            Then,
            Unknown
        }

        public GivenWhenThenProcedure()
        {

        }

        public void Background(string that)
        {
            base.AddPrerequisiteStep(that);
        }

        public void Background(string that, Action setup)
        {
            base.AddPrerequisiteStep(that);
            setup.Invoke();
        }

        #region (that)
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public GivenWhenThenProcedure Given(string that)
        {
            that = that.ToGivenFormat();
            base.AddStep((Givens.Count == 0)
               ? $"Given {that}"
               : $"And {that}"
            );

            Givens.Add(that);
            LastAction = GivenWhenThenAction.Given;

            return this;
        }
        public GivenWhenThenProcedure When(string that)
        {
            that = that.ToWhenFormat();

            base.AddStep((Whens.Count == 0)
               ? $"When {that}"
               : $"And {that}"
            );

            Whens.Add(that);
            LastAction = GivenWhenThenAction.When;

            return this;
        }

        public GivenWhenThenProcedure Then(string that)
        {
            that = that.ToThenFormat();

            string expectOutcome = (Thens.Count == 0) ? $"Then {that}" : $"And {that}";
            base.AddStep("Confirm the expected result.", expectOutcome);
            LastAction = GivenWhenThenAction.Then;
            return this;
        }

        public GivenWhenThenProcedure And(string that)
        {
            if (LastAction == GivenWhenThenAction.Unknown) throw new InvalidOperationException("You cannot invoke the AND statement without calling a GIVEN/WHEN/THEN first.");

            return (LastAction == GivenWhenThenAction.When)
                ? When(that)
                : (LastAction == GivenWhenThenAction.Then)
                ? Then(that)
                : Given(that);
        }

        #endregion (that)

        #region (that, by)
        // ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
        public GivenWhenThenProcedure Given(string that, Action by)
        {
            Given(that);
            by.Invoke();
            return this;
        }

        public GivenWhenThenProcedure When(string that, Action by)
        {
            When(that);
            by.Invoke();
            return this;
        }

        public GivenWhenThenProcedure Then(string that, Action by)
        {
            Then(that);
            by.Invoke();
            return this;
        }

        public GivenWhenThenProcedure And(string that, Action by)
        {
            if (LastAction == GivenWhenThenAction.Unknown) throw new InvalidOperationException("You cannot invoke the AND statement without calling a GIVEN/WHEN/THEN first.");

            return (LastAction == GivenWhenThenAction.When)
                ? When(that, by)
                : (LastAction == GivenWhenThenAction.Then)
                ? Then(that, by)
                : Given(that, by);
        }

        #endregion (that, by)

        #region (thatFormat, input)

        /// <summary>
        /// Uses `String.Format` to construct the step. 
        /// Example
        /// - .Given("there are {0} cumcumbers", GetCucumbersCount()); 
        /// </summary>
        public GivenWhenThenProcedure Given<T>(string thatFormat, T input)
        {
            Given(string.Format(thatFormat, input));
            return this;
        }

        /// <summary>
        /// Uses `String.Format` to construct the step. 
        /// Example
        /// - .When("I eat {0} cumcumbers", GetCucumbersCount()); 
        /// </summary>
        public GivenWhenThenProcedure When<T>(string thatFormat, T input)
        {
            When(string.Format(thatFormat, input));
            return this;
        }

        /// <summary>
        /// Uses `String.Format` to construct the step. 
        /// Example
        /// - .Then("I should have {0} cumcumbers", GetCucumbersCount()); 
        /// </summary>
        public GivenWhenThenProcedure Then<T>(string thatFormat, T input)
        {
            Then(string.Format(thatFormat, input));
            return this;
        }

        /// <summary>
        /// Uses `String.Format` to construct the step. 
        /// Example
        /// - .And("Fred ate {0} cumcumbers", fred.GetCucumbersCount()); 
        /// </summary>
        public GivenWhenThenProcedure And<T>(string thatFormat, T input)
        {
            if (LastAction == GivenWhenThenAction.Unknown) throw new InvalidOperationException("You cannot invoke the AND statement without calling a GIVEN/WHEN/THEN first.");

            return (LastAction == GivenWhenThenAction.When)
                ? When(thatFormat, input)
                : (LastAction == GivenWhenThenAction.Then)
                ? Then(thatFormat, input)
                : Given(thatFormat, input);
        }

        #endregion (thatFormat, input)

        #region (thatFormat, input, output)

        /// <summary>
        /// A short cut of writing formating a step and creating a variable in a single line. 
        /// This leverages String.Format(). 
        /// 
        /// Example
        /// Given(that: "I there are {0} cucumbers", input: 50, out int cucumberCount)
        /// </summary>
        public GivenWhenThenProcedure Given<T>(string thatFormat, T input, out T output)
        {
            Given(string.Format(thatFormat, input));
            output = input;
            return this;
        }

        /// <summary>
        /// A short cut of writing formating a step and creating a variable in a single line. 
        /// This leverages String.Format(). 
        /// 
        /// Example
        /// When(that: "I eat {0} cucumbers", input: 50, out int cucumberCount)
        /// </summary>
        public GivenWhenThenProcedure When<T>(string thatFormat, T input, out T output)
        {
            When(string.Format(thatFormat, input));
            output = input;
            return this;
        }

        /// <summary>
        /// A short cut of writing formating a step and creating a variable in a single line. 
        /// This leverages String.Format(). 
        /// 
        /// Example
        /// Then(that: "I should have {0} cucumbers left", input: 50, out int cucumberCount)
        /// </summary>
        public GivenWhenThenProcedure Then<T>(string thatFormat, T input, out T output)
        {
            When(string.Format(thatFormat, input));
            output = input;
            return this;
        }

        public GivenWhenThenProcedure And<T>(string thatFormat, T input, out T output)
        {
            if (LastAction == GivenWhenThenAction.Unknown) throw new InvalidOperationException("You cannot invoke the AND statement without calling a GIVEN/WHEN/THEN first.");

            T finalOutput;
            GivenWhenThenProcedure result;
            if (LastAction == GivenWhenThenAction.When)
            {
                result = When<T>(thatFormat, input, out finalOutput);
            }
            else if (LastAction == GivenWhenThenAction.Then)
            {
                result = Then<T>(thatFormat, input, out finalOutput);
            }
            else
            {
                result = Given<T>(thatFormat, input, out finalOutput);
            }

            output = finalOutput;

            return result;
        }

        #endregion (thatFormat, value, input)

        #region (template, input)

        public GivenWhenThenProcedure GivenTemplate<T>(string template, T input)
        {
            var thatTemplate = Nustache.Core.Render.StringToString((Givens.Count == 0)
                ? template.ToGivenFormat()
                : template.ToAndFormat(), input);

            Givens.Add(template);

            base.AddStep(thatTemplate);
            LastAction = GivenWhenThenAction.Given;

            return this;
        }

        public GivenWhenThenProcedure WhenTemplate<T>(string template, T input)
        {
            var thatTemplate = Nustache.Core.Render.StringToString((Whens.Count == 0)
                ? template.ToWhenFormat()
                : template.ToAndFormat(), input);

            Whens.Add(template);

            base.AddStep((Whens.Count == 0) ? $"When {thatTemplate}" : $"And {thatTemplate}");
            LastAction = GivenWhenThenAction.When;

            return this;
        }

        public GivenWhenThenProcedure ThenTemplate<T>(string template, T input)
        {
            var thatTemplate = Nustache.Core.Render.StringToString((Thens.Count == 0)
                ? template.ToThenFormat()
                : template.ToAndFormat(), input);

            Thens.Add(template);

            base.AddStep(thatTemplate);
            LastAction = GivenWhenThenAction.Then;

            return this;
        }

        public GivenWhenThenProcedure AndTemplate<T>(string template, T input)
        {
            if (LastAction == GivenWhenThenAction.Unknown) throw new InvalidOperationException("You cannot invoke the AND statement without calling a GIVEN/WHEN/THEN first.");

            return (LastAction == GivenWhenThenAction.When)
                ? WhenTemplate(template, input)
                : (LastAction == GivenWhenThenAction.Then)
                ? ThenTemplate(template, input)
                : GivenTemplate(template, input);
        }


        #endregion (template, input)

        #region (template, Dictionary<> input)

        public GivenWhenThenProcedure GivenTemplate(string template, Dictionary<string, string> input)
        {
            return GivenTemplate<Dictionary<string, string>>(template, input);
        }

        public GivenWhenThenProcedure WhenTemplate(string template, Dictionary<string, string> input)
        {
            return WhenTemplate<Dictionary<string, string>>(template, input);
        }

        public GivenWhenThenProcedure ThenTemplate(string template, Dictionary<string, string> input)
        {
            return ThenTemplate<Dictionary<string, string>>(template, input);
        }

        public GivenWhenThenProcedure AndTemplate(string template, Dictionary<string, string> input)
        {
            return AndTemplate<Dictionary<string, string>>(template, input);
        }


        #endregion (template, Dictionary<> input)



        #region (template, input, output)

        public GivenWhenThenProcedure GivenTemplate<T>(string template, T input, out T output)
        {
            output = input;
            return GivenTemplate(template, input);
        }

        public GivenWhenThenProcedure WhenTemplate<T>(string template, T input, out T output)
        {
            output = input;
            return WhenTemplate(template, input);
        }

        public GivenWhenThenProcedure ThenTemplate<T>(string template, T input, out T output)
        {
            output = input;
            return ThenTemplate(template, input);
        }

        public GivenWhenThenProcedure AndTemplate<T>(string template, T input, out T output)
        {
            if (LastAction == GivenWhenThenAction.Unknown) throw new InvalidOperationException("You cannot invoke the AND statement without calling a GIVEN/WHEN/THEN first.");

            T finalOutput;
            GivenWhenThenProcedure result;
            if (LastAction == GivenWhenThenAction.When)
            {
                result = When<T>(template, input, out finalOutput);
            }
            else if (LastAction == GivenWhenThenAction.Then)
            {
                result = Then<T>(template, input, out finalOutput);
            }
            else
            {
                result = Given<T>(template, input, out finalOutput);
            }

            output = finalOutput;

            return result;
        }

        #endregion (template, input, output)

        #region (that, by, result)

        public GivenWhenThenProcedure Given<T>(string that, Func<T> by, out T result)
        {
            Given(that);
            result = by.Invoke();
            return this;
        }

        public GivenWhenThenProcedure When<T>(string that, Func<T> by, out T result)
        {
            When(that);
            result = by.Invoke();
            return this;
        }

        public GivenWhenThenProcedure Then<T>(string that, Func<T> by, out T result)
        {
            Then(that);
            result = by.Invoke();
            return this;
        }

        public GivenWhenThenProcedure And<T>(string that, Func<T> by, out T result)
        {
            T finalOutput;
            GivenWhenThenProcedure finalResult;
            if (LastAction == GivenWhenThenAction.When)
            {
                finalResult = When<T>(that, by, out finalOutput);
            }
            else if (LastAction == GivenWhenThenAction.Then)
            {
                finalResult = Then<T>(that, by, out finalOutput);
            }
            else
            {
                finalResult = Given<T>(that, by, out finalOutput);
            }

            result = finalOutput;

            return finalResult;
        }

        #endregion


        public GivenWhenThenProcedure Then<T>(string thatFormat, T expectedResult, Action<T> by)
        {
            Then(string.Format(thatFormat, expectedResult));
            by.Invoke(expectedResult);
            return this;
        }

        public GivenWhenThenProcedure ThenTemplate<T>(string template, T expectedResult, Action<T> by)
        {
            var thatStatement = Nustache.Core.Render.StringToString(template.ToGivenFormat(), expectedResult);
            Thens.Add(template);

            string expectOutcome = (Thens.Count == 0) ? $"Then {thatStatement}" : $"And {thatStatement}";
            base.AddStep("Confirm the expected result.", expectOutcome);

            by.Invoke(expectedResult);
            return this;
        }

        public GivenWhenThenProcedure Then(BusinessRule businessRule
            , Func<VerificationStep, VerificationStep> ToVerify)
        {
            var that = $"{businessRule.Title}";
            BusinessRules.Add(businessRule);

            return Then(that, ToVerify);
        }

        public GivenWhenThenProcedure And(BusinessRule businessRule
            , Func<VerificationStep, VerificationStep> ToVerify)
        {
            return Then(businessRule, ToVerify);
        }

        public GivenWhenThenProcedure Then(string that, Func<VerificationStep, VerificationStep> ToVerify)
        {
            that = that.ToThenFormat();
            string expectOutcome = (Thens.Count == 0) ? $"Then {that}" : $"And {that}";

            var verification = ToVerify.Invoke(new VerificationStep());
            base.AddStep(verification.GetActionStep().ToArray(), expectOutcome);
            base.BusinessRules.AddRange(verification.GetBusinessRules());

            Thens.Add(that);
            LastAction = GivenWhenThenAction.Then;

            return this;
        }

        public GivenWhenThenProcedure And(string that, Func<VerificationStep, VerificationStep> ToVerify)
        {
            return Then(that, ToVerify);
        }

        public GivenWhenThenProcedure Then(string that, string by)
        {
            that = that.ToThenFormat();

            string expectOutcome = (Thens.Count == 0) ? $"Then {that}" : $"And {that}";
            base.AddStep(by, expectOutcome);
            LastAction = GivenWhenThenAction.Then;

            return this;
        }

    }
}
