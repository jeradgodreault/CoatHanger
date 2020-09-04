using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core.Attributes
{


    [AttributeUsage(validOn: AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ReleaseAttribute : Attribute
    {
        public string CurrentReleasedVersion { get; private set; }
        public string CreatedReleasedVersion { get; private set; }

        /// <summary>
        /// Release attributes are used to help generate release documents. 
        /// </summary>
        /// <param name="currentReleaseVersion">The current upcoming release version this test case will included.</param>
        /// <param name="modifiedReleaseVersion">The original/created release version this test case was first used.</param>
        public ReleaseAttribute(string currentReleaseVersion, string createdReleaseVersion)
        {
            // null guards.
            if (currentReleaseVersion == null || currentReleaseVersion == "") throw new ArgumentNullException($"The {nameof(currentReleaseVersion)} variable cannot be null for the {nameof(ReleaseAttribute)}");
            if (currentReleaseVersion == null || currentReleaseVersion == "") throw new ArgumentNullException($"The {nameof(createdReleaseVersion)} variablecannot be null for the {nameof(ReleaseAttribute)}");

            CurrentReleasedVersion = currentReleaseVersion;
            CreatedReleasedVersion = createdReleaseVersion;
        }


        #region constructor overload for the different ways people might represent a release.

        /// <summary>
        /// Release attributes are used to help generate release documents. 
        /// </summary>
        /// <param name="currentReleaseVersion">The current upcoming release version this test case will included.</param>
        /// <param name="modifiedReleaseVersion">The original/created release version this test case was first used.</param>
        public ReleaseAttribute(int createdReleaseVersion, int modifiedReleaseVersion) : this(createdReleaseVersion.ToString(), modifiedReleaseVersion.ToString())
        {
            // empty
        }

        #endregion

    }

    [AttributeUsage(validOn: AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class RegressionTestingAttribute : Attribute
    {
        public string ReleasedVersion { get; private set; }

        /// <summary>
        /// The regression attribute are used to help generate release documents. In some releases you may want to 
        /// expliclity highlight certain test cases to managment to supplement a upcoming release. 
        /// 
        /// Yours going to run automated test cases anyways.  
        /// </summary>
        /// <param name="releaseVersion">The release version you want associate this regression testing.</param>
        public RegressionTestingAttribute(string releaseVersion)
        {
            // null guards.
            if (releaseVersion == null || releaseVersion == "") throw new ArgumentNullException($"The {nameof(releaseVersion)} variable cannot be null for the {nameof(ReleaseAttribute)}");

            ReleasedVersion = releaseVersion;
        }


        #region constructor overload for the different ways people might represent a release.

        /// <summary>
        /// The regression attribute are used to help generate release documents. In some releases you may want to 
        /// expliclity highlight certain test cases to managment to supplement a upcoming release. 
        /// 
        /// Yours going to run automated test cases anyways.  
        /// </summary>
        /// <param name="releaseVersion">The release version you want associate this regression testing.</param>
        public RegressionTestingAttribute(int releaseVersion) : this(releaseVersion.ToString())
        {
            // empty
        }

        #endregion


    }
}
