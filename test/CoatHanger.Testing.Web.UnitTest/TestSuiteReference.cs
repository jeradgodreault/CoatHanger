
using CoatHanger.Core;
using System.Collections.Generic;

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

    public class CHWeatherProduct : IProduct
    {
        public string ProductID => "CHW";

        public string Title => "CoatHanger Weather Product";

        public string Summary 
            => @"CoatHanger Weather Product is a lorem ipsum dolor sit amet, 
consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. 
Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.
Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. 
Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.

At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque 
corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa 
qui officia deserunt mollitia animi, id est laborum et dolorum fuga. Et harum quidem rerum facilis est et expedita 
distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat 
facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et aut officiis 
debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae. 

Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut 
perferendis doloribus asperiores repellat.
";
    }

    public class WeatherForcastFeature : IProductFeature
    {
        public string FeatureID => "CHW.A";

        public string Title => "Weather Forcast Feature";

        public string Summary
            => @"The weather forcast feature is a lorem ipsum dolor sit amet, consectetur adipiscing elit, 
sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

        public IProduct Product => new CHWeatherProduct();
    }

    public class AboutPageFeature : IProductFeature
    {
        public string FeatureID => "CHW.B";

        public string Title => "About Page Feature";

        public string Summary
            => @"The about page feature is a lorem ipsum dolor sit amet, consectetur adipiscing elit, 
sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.";

        public IProduct Product => new CHWeatherProduct();
    }

    public class TemperatureCalculationFunction : IFeatureFunction
    {
        public string FunctionID => "CHW.A.1";

        public string Title => "Temperature Calculation";

        public string Summary => "Determine if the temperature is freezing, cold, or hot.";

        public IProductFeature Feature => new WeatherForcastFeature();
    }

    public class WeatherPageLayoutFunction : IFeatureFunction
    {
        public string FunctionID => "CHW.A.2";

        public string Title => "Weather Page Layout";

        public string Summary => "A webpage that will display the weather information.";

        public IProductFeature Feature => new WeatherForcastFeature();
    }

    public class AboutPageLayoutFunction : IFeatureFunction
    {
        public string FunctionID => "CHW.B.1";

        public string Title => "About Page Layout";

        public string Summary => "a webpage that will display the about page.";

        public IProductFeature Feature => new AboutPageFeature();
    }

}
