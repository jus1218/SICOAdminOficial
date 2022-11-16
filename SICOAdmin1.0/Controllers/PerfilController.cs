using SICOAdmin1._0.Filters;
using SICOAdmin1._0.Models;
using SICOAdmin1._0.Models.Perfil;
using SICOAdmin1._0.Models.User;
using SICOAdmin1._0.Models.ViewModels;
using System;
using System.Collections.Generic;
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
        //[AuthorizeUser(pAccion: 10)]// cambiar
        [AuthorizeUser(pAccion: 13)]
        public ActionResult Index()
        {
            List<SP_C_MostrarPerfil_Result> lst = null;
            List<PERFIL> lstModel = new List<PERFIL>();
            PERFIL objM;
            /*---------------------------------------------------------------*/
            /*---------------Procedimiento SP_C_MostrarPerfil----------------*/
            /*---------------------------------------------------------------*/
            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                lst = db.SP_C_MostrarPerfil().ToList();

            }
            foreach (var e in lst)
            {
                objM = new PERFIL();
                objM.IdPerfil = e.IdPerfil;
                objM.Nombre = e.Nombre;
                objM.Descripcion = e.Descripcion;
                objM.Activo = e.Activo;
                objM.FechaCreacion = e.FechaCreacion;
                objM.UsuarioCreacion = e.UsuarioCreacion;
                objM.FechaModificacion = e.FechaModificacion;
                objM.UsuarioModificacion = e.UsuarioModificacion;
                lstModel.Add(objM);
            }

            return View(lstModel);

        }
        #endregion

        /*Crea un nuevo Perfil con un modal*/
        #region Nuevo
        /*---------------------------------------------------------------*/
        /*-------------------Procedimiento SP_P_AgregarPerfil------------*/
        /*---------------------------------------------------------------*/
        public ActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Nuevo(PERFIL model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    using (var db = new SICOAdminEntities())
                    {
                        //int num = db.SP_P_CrearPerfil(model.Nombre, model.Descripcion, model.Activo, model.UsuarioCreacion, model.UsuarioModificacion);
                    }

                    return Redirect(Url.Content("~/Perfil/Index"));
                }
                return View(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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
        #endregion

        /*Activa o inactiva el estado del perfil*/
        #region EstadoPerfil
        [HttpPost]
        public ActionResult ModificarEstadoPerfil(int Id)
        {
            try
            {
                using (SICOAdminEntities db = new SICOAdminEntities())
                {
                    //db.SP_P_ModificarEstadoPerfil(Id);
                    db.SaveChanges();
                }
                return Content("1");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
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

            string UsuCreacion = ((User)Session["User"]).userName;
            if (checkedIds == null)
            {
                checkedIds = new List<int>();
            }
            using (var context = new SICOAdminEntities())
            {
                var Arbols = context.SP_P_DibujarArbol(idPerfil).ToList();
                context.SP_P_EliminarAll_AccionPerfil(idPerfil);
                foreach (var arbol in Arbols)
                {
                    arbol.check = checkedIds.Contains(arbol.IdAccion);
                    if (arbol.check == true)
                    {
                        context.SP_P_GuardarCheck(arbol.IdAccion, idPerfil, UsuCreacion);

                    }
                }

            }

            return this.Json(true);
        }

        #endregion

        /*Muestra la Tabla de Perfil con todos los atributos*/
        #region Bitacora
        #endregion
    }
}