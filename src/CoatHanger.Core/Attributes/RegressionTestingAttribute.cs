using System;

namespace CoatHanger
{
    [AttributeUsage(validOn: AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RegressionTestingAttribute : ReleaseAttribute
    {
        /// <summary>
        /// The regression attribute are used to help generate release documents. In some releases you may want to 
        /// expliclity highlight certain test cases to stackholder/mangers. 
        /// 
        /// Yours going to run automated test cases anyways.  
        /// </summary>
        /// <param name="releaseVersion">The release version you want associate this regression testing.</param>
        public RegressionTestingAttribute(params string[] releaseVersions) : base(releaseVersions)
        {
            //empty, use base class.
        }

        /// <summary>
        /// The regression attribute are used to help generate release documents. In some releases you may want to 
        /// expliclity highlight certain test cases to stackholder/mangers. 
        /// 
        /// Yours going to run automated test cases anyways.  
        /// </summary>
        /// <param name="releaseVersion">The release version you want associate this regression testing.</param>
        public RegressionTestingAttribute(params int[] releaseVersions) : base(releaseVersions)
        {
            //empty, use base class.
        }

    }
}
