using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core.Enums
{
    public enum VerificationCategory
    {
        /// <summary>
        /// Unit testing is the practice of testing small pieces of code, typically individual functions, alone and isolated.         
        /// Used to generate a Module Specfications. 
        /// </summary>
        Unit,
        /// <summary>
        /// Used to generated a Design Specification
        /// </summary>
        Integration,
        /// <summary>
        /// Does the system satisfy the end user's requirements.
        /// Outcome: A software requirement specification. 
        /// </summary>
        System,
    }

    public enum Browser
    {
        Default,
        Chrome,
        Firefox,
        Edge,
        InternetExplorerV11,
        InternetExplorerV10,
        InternetExplorerV9,
    }
    
    public enum Severity
    {
        /// <summary>
        /// Any catastrophic system failures could lead the user to non-usability of the applications
        /// </summary>
        Critical = 1,
        
        /// <summary>
        /// A major defect occurs when the functionality is functioning grossly away from the expectations or not doing what it should be doing. 
        /// </summary>
        Major = 2,
        
        /// <summary>
        /// A moderate defect occurs when the product or application doesn't meet certain criteria or still exhibits some unnatural behavior, however, the functionality as a whole is not impacted. 
        /// </summary>
        Moderate = 3,
        
        /// <summary>
        /// A minor low severity bug occurs when there is almost no impact on the functionality but it is still a valid defect that should be corrected. 
        /// </summary>
        Low = 4
    }
}
