using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core.Enums
{
    /// <summary>
    /// FUNCTIONAL REQUIREMENT (FR) is a description of the service that the software must offer. 
    /// It describes a software system or its component.
    /// </summary>
    public enum FunctionalRequirement
    {
        /// <summary>
        /// What do you want your system to do? What are the features you need so you can achieve your goals?
        /// </summary>
        BusinessRule,
        /// <summary>
        /// These requirements examine every transaction’s entry, changing, deleting, canceling, and error checking. 
        /// </summary>
        TransactionHandling,
        AdministrativeFunction,
        /// <summary>
        /// They concern the information users share with the system and their authentication level.
        /// </summary>
        Authentication,
        /// <summary>
        /// These functions determine various system access levels and decide who can CRUD (change, read, update, or delete) information.
        /// </summary>
        Authorization,
        /// <summary>
        /// Audit tracking is the process of tracking critical data.
        /// </summary>
        AuditTracking,
        /// <summary>
        /// These functions concern the external interface of systems other than the main system.
        /// </summary>
        ExternalInterface,
        /// <summary>
        /// Your organization might require certifications to work on the system, such as security certifications.
        /// </summary>
        Certification,
        /// <summary>
        /// This section of requirements will tell you how users can search and retrieve data.
        /// </summary>
        Search,
        /// <summary>
        /// These are laws, regulations from the government, and even internal policies that the organizations and their systems must follow.
        /// </summary>
        Legal,
        /// <summary>
        /// These are laws, regulations from the government, and even internal policies that the organizations and their systems must follow.
        /// </summary>
        Regulatory
    }

    /// <summary>
    /// NON-FUNCTIONAL REQUIREMENT (NFR) specifies the quality attribute of a software system. 
    /// They judge the software system based on constraints that are critical to the success of the software system. 
    /// </summary>
    public enum NonFunctionalRequirement  
    {
        /// <summary>
        /// System performance defines how fast a system can respond to a particular user’s action 
        /// under a certain workload.
        /// </summary>
        Performance,

        Scalability,
        /// <summary>
        /// This feature indicates your system’s storage capacity, which is dependant on its type 
        /// and characteristics.
        /// </summary>
        Capacity,
        Availability,
        /// <summary>
        /// Reliability is the probability and percentage of the software performing without failure for a 
        /// specific number of uses or amount of time.
        /// </summary>
        Reliability,
        /// <summary>
        /// Recoverability is the ability to recover from a crash or a failure in the system and returning to 
        /// full operations.
        /// </summary>
        Recoverability,
        /// <summary>
        /// This feature indicates the average time and ease and rapidity with which a system can be restored 
        /// after a failure.
        /// </summary>
        Maintainability,
        Serviceability,
        Security,
        Regulatory,
        Manageability,
        Environmental,
        DataIntegrity,
        /// <summary>
        /// This feature concerns the users; it indicates how effectively they can learn and use a system.
        /// </summary>
        Usability,
        /// <summary>
        /// All system components must follow a common and standard set of exchange formats to exchange data; 
        /// the lack of interoperability happens when people do not follow standards.
        /// </summary>
        Interoperability,
    }
}
