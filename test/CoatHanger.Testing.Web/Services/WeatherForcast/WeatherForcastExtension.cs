using CoatHanger.Testing.Web.Services.WeatherForcast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoatHanger.Testing.Web.Services.WeatherForcast
{
    public static class WeatherForcastExtension
    {
        public static List<WeatherForcastListItemViewModel> ToViewModel(this List<WeatherForecastDomainModel> model, FormcastService service)
        {
            return model
                .Select(wf => new WeatherForcastListItemViewModel()
                {
                    Date = wf.Date,
                    TemperatureC = wf.TemperatureC,
                    Summary = service.GetTemperatureSummary(wf.TemperatureC)
                })
                .ToList();
        }
    }
}
