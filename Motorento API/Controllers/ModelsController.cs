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
    public class ModelsController : ControllerBase
    {

        private DAL.DbManager db = new DAL.DbManager();

        // Get all car models from database
        [HttpGet("[action]")]
        public IActionResult Get()
        {
            try
            {
                return Ok(db.GetAllModels());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Get a specific car model from database, identified by id
        [HttpGet("[action]/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(db.GetModelById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Add a new car model to the database
        [HttpPost("[action]")]
        [Authorize(Roles = "admin")]
        public IActionResult Post([FromBody] Model m)
        {
            try
            {
                db.AddModel(m);
                return Created("http://localhost:26185/models/get/" + m.Id, m);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Edit a car model's details in the database, identified by id
        [HttpPut("[action]/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Put(int id, [FromBody] Model m)
        {
            try
            {
                return Ok(db.EditModel(id, m));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Delete a car model from the database, identified by id
        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok(db.DeleteModel(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
