using EX2.BL;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EX2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlatsController : ControllerBase
    {
        // GET: api/<FlatsController>
        [HttpGet]
        public List<Flat> ReadFlats()
        {
            Flat flat = new Flat();
            return flat.ReadFlats();
        }

        [HttpGet("maxPrice")]
        public IEnumerable<Flat> GetFlatMaxPriceByCity(double price, string city)
        {
            return Flat.GetFlatMaxPriceByCity(price, city);
        }

        // POST api/<FlatsController>
        [HttpPost]
        public int Post([FromBody] Flat flat)
        {
            return flat.InsertFlat();
        }
    }
}
