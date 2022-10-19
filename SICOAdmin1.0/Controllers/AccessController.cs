using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SICOAdmin1._0.Models;
using SICOAdmin1._0.Models.User;

namespace SICOAdmin1._0.Controllers
{
    public class AccessController : Controller
    {
        // GET: Access
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string userName, string password)
        {
            ObjectParameter resultado = new ObjectParameter("resultado", false);
            ObjectParameter mensaje = new ObjectParameter("mensaje", "");
            try
            {

                using (SICOAdminEntities db = new SICOAdminEntities())
                {
                    //db.SP_P_Logeo(userName, password, resultado, mensaje);



                    bool bit = Convert.ToBoolean(resultado.Value);
                    string message = Convert.ToString(mensaje.Value);

                    if (true)
                    {
                        SP_C_BuscarUsuario_Result user = db.SP_C_BuscarUsuario(userName).FirstOrDefault();
                        User oUser = new User();

                        oUser.userName = user.Usuario;
                        oUser.name = user.Nombre;
                        switch (user.Tipo.ToString())
                        {
                            case "Ad":
                                oUser.type = TypesU.Administrador;
                                break;
                            case "Co":
                                oUser.type = TypesU.Consulta;
                                break;
                            case "Tr":
                                oUser.type = TypesU.Transaccional;
                                break;
                        }
                        oUser.active = user.Activo;
                        oUser.locked = user.Bloqueado;
                        oUser.password = user.Contrasena;
                        oUser.email = user.CorreoElectronico;
                        oUser.daysChangePassword = user.DiasCambioContrasena;
                        oUser.failesAttempts = user.IntentosFallidos;
                        oUser.lastChangedPassword = (DateTime)user.UltCambioContrasena;
                        oUser.lastEntry = user.UltIngreso;
                        oUser.userCreation = user.UsuarioCreacion;
                        oUser.dateCreation = user.FechaCreacion;
                        oUser.userModification = user.UsuarioModificacion;
                        oUser.dateModification = (DateTime)user.FechaModificacion;


                        Session["User"] = oUser;

                        ViewBag.Alert = message;

                        return Content("1");
                    }
                    else
                    {
                        ViewBag.Alert = message;
                        return Content(message);
                    }
                }
            }
            catch (Exception ex)
            {
                return Content("Ocurrio un error :( \n" + ex.Message);
            }
        }
   
    }
}