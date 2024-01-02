using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Motorento_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        private DAL.DbManager db = new DAL.DbManager();

        // Get all orders from the database
        [HttpGet("[action]")]
        [Authorize(Roles = "admin")]
        public IActionResult Get()
        {
            try
            {
                return Ok(db.GetAllOrders());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Get a single order's data from the database, identified by id
        [HttpGet("[action]/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(db.GetOrderById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //Get all orders made by a specific user, identified by said user's user-name
        [HttpGet("[action]/{username}")]
        [Authorize(Roles = "admin,user")]
        public IActionResult GetOrdersByUserName(string username)
        {
            try
            {
                return Ok(db.GetOrdersByUserName(username));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //Get an order's car model, identified by the model id
        [HttpGet("[action]/{id}")]
        [Authorize(Roles = "employee")]
        public IActionResult GetModelFromOrder(int id)
        {
            try
            {
                return Ok(db.GetModelFromOrder(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Get relevant data about every order made by a specific user, (identified by user name) for viewing in that user's order-history page
        [HttpGet("[action]/{username}")]
        [Authorize(Roles = "user")]
        public IActionResult GetHistoryOrdersByUserName(string username)
        {
            try
            {
                return Ok(this.db.GetHistoryOrdersByUserName(username));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Create new order in database
        [HttpPost("[action]")]
        [Authorize(Roles = "admin,employee,user")]
        public IActionResult Post([FromBody] Order newOrder)
        {
            try
            {
                db.AddOrder(newOrder);
                return Created("http://localhost:26185/api/models/get/" + newOrder.Id, newOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Perform car return procedure, including updating order as returned at current date
        [HttpPut("[action]")]
        [Authorize(Roles = "admin,employee")]
        public IActionResult Return(Car returningCar)
        {
            try
            {
                return Ok(db.Return(returningCar));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Edit a specific order's details in database, identifying the order by its id
        [HttpPut("[action]/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Put(int id, [FromBody] Order d)
        {
            try
            {
                return Ok(db.EditOrder(id, d));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Delete a specific order from the database, identified by id
        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok(db.DeleteOrder(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
