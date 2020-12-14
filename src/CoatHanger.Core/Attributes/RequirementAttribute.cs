using CoatHanger.Core.Enums;
using System;

namespace CoatHanger
{
    [AttributeUsage(validOn: AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class RequirementAttribute : Attribute
    {
        public FunctionalRequirement FunctionalRequirement { get; set; }
        public NonFunctionalRequirement NonFunctionalRequirement { get; set; }


        public RequirementAttribute(FunctionalRequirement requirement)
            => FunctionalRequirement = requirement;

        public RequirementAttribute(NonFunctionalRequirement requirement)
            => NonFunctionalRequirement = requirement;
    }
}
