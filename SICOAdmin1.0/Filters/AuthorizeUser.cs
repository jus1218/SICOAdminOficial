using SICOAdmin1._0.Models;
using SICOAdmin1._0.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SICOAdmin1._0.Filters
{
    [AttributeUsage(AttributeTargets.Method,AllowMultiple = false)]
    public class AuthorizeUser: AuthorizeAttribute
    {
        private User objUser;
        //private MiSistemasEntitites db = new MiSistemasEntitites();
        private int accion;


        public AuthorizeUser(int pAccion) {
            this.accion = pAccion;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //base.OnAuthorization(filterContext);
            List<int?> lstActions = null;

            try
            {
                objUser = (User)HttpContext.Current.Session["User"];
                using (SICOAdminEntities db = new SICOAdminEntities())
                {
                    lstActions = db.SP_C_AuthorizeUser(objUser.userName).ToList();
                }

                HttpContext.Current.Session["lstActions"] = lstActions;

                //lstActions.Count() < 1
                if (!(lstActions.Contains(accion)))
                {//Por si no tiene permisos
                    filterContext.Result = new RedirectResult("~/Home/Index");
                }

            }
            catch (Exception e){
                filterContext.Result = new RedirectResult("~/Home/Index");
            }
            

        }



    }
}