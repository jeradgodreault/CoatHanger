using System;
using System.Collections.Generic;
using System.Text;
using CoatHanger.Core.Style;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoatHanger.Core.Testing.UnitTest.GwtTest
{
    [TestClass]
    public class GwtStringExtensionTest
    {

        [TestMethod("When I have a sentence that start with a capital letter")]
        public void WhenGivenFormatWithSentenceStartingWithCapitalLetter()
        {
            // Given I have a sentence that start with a capital letter
            var given = "That I have a sentence";

            // When the statement is formatted. 
            var result = given.ToFormattedGwt(removeContextWord: "given ");

            // Then the first letter will be lower case. 
            Assert.AreEqual("that I have a sentence", result);
        }

        [TestMethod("When I have a sentence that start with a lower case letter")]
        public void WhenGivenFormatWithSentenceStartingWithLowerCaseLetter()
        {
            // Given I have a sentence that start with a lower case letter
            var given = "that I have a sentence";

            // When the statement is formatted. 
            var result = given.ToFormattedGwt(removeContextWord: "given ");

            // Then will be no changes.
            Assert.AreEqual("that I have a sentence", result);
        }

        [TestMethod("When the sentence start with leading whitespace")]
        public void WhenGivenFormatWithSentenceHavingLeadingWhitespace()
        {
            // Given that I have a sentence with leading whitespace
            var given = "    sentence example.";

            // When the statement is formatted. 
            var result = given.ToFormattedGwt(removeContextWord: "given ");

            // Then whitespace is removed..
            Assert.AreEqual("sentence example.", result);
        }

        [TestMethod("When I have a sentence with leading whitespace and sentence starts with an upper case letter")]
        public void WhenGivenFormatWithSentenceHavingLeadingWhitespaceAnd()
        {
            // Given that I have a sentence with leading whitespace
            // and sentence starts with an upper case letter
            var given = "    That I have a sentence with leading whitespace and first word is capitalize";

            // When the statement is formatted. 
            var result = given.ToFormattedGwt(removeContextWord: "given ");

            // Then whitespace is removed
            // And first character is lowercase
            Assert.AreEqual("that I have a sentence with leading whitespace and first word is capitalize", result);
        }

        [TestMethod("When I have a sentence that starts with the word 'given'")]
        public void WhenGivenFormatWithSentenceStartWithTheWordGiven()
        {
            // Given that I have a sentence that starts with the word "given"
            var given = "Given that I have a sentence starting with the word 'given'.";

            // When the statement is formatted. 
            var result = given.ToFormattedGwt(removeContextWord: "given ");

            // Then "given" word is removed. 
            Assert.AreEqual("that I have a sentence starting with the word 'given'.", result);
        }

        [TestMethod("I have a sentence that starts with the word 'given' but not at the start")]
        public void WhenGivenFormatWithSentenceDoesNotStartWithTheWordGiven()
        {
            // Given that I have a sentence that starts with the word "given"
            var given = "that given I have a sentence containing the 'given'.";

            // When the statement is formatted. 
            var result = given.ToFormattedGwt(removeContextWord: "given ");

            // Then "given" word is not removed. 
            Assert.AreEqual("that given I have a sentence containing the 'given'.", result);
        }

    }
}
