using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AStar.Dev.Versioning.Demo.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController (ILogger<WeatherForecastController> logger) : ControllerBase
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet]
    [Produces("application/json")]
    [ProducesErrorResponseType(typeof(ProblemDetails))]

    //[ProducesResponseType(typeof(IEnumerable<WeatherForecast>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<IEnumerable<WeatherForecast>> GetForecast()
    {
        logger.LogInformation("GetForecast called."); // added to stop the "not used" warning
        var rng = new Random();

        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
                                                      {
                                                          Date         = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                                                          TemperatureC = rng.Next(-20, 55),
                                                          Summary      = Summaries[rng.Next(Summaries.Length)]
                                                      })
                         .ToArray();
    }
}
