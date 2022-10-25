using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Core.Objects;
using SICOAdmin1._0.Models;
using SICOAdmin1._0.Models.Consecutivo;
using SICOAdmin1._0.Models.User;

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


        [HttpPost]
        public ActionResult Add(Consecutivo model)
        {
            //model.UsuarioCreacion = "Daniel";
            int Response = 0;
            string Message = "";
            ObjectParameter resultSP = new ObjectParameter("resultado", 0);
            ObjectParameter mensajeSP = new ObjectParameter("mensage", 0);
            TempData.Clear();
            if (!ModelState.IsValid)
            {               
                 return RedirectToAction(Url.Content("/Index"));
            }
            try
            {
                using (var DB = new SICOAdminEntities())
                {
                    DB.SP_P_CrearConsecutivo(model.Alias, model.Mascara, model.ProximoValor, model.Activo,
                                  ((User)Session["User"]).userName, resultSP, mensajeSP);
                    Response = Convert.ToInt32(resultSP.Value);
                    Message = Convert.ToString(mensajeSP.Value);
                }

                TempData["Resultado"] = Response;
                TempData["Message"] = Message;
                TempData.Keep("Resultado");
                TempData.Keep("Message");
                
                 return RedirectToAction(Url.Content("/Index"));             
            }
            catch (Exception ex)
            {
                TempData["Message"] = Message;
                TempData["Resultado"] = 0;                
                return RedirectToAction(Url.Content("/Index"));
               
            }
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



        [HttpPost]
        public ActionResult Update(Consecutivo pModel)
        {
            int Response = 0;
            string Message = "";
            TempData.Clear();
            bool verificarDatos = false;
            Consecutivo model = null;
            ObjectParameter resultSP = new ObjectParameter("resultado", 0);
            ObjectParameter mensajeSP = new ObjectParameter("mensaje", 0);
            if (pModel.IdConsecutivo == 0)
            {
                return Redirect(Url.Content("/Index"));
            }      
            try
            {
                model = getConsecutivo(pModel.IdConsecutivo);
                if (model != null && model != pModel)
                {
                    using (SICOAdminEntities db = new SICOAdminEntities())
                    {
                        db.SP_P_ActualizarConsecutivo(pModel.IdConsecutivo, pModel.Alias, pModel.Mascara, pModel.ProximoValor, pModel.Activo, 
                                  ((User)Session["User"]).userName, resultSP, mensajeSP);
                        Response = Convert.ToInt32(resultSP.Value);
                        Message = Convert.ToString(mensajeSP.Value);
                    }
                }
                TempData["Message"] = Message;
                TempData["Resultado"] = Response;
                TempData.Keep("Resultado");
                TempData.Keep("Message");
                return RedirectToAction(Url.Content("/Index"));
            }
            catch (Exception ex)
            {
                TempData["Message"] = Message;
                TempData["Resultado"] = 0;
                return RedirectToAction(Url.Content("/Index"));
            }
        }


        public JsonResult ModificarEstado(int idCons)
        {
            int Response = -3;
            //TempData.Clear();
            ObjectParameter resultadoSP = new ObjectParameter("resultado", 0);

            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                db.SP_P_ModificarEstadoConsecutivo(idCons, resultadoSP);
                Response = Convert.ToInt32(resultadoSP.Value);
            }
            return Json(Response, JsonRequestBehavior.AllowGet);
        }       
        public JsonResult pintarDatos(int idCons)
        {
            Consecutivo model = getConsecutivo(idCons);
            var obj = new
            {
                model.IdConsecutivo,
                model.Alias,
                model.Mascara,
                model.ProximoValor,
                model.Activo,
                model.UsuarioCreacion,
                model.UsuarioModificacion,
                FechaCreacion = model.FechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                FechaModificacion = model.FechaModificacion.ToString("dd/MM/yyyy H:mm:ss")
            };

            return Json(obj, JsonRequestBehavior.AllowGet);
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