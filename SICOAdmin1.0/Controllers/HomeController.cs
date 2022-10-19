using SICOAdmin1._0.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SICOAdmin1._0.Controllers
{
    public class HomeController : Controller
    {
        static bool estaLogeado = false;
        public ActionResult Index()
        {
            if (!estaLogeado){
                Session["User"] = ViewBag.logUser = new User()
                {
                    userName = "sin login",
                    name = "sin login"
                };
                estaLogeado = true;
            }
            else
                ViewBag.logUser = (User)Session["User"];



            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Login", "Access");
        }
    }
}