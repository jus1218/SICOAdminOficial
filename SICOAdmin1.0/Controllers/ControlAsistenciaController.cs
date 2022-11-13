using SICOAdmin1._0.Models;
using SICOAdmin1._0.Models.User;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SICOAdmin1._0.Controllers
{
    public class ControlAsistenciaController : Controller
    {
        // ======================================= VARIABLES =======================================
        List<SP_C_MostrarControlAsistencia_Result> lstControlAsist = new List<SP_C_MostrarControlAsistencia_Result>();
        SP_C_BuscarControlAsistencia_Result lstControlAsistBusc = new SP_C_BuscarControlAsistencia_Result();
        List<ControlAsistencia> lstNew = new List<ControlAsistencia>();


        List<SP_C_MostrarColaboradores_Result> lstColaboradores = new List<SP_C_MostrarColaboradores_Result>();
        ObjectParameter msj = new ObjectParameter("msj", "");
        // GET: ControlAsistencia


        // ======================================= VISTAS =======================================
        public ActionResult Index()
        {
            lstControlAsist.Clear();
            lstColaboradores.Clear();

            try
            {

                using (var db = new SICOAdminEntities())
                {
                    lstControlAsist = db.SP_C_MostrarControlAsistencia().ToList();
                    lstColaboradores = db.SP_C_MostrarColaboradores("", "tod").ToList();
                }
                //Convertimos la hora y el tipo de jornada
                //Esto se hace aqui porque el entityFrameword no deja modfificar el tipo de horas de decimal a time o string
                List<ControlAsistencia> ddlControlAsistencia = lstControlAsist.ConvertAll(d =>
                {
                    return new ControlAsistencia()
                    {
                        IdAsistencia = d.IdAsistencia,
                        NomColaborador = d.NomColaborador,
                        FechaHoraIngreso = d.FechaHoraIngreso,
                        FechaHoraSalida = d.FechaHoraSalida,
                        TipoJornada = TipoJornadaConvert(d.TipoJornada),
                        HorasRegulares = convertToHora(d.HorasRegulares),
                        HorasExtras = convertToHora(d.HorasExtras),
                        HoraDoble = convertToHora(d.HoraDobles),
                        HorasExtrasDobles = convertToHora(d.HorasExtrasDobles),
                        UsuarioCreacion = d.UsuarioCreacion,
                        UsuarioModificacion = d.UsuarioModificacion
                    };
                });







                List<SelectListItem> ddlColaboradores = lstColaboradores.ConvertAll(d =>
                {
                    return new SelectListItem()
                    {
                        Text = "# " + d.IdColaborador.ToString() + " - " + d.Nombre.ToString(),
                        Value = d.IdColaborador.ToString(),
                        Selected = false
                    };
                });

                TempData["lstControlAsist"] = ddlControlAsistencia;
                TempData["lstColaboradores"] = ddlColaboradores;
            }
            catch (Exception e){
                Console.WriteLine(e.Message);
            }
            return View();
        }



        // ======================================= FUNCIONES =======================================
        public Boolean selectTrue(String idColaborador, String idColaboradorSelect)
        {

            return idColaborador.Equals(idColaboradorSelect) ? true : false;
        }
        public String convertToHora(decimal minutos)
        {
            String[] array = minutos.ToString().Split(',');

            array[1] = "0," + array[1];

            char[] min = (Double.Parse(array[1]) * 60).ToString().ToArray();

            if (array[0].Length < 2) array[0] = "0" + array[0];//Se hace para el input pueda leerlo


            if (min.Length > 1 && min[1] != ',')
                return array[0] + ":" + min[0] + min[1];


            return array[0] + ":" + "0" + min[0];
        }

        public String TipoJornadaConvert(String sigla)
        {

            switch (sigla)
            {
                case "DR":

                    sigla = "Diurna";
                    break;
                case "NT":
                    sigla = "Nocturna";
                    break;
                case "MT":
                    sigla = "Mixta";
                    break;
                default:
                    sigla = "No definido";
                    break;
            }
            return sigla;
        }


        // ======================================= RETORNOS JSON =======================================
        [HttpPost]
        public JsonResult AgregarAttendanceControl(CONTROL_ASISTENCIA obj)//
        {
            int resp = 0;

            Boolean HorasCoincide = obj.FechaHoraIngreso == null ? true : false;

            using (var db = new SICOAdminEntities())
            {
                resp = db.SP_P_CrearControlAsistencia(obj.IdColaborador, obj.FechaHoraIngreso, obj.FechaHoraSalida,
                    obj.TipoJornada, obj.HorasRegulares, obj.HorasExtras, obj.HoraDobles, obj.HorasExtrasDobles, ((User)Session["User"]).userName, msj);
            }

            return Json(new
            {
                resp = resp,
                mensaje = msj.Value
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditarAttendanceControl(CONTROL_ASISTENCIA obj) {
            int resp = 0;


           
            //obj.HorasRegulares = decimal.Round(obj.HorasRegulares, 4);


            using (var db = new SICOAdminEntities()){
                resp = db.SP_P_ModificarControlAsistencia(obj.IdAsistencia,obj.IdColaborador, obj.FechaHoraIngreso, obj.FechaHoraSalida,
                    obj.TipoJornada, obj.HorasRegulares, obj.HorasExtras, obj.HoraDobles, obj.HorasExtrasDobles,
                    ((User)Session["User"]).userName, msj);
            }

            return Json(new{
                resp = resp,
                mensaje = msj.Value
            }, JsonRequestBehavior.AllowGet);
        }
        // busca el ctrol asistencia para llenar los inputs
        [HttpPost]
        public JsonResult EditAttendanceControl(ControlAsistencia obj)
        {


            using (var db = new SICOAdminEntities())
            {
                lstControlAsistBusc = db.SP_C_BuscarControlAsistencia(obj.IdAsistencia).First();
            }



            var obj2 = new
            {
                idAistencia = lstControlAsistBusc.IdAsistencia,
                idColaborador = lstControlAsistBusc.IdColaborador,
                fechaHoraIngreso = lstControlAsistBusc.FechaHoraIngreso.ToString("yyy-MM-dd.HH:mm"),//yyyy-MMMM-dd 
                fechaHoraSalida = lstControlAsistBusc.FechaHoraSalida.ToString("yyy-MM-dd.HH:mm"),
                tipoJornada = lstControlAsistBusc.TipoJornada,
                horasRegulares = convertToHora(lstControlAsistBusc.HorasRegulares),
                horasExtras = convertToHora(lstControlAsistBusc.HorasExtras),
                horaDobles = convertToHora(lstControlAsistBusc.HoraDobles),
                horasExtrasDobles = convertToHora(lstControlAsistBusc.HorasExtrasDobles)

            };
            
            return Json(obj2, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult viewMore(int id)//
        {

            using (var db = new SICOAdminEntities())
            {
                lstControlAsistBusc = db.SP_C_BuscarControlAsistencia(id).First();
            }



            var obj = new
            {
                idAistencia = lstControlAsistBusc.IdAsistencia,
                idColaborador = lstControlAsistBusc.IdColaborador,
                nomColaborador = lstControlAsistBusc.nomColaborador,
                fechaHoraIngreso = lstControlAsistBusc.FechaHoraIngreso.ToString("yyy-MM-dd HH:mm"),//yyyy-MMMM-dd 
                fechaHoraSalida = lstControlAsistBusc.FechaHoraSalida.ToString("yyy-MM-dd HH:mm"),
                tipoJornada = TipoJornadaConvert(lstControlAsistBusc.TipoJornada),
                horasRegulares = convertToHora(lstControlAsistBusc.HorasRegulares),
                horasExtras = convertToHora(lstControlAsistBusc.HorasExtras),
                horaDobles = convertToHora(lstControlAsistBusc.HoraDobles),
                horasExtrasDobles = convertToHora(lstControlAsistBusc.HorasExtrasDobles),
                UsuarioCreacion = lstControlAsistBusc.UsuarioCreacion,
                UsuarioModificacion = lstControlAsistBusc.UsuarioModificacion,

            };

            return Json(obj, JsonRequestBehavior.AllowGet);

        }



        // ======================================= VISTAS PARCIALES =======================================
        public PartialViewResult _TableAttendanceControl()
        {
            using (var db = new SICOAdminEntities())
            {

                lstControlAsist = db.SP_C_MostrarControlAsistencia().ToList();

            }


            List<ControlAsistencia> ddlControlAsistencia = lstControlAsist.ConvertAll(d =>
            {
                return new ControlAsistencia()
                {
                    IdAsistencia = d.IdAsistencia,
                    NomColaborador = d.NomColaborador,
                    FechaHoraIngreso = d.FechaHoraIngreso,
                    FechaHoraSalida = d.FechaHoraSalida,
                    TipoJornada = TipoJornadaConvert(d.TipoJornada),
                    HorasRegulares = convertToHora(d.HorasRegulares),
                    HorasExtras = convertToHora(d.HorasExtras),
                    HoraDoble = convertToHora(d.HoraDobles),
                    HorasExtrasDobles = convertToHora(d.HorasExtrasDobles),
                    UsuarioCreacion = d.UsuarioCreacion,
                    UsuarioModificacion = d.UsuarioModificacion
                };
            });






            TempData["lstControlAsist"] = ddlControlAsistencia;


            return PartialView("_TableAttendanceControl", ddlControlAsistencia);
        }

        public PartialViewResult _SelectCollaborators(int idColaborador)
        { //id del colaborador seleccionado

            using (var db = new SICOAdminEntities())
            {

                lstColaboradores = db.SP_C_MostrarColaboradores("", "tod").ToList();
            }


            List<SelectListItem> ddlColaboradores = lstColaboradores.ConvertAll(d =>
            {
                return new SelectListItem()
                {
                    Text = "# " + d.IdColaborador.ToString() + " - " + d.Nombre.ToString(),
                    Value = d.IdColaborador.ToString(),
                    Selected = selectTrue(d.IdColaborador.ToString(), idColaborador.ToString())
                };
            });

            return PartialView("_SelectCollaborators", ddlColaboradores);
        }

        //public PartialViewResult _DateTimeFecha(String fecha, String tipo)
        //{
        //    //Hice esto porque si se envia con el formato normal pierde las Horas y minutos
        //    fecha = fecha.Replace('.', ' ');

        //    return tipo.Equals("1") ? PartialView("_DateTimeFechaIngreso", fecha) : PartialView("_DateTimeFechaSalida", fecha);
        //}

        //public PartialViewResult _SelectTypeWorkDay(String tipoJornada)
        //{
        //    //Hice esto porque si se envia con el formato normal pierde las Horas y minutos

        //    return PartialView("_SelectTypeWorkDay", tipoJornada);
        //}



    }
}