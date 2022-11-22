using SICOAdmin1._0.Models;
using System.Data.Entity.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SICOAdmin1._0.Controllers
{
    public class JobController : Controller
    {
     //------------------------------------------------------------   /*Variables*/
     #region variables
        List<SP_C_MostrarPuestos_Result> lstJobs = new List<SP_C_MostrarPuestos_Result>();
        List<Job> lsNew = new List<Job>();

        ObjectParameter msj = new ObjectParameter("msj", "");

        #endregion
     //------------------------------------------------------------   /*Vistas*/ 
     #region Vistas
        public ActionResult Index()
        {
            lstJobs.Clear();

            try
            {
                using (var db = new SICOAdminEntities())
                {
                    lstJobs = db.SP_C_MostrarPuestos().ToList();
                }
                List<Job> ddlJobs = lstJobs.ConvertAll(d => {

                    return new Job()
                    {
                        IdPuesto = d.IdPuesto,
                        Descripcion = d.Descripcion,
                        SalarioMinimo = ((int)d.SalarioMinimo),
                        SalarioPromedio = ((int)d.SalarioPromedio),
                        SalarioMaximo = ((int)d.SalarioMaximo),
                        CodigoCCSS = d.CodigoCCSS,
                        CodigoINS = d.CodigoINS
                    };

                });

                TempData["lstJobs"] = ddlJobs;

            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return View();
        }
        #endregion

     //------------------------------------------------------------   /*Retornos Json*/
     #region Nuevo
        [HttpPost]
        public ActionResult Nuevo(Job model)
        {
            model.UsuarioModificacion = "admin";
            int Response = 0;
            ObjectParameter resultSP = new ObjectParameter("resultado", 0);
            TempData.Clear();

            try
            {
                if (!ModelState.IsValid)
                {

                    using (var db = new SICOAdminEntities())
                    {
                        int num = db.SP_P_CrearPuesto(
                                                      model.Descripcion,
                                                      model.SalarioMinimo,
                                                      model.SalarioPromedio,
                                                      model.SalarioMaximo,
                                                      model.CodigoCCSS,
                                                      model.CodigoINS,
                                                      model.UsuarioModificacion,
                                                      model.UsuarioModificacion,
                                                      resultSP);
                        db.SaveChanges();
                        Response = Convert.ToInt32(resultSP.Value);
                    }
                    TempData["Resultado"] = Response;
                    TempData.Keep("Resultado");
                    return Redirect(Url.Content("~/Puesto/Index"));
                }
                return Redirect(Url.Content("~/Puesto/Index"));
            }
            catch (Exception ex)
            {
                TempData["Resultado"] = 0;
                return Redirect(Url.Content("~/Puesto/Index"));
            }
        }

        #endregion

     //------------------------------------------------------------   /*Vistas parciales*/
     #region VistasPaciales
        public PartialViewResult _TableJob()
        {
            using (var db = new SICOAdminEntities())
            {
                lstJobs = db.SP_C_MostrarPuestos().ToList();
            }
            List<Job> ddlJobs = lstJobs.ConvertAll(d => {
                return new Job()
                {
                    IdPuesto = d.IdPuesto,
                    Descripcion = d.Descripcion,
                    SalarioMinimo = d.SalarioMinimo,
                    SalarioPromedio = d.SalarioPromedio,
                    SalarioMaximo = d.SalarioMaximo,
                    CodigoCCSS = d.CodigoCCSS,
                    CodigoINS = d.CodigoINS
                };

            });
            TempData["lstJobs"] = ddlJobs;


            return PartialView("__TableJob", ddlJobs);


        }
        #endregion


        #region Json
        //public JsonResult AgregarJob(JOB obj)//
        //{
        //    int resp = 0;

        //    Boolean HorasCoincide = obj.FechaHoraIngreso == null ? true : false;

        //    using (var db = new SICOAdminEntities())
        //    {
        //        resp = db.SP_P_CrearControlAsistencia(obj.IdColaborador, obj.FechaHoraIngreso, obj.FechaHoraSalida,
        //            obj.TipoJornada, obj.HorasRegulares, obj.HorasExtras, obj.HoraDobles, obj.HorasExtrasDobles, ((User)Session["User"]).userName, msj);
        //    }

        //    return Json(new
        //    {
        //        resp = resp,
        //        mensaje = msj.Value
        //    }, JsonRequestBehavior.AllowGet);
        //}

        #endregion





    }
}