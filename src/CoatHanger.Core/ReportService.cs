using CoatHanger.Core.Models;
using Nustache.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Core
{
    public class ReportService
    {



        public static string GetTestResult(CoatHangerSuite sutie)
        {
            var html = @"

<div>

{{#TestSuites}}

{{/TestSuites}}
</div>

";
            

            return "";
        }
    }
}
