using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core
{
    public interface ISuite
    {
        string GetDisplayName();
        string GetSuitePath();
    }
}
