using System;
using System.Collections.Generic;
using System.Linq;

namespace CoatHanger
{


    [AttributeUsage(validOn: AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ReleaseAttribute : Attribute
    {
        public string CreatedReleasedVersion { get; private set; }
        public List<string> ModifiedReleases { get; set; } = new List<string>();
        public string LastestVersion => ModifiedReleases.Last();

        /// <summary>
        /// Release attributes are used to help generate release documents. 
        /// </summary>
        /// <param name="createdReleased">The current upcoming release version this test case will included.</param>
        public ReleaseAttribute(string createdReleased)
        {
            // null guards.
            if (createdReleased == null || createdReleased == "") throw new ArgumentNullException($"The {nameof(createdReleased)} variable cannot be null for the {nameof(ReleaseAttribute)}");

            CreatedReleasedVersion = createdReleased;
            ModifiedReleases.Add(createdReleased); // if modified not provided, assume it only version.
        }

        /// <summary>
        /// Release attributes are used to help generate release documents. 
        /// </summary>
        /// <param name="createdReleased">The current upcoming release version this test case will included.</param>
        /// <param name="modifiedReleaseVersion">The release versions this test case was modified significantly.</param>
        public ReleaseAttribute(string createdReleased, params string[] modifiedReleaseVersion)
        {
            // null guards.
            if (createdReleased == null || createdReleased == "") throw new ArgumentNullException($"The {nameof(createdReleased)} variable cannot be null for the {nameof(ReleaseAttribute)}");
            if (modifiedReleaseVersion == null || modifiedReleaseVersion.Length == 0) throw new ArgumentNullException($"The {nameof(modifiedReleaseVersion)} variable cannot be null or emtpy for the {nameof(ReleaseAttribute)}");
            
            CreatedReleasedVersion = createdReleased;
            ModifiedReleases.AddRange(modifiedReleaseVersion);
        }


        #region constructor overload for the different ways people might represent a release.

        /// <summary>
        /// Release attributes are used to help generate release documents. 
        /// </summary>
        /// <param name="currentReleaseVersion">The current upcoming release version this test case will included.</param>
        /// <param name="modifiedReleaseVersion">The release versions this test case was modified significantly.</param>
        public ReleaseAttribute(int createdReleaseVersion, params int[] modifiedReleaseVersion) 
        {
            CreatedReleasedVersion = createdReleaseVersion.ToString();
            ModifiedReleases = modifiedReleaseVersion.Select(value => value.ToString()).ToList();
        }

        #endregion

    }

    [AttributeUsage(validOn: AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
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
    }
}
