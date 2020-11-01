using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core
{
    public class SystemSpecification : IProduct
    {
        public virtual string GetDisplayName() => "System";
        public virtual string GetSuitePath() => "/" + GetDisplayName();
    }
}
