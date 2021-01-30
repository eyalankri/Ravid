using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;
using Ravid.Enums;

namespace Ravid
{
    public class RedirectingActionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //base.OnActionExecuting(filterContext);
            //var roleName = filterContext.HttpContext.User.FindFirst(ClaimTypes.Role).Value;
            
            //if (roleName != nameof(UserRoles.Administrator))
            //{
            //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            //    {
            //        controller = "Home",
            //        action = "Index"
            //    }));
            //}
          
        }
    }
}
