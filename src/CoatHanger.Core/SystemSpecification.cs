using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core
{
    public class SystemSpecification : ISuite
    {
        public virtual string GetDisplayName() => "System";
        public virtual string GetSuitePath() => "/" + GetDisplayName();
    }
}
