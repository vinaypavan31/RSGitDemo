using ECart.Helpers;
using ECart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace ECart.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("getallusers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _context.User.ToList();

                if (users.Count == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(users);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet("getuser/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _context.User.Find(id);

                if (user == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(user);
                }
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("adduser")]
        [Authorize(Roles = "Admin")]
        public IActionResult AddNewUser(User user)
        {
            try
            {
                _context.Add(user);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPut("updateuser/{id}")]
        public IActionResult UpdateUser(User userUpdate)
        {
            try
            {
                if (userUpdate == null || userUpdate.Id == 0)
                {
                    return BadRequest();
                }

                var user = _context.User.Find(userUpdate.Id);

                if (user == null)
                {
                    return NotFound();
                }

                user.Name = userUpdate.Name;
                user.Email = userUpdate.Email;
                user.Address = userUpdate.Address;
                user.Phone = userUpdate.Phone;
                user.Role = userUpdate.Role;

                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("deleteuser/{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var user = _context.User.Find(id);

                if (user == null)
                {
                    return NotFound();
                }

                _context.User.Remove(user);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
            //THIS IS ANOTHER LINE 
        }
    }
}

