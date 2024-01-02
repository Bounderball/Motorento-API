using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DAL.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Motorento_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private DAL.DbManager db = new DAL.DbManager();

        IConfiguration config;

        public UsersController(IConfiguration conf)
        {
            config = conf;
        }

        // Get all users from database
        [HttpGet("[action]")]
        [Authorize(Roles = "admin")]
        public IActionResult Get()
        {
            try
            {
                return Ok(db.GetAllUsers());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //Get a single user from database, by user name
        [HttpGet("[action]/{username}")]
        [Authorize(Roles = "admin")]
        public IActionResult Get(string username)
        {
            try
            {
                return Ok(db.GetUserByUserName(username));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Register a new user
        [HttpPost("[action]")]
        public IActionResult Register(User newUser)
        {
            try
            {
                db.Register(newUser);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Login as existing user, and receive a corresponding token, providing clearance to read and edit revelant data in the database via the web page
        [HttpPost("[action]")]
        public IActionResult Login([FromBody] User loginDetails)
        {
            try
            {
                loginDetails.Role = db.Login(loginDetails); //Perform user name and password authentication, and get the user's role from the database

                var securityKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(config["jwt1:SecretKey1"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[] {
                  new Claim(JwtRegisteredClaimNames.Sub,"abc"),
                  new Claim(ClaimTypes.Name,loginDetails.UserName),
                  new Claim(ClaimTypes.Role,loginDetails.Role),   //get the user's role from the database
                  new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };
                var token = new JwtSecurityToken(issuer: config["jwt1:Issuer1"],
                    audience: config["jwt1:Audience1"], claims: claims,
                    expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);
                string tok = new JwtSecurityTokenHandler().WriteToken(token);
                return Ok(new { token1 = tok, userDetails1 = loginDetails });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Edit a single user's data in the database
        [HttpPut("[action]")]
        [Authorize(Roles = "admin")]
        public IActionResult Put([FromBody] User u)
        {
            try
            {
                return Ok(db.EditUser(u));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // Delete a single user from the database, identified by user name
        [HttpDelete("[action]/{username}")]
        [Authorize(Roles = "admin")]
        public IActionResult Delete(string username)
        {
            try
            {
                return Ok(db.DeleteUser(username));
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
