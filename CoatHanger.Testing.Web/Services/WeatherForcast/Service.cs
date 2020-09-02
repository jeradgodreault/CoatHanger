using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoatHanger.Testing.Web.Services.WeatherForcast
{
    public class FormcastService
    {
        /// <summary>
        /// Format the tempature into human-friendly version based on some **arbitrary** business requirements. 
        /// </summary>
        public string GetTemperatureSummary(int tempature)
        {
            if (tempature <= 0)
            {
                return "Freezing";
            }
            else if (tempature < 20)
            {
                return "Cool";
            }
            else if (tempature < 25)
            {
                return "Mild";
            }
            else if (tempature < 30)
            {
                return "Hot";
            }
            else
            {
                return "Scorching";
            }
        }

        /// <summary>
        /// Example of a data access layer call. Imagine this is hitting some database. 
        /// </summary>
        public List<WeatherForecastDomainModel> GetWeatherForecasts()
        {
            return new List<WeatherForecastDomainModel>()
                {
                    new WeatherForecastDomainModel()
                    {
                        Date = new DateTime(year: 2020, month: 05, day: 01),
                        TemperatureC = 10
                    },
                    new WeatherForecastDomainModel()
                    {
                        Date = new DateTime(year: 2020, month: 05, day: 02),
                        TemperatureC = 13
                    },
                    new WeatherForecastDomainModel()
                    {
                        Date = new DateTime(year: 2020, month: 05, day: 03),
                        TemperatureC = -4
                    },
                    new WeatherForecastDomainModel()
                    {
                        Date = new DateTime(year: 2020, month: 05, day: 04),
                        TemperatureC = 0
                    },
                    new WeatherForecastDomainModel()
                    {
                        Date = new DateTime(year: 2020, month: 05, day: 05),
                        TemperatureC = 2
                    },
                    new WeatherForecastDomainModel()
                    {
                        Date = new DateTime(year: 2020, month: 05, day: 06),
                        TemperatureC = 19
                    },
                    new WeatherForecastDomainModel()
                    {
                        Date = new DateTime(year: 2020, month: 05, day: 07),
                        TemperatureC = 20
                    },
                    new WeatherForecastDomainModel()
                    {
                        Date = new DateTime(year: 2020, month: 05, day: 08),
                        TemperatureC = 21
                    },
                    new WeatherForecastDomainModel()
                    {
                        Date = new DateTime(year: 2020, month: 05, day: 09),
                        TemperatureC = 25
                    },
                    new WeatherForecastDomainModel()
                    {
                        Date = new DateTime(year: 2020, month: 05, day: 10),
                        TemperatureC = 26
                    },
                    new WeatherForecastDomainModel()
                    {
                        Date = new DateTime(year: 2020, month: 05, day: 10),
                        TemperatureC = 30
                    },
                    new WeatherForecastDomainModel()
                    {
                        Date = new DateTime(year: 2020, month: 05, day: 10),
                        TemperatureC = 32
                    },
                };

        }

    }
}
