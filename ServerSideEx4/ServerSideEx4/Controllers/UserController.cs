using Microsoft.AspNetCore.Mvc;
using ServerSideEx4.BL;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServerSideEx4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<User> Get()
        {
            return BL.User.Read();
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UserController>
        [HttpPost("register")]
        public User Register([FromBody] User user)
        {
            return user.Register();
        }
        // POST api/<UserController>/login
        [HttpPost("login")]
        public User Login(string email, [FromBody] string password)
        {
           
           User user = BL.User.Login(email, password);
           if (user == null) {
                throw new Exception("User not found");
            }
            return user;
        }
        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (BL.User.Delete(id))
                return Ok(id);
            else return NotFound("There is no user with this id:" + id);
        }
    }
}
