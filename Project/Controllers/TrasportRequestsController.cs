using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ravid.Dto;
using Ravid.Enums;
using Ravid.Models;
using Ravid.Utilities;

namespace Ravid.Controllers
{

    [Authorize(AuthenticationSchemes = "CookieAuthentication")] // the name of the authntication in startup.cs
    [RedirectingAction]
    public class TrasportRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly string _emailAccount = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("Gmail")["Account"];

        public TrasportRequestsController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            
        }


        public async Task<IActionResult> Index()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var roleName = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;

            Task<List<TrasportRequestDto>> dtoList;

            if (roleName == nameof(UserRoles.Administrator))
            {
                dtoList = (from u in _context.Users
                           join t in _context.TrasportRequests
                           on u.UserId
                               equals
                           t.UserId
                           orderby t.DateCreated descending
                           select new TrasportRequestDto
                           {
                               UserId = u.UserId,
                               Comment = t.Comment,
                               ForDate = t.ForDate,
                               TrasportRequestStatus = t.TrasportRequestStatus,
                               Company = u.Company,
                               DateCreated = t.DateCreated,
                               DeliveryFor = t.DeliveryFor,
                               NumberOfPlates = t.NumberOfPlates,
                               TrasportRequestId = t.TrasportRequestId

                           }).ToListAsync();
            }
            else
            {
                dtoList = (from u in _context.Users
                           join t in _context.TrasportRequests
                           on u.UserId
                               equals
                           t.UserId
                           where
                                u.UserId == Guid.Parse(userId)
                           orderby t.DateCreated descending
                           select new TrasportRequestDto
                           {
                               UserId = u.UserId,
                               Comment = t.Comment,
                               ForDate = t.ForDate,
                               TrasportRequestStatus = t.TrasportRequestStatus,
                               Company = u.Company,
                               DateCreated = t.DateCreated,
                               DeliveryFor = t.DeliveryFor,
                               NumberOfPlates = t.NumberOfPlates,
                               TrasportRequestId = t.TrasportRequestId
                           }).ToListAsync();

            }
            return View(await dtoList);
        }


        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trasportRequest = await _context.TrasportRequests
                .FirstOrDefaultAsync(m => m.TrasportRequestId == id);
            if (trasportRequest == null)
            {
                return NotFound();
            }

            return View(trasportRequest);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrasportRequestId,ForDate,Comment,DateCreated,TrasportRequestStatusId, DeliveryFor, NumberOfPlates")] TrasportRequest t)
        {
            if (ModelState.IsValid)
            {
                t.TrasportRequestId = Guid.NewGuid();
                t.TrasportRequestStatus = Enum.GetName(typeof(TrasportRequestStatus), 1);
                var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
                t.UserId = Guid.Parse(userId);

                _context.Add(t);
                await _context.SaveChangesAsync();


                var u = await _context.Users.FirstOrDefaultAsync(u => u.UserId == Guid.Parse(userId));

                var content = "<p>יש לך בקשת הובלה חדשה</p>";
                content += "<br><b>שם הלקוח:</b> " + u.Company;
                content += "<br><b>לתאריך:</b> " + t.ForDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                content += "<br><b>משלוח אל:</b> " + t.DeliveryFor;
                content += "<br><b>הערות: </b>" + t.Comment;

                _ = Task.Run(() => SendEmail.SendEmailWithGmail(content, _emailAccount, "בקשת הובלה חדשה", false));


                return RedirectToAction(nameof(Index));
            }
            return View(t);
        }


        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.TrasportRequests.FindAsync(id);
            var dto = _mapper.Map<TrasportRequestDto>(model);

            var dic = new Dictionary<int, string>();
            foreach (var s in Enum.GetValues(typeof(Enums.TrasportRequestStatus)))
            {
                dic.Add((int)s, s.ToString());
            }
            dto.TrasportRequestStatusDic = dic;

            if (dto == null)
            {
                return NotFound();
            }
            return View(dto);
        }        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("UserId,TrasportRequestId,ForDate,Comment,DateCreated,TrasportRequestStatusId,TrasportRequestStatus,NumberOfPlates,DeliveryFor")] TrasportRequest t)
        {
            if (id != t.TrasportRequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    int x;
                    bool isInt = int.TryParse(t.TrasportRequestStatus, out x);
                    if (isInt)
                    {
                        t.TrasportRequestStatus = Enum.GetName(typeof(Enums.TrasportRequestStatus), int.Parse(t.TrasportRequestStatus));
                    }                   

                    _context.Update(t);
                    await _context.SaveChangesAsync();


                    if (t.TrasportRequestStatus == Enum.GetName(typeof(TrasportRequestStatus), 2))
                    { 
                        // אושרה
                        var u = await _context.Users.FirstOrDefaultAsync(u => u.UserId ==  t.UserId);

                        var content = "<p>הודעה על שינוי סטטוס של בקשת ההובלה שלך</p>";
                        content += "<br><b>שם הלקוח:</b> " + u.Company;
                        content += "<br><b>לתאריך:</b> " + t.ForDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        content += "<br><b>משלוח אל:</b> " + t.DeliveryFor;
                        content += "<br><b>הערות: </b>" + t.Comment;
                        content += "<br><b>סטטוס: </b>" + t.TrasportRequestStatus;

                        _ = Task.Run(() => SendEmail.SendEmailWithGmail(content, u.Email, "שינוי סטטוס של בקשת ההובלה שלך", false));
                    }


                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrasportRequestExists(t.TrasportRequestId))
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
            return View(t);
        }



        // GET: TrasportRequests/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value != nameof(UserRoles.Administrator))
            {
                return RedirectToAction(nameof(Index));
            }

            var trasportRequest = await _context.TrasportRequests
                .FirstOrDefaultAsync(m => m.TrasportRequestId == id);
            if (trasportRequest == null)
            {
                return NotFound();
            }

            return View(trasportRequest);
        }

        // POST: TrasportRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {

            if (_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value != nameof(UserRoles.Administrator))
            {
                return RedirectToAction(nameof(Index));
            }

            var trasportRequest = await _context.TrasportRequests.FindAsync(id);
            _context.TrasportRequests.Remove(trasportRequest);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrasportRequestExists(Guid id)
        {
            return _context.TrasportRequests.Any(e => e.TrasportRequestId == id);
        }
    }
}
