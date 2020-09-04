using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core.Enums
{
    public enum TestingCategory
    {
        /// <summary>
        /// A level of the software testing process where individual units/components of a software/system are tested. 
        /// The purpose is to validate that each unit of the software performs as designed.
        /// </summary>
        Unit,

        /// <summary>
        /// A level of the software testing process where individual units are combined and tested as a group. 
        /// The purpose of this level of testing is to expose faults in the interaction between integrated units.
        /// </summary>
        Integration,

        /// <summary>
        ///  A level of the software testing process where a complete, integrated system/software is tested. 
        ///  The purpose of this test is to evaluate the system’s compliance with the specified requirements.
        /// </summary>
        System,

        /// <summary>
        /// A level of the software testing process where a system is tested for acceptability. 
        /// The purpose of this test is to evaluate the system’s compliance with the business requirements and assess whether it is acceptable for delivery.
        /// </summary>
        Acceptance
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
        Critical = 0,
        
        /// <summary>
        /// A major defect occurs when the functionality is functioning grossly away from the expectations or not doing what it should be doing. 
        /// </summary>
        Major = 1,
        
        /// <summary>
        /// A moderate defect occurs when the product or application doesn't meet certain criteria or still exhibits some unnatural behavior, however, the functionality as a whole is not impacted. 
        /// </summary>
        Moderate = 2,
        
        /// <summary>
        /// A minor low severity bug occurs when there is almost no impact on the functionality but it is still a valid defect that should be corrected. 
        /// </summary>
        Low = 3
    }
}
