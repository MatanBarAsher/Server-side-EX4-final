using EX2.BL;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EX2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        // GET: api/<UsersController>
        [HttpGet]
        public List<User> ReadUsers()
        {
            User user = new User();
            return user.ReadUsers();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        public int Post([FromBody] User user)
        {
            return user.InsertUser();
        }

        [HttpPost("Login")]
        public User CheckLogin([FromBody] User user)
        {
            return user.CheckLogin();
        }

        [HttpPut("Update")]
        public User Update([FromBody] User user)
        {
            return user.UpdateUser(user);
        }

    }
}
