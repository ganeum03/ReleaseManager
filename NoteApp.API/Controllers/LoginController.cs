using Microsoft.AspNetCore.Mvc;
using NoteApp.API.Model;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NoteApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDbContext ApplicationContex;
        public LoginController(AppDbContext _ApplicationContex)
        {
            ApplicationContex = _ApplicationContex;
        }
        // GET: api/<Login>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<Login>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<Login>
        [HttpPost]
        public ActionResult<NoteLogin> Post([FromBody] NoteLogin data)
        {
            var entity = ApplicationContex.NoteLogin.Where(e => e.UserId == data.UserId && e.Password == data.Password).FirstOrDefault();

            if (entity == null)
            {
                return NotFound();
            }

            return Ok(entity);
        }

        // PUT api/<Login>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Login>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

}
