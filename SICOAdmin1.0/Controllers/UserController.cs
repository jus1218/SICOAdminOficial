
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

                ViewBag.bitacora = objM;
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
            try
            {
                if (!ModelState.IsValid)
                {
                    using (var db = new SICOAdminEntities())
                    {
                        //int num = db.SP_P_AddUser(model.userName, model.name, Convert.ToString(model.type), model.active, model.locked,
                        //          model.password, model.email, model.daysChangePassword, (byte)model.failesAttempts, ((User)Session["User"]).userName, ((User)Session["User"]).userName);
                    }

                    return Redirect(Url.Content("~/User/Index"));
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
            try
            {
                if (user != null)
                {
                    using (var db = new SICOAdminEntities())
                    {
                        //db.SP_P_ModificarUsuario(user.userName, user.name, Convert.ToString(user.type), user.active, user.locked,
                        //                        user.password, user.email, user.daysChangePassword, (byte)user.failesAttempts, ((User)Session["User"]).userName, DateTime.Now);
                    }
                    return Redirect(Url.Content("~/User/Index"));
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
            if (!(obj.estaBuscando))
            {
                if (obj.accion.Equals('S')) obj.NumPagina += 1; //Enviar al SP
                else obj.NumPagina -= 1;
            }
            else ViewBag.PagActual = obj.NumPagina;

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



        //[HttpPost]// super mega importante
        //public ActionResult TableUserJson(string userName)
        //{
        //    lstPerfiles = null;
        //    //logistica datatable
        //    var draw = Request.Form.GetValues("draw").FirstOrDefault();
        //    var start = Request.Form.GetValues("start").FirstOrDefault();
        //    var length = Request.Form.GetValues("length").FirstOrDefault();
        //    var sortColumn = Request.Form.GetValues("columns["+ Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
        //    var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
        //    var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


        //    pageSize = length != null ? Convert.ToInt32(length) : 0;
        //    skip = start != null ? Convert.ToInt32(start) : 0;// en que pagina va lo demas lo ignora
        //    recordsTotal = 0;



        //    using (SICOAdminEntities db = new SICOAdminEntities())
        //    {
        //        lstPerfiles = db.SP_P_PerfilesDelUsuario(userName).ToList();
        //        recordsTotal = lstPerfiles.Count();
        //        lstPerfiles = lstPerfiles.Skip(skip).Take(pageSize).ToList();
        //    }


        //    return Json(new { draw = draw,recordsFiltered = recordsTotal,recordsTotal=recordsTotal,data = lstPerfiles });
        //}






        public void Bitacora(string userName)
        {
            User oUser = new User();
            List<User> bitacora = null;
            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                SP_C_BuscarUsuario_Result user = db.SP_C_BuscarUsuario(userName).FirstOrDefault();

                //oUser.userName = user.Usuario;
                //oUser.name = user.Nombre;
                //switch (user.Tipo.ToString())
                //{
                //    case "Ad":
                //        oUser.type = TypesU.Administrador;
                //        break;
                //    case "Co":
                //        oUser.type = TypesU.Consulta;
                //        break;
                //    case "Tr":
                //        oUser.type = TypesU.Transaccional;
                //        break;
                //}
                //oUser.active = user.Activo;
                //oUser.locked = user.Bloqueado;
                //oUser.password = user.Contrasena;
                //oUser.email = user.CorreoElectronico;
                //oUser.daysChangePassword = user.DiasCambioContrasena;
                //oUser.failesAttempts = user.IntentosFallidos;
                oUser.lastChangedPassword = (DateTime)user.UltCambioContrasena;
                oUser.lastEntry = user.UltIngreso;
                oUser.userCreation = user.UsuarioCreacion;
                oUser.dateCreation = user.FechaCreacion;
                oUser.userModification = user.UsuarioModificacion;
                oUser.dateModification = (DateTime)user.FechaModificacion;
            }

            ViewBag.bitacora = oUser;
            //return View(bitacora);
        }
    }
}
