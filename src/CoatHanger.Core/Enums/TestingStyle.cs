using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core.Enums
{
    public enum TestingStyle
    {
        /// <summary>
        /// Boundary testing is the process of testing between extreme ends or boundaries between partitions of the input values.
        /// So these extreme ends like Start- End, Lower- Upper, Maximum-Minimum, Just Inside-Just Outside values are called boundary values and 
        /// the testing is called "boundary testing".
        /// 
        /// The basic idea in normal boundary value testing is to select input variable values at their:
        /// - Minimum
        /// - Just above the minimum
        /// - A nominal value
        /// - Just below the maximum
        /// - Maximum
        /// </summary>
        BoundaryValueTesting,
        /// <summary>
        /// The objective of Postive Testing (or Happy Path Testing) is to test an application successfully on a positive flow. 
        /// It does not look for negative or error conditions. The focus is only on the valid and positive inputs 
        /// through which application generates the expected output.
        /// </summary>
        PostiveTesting,

        /// <summary>
        ///  A Negative Testing technique is performed using incorrect data, invalid data or input. 
        ///  It validates that if the system throws an error of invalid input and behaves as expected.
        /// </summary>
        NegativeTesting,

        /// <summary>
        /// The objective of this testing is to find the defects and break the application 
        /// by executing any flow of the application or any random functionality.
        /// 
        /// There is generally no reference to the test case or any plan or documentation in place for such type of testing.
        /// </summary>
        AdhocTesting,

        /// <summary>
        /// Equivalence partitioning or equivalence class partitioning (ECP) is a software testing 
        /// technique that divides the input data of a software unit into partitions of equivalent 
        /// data from which test cases can be derived. In principle, test cases are designed to 
        /// cover each partition at least once.
        /// </summary>
        EquivalencePartitioning,

        /// <summary>
        /// It is a type of White box Testing and is carried out during Unit Testing. 
        /// Branch Testing, the name itself suggests that the code is tested thoroughly by traversing at every branch.
        /// </summary>
        BranchTesting,
    }
}
