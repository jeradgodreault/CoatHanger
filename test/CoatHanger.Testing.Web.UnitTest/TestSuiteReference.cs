using CoatHanger.Core;

namespace CoatHanger.Testing.Web.UnitTest
{
    /// <summary>
    /// Represent a tree hierarchy of test suite. 
    /// -- System Specification
    /// |-- Weather Forecast Feature   
    /// |   |-- Temperature Calculation Function
    /// |   |   |-- Test Case 1B
    /// |   |   `-- Test Case 2B
    /// |   `-- Weather Grid Function
    /// |       |-- Test Case 1C
    /// |       `-- Test Case 2C
    /// `-- About Suite Feature
    ///     -- Layout Function
    ///         |-- Test Case 1D
    ///         `-- Test Case 2D
    /// </summary>
    public class WeatherForcastFeature : SystemSpecification
    {
        public override string GetDisplayName() => "Weather Forcast";
        public override string GetSuitePath() => base.GetSuitePath() + "/" + GetDisplayName();
    }

    public class TemperatureCalculationFunction : WeatherForcastFeature
    {
        public override string GetDisplayName() => "Temperature Calculation";
        public override string GetSuitePath() => base.GetSuitePath() + "/" + GetDisplayName();
    }

    public class WeatherPageFunction : WeatherForcastFeature
    {
        public override string GetDisplayName() => "Weather Page Layout";
        public override string GetSuitePath() => base.GetSuitePath() + "/" + GetDisplayName();
    }

    public class AboutPageFunction : SystemSpecification
    {
        public override string GetDisplayName() => "About";
        public override string GetSuitePath() => base.GetSuitePath() + "/" + GetDisplayName();
    }
}
