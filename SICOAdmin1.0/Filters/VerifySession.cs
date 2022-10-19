using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SICOAdmin1._0.Controllers;
using SICOAdmin1._0.Models.User;

namespace SICOAdmin1._0.Filters
{
    public class VerifySession : ActionFilterAttribute
    {
        private User oUser;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                base.OnActionExecuting(filterContext);

                oUser = (User)HttpContext.Current.Session["User"];
                if (oUser == null)
                {

                    if (filterContext.Controller is AccessController == false)
                    {
                        filterContext.HttpContext.Response.Redirect("~/Access/Login");
                    }
                }
                else
                {
                    if (filterContext.Controller is AccessController == true)
                    {
                        filterContext.HttpContext.Response.Redirect("~/Home/Index");
                    }
                }

            }
            catch (Exception)
            {
                filterContext.Result = new RedirectResult("~/Access/Login");
            }
        }
    }
}