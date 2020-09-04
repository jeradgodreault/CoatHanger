using System;

namespace CoatHanger
{
    public class TestDesignerAttribute : Attribute
    {
        public string UserName { get; private set; }

        public TestDesignerAttribute(string userName)
        {
            UserName = userName;
        }
    }
}
