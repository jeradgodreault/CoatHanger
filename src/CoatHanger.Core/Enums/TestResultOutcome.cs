using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core.Enums
{
    public enum TestResultOutcome
    {

        /// <summary>
        /// Test was executed, but there were issues. Issues may involve exceptions 
        /// or failed assertions.
        /// </summary>
        Failed = 0,

        /// <summary>
        /// Test has completed, but we can't say if it passed or failed. May be used for 
        /// aborted tests.
        /// </summary>
        Inconclusive = 1,

        /// <summary>
        /// Test was executed without any issues.
        /// </summary>
        Passed = 2,

        /// <summary>
        /// Test is currently executing.
        /// </summary>
        InProgress = 3,

        /// <summary>
        /// There was a system error while we were trying to execute a test.
        /// </summary>
        Error = 4,

        /// <summary>
        /// The test timed out.
        /// </summary>
        Timeout = 5,

        /// <summary>
        /// Test was aborted by the user.
        /// </summary>
        Aborted = 6,

        /// <summary>
        /// Test is in an unknown state
        /// </summary>
        Unknown = 7,

        /// <summary>
        /// Test cannot be executed.
        /// </summary>
        NotRunnable = 8
    }
}
