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
    public class BranchesController : ControllerBase
    {

        private DAL.DbManager db = new DAL.DbManager();

        // Get all branches from the database
        [HttpGet("[action]")]
        public IActionResult Get()
        {
            try
            {
                return Ok(db.GetAllBranches());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Get a specific branch from the database, identified by id
        [HttpGet("[action]/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                return Ok(db.GetBranchById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Add a new branch to the database
        [HttpPost("[action]")]
        [Authorize(Roles = "admin")]
        public IActionResult Post([FromBody] Branch b)
        {
            try
            {
                db.AddBranch(b);
                return Created("http://localhost:26185/branches/get/" + b.Id, b);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Edit a branch's details in the database, identified by id
        [HttpPut("[action]/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Put(int id, [FromBody] Branch b)
        {
            try
            {
                return Ok(db.EditBranch(id, b));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Delete a specific branch from the database, identified by id
        [HttpDelete("[action]/{id}")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                return Ok(db.DeleteBranch(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

    }
}
