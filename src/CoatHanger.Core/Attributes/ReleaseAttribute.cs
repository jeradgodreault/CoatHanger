using System;
using System.Collections.Generic;
using System.Linq;

namespace CoatHanger
{


    [AttributeUsage(validOn: AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class ReleaseAttribute : Attribute
    {        
        public List<string> ReleaseVersions { get; set; } = new List<string>();
        public string LastestRelease => ReleaseVersions.Last();
        public string CreatedRelease => ReleaseVersions.First();

        /// <summary>
        /// Release attributes are used to help generate release documents. 
        /// </summary>
        /// <param name="createdReleased">The current upcoming release version this test case will included.</param>
        /// <param name="modifiedReleaseVersion">The release versions this test case was modified significantly.</param>
        public ReleaseAttribute(params string[] modifiedReleaseVersion)
        {
            // null guards.
            if (modifiedReleaseVersion == null || modifiedReleaseVersion.Length == 0) throw new ArgumentNullException($"The {nameof(modifiedReleaseVersion)} variable cannot be null for the {nameof(ReleaseAttribute)}");            if (modifiedReleaseVersion == null || modifiedReleaseVersion.Length == 0) throw new ArgumentNullException($"The {nameof(modifiedReleaseVersion)} variable cannot be null or emtpy for the {nameof(ReleaseAttribute)}");
            
            ReleaseVersions.AddRange(modifiedReleaseVersion);
        }

        /// <summary>
        /// Release attributes are used to help generate release documents. 
        /// </summary>
        /// <param name="releaseVersion">The release versions this test case was modified significantly.</param>
        public ReleaseAttribute(params int[] releaseVersion) 
        {
            // null guards.
            if (releaseVersion == null || releaseVersion.Length == 0) throw new ArgumentNullException($"The {nameof(releaseVersion)} variable cannot be null for the {nameof(ReleaseAttribute)}"); if (releaseVersion == null || releaseVersion.Length == 0) throw new ArgumentNullException($"The {nameof(releaseVersion)} variable cannot be null or emtpy for the {nameof(ReleaseAttribute)}");

            ReleaseVersions = releaseVersion.Select(value => value.ToString()).ToList();
        }
    }
}
