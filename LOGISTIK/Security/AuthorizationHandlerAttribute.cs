using LOGISTIK.App_Constant;
using LOGISTIK.Data;
using LOGISTIK.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LOGISTIK.Security
{
    public class AuthorizationHandlerAttribute : AuthorizeAttribute
    {
        private LOGISTIKContext db = new LOGISTIKContext();

        public AuthorizationHandlerAttribute()
        {
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var routeData = httpContext.Request.RequestContext.RouteData;
            bool authorized = false;

            if (GeneralContants.ALL_ACCESS != true)
            {
                var currUser = HttpContext.Current.User.Identity.Name.ToLower();
                var haveAcess = db.Users.FirstOrDefault(x => x.WindowsAccount.ToLower() == currUser);
                if (haveAcess != null)
                {
                    var myAccess = haveAcess.Role;
                    var currentRoles = Roles.Split(',').ToList();
                    if (currentRoles.Contains(myAccess))
                    {
                        authorized = true;
                    }
                }
            }

            return authorized;
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult { ViewName = "AccessDenied" };
        }
    }
}