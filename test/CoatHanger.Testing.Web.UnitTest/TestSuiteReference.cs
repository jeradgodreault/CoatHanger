using CoatHanger.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoatHanger.Testing.Web.UnitTest
{
    public class SystemSuite : ISuite
    {
        public virtual string GetDisplayName() => "System";
        public virtual string GetSuitePath() => "/" + GetDisplayName();
    }

    public class WeatherForcastSuite : SystemSuite
    {
        public override string GetDisplayName() => "Weather Forcast";
        public override string GetSuitePath() => base.GetSuitePath() + "/" + GetDisplayName();
    }

    public class TemperatureCalculationSuite : WeatherForcastSuite
    {
        public override string GetDisplayName() => "Temperature Calculation";
        public override string GetSuitePath() => base.GetSuitePath() + "/" + GetDisplayName();
    }
}
