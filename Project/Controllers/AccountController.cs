using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Ravid.Models;
using System.Linq;
using Ravid.Enums;
using Microsoft.AspNetCore.Cors;
using Ravid.Utilities;
using Ravid.Dtos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Collections.Generic;

namespace Ravid.Controllers
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(AuthenticationSchemes = "CookieAuthentication")] // the name of the authntication in startup.cs
    [Route("[controller]")]
    public partial class AccountController : Controller
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _db;

        public AccountController(IConfiguration config, ApplicationDbContext db)
        {
            _config = config;
            _db = db;
        }

        [AllowAnonymous]
        [Route("Login")]
        public IActionResult Login()
        {
            return View();
        }




        [AllowAnonymous]
        [HttpPost]
        [EnableCors("MyPolicy")]
        [Route("UserLogin")]
        public IActionResult UserLogin([FromBody] LoginDto user)
        {

            var getRoleAndUserId = GetRoleAndUserId(user);
            if (getRoleAndUserId == null)
            {
                return BadRequest("User was not found.");
            }


            var userClaims = new List<Claim>()
                {
                     new Claim(ClaimTypes.Role, getRoleAndUserId.RoleName),
                     new Claim(ClaimTypes.Email, user.Email),
                     new Claim(ClaimTypes.NameIdentifier, getRoleAndUserId.UserId.ToString()),
                 };

            var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");

            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });
            HttpContext.SignInAsync(userPrincipal);


            //var tokenString = BuildToken();
            //IActionResult response = Ok(new { token = tokenString });
            IActionResult response = Ok();// token is not used here any more! 
            return response;
        }




        private RoleAndUserId GetRoleAndUserId(LoginDto login)
        {
            var password = Encryption.Sha256(login.Password);

            var roleAndUserId = (
                  from u in _db.Users
                  join uir in _db.UserInRoles on u.UserId equals uir.UserId
                  join r in _db.Roles on uir.RoleId equals r.RoleId
                  where
                         u.Password == password &&
                         u.Email == login.Email
                  select new RoleAndUserId
                  {
                      UserId = u.UserId,
                      RoleName = r.RoleName,

                  }).FirstOrDefault();

            if (roleAndUserId != null)
            {
                return roleAndUserId;
            }
            return null;             

        }






        /// --- not in use :
        #region NotInUser

        private string BuildToken()
        {
            var x = _config["Jwt:Key"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        [HttpGet]
        [Route("IsTokenValid")]
        public IActionResult IsTokenValid()
        {

            return Ok();
        }



        [AllowAnonymous]
        [HttpPost]
        [EnableCors("MyPolicy")]
        [Route("CreateToken")]
        public IActionResult CreateToken([FromBody] LoginDto login)
        {
            try
            {
                IActionResult response = Unauthorized();

                if (!ModelState.IsValid) { return response; }


                var roleName = GetRoleAndUserId(login); // Admins only

                if (roleName != null)
                {
                    var tokenString = BuildToken();
                    response = Ok(new { token = tokenString });
                }

                return response;
            }
            catch (Exception ex)
            {

                return Json(new { status = "error", message = ex.ToString() });
            }
        }
        #endregion




    }


    public class RoleAndUserId
    {
        public string RoleName { get; set; }
        public Guid UserId { get; set; }
    }
}