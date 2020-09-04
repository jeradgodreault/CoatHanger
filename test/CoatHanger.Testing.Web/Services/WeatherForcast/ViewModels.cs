using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoatHanger.Testing.Web.Services.WeatherForcast
{
    public class WeatherForcastListItemViewModel
    {
        /// <summary>
        /// Data must be formatted based on arbitrary business rule. 
        /// </summary>
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MMM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [DisplayName("Temperature")]
        [Range(minimum: -62, maximum: 45)]
        [DisplayFormat(DataFormatString = "{0:# °}")]
        public int TemperatureC { get; set; }

        public string Summary { get; set; }
    }
}
