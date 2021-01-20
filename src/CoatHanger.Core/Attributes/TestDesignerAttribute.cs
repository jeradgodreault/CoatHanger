using System;

namespace CoatHanger
{
    /// <summary>
    /// Indicates who owns the test case.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestDesignerAttribute : Attribute
    {
        public string UserName { get; private set; }

        public TestDesignerAttribute(string userName)
        {
            UserName = userName;
        }
    }

    /// <summary>
    /// Indicates who owns the requirement.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class RequirementDesignerAttribute : Attribute
    {
        public string UserName { get; private set; }

        public RequirementDesignerAttribute(string userName)
        {
            UserName = userName;
        }
    }

}
