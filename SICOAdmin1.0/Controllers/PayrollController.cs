using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Core.Objects;
using SICOAdmin1._0.Models;
using SICOAdmin1._0.Models.Payroll;
using SICOAdmin1._0.Models.User;
using SICOAdmin1._0.Filters;

namespace SICOAdmin1._0.Controllers
{
    public class PayrollController : Controller
    {
        // GET: Payroll
        [AuthorizeUser(pAccion: 29)]
        public ActionResult Index()
        {
            Payroll payroll;
            List<SP_C_MostarNominas_Result> lst = null;
            List<Payroll> lstPayrolls = new List<Payroll>();
            //LstCons son los consecutivos activos
            List<SP_C_ConsecutivosActivos_Result> lstCons = new List<SP_C_ConsecutivosActivos_Result>();
            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                lst = db.SP_C_MostarNominas("tod", 0).ToList();
                lstCons = db.SP_C_ConsecutivosActivos().ToList();
            }

            foreach (var e in lst)
            {
                payroll = new Payroll();
                payroll.IdNomina = e.IdNomina;
                payroll.Descripcion = e.Descripcion;
                if (e.Frecuencia == "SM")
                {
                    payroll.Frecuencia = "Semanal";
                }
                else if (e.Frecuencia == "BSM")
                {
                    payroll.Frecuencia = "Bisemanal";
                }
                else if (e.Frecuencia == "QUIN")
                {
                    payroll.Frecuencia = "Quincenal";
                }
                else if (e.Frecuencia == "MEN")
                {
                    payroll.Frecuencia = "Mensual";
                }
                
                payroll.IdConsecutivo = e.IdConsecutivo;

                lstPayrolls.Add(payroll);
            }

            List<SelectListItem> activeCons = lstCons.ConvertAll(
                 d =>
                 {
                     return new SelectListItem()
                     {
                         Value = d.IdConsecutivo.ToString(),
                         Text = "# " + d.IdConsecutivo.ToString() +" - "+ d.Alias.ToString(),
                         Selected = false
                     };
                 });
            
            ViewBag.lstConsecutivos = activeCons;
            ViewBag.Payrolls = lstPayrolls;
        
            return View();
        }

        [AuthorizeUser(pAccion: 27)]
        [HttpPost]
        public ActionResult Add(Payroll pModel)
        {
            int Response = 0;
            string Message = "";
            ObjectParameter resultSP = new ObjectParameter("resultado", 0);
            ObjectParameter mensage = new ObjectParameter("mensage", 0);
            TempData.Clear();
            try
            {
                using (var db = new SICOAdminEntities())
                {

                    db.SP_P_CrearNomina(pModel.Descripcion, pModel.Frecuencia, pModel.IdConsecutivo,
                        ((User)Session["User"]).userName, resultSP, mensage);

                    Response = Convert.ToInt32(resultSP.Value);
                    Message = Convert.ToString(mensage.Value);
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
                TempData.Keep("Resultado");
                TempData.Keep("Message");
                return RedirectToAction(Url.Content("/Index"));
            }
        }

        [AuthorizeUser(pAccion: 28)]
        [HttpPost]
        public ActionResult Update(Payroll pModel)
        {
            string Message = "";
            int Response = 0;
            ObjectParameter resultSP = new ObjectParameter("resultado", 0);
            ObjectParameter mensajeSP = new ObjectParameter("mensage", 0);
            TempData.Clear();


            
            try {
                using (SICOAdminEntities db = new SICOAdminEntities())
                {
                    
                    db.SP_P_ActualizarNomina(pModel.IdNomina, pModel.Descripcion, pModel.Frecuencia, pModel.IdConsecutivo,
                        ((User)Session["User"]).userName, resultSP, mensajeSP);

                    Response = Convert.ToInt32(resultSP.Value);
                    Message = Convert.ToString(mensajeSP.Value);
                }
                TempData["Message"] = Message;
                TempData["Resultado"] = Response;
                TempData.Keep("Resultado");
                TempData.Keep("Message");
                return RedirectToAction(Url.Content("/Index"));
            }
            catch (Exception ex) {

                TempData["Message"] = Message;
                TempData["Resultado"] = 0;
                TempData.Keep("Resultado");
                TempData.Keep("Message");
                return RedirectToAction(Url.Content("/Index"));
            }            
        }
        public static Payroll GetPayroll(int pId)
        {
            Payroll obj = new Payroll();
            SP_C_MostarNominas_Result Payroll = new SP_C_MostarNominas_Result();
            using (SICOAdminEntities db = new SICOAdminEntities()) {
                Payroll = db.SP_C_MostarNominas("BIT", pId).First();
            }
            obj.IdNomina = Payroll.IdNomina;
            obj.Descripcion = Payroll.Descripcion;
            obj.Frecuencia = Payroll.Frecuencia;
            obj.IdConsecutivo = Payroll.IdConsecutivo;
            
            obj.UsuarioCreacion = Payroll.UsuarioCreacion;
            obj.FechaCreacion = Payroll.FechaCreacion;

            obj.UsuarioModificacion = Payroll.UsuarioModificacion;
            obj.FechaModificacion = Payroll.Fecha_Modificacion;
            return obj;
        }
        public JsonResult pintarDatos(int pId)
        {
            Payroll objPayroll = GetPayroll(pId);
            var obj = new
            {
                objPayroll.IdNomina,
                objPayroll.Descripcion,
                objPayroll.Frecuencia,
                objPayroll.IdConsecutivo,
                objPayroll.UsuarioCreacion,
                objPayroll.UsuarioModificacion,
                FechaCreacion = objPayroll.FechaCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                FechaModificacion = objPayroll.FechaModificacion.ToString("dd/MM/yyyy H:mm:ss")
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}