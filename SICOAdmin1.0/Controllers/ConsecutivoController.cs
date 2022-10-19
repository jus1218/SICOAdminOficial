using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Core.Objects;
using SICOAdmin1._0.Models;
using SICOAdmin1._0.Models.Consecutivo;
namespace SICOAdmin1._0.Controllers
{
    public class ConsecutivoController : Controller
    {
        // GET: Consecutivo
        public ActionResult Index()
        {
            List<SP_C_MostrarConsecutivos_Result> lst = null;

            List<Consecutivo> lstConsecutivo = new List<Consecutivo>();
            Consecutivo objM;
            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                lst = db.SP_C_MostrarConsecutivos().ToList();

            }

            foreach (var e in lst)
            {
                objM = new Consecutivo();
                objM.IdConsecutivo = e.IdConsecutivo;
                objM.Alias = e.Alias;
                objM.Mascara = e.Mascara;
                objM.ProximoValor = e.ProximoValor;
                objM.Activo = e.Activo;
                objM.UsuarioCreacion = e.UsuarioCreacion;
                objM.UsuarioModificacion = e.UsuarioModificacion;
                objM.FechaCreacion = e.FechaCreacion;
                objM.FechaModificacion = e.FechaModificacion;

                lstConsecutivo.Add(objM);

            }
            ViewBag.Consecutivos = lstConsecutivo;

            return View();
        }


        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Consecutivo model)
        {

            model.UsuarioCreacion = "CarlosDaniel";

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                using (var DB = new SICOAdminEntities())
                {
                   // int Response = DB.SP_P_CrearConsecutivo(model.Alias, model.Mascara, model.ProximoValor, model.Activo, model.UsuarioCreacion);

                }
                return Redirect(Url.Content("/Consecutivo/Index"));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }


        }


        [HttpGet]
        public ActionResult Show()
        {

            List<Consecutivo> lstModel = getListConsecutivos();
            ViewBag.Consecutivos = lstModel;
            return View();
        }
        public static Consecutivo getConsecutivo(int pId)
        {

            Consecutivo ObjConsecutivo = new Consecutivo();
            SP_C_BuscarConsecutivo_Result ObjResult = new SP_C_BuscarConsecutivo_Result();

            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                ObjResult = db.SP_C_BuscarConsecutivo(pId).First();
            }
            ObjConsecutivo.IdConsecutivo = ObjResult.IdConsecutivo;
            ObjConsecutivo.Alias = ObjResult.Alias;
            ObjConsecutivo.Mascara = ObjResult.Mascara; 
            ObjConsecutivo.ProximoValor = ObjResult.ProximoValor;
            ObjConsecutivo.Activo = ObjResult.Activo;

            ObjConsecutivo.UsuarioCreacion = ObjResult.UsuarioCreacion;

            ObjConsecutivo.UsuarioModificacion = ObjResult.UsuarioModificacion;

            ObjConsecutivo.FechaCreacion = ObjResult.FechaCreacion;

            ObjConsecutivo.FechaModificacion = ObjResult.FechaModificacion;


            return ObjConsecutivo;
        }


        [HttpGet]
        public ActionResult Update()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Update(Consecutivo pModel)
        {
            int Response = 0;
            Consecutivo model = null;
            if (pModel.IdConsecutivo == 0)
            {
                return Redirect(Url.Content("/Consecutivo/Index"));
            }
            else
            {
                model = getConsecutivo(pModel.IdConsecutivo);
                pModel.UsuarioModificacion = "CarlosDaniel";
            }
            try
            {
                if (model != null)
                {
                    using (SICOAdminEntities db = new SICOAdminEntities())
                    {
                        //Response = db.SP_P_ActulizarConsecutivo(pModel.IdConsecutivo, pModel.Alias, pModel.Mascara, pModel.ProximoValor, pModel.Activo, pModel.UsuarioModificacion);
                    }
                    return Redirect(Url.Content("/Consecutivo/Index"));
                }
                else
                {
                    return View(pModel);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public JsonResult pintarDatos(int idCons)
        {
            Consecutivo model = getConsecutivo(idCons);           

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public List<Consecutivo> getListConsecutivos()
        {
            List<SP_C_MostrarConsecutivos_Result> lst = null;

            List<Consecutivo> lstModel = new List<Consecutivo>();
            Consecutivo objM;
            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                lst = db.SP_C_MostrarConsecutivos().ToList();
            }

            foreach (var e in lst)
            {
                objM = new Consecutivo();
                objM.IdConsecutivo = e.IdConsecutivo;
                objM.Alias = e.Alias;
                objM.Mascara = e.Mascara;
                objM.ProximoValor = e.ProximoValor;
                objM.Activo = e.Activo;
                objM.UsuarioCreacion = e.UsuarioCreacion;
                objM.UsuarioModificacion = e.UsuarioModificacion;
                objM.FechaCreacion = e.FechaCreacion;
                objM.FechaModificacion = e.FechaModificacion;

                lstModel.Add(objM);
            }
            return lstModel;
        }
    }
}