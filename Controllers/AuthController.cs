using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ECart.Helpers;
using ECart.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IConfiguration _config;
    public AuthController(DataContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }
    //change in auth 

    [HttpGet("getalllogindetails")]
    //[Authorize]
    public IActionResult GetAllLoginDetails()
    {
        try
        { 
            var userdetails= _context.UserDetails.ToList();
          
            if(userdetails.Count==0)
            {
                return NotFound("users not yet enrolled");
            }
            else
            {
                return Ok(userdetails);
            }
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    

    [HttpGet("getlogindetails/{id}")]
    //[Authorize(Roles ="Admin")]
    public IActionResult GetLoginDetailsById(int id)
    {
        try
        {
            var userdetails = _context.UserDetails.Find(id);
            if(userdetails==null)
            {
                return NotFound($"user details not found with Id {id}");
            }
            else
            {
                return Ok(userdetails);
            }
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("register")]
    public ActionResult<UserDetails> Register(UserDetails userDetails)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.UserDetails.Add(userDetails);
        _context.SaveChanges();

        return CreatedAtAction("GetUser", new { id = userDetails.Id }, userDetails);
    }

     private string GenerateJwtToken(UserDetails existingUser)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new List<Claim>
        { 
            new Claim("id", existingUser.Id.ToString()),
            new Claim("role", existingUser.Role.ToString())
        };
        var token = new JwtSecurityToken
        (
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials
        );
        return new JwtSecurityTokenHandler().WriteToken(token);

    }
    [HttpPost("login")]
    public ActionResult<UserDetails> Login([FromBody] UserDetails loginDetails)
    {
        if (ModelState.IsValid)
        {
            var existingUser = _context.UserDetails.FirstOrDefault(u => u.Username == loginDetails.Username && u.Password == loginDetails.Password);
            if (existingUser == null)
            {
                return NotFound();
            }
            LoginDto loginDto = new LoginDto
            {
                Id = existingUser.Id,
                Username = loginDetails.Username,
                Role = existingUser.Role,
                Token = GenerateJwtToken(existingUser)
            };
            return Ok(loginDto);
        }
        return BadRequest(ModelState);
    }


    private bool LoginDetailsExists(int id)
    {
        // Check if a LoginDetails with the given ID exists in the database
        return _context.UserDetails.Any(x => x.Id == id);
    }
    
    [HttpPut("addlogindetails/{id}")]
    //[Authorize(Roles ="Admin")]
    public IActionResult AddLoginDetails([FromRoute] int id, [FromBody] UserDetails userDetails)
    {
        if (!ModelState.IsValid)
        {
            // If the model state is not valid, return a Bad Request response with validation errors
            return BadRequest(ModelState);
        }
        if (id != userDetails.Id)
        {
            return BadRequest("id does not match.");
        }
        try
        {
            // Update the LoginDetails in the database and save changes
            _context.Entry(userDetails).State = EntityState.Modified;
            _context.SaveChanges();
            // Return a No Content response indicating success
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            //  check if the LoginDetails with the given ID exists
            if (!LoginDetailsExists(id))
            {
                return NotFound("id not found.");
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
            }
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
        }
    }

    [HttpDelete("deletelogindetails{id}")]
    //[Authorize(Roles ="Admin")]
    public IActionResult DeleteLoginDetails([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            // If the model state is not valid, return a Bad Request response with validation errors
            return BadRequest(ModelState);
        }

        try
        {
            // Retrieve the LoginDetails with the given ID
            UserDetails userDetails = _context.UserDetails.SingleOrDefault(x => x.Id == id);
            if (userDetails == null)
            {
                return NotFound("id not found.");
            }

            // Remove the LoginDetails from the database and save changes
            _context.UserDetails.Remove(userDetails);
            _context.SaveChanges();

            return Ok(userDetails);
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal Server Error");
        }
    }
   
    private bool UserExists(int id)
    {
        // Check if a LoginDetails with the given ID exists in the database
        return _context.UserDetails.Any(x => x.Id == id);

    }

}

 
