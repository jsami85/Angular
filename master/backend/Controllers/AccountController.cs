using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using CoreNg.Models;
using CoreNg.Services;

namespace CoreNg.Controllers
{
    public class Credentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserCreate input)
        {
            var user = _userService.Add(input);


            var identityUser = new IdentityUser(user.Email);
            identityUser.Id = user.Email;
            return Ok(CreateToken(user.Id));
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Credentials credentials)
        {
            var user = _userService.Verify(credentials.Email, credentials.Password);

            if (user == null) return BadRequest();
            return Ok(CreateToken(user.Id));
        }

        static string CreateToken(int id)
        {
            var claims = new Claim[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, id.ToString())
                    };

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("this is the secret phrase"));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(signingCredentials: signingCredentials, claims: claims);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
