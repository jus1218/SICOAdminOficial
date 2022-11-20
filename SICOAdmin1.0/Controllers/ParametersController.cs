using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Core.Objects;
using SICOAdmin1._0.Models;
using SICOAdmin1._0.Models.Parameter;
using SICOAdmin1._0.Models.User;
using System.Globalization;
using SICOAdmin1._0.Filters;

namespace SICOAdmin1._0.Controllers
{
    public class ParametersController : Controller
    {
        // GET: Parameters
        [AuthorizeUser(pAccion: 22)]
        public ActionResult Index()
        {

            List<SP_C_MostrarParametros_Result> lst = null;
            List<Parameter> lstParameters = new List<Parameter>();
            Parameter parameter;
            using (var db = new SICOAdminEntities())
            {
                lst = db.SP_C_MostrarParametros("tod", 0).ToList();
            }

            foreach (var e in lst)
            {

                parameter = new Parameter();
                parameter.IdParameter = e.IdParametro;
                parameter.MonthlyPayment = Convert.ToString(e.Mensualidad);
                parameter.IdConsecRecibo = e.IdConsecRecibo;
                parameter.InterestBlackberry = Convert.ToString(e.InteresMora);
                parameter.DocumentTypePayment = e.TipoDocumentoCobro;
                parameter.PaymentCondition = e.CondicionPago;
                parameter.Status = e.Estado;

                lstParameters.Add(parameter);
            }
            ViewBag.Parameters = lstParameters;
            return View();
        }
        [AuthorizeUser(pAccion: 20)]
        [HttpPost]

        public ActionResult Add(Parameter pModel)
        {
            int Response = 0;
            string Message = "";
            ObjectParameter resultSP = new ObjectParameter("resultado", 0);
            ObjectParameter mensajeSP = new ObjectParameter("mensaje", 0);
            TempData.Clear();


            CultureInfo culture = new CultureInfo("en-US");
            decimal mora = Convert.ToDecimal(pModel.InterestBlackberry, culture);
            decimal mensualidad = Convert.ToDecimal(pModel.MonthlyPayment, culture);

            

            //Ambas maneras funcionan decimal.Parse o Convert.ToDecimal 
            //decimal mora = decimal.Parse(pModel.InteresMora, culture);
            //decimal mensualidad = decimal.Parse(pModel.Mensualidad, culture);

            try
            {
                using (var DB = new SICOAdminEntities())
                {
                    DB.SP_P_CrearParametro(mensualidad, pModel.IdConsecRecibo, mora, pModel.DocumentTypePayment, pModel.PaymentCondition, pModel.Status,
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
                TempData["Message"] = "Error al Ingresar";
                TempData["Resultado"] = 0;
                return RedirectToAction(Url.Content("/Index"));
            }
        }
        [AuthorizeUser(pAccion: 21)]
        [HttpPost]
        public ActionResult Update(Parameter pModel) {
            int Response = 0;
            string Message = "";
            ObjectParameter resultSP = new ObjectParameter("resultado", 0);
            ObjectParameter mensajeSP = new ObjectParameter("mensaje", 0);
            TempData.Clear();
            
            CultureInfo culture = new CultureInfo("en-US");
            decimal mora = Convert.ToDecimal(pModel.InterestBlackberry, culture);
            decimal mensualidad = Convert.ToDecimal(pModel.MonthlyPayment, culture);
            try {
                using (var DB = new SICOAdminEntities())
                {
                    DB.SP_P_ActualizarParametro(pModel.IdParameter, mensualidad, pModel.IdConsecRecibo, mora,
                        pModel.DocumentTypePayment, pModel.PaymentCondition, pModel.Status, ((User)Session["User"]).userName, resultSP, mensajeSP);
                    Response = Convert.ToInt32(resultSP.Value);
                    Message = Convert.ToString(mensajeSP.Value);
                }
                TempData["Resultado"] = Response;
                TempData["Message"] = Message;
                TempData.Keep("Resultado");
                TempData.Keep("Message");
                return RedirectToAction(Url.Content("/Index"));
            }
            catch (Exception ex) {

                TempData["Message"] = "Error al Actualizar";
                TempData["Resultado"] = 0;
                return RedirectToAction(Url.Content("/Index"));
            }
                            
        }

        public JsonResult showData(int idParam)
        {
            CultureInfo culture = new CultureInfo("en-US");
            Parameter param = GetParameter(idParam);
            var obj = new
            {
                IdParametro = param.IdParameter,
                Mensualidad = Convert.ToDecimal(param.MonthlyPayment),
                InteresMora = Convert.ToDecimal(param.InterestBlackberry),
                param.IdConsecRecibo,
                TipoDocumentoCobro=  param.DocumentTypePayment,
                CondicionPago= param.PaymentCondition,
                Estado = param.Status,
                UsuarioCreacion= param.UserCreacion,
               UsuarioModificacion =param.UserModification,
                FechaCreacion = param.DateCreacion.ToString("dd/MM/yyyy H:mm:ss"),
                FechaModificacion = param.DateModification.ToString("dd/MM/yyyy H:mm:ss")
            };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public Parameter GetParameter(int pParam)
        {
            Parameter param = new Parameter();
            SP_C_MostrarParametros_Result objResult = new SP_C_MostrarParametros_Result();

            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                objResult = db.SP_C_MostrarParametros("BIT", pParam).First();
            }
            param.IdParameter= objResult.IdParametro;
            param.MonthlyPayment = Convert.ToString( objResult.Mensualidad);//.ToString("C");
            param.IdConsecRecibo = objResult.IdConsecRecibo;
            param.InterestBlackberry = objResult.InteresMora.ToString(".00");
            param.DocumentTypePayment = objResult.TipoDocumentoCobro;
            param.PaymentCondition = objResult.CondicionPago;
            param.Status = objResult.Estado;
            param.UserCreacion = objResult.UsuarioCreacion;
            param.UserModification = objResult.UsuarioModificacion;
            param.DateCreacion = objResult.FechaCreacion;
            param.DateModification = objResult.FechaModificacion;
            return param;
        }
        public JsonResult changeStatus(int idParam)
        {
            int Response = -3;
            ObjectParameter resultSP = new ObjectParameter("resultado", 0);
            using (var db = new SICOAdminEntities())
            {
                db.SP_P_ModificarEstadoParametro(idParam, resultSP);
                Response = Convert.ToInt32(resultSP.Value);

            }


            return Json(Response, JsonRequestBehavior.AllowGet);
        }

    }
}