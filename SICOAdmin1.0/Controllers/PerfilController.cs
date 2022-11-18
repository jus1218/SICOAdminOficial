using SICOAdmin1._0.Filters;
using SICOAdmin1._0.Models;
using SICOAdmin1._0.Models.Perfil;
using SICOAdmin1._0.Models.User;

using SICOAdmin1._0.Models.ViewModels;

using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SICOAdmin1._0.Controllers
{
    public class PerfilController : Controller
    {

        // GET: Perfil
        /*Muestra la Tabla*/
        #region Mostrar
        
        [AuthorizeUser(pAccion: 13)]
        public ActionResult Index()
        {
            List<SP_C_MostrarPerfil_Result> lst = null;
            List<PerfilViewModel> lstModel = new List<PerfilViewModel>();
            PerfilViewModel objM;

            /*---------------------------------------------------------------*/
            /*---------------Procedimiento SP_C_MostrarPerfil----------------*/
            /*---------------------------------------------------------------*/
            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                lst = db.SP_C_MostrarPerfil("tod", null).ToList();

            }
            foreach (var e in lst)
            {
                objM = new PerfilViewModel();
                objM.IdPerfil = e.IdPerfil;
                objM.Nombre = e.Nombre;
                objM.Descripcion = e.Descripcion;
                objM.Activo = e.Activo;
                objM.UsuarioModificacion = e.UsuarioModificacion;
                lstModel.Add(objM);
            }
            ViewBag.Perfil = lstModel;
            return View();
        }
        #endregion

        /*Crea un nuevo Perfil con un modal*/
        #region Nuevo
        /*---------------------------------------------------------------*/
        /*-------------------Procedimiento SP_P_AgregarPerfil------------*/
        /*---------------------------------------------------------------*/
        [HttpPost]
        public ActionResult Nuevo(PerfilViewModel model)
        {
           
            int Response = -3;
            ObjectParameter resultSP = new ObjectParameter("resultado", -3);
            TempData.Clear();

            try
            {
                if (!ModelState.IsValid)
                {

                    using (var db = new SICOAdminEntities())
                    {
                        int num = db.SP_P_CrearPerfil(model.Nombre, model.Descripcion, model.Activo, ((User)Session["User"]).userName, ((User)Session["User"]).userName, resultSP);
                        Response = Convert.ToInt32(resultSP.Value);

                    }

                    TempData["Resultado"] = Response;
                    TempData.Keep("Resultado");
                    return Redirect(Url.Content("~/Perfil/Index"));


                }
                return Redirect(Url.Content("~/Perfil/Index"));
            }
            catch (Exception ex)
            {

                TempData["Resultado"] = 0;
                return Redirect(Url.Content("~/Perfil/Index"));
            }
        }


        #endregion

        /*Edita el perfil creado*/
        #region Editar
        /*---------------------------------------------------------------*/
        /*------------------Procedimiento SP_P_Editar--------------------*/
        /*---------------------------------------------------------------*/
        [AuthorizeUser(pAccion: 12)]
        public ActionResult Editar(int Id)
        {
            List<SP_P_UsuariosDelPerfil_Result> lstUsuariosPerfil = new List<SP_P_UsuariosDelPerfil_Result>();
            PerfilViewModel model = new PerfilViewModel();
            List<f_opcionesUsuariosPerfil_Result> opcUsuario = null;
            


            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                var oPerfil = db.PERFIL.Find(Id); //buscar perfil
                model.IdPerfil = oPerfil.IdPerfil;
                model.Nombre = oPerfil.Nombre;
                model.Descripcion = oPerfil.Descripcion;
                model.Activo = oPerfil.Activo;
                model.UsuarioModificacion = oPerfil.UsuarioModificacion;

                lstUsuariosPerfil = db.SP_P_UsuariosDelPerfil(Id).ToList();
                opcUsuario = db.f_opcionesUsuariosPerfil(Id).ToList();
            }


            List<SelectListItem> ddlUsuarios = opcUsuario.ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Nombre.ToString(),
                    Value = d.Usuario.ToString(),
                    Selected = false
                };
            });


            ViewBag.lstUser = ddlUsuarios;


            ViewBag.lstUsuariosPerfil = lstUsuariosPerfil;

            return View(model);
        }
        [HttpPost]
        public ActionResult Editar(PerfilViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int Response = -3;
                    ObjectParameter resultSP = new ObjectParameter("resultado", -3);
                    TempData.Clear();
                    using (SICOAdminEntities db = new SICOAdminEntities())
                    {
                        var oPerfil = db.PERFIL.Find(model.IdPerfil);
                        db.SP_P_ModificarPerfil(model.IdPerfil, model.Nombre, model.Descripcion, model.Activo, ((User)Session["User"]).userName, resultSP);
                        db.SaveChanges();

                    }
                    TempData["Resultado"] = Response;
                    TempData.Keep("Resultado");
                    return Redirect("~/Perfil/Index");
                }
                else
                    return View(model);
            }
            catch (Exception ex)
            {
                TempData["Resultado"] = 0;
                return Redirect(Url.Content("~/Perfil/Index"));
            }
        }

        #endregion

        /*Activa o inactiva el estado del perfil*/
        #region EstadoPerfil
        public ActionResult ModificarEstadoPerfil(int Id)
        {

            int Response = -3;
            ObjectParameter resultadoSP = new ObjectParameter("resultado", 0);

            {
                using (SICOAdminEntities db = new SICOAdminEntities())
                {
                    db.SP_P_ModificarEstadoPerfil(Id, resultadoSP);
                    Response = Convert.ToInt32(resultadoSP.Value);

                }
                return Json(Response, JsonRequestBehavior.AllowGet);
            }


        }

        #endregion

        /*Agrega los Usuarios a los Perfiles, y muestra que Usuarios que pertenece a cada Perfil*/
        #region PerfilUsuario
        /*----------------------Procedimiento SP_C_MostrarUsuarioPerfil---------*/
        public PartialViewResult _UsuariosPerfil(int id)//int id
        {
            List<SP_P_UsuariosDelPerfil_Result> lstUsuariosPerfil = new List<SP_P_UsuariosDelPerfil_Result>();
            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                lstUsuariosPerfil = db.SP_P_UsuariosDelPerfil(id).ToList();
            }
            return PartialView("_UsuariosPerfil", lstUsuariosPerfil);
        }
        /*----------------------Meuestra Opciones de Usuario---------*/
        public PartialViewResult _SelectOpcUser(int id)//int id
        {
            List<f_opcionesUsuariosPerfil_Result> opcUsuario = null;
            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                opcUsuario = db.f_opcionesUsuariosPerfil(id).ToList();
            }

            List<SelectListItem> ddlUsuarios = opcUsuario.ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = d.Nombre.ToString(),
                    Value = d.Usuario.ToString(),
                    Selected = false
                };
            });


            return PartialView("_SelectOpcUser", ddlUsuarios);
        }
        /*----------------------Agrega un Usuario a Perfil---------*/
        [HttpPost]
        public JsonResult agregarUsuarioPerfil(UsuarioPerfil obj)//
        {
            int resp = 0;
            using (var db = new SICOAdminEntities()){
                resp = db.SP_P_CrearUsuarioPerfil(obj.Usuario, obj.IdPerfil, ((User)Session["User"]).userName);
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }
        /*----------------------Elimina un Usuario a Perfil---------*/
        [HttpPost]
        public JsonResult eliminarUsuarioPerfil(UsuarioPerfil obj)//
        {
            int resp = 0;
            using (var db = new SICOAdminEntities())
            {
                resp = db.SP_P_EliminarUsuarioPerfil(obj.Usuario, obj.IdPerfil);
            }

            return Json(resp, JsonRequestBehavior.AllowGet);
        }
        #endregion

        /*Muestra el arbol de previlegios para cada Perfil*/
        #region Previlegios
        public JsonResult Get(string query, int id)
        {
            List<SP_P_DibujarArbol_Result> Arbols;
            List<Models.Perfil.ArbolPrevilegios> records;
            using (var context = new SICOAdminEntities())
            {
                Arbols = context.SP_P_DibujarArbol(id).ToList();

                if (!string.IsNullOrWhiteSpace(query))
                {
                    Arbols = Arbols.Where(q => q.Descripcion.Contains(query)).ToList();
                }

                records = Arbols.Where(l => l.Padre == 0).OrderBy(l => l.NumeroHermano)          /*Aqui tambien estuvo*/
                    .Select(l => new ArbolPrevilegios
                    {
                        idAccion = l.IdAccion,
                        @checked = (bool)l.check,
                        text = l.Descripcion,
                        children = GetChildren(Arbols, l.IdAccion)
                    }).ToList();
            }

            return this.Json(records, JsonRequestBehavior.AllowGet);

        }

        private List<Models.Perfil.ArbolPrevilegios> GetChildren(List<SP_P_DibujarArbol_Result> Arbols, int parentId)               /*Estuvo aqui*/
        {
            return Arbols.Where(l => l.Padre == parentId).OrderBy(l => l.NumeroHermano)
                .Select(l => new Models.Perfil.ArbolPrevilegios
                {
                    idAccion = l.IdAccion,
                    @checked = (bool)l.check,
                    text = l.Descripcion,
                    children = GetChildren(Arbols, l.IdAccion)
                }).ToList();
        }

        [AuthorizeUser(pAccion: 31)]
        public JsonResult SaveCheckedNodes(List<int> checkedIds, int idPerfil)         /*guardo aqui*/
        {
            bool seLogro = false;
            string idCheck = "";

            string UsuCreacion = ((User)Session["User"]).userName;
            if (checkedIds == null)
            {
                checkedIds = new List<int>();
            }

            try
            {
                using (var context = new SICOAdminEntities())
                {
                    var Arbols = context.SP_P_DibujarArbol(idPerfil).ToList();
                    context.SP_P_EliminarAll_AccionPerfil(idPerfil);
                    foreach (var arbol in Arbols)
                    {
                        arbol.check = checkedIds.Contains(arbol.IdAccion);
                        if (arbol.check == true)
                        {
                            idCheck += arbol.IdAccion.ToString()+",";
                            //context.SP_P_GuardarCheck(arbol.IdAccion, idPerfil, UsuCreacion);

                        }
                    }


                    idCheck = idCheck.Remove(idCheck.Length - 1);//Quitamos el ultimo caracter  ","
                    context.SP_P_GuardarCheck(idCheck, idPerfil, UsuCreacion);


                }
                seLogro = true;

            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }




            return this.Json(seLogro);
        }

        #endregion

        /*Muestra la Tabla de Perfil con todos los atributos*/
        #region Bitacora
        public JsonResult DatosBitacora(int? IdPer)
        {
            PERFIL model = new PERFIL();
            SP_C_MostrarPerfil_Result perf = new SP_C_MostrarPerfil_Result();
            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                perf = db.SP_C_MostrarPerfil("BIT", IdPer).FirstOrDefault();

                model.UsuarioCreacion = perf.UsuarioCreacion;
                model.FechaCreacion = perf.FechaCreacion;
                model.UsuarioModificacion = perf.UsuarioModificacion;
                model.FechaModificacion = (DateTime)perf.FechaModificacion;

                
            }
            var objP = new
            {
               model.UsuarioCreacion,
               FechaCreacion = model.FechaCreacion.ToString("dd / MM / yyyy H: mm:ss"),
               model.UsuarioModificacion,
               FechaModificacion = model.FechaModificacion.ToString("dd / MM / yyyy H: mm:ss"),

             };

                return Json(objP, JsonRequestBehavior.AllowGet);
        }

    }

        #endregion
}
