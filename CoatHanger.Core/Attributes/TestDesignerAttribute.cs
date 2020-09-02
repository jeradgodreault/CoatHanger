using CoatHanger.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core.Attributes
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
