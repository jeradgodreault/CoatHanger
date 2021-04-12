using CoatHanger.Core.Models;
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
        /// Assumption first entry in the array is the created release.
        /// </summary>
        public ReleaseAttribute(params string[] releaseHistory)
        {
            // null guards.
            if (releaseHistory == null || releaseHistory.Length == 0) throw new ArgumentNullException($"The {nameof(releaseHistory)} variable cannot be null for the {nameof(ReleaseAttribute)}"); if (releaseHistory == null || releaseHistory.Length == 0) throw new ArgumentNullException($"The {nameof(releaseHistory)} variable cannot be null or emtpy for the {nameof(ReleaseAttribute)}");

            ReleaseVersions.AddRange(releaseHistory);
        }

        /// <summary>
        /// Release attributes are used to help generate release documents. 
        /// </summary>
        /// <param name="releaseVersion">The release versions this test case was modified significantly.</param>
        public ReleaseAttribute(params int[] releaseVersion)
        {
            // null guards.
            if (releaseVersion == null || releaseVersion.Length == 0) throw new ArgumentNullException($"The {nameof(releaseVersion)} variable cannot be null for the {nameof(ReleaseAttribute)}"); if (releaseVersion == null || releaseVersion.Length == 0) throw new ArgumentNullException($"The {nameof(releaseVersion)} variable cannot be null or emtpy for the {nameof(ReleaseAttribute)}");

            ReleaseVersions = releaseVersion
                .OrderBy(value => value)
                .Select(value => value.ToString())
                .ToList();
        }

        /// <summary>
        /// Release attributes are used to help generate release documents. 
        /// </summary>
        /// <param name="releaseVersion">The release versions this test case was modified significantly.</param>
        public ReleaseAttribute(SemVer releaseVersion)
        {
            ReleaseVersions.Add(releaseVersion.ToString());
        }

        /// <summary>
        /// Release attributes are used to help generate release documents. 
        /// </summary>
        /// <param name="releaseVersion">The release versions this test case was modified significantly.</param>
        public ReleaseAttribute(params SemVer[] releaseVersion)
        {
            // null guards.
            if (releaseVersion == null || releaseVersion.Length == 0) throw new ArgumentNullException($"The {nameof(releaseVersion)} variable cannot be null for the {nameof(ReleaseAttribute)}"); if (releaseVersion == null || releaseVersion.Length == 0) throw new ArgumentNullException($"The {nameof(releaseVersion)} variable cannot be null or emtpy for the {nameof(ReleaseAttribute)}");

            ReleaseVersions = releaseVersion
                .Cast<SemVer>()
                .OrderBy(value => value.Sequence())
                .Select(value => value.ToString())
                .ToList();
        }

    }
}
