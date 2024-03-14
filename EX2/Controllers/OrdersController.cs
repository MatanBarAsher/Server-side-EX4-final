using EX2.BL;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EX2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        // GET: api/<OrdersController>
        [HttpGet]
        public List<Vacation> ReadVacations()
        {
            Vacation vacation = new Vacation();
            return vacation.ReadVacations();
        }

        // POST api/<OrdersController>
        [HttpPost]
        public int Post([FromBody] Vacation order)
        {
            return order.InsertVacation();
        }

        // A routing to a method that will return all the orders between start and end dates 
        [HttpGet("getByDates/{startDate}/{endDate}")]
        public IEnumerable<Vacation> GetOrdersByDates([FromRoute] DateTime startDate, [FromRoute] DateTime endDate)
        {
            return Vacation.GetOrdersByDates(startDate, endDate);
        }

        [HttpGet("Report/{month}")]
        public object GetReport([FromRoute] string month)
        {
            try
            {
                int m = Convert.ToInt32(month);
                return Vacation.Report(m);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid month format. Please provide a valid numeric month.");
            }
        }
    }
}
