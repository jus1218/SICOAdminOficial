using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SICOAdmin1._0.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //using () { 
            
            
            //}
            //List<int?> lstActions = null;
            //lstActions = db.SP_C_AuthorizeUser(oUser.userName).ToList();
            //TempData.Clear();
            //TempData["privilegios"] = lstActions;



            return View();
        }

        public ActionResult Logout()
        {
            Session["User"] = null;
            return RedirectToAction("Login", "Access");
        }
    }
}