using System;
using System.Collections.Generic;
using System.Linq;

namespace CoatHanger
{
    [AttributeUsage(validOn: AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RegressionReleaseAttribute : Attribute
    {
        public List<string> RegressionReleaseVersions { get; set; } = new List<string>();
        public string LastRegressionRelease => RegressionReleaseVersions.Last();
        public string FirstRegresionRelease => RegressionReleaseVersions.First();

        /// <summary>
        /// The regression attribute are used to help generate release documents. In some releases you may want to 
        /// expliclity highlight certain test cases to stackholder/mangers. 
        /// 
        /// Yours going to run automated test cases anyways.  
        /// </summary>
        /// <param name="releaseVersion">The release version you want associate this regression testing.</param>
        public RegressionReleaseAttribute(params string[] regressionReleaseHistory)
        {
            // null guards.
            if (regressionReleaseHistory == null || regressionReleaseHistory.Length == 0)
            {
                throw new ArgumentNullException($"The {nameof(regressionReleaseHistory)} variable cannot be null for the {nameof(ReleaseAttribute)}"); 
            }

            RegressionReleaseVersions.AddRange(regressionReleaseHistory);
        }

        /// <summary>
        /// The regression attribute are used to help generate release documents. In some releases you may want to 
        /// expliclity highlight certain test cases to stackholder/mangers. 
        /// 
        /// Yours going to run automated test cases anyways.  
        /// </summary>
        /// <param name="releaseVersion">The release version you want associate this regression testing.</param>
        public RegressionReleaseAttribute(params int[] regressionReleaseHistory)
        {
            // null guards.
            if (regressionReleaseHistory == null || regressionReleaseHistory.Length == 0)
            {
                throw new ArgumentNullException($"The {nameof(regressionReleaseHistory)} variable cannot be null for the {nameof(ReleaseAttribute)}"); 
            }

            RegressionReleaseVersions = regressionReleaseHistory
                .OrderBy(value => value)
                .Select(value => value.ToString())
                .ToList();
        }

    }
}
