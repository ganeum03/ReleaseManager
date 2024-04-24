using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace NoteApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly AppDbContext ApplicationContex;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, AppDbContext _ApplicationContex)
        {
            _logger = logger;
            ApplicationContex = _ApplicationContex;
        }

        [HttpGet]
        public async Task<object> Get()
        {
            //var t=(from A in ApplicationContex.RoleMaster select new RoleMaster { Roledesc=A.Roledesc, Roleid=A.Roleid}).ToList();
            //Response.ContentType = "application/json";
            return Ok();
            //var rng = new Random();
            //return Enumerable.Range(1, 5).Select(index => new RoleMaster
            //{
            //    //Date = DateTime.Now.AddDays(index),
            //    //TemperatureC = rng.Next(-20, 55),
            //    //Summary = Summaries[rng.Next(Summaries.Length)]
            //})
            //.ToArray();
        }
    }
}
