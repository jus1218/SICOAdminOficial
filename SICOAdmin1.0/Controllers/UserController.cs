
using SICOAdmin1._0.Models;
using SICOAdmin1._0.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;


namespace SICOAdmin.Controllers
{
    public class UserController : Controller
    {
        List<SP_P_PerfilesDelUsuario_Result> lstUsuariosPerfil = null;
        List<SP_P_PerfilesDelUsuario_Result> lstPerfiles = null;
        List<f_opcionesPerfilesUsuario_Result> viewPerfiles = null;
        public string usuario = "";
        //Varibles para la tabla asincronica
        ObjectParameter totalPag = new ObjectParameter("totalPag", 0);
        int PagActual = 0;

        // GET: User
        public ActionResult Index()
        {
            List<SP_C_MostrarUsuarios_Result> lst = null;

            List<User> lstModel = new List<User>();

            User objM;

            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                lst = db.SP_C_MostrarUsuarios("tod", "").ToList();
            }



            foreach (var e in lst)
            {
                objM = new User();
                objM.userName = e.Usuario;
                objM.name = e.Nombre;
                switch (e.Tipo.ToString())
                {
                    case "Ad":
                        objM.type = TypesU.Administrador;
                        break;
                    case "Co":
                        objM.type = TypesU.Consulta;
                        break;
                    case "Tr":
                        objM.type = TypesU.Transaccional;
                        break;
                }
                objM.email = e.CorreoElectronico;
                objM.daysChangePassword = e.DiasCambioContrasena;
                objM.failesAttempts = e.IntentosFallidos;
                objM.active = e.Activo;
                objM.locked = e.Bloqueado;
                objM.lastChangedPassword = (DateTime)e.UltCambioContrasena;
                objM.lastEntry = (DateTime)e.UltIngreso;
                objM.userCreation = e.UsuarioCreacion;
                objM.dateCreation = e.FechaCreacion;
                objM.userModification = e.UsuarioModificacion;
                objM.dateModification = (DateTime)e.FechaModificacion;

                lstModel.Add(objM);
            }



            return View(lstModel);
        }

        [HttpGet]
        public ActionResult addUser()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addUser(User model)
        {
            ObjectParameter resultado = new ObjectParameter("resultado", false);
            ObjectParameter mensaje = new ObjectParameter("mensaje", "");
            try
            {
                if (model != null)
                {
                    using (var db = new SICOAdminEntities())
                    {
                        int num = db.SP_P_AddUser(model.userName, model.name, Convert.ToString(model.type), model.active, model.locked,
                                  model.password, model.email, model.daysChangePassword, (byte)model.failesAttempts,
                                  ((User)Session["User"]).userName, ((User)Session["User"]).userName, resultado, mensaje);
                    }

                    bool bit = Convert.ToBoolean(resultado.Value);
                    string message = Convert.ToString(mensaje.Value);

                    if (bit)
                    {
                        TempData.Clear();
                        TempData["MessageAdd"] = "1" + message;
                        return RedirectToAction(Url.Content("/Index"));
                    }
                    else
                    {
                        TempData.Clear();
                        TempData["MessageAdd"] = "0" + message;
                        return RedirectToAction(Url.Content("/Index"));
                    }
                }
                return View(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActionResult Edit(string userName)
        {
            List<SP_C_BuscarUsuario_Result> lst = null;


            ViewBag.usuarioLog = userName;
            usuario = userName;
            ViewBag.perfilEliminar = 0;

            User objM = new User();



            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                lst = db.SP_C_BuscarUsuario(userName).ToList();
                lstPerfiles = db.SP_P_PerfilesDelUsuario(userName, PagActual, 1, "", totalPag).ToList();
                viewPerfiles = db.f_opcionesPerfilesUsuario(userName).ToList();
            }

            foreach (var e in lst)
            {
                objM.userName = e.Usuario;
                objM.name = e.Nombre;
                switch (e.Tipo.ToString())
                {
                    case "Ad":
                        objM.type = TypesU.Administrador;
                        break;
                    case "Co":
                        objM.type = TypesU.Consulta;
                        break;
                    case "Tr":
                        objM.type = TypesU.Transaccional;
                        break;
                }
                objM.active = e.Activo;
                objM.locked = e.Bloqueado;
                objM.password = e.Contrasena;
                objM.email = e.CorreoElectronico;
                objM.daysChangePassword = e.DiasCambioContrasena;
                objM.failesAttempts = e.IntentosFallidos;
                objM.lastChangedPassword = (DateTime)e.UltCambioContrasena;
                objM.lastEntry = e.UltIngreso;
                objM.userCreation = e.UsuarioCreacion;
                objM.dateCreation = e.FechaCreacion;
                objM.userModification = e.UsuarioModificacion;
                objM.dateModification = (DateTime)e.FechaModificacion;
            }

            //lstPerfiles.ForEach(p => p = perfil);
            //Lista de perfiles donde esta el usuario
            List<SelectListItem> ddlperfiles = viewPerfiles.ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Descripcion.ToString(),
                    Value = d.IdPerfil.ToString(),
                    Selected = false
                };
            });

            ViewBag.PagActual = PagActual + 1;
            ViewBag.totalPag = totalPag;//Total de veces que puede tocar el btn

            ViewBag.userPerfil = lstPerfiles;
            ViewBag.viewPerfiles = ddlperfiles;

            return View(objM);
        }

        [HttpPost]
        public ActionResult updateUser(User user)
        {
            ObjectParameter resultado = new ObjectParameter("resultado", false);
            ObjectParameter mensaje = new ObjectParameter("mensaje", "");
            try
            {

                if (user != null)
                {
                    using (var db = new SICOAdminEntities())
                    {
                        db.SP_P_ModificarUsuario(user.userName, user.name, Convert.ToString(user.type), user.active, user.locked, user.password,
                                                user.email, user.daysChangePassword, (byte)user.failesAttempts,
                                                ((User)Session["User"]).userName, DateTime.Now, resultado, mensaje);

                        bool bit = Convert.ToBoolean(resultado.Value);
                        string message = Convert.ToString(mensaje.Value);

                        if (bit)
                        {
                            TempData.Clear();
                            TempData["MessageUp"] = "1" + message;
                            return RedirectToAction(Url.Content("/Index"));
                        }
                        else
                        {
                            TempData.Clear();
                            TempData["MessageUp"] = "0" + message;
                            return RedirectToAction(Url.Content("/Index"));
                        }
                    }
                    //return RedirectToAction(Url.Content("/Index"));
                }
                return View(user);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // ############################################## ASOCIACIONES USUARIO CON PERFIL ##############################################
        [HttpPost]
        public JsonResult agregarUsuarioPerfil(UsuarioPerfil obj)//
        {
            int resp = 0;
            using (var db = new SICOAdminEntities())
            {
                resp = db.SP_P_CrearUsuarioPerfil(obj.Usuario, obj.IdPerfil, obj.UsuarioCreacion);
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult EliminarUsuarioPerfil(UsuarioPerfil obj)//
        {
            int resp = 0;
            using (var db = new SICOAdminEntities())
            {
                resp = db.SP_P_EliminarUsuarioPerfil(obj.Usuario, obj.IdPerfil);
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }




        // ############################################## VISTAS PARCIALES ##############################################
        public PartialViewResult _PerfilesUsuario(Pagina obj)//int id
        {
            //Si no busca viene nulo
            if (obj.palabraBuscar == null) obj.palabraBuscar = "";

            // Validacion si el usuario esta buscado o solo pasando de pagina
            //if (!(obj.estaBuscando))
            //{
                if (obj.accion.Equals('S')) obj.NumPagina += 1; //Enviar al SP
                else if (obj.accion.Equals('N')) obj.NumPagina -= 1;
            //}
            //else ViewBag.PagActual = obj.NumPagina;

            // Restricciones para que no busque paginas que no existe
            if (obj.NumPagina > obj.totalPaginas - 1) obj.NumPagina = Convert.ToInt32(totalPag.Value);
            else if (obj.NumPagina < 0) obj.NumPagina = 0;



            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                lstUsuariosPerfil = db.SP_P_PerfilesDelUsuario(obj.Usuario, obj.NumPagina, obj.CantRegistros, obj.palabraBuscar, totalPag).ToList();
            }





            //Datos a la vista
            ViewBag.PagActual = obj.NumPagina + 1;
            ViewBag.totalPag = totalPag;
            return PartialView("_PerfilesUsuario", lstUsuariosPerfil);
        }



        public PartialViewResult _SelectOpcProfile(string id)//int id
        {
            List<f_opcionesPerfilesUsuario_Result> opcUsuario = null;
            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                opcUsuario = db.f_opcionesPerfilesUsuario(id).ToList();
            }

            List<SelectListItem> ddlUsuarios = opcUsuario.ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Descripcion.ToString(),
                    Value = d.IdPerfil.ToString(),
                    Selected = false
                };
            });


            return PartialView("_SelectOpcProfile", ddlUsuarios);
        }


        public JsonResult GetBitacora(string userName)//NO SE USA AÚN
        {
            User oUser = new User();
            SP_C_MostrarUsuarios_Result user = new SP_C_MostrarUsuarios_Result();
            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                user = db.SP_C_MostrarUsuarios("BIT", userName).FirstOrDefault();

                oUser.lastEntry = user.UltIngreso;
                oUser.lastChangedPassword = (DateTime)user.UltCambioContrasena;
                oUser.userCreation = user.UsuarioCreacion;
                oUser.dateCreation = user.FechaCreacion;
                oUser.userModification = user.UsuarioModificacion;
                oUser.dateModification = (DateTime)user.FechaModificacion;
            }

            var objU = new
            {
                lastEntry = oUser.lastEntry.ToString("dd/MM/yyyy H:mm:ss"),
                lastChangedPassword = oUser.lastChangedPassword.ToString("dd/MM/yyyy H:mm:ss"),
                oUser.userCreation,
                dateCreation = oUser.dateCreation.ToString("dd/MM/yyyy H:mm:ss"),
                oUser.userModification,
                dateModification = oUser.dateModification.ToString("dd/MM/yyyy H:mm:ss"),
            };


            return Json(objU, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CambiarContrasena(string userName, string password, string newPassword)
        {
            ObjectParameter resultado = new ObjectParameter("resultado", false);
            ObjectParameter mensaje = new ObjectParameter("mensaje", "");
            try
            {
                using (SICOAdminEntities db = new SICOAdminEntities())
                {
                    db.SP_P_CambioContrasena(userName, password, newPassword, resultado, mensaje);

                    bool bit = Convert.ToBoolean(resultado.Value);
                    string message = Convert.ToString(mensaje.Value);


                    if (bit)
                    {
                        ViewBag.Alert = message;
                        return Redirect(Url.Content("~/Access/Login"));
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
