using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ravid.Dto;
using Ravid.Dtos;
using Ravid.Enums;
using Ravid.Models;
using Ravid.Utilities;

namespace Ravid.Controllers
{

    [Authorize(AuthenticationSchemes = "CookieAuthentication")] // the name of the authntication in startup.cs
    [RedirectingAction]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserController(ApplicationDbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;


        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;

            IQueryable<User> users;
            if (role == nameof(UserRoles.Administrator))
            {
                users = (from row in _context.Users
                         orderby row.DateRegistered descending
                         select row);
                return View(await users.ToListAsync());
            }



            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            users = (from row in _context.Users
                     where row.UserId == Guid.Parse(userId)
                     select row);

            return View(await users.ToListAsync());
        }


        // GET: User/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }



        public IActionResult Create()
        {

            var model = new UserDto();
            model.RoleList = _context.Roles.ToList();

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,FirstName,LastName,Email,Password,Phone1,Phone2,DateRegistered,IsDeleted,Company,Comment,RoleId")] UserDto userDto)
        {

            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(userDto);


                var email = (
                       from u in _context.Users
                       where u.Email == userDto.Email
                       select u.Email.FirstOrDefault()
                       );

                if (email.Any())
                {
                    IActionResult response = BadRequest();
                    return response; // email exists.
                }


                user.UserId = Guid.NewGuid();
                var rawPassword = user.Password;
                user.Password = Encryption.Sha256(user.Password);

                var userInRole = new UserInRole
                {
                    RoleId = int.Parse(userDto.RoleId),
                    UserId = user.UserId
                };



                _context.Add(user);
                _context.Add(userInRole);

                await _context.SaveChangesAsync();


                var siteUrl = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Website")["Url"];

                var content = "<p>פרטי ההתחברות שלך לממשק של רביד</p>";
                content += "<br><b>אימייל:</b> " + user.Email;
                content += "<br><b>סיסמה:</b> " + rawPassword;
                content += $"<p><a href=\"{siteUrl}\">לחץ כאן להתחברות</a></p>";

                _ = Task.Run(() => SendEmail.SendEmailWithGmail(content, user.Email, "פרטי ההתחברות שלך לממשק של רביד", false));

                return RedirectToAction(nameof(Index));
            }



            userDto.RoleList = _context.Roles.ToList();
            return View(userDto);
        }



        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserId,FirstName,LastName,Email,Password,Phone1,Phone2,DateRegistered,IsDeleted,Company,Comment")] User user)
        {


            if (id != user.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }


        public async Task<IActionResult> Delete(Guid? id)
        {
            var role = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            if (role != nameof(UserRoles.Administrator))
            {
                return RedirectToAction(nameof(Index));
            }

            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] changePasswordDto dto)
        {

            if (string.IsNullOrEmpty(dto.Password) || dto.UserId == Guid.Empty)
            {
                return NotFound();
            }
            try
            {
                var user = await _context.Users.FindAsync(dto.UserId);
                user.Password = Encryption.Sha256(dto.Password);
                _context.Update(user);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                var a = ex;

                throw;
            }





            IActionResult response = Ok();
            return response;
        }


        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
