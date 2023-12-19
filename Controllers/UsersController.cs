using COeX_India1._2.Data;
using COeX_India1._2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace COeX_India1._2.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class UsersController:Controller
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _config;

        public UsersController(ApplicationDbContext dbContext, IConfiguration config)
        {
            _dbContext=dbContext;
            _config=config;
        }

        private string GenerateToken(User user)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                 {
                    new Claim(JwtRegisteredClaimNames.Sub, _config["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("userId", user.UserId.ToString()),
                    new Claim("userType", user.UserType.ToString())
                 };

                var token = new JwtSecurityToken
                    (
                    _config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddYears(12),
                    signingCredentials: credentials

                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        [AllowAnonymous]
        [HttpPost("loginSiding")]

        public async Task<ActionResult> loginSiding(LoginModel req)
        {
            try
            {
                if(string.IsNullOrEmpty(req.Username))
                {
                    return BadRequest(new Response(false, "UserName is not valid"));
                }
                if(string.IsNullOrEmpty(req.Password))
                {
                    return BadRequest(new Response(false, "Password is not valid"));
                }

                if(req.LogginAs==Models.LoginModel.ELoggingAs.SidingUser)
                {
                    if(req.SidingId==0) { return BadRequest(new Response(false, "Please enter a valid Siding Id")); }
                    var siding= await _dbContext.Sidings.Where(s=> s.Id==req.SidingId).FirstOrDefaultAsync();
                    if(siding==null)
                    {
                        return BadRequest(new Response(false, "Siding Not found"));
                    }
                }
                var user= await _dbContext.Users.Where(u=> u.Username ==req.Username && u.Password==req.Password).FirstOrDefaultAsync();
                if(user==null)
                {
                    return BadRequest(new Response(false, "Username or password not found"));
                }

                var token = GenerateToken(user);
                return Ok(new LoginResponse(true, token));
            }
            catch (Exception ex)
            {
                return Problem("OOps! something went wromg");
            }
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]

        public async Task<ActionResult> Signup(User user)
        {
            try
            {                              

                if(string.IsNullOrWhiteSpace(user.Username))
                { return BadRequest(new Response(false, "Please enter a valid userId")); }

                if (string.IsNullOrWhiteSpace(user.Password))
                {
                    return BadRequest(new Response(false, "Please enter a valid password"));
                }
                
                if(user.UserType!= Models.User.EUserType.Admin && user.UserType != Models.User.EUserType.SidingUser) { return BadRequest(new Response(false, "Please enter a valid userType")); }

                if(user.UserType == Models.User.EUserType.SidingUser)
                {
                    if (user.SidingId == 0)
                    {
                        return BadRequest(new Response(false, "please Enter a valid SidingId"));
                    }
                }

                var newUser= await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                return Ok(new Response(true, "User Added Successfully"));
            }
            catch (Exception ex)
            {
                return Problem("Oops something went wrong");
            }
        }


    }
}

