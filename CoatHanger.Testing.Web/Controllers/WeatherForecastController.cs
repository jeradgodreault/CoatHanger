using CoatHanger.Testing.Web.Services.WeatherForcast;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CoatHanger.Testing.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private FormcastService _formcastService;

        public WeatherForecastController()
        {
            _formcastService = new FormcastService();
        }

        public WeatherForecastController(FormcastService service)
        {

            _formcastService = service;
        }

        [HttpGet]
        public IEnumerable<WeatherForcastListItemViewModel> Get()
        {
            var domainModels = _formcastService.GetWeatherForecasts();

            return domainModels.ToViewModel(_formcastService);
        }

        public class WeatherForecastDomainModel
        {            
            public DateTime Date { get; set; }            
            public int TemperatureC { get; set; }
        }




 

      
    }
}
