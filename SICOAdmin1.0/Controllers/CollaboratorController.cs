using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SICOAdmin1._0.Models.Collaborator;
using SICOAdmin1._0.Models.User;
using SICOAdmin1._0.Models;
using System.Data.Entity.Core.Objects;

namespace SICOAdmin1._0.Controllers
{
    public class CollaboratorController : Controller
    {
        // GET: Collaborator
        public ActionResult Index()
        {
            List<SP_C_MostrarColaboradores_Result> lstCollaboratorsDB = null;
            List<SP_C_MostarNominas_Result> lstPayrollsDB = null;
            List<SP_C_MostrarPuestos_Result> lstPositionsDB = null;
            List<Collaborator> lstCollaborators = new List<Collaborator>();

            


            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                lstCollaboratorsDB = db.SP_C_MostrarColaboradores("", "tod").ToList();
                lstPayrollsDB = db.SP_C_MostarNominas("tod", 0).ToList();
                lstPositionsDB = db.SP_C_MostrarPuestos().ToList();

                foreach (var c in lstCollaboratorsDB)
                {
                    lstCollaborators.Add(GetCollaborator(c.Identificacion));
                }


            }

            List<SelectListItem> payrolls = lstPayrollsDB.ConvertAll(
                 d =>
                 {
                     return new SelectListItem()
                     {
                         Value = d.IdNomina.ToString(),
                         Text = d.Descripcion,
                         Selected = false
                     };
                 });

            List<SelectListItem> positions = lstPositionsDB.ConvertAll(
                 d =>
                 {
                     return new SelectListItem()
                     {
                         Value = d.IdPuesto.ToString(),
                         Text = d.Descripcion,
                         Selected = false
                     };
                 });

            ViewBag.Positions = positions;
            ViewBag.Payrolls = payrolls;
            ViewBag.Collaborators = lstCollaborators;
            return View();
        }

        public Collaborator GetCollaborator(string id)
        {
            Collaborator objC = new Collaborator();
            SP_C_MostrarColaboradores_Result objDB = new SP_C_MostrarColaboradores_Result();

            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                objDB = db.SP_C_MostrarColaboradores(id, "one").FirstOrDefault();

                objC.Name = objDB.Nombre;
                objC.Gender = Convert.ToChar(objDB.Genero);
                objC.Active = objDB.Activo;
                objC.State = objDB.Estado;
                objC.Address = objDB.Direccion;
                objC.Telephone1 = objDB.Telefono1;
                objC.Telephone2 = objDB.Telefono2;
                objC.Identification = objDB.Identificacion;
                objC.Nationality = objDB.Nacionalidad;
                objC.DateBirth = objDB.FechaNacimiento;
                objC.DateEntry = objDB.FechaIngreso;
                objC.DateDeparture = objDB.FechaSalida;
                objC.IdNomina = objDB.IdNomina;
                //objC.CivilStatus = objDB.EstadoCivil;
                switch (objDB.EstadoCivil.ToString())
                {
                    case "Ca":
                        objC.CivilStatus = "Casado(a)";
                        break;
                    case "So":
                        objC.CivilStatus = "Soltero(a)";
                        break;
                }
                objC.VacationBalance = objDB.SaldoVacaciones;
                objC.LastVacationCalc = objDB.UltCalculoVacaciones;
                objC.IdPuesto = objDB.IdPuesto;
                //objC.FormPayment = objDB.FormaPago;
                switch (objDB.FormaPago.ToString())
                {
                    case "Tr":
                        objC.FormPayment = "Transferencia";
                        break;
                    case "Ef":
                        objC.FormPayment = "Efectivo";
                        break;
                    case "Ch":
                        objC.FormPayment = "Cheque";
                        break;
                }
                objC.BankAccount = objDB.CuentaBancaria;
                objC.Bank = objDB.Banco;
                objC.Email = objDB.CorreoElectronico;
                objC.EmergencyContact = objDB.ContactoEmergencia;
                objC.TelephoneContact = objDB.TelefonoContacto;
                objC.ReferenceSalary = objDB.SalarioReferencia;
                objC.UserCreation = objDB.UsuarioCreacion;
                objC.DateCreation = objDB.FechaCreacion;
                objC.UserModification = objDB.UsuarioModificacion;
                objC.DateModification = objDB.FechaModificacion;

            }
            return objC;
        }




        [HttpGet]
        public ActionResult AddCollaborator()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCollaborator(Collaborator CModel)
        {
            ObjectParameter resultado = new ObjectParameter("resultado", false);
            ObjectParameter mensaje = new ObjectParameter("mensaje", "");
            string Message = "";
            bool result = false;
            try
            {
                if (CModel != null)
                {
                    if (CModel.FormPayment != "Tr")
                    {
                        CModel.BankAccount = "N/A";
                        CModel.Bank = "N/A";
                    }

                    using (SICOAdminEntities db = new SICOAdminEntities())
                    {
                        db.SP_P_CrearColaborador(CModel.Name, Convert.ToString(CModel.Gender), CModel.Active, CModel.State, CModel.Address, CModel.Telephone1, CModel.Telephone2,
                                                 CModel.Identification, CModel.Nationality, CModel.DateBirth, CModel.DateEntry, CModel.DateDeparture, CModel.IdNomina,
                                                 CModel.CivilStatus, CModel.VacationBalance, CModel.LastVacationCalc, CModel.IdPuesto, CModel.FormPayment, CModel.BankAccount, CModel.Bank,
                                                 CModel.Email, CModel.EmergencyContact, CModel.TelephoneContact, CModel.ReferenceSalary,
                                                 ((User)Session["User"]).userName, ((User)Session["User"]).userName, resultado, mensaje);

                        Message = Convert.ToString(mensaje.Value);
                        result = Convert.ToBoolean(resultado.Value);
                    }

                    TempData.Clear();
                    if (result)
                    {
                        TempData["Resultado"] = "1";
                        TempData["Message"] = Message;
                    }
                    else
                    {
                        TempData["Resultado"] = "0";
                        TempData["Message"] = Message;
                    }
                    TempData.Keep("Resultado");
                    TempData.Keep("Message");
                }

                return RedirectToAction(Url.Content("/Index"));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Update(Collaborator pModel)
        {
            try
            {
                return RedirectToAction(Url.Content("/Index"));
            }
            catch (Exception ex)
            {

                return RedirectToAction(Url.Content("/Index"));
            }
        }

        public JsonResult GetBitacora(string id)
        {
            Collaborator objC = new Collaborator();
            SP_C_MostrarColaboradores_Result objCDB = new SP_C_MostrarColaboradores_Result();
            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                objCDB = db.SP_C_MostrarColaboradores(id, "one").FirstOrDefault();

                objC.UserCreation = objCDB.UsuarioCreacion;
                objC.DateCreation = objCDB.FechaCreacion;
                objC.UserModification = objCDB.UsuarioModificacion;
                objC.DateModification = (DateTime)objCDB.FechaModificacion;
            }

            var objCJS = new
            {
                objC.UserCreation,
                DateCreation = objC.DateCreation.ToString("dd/MM/yyyy H:mm:ss"),
                objC.UserModification,
                DateModification = objC.DateModification.ToString("dd/MM/yyyy H:mm:ss"),
            };


            return Json(objCJS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDetails(string id)
        {
            Collaborator objC = new Collaborator();
            SP_C_MostrarColaboradores_Result objCDB = new SP_C_MostrarColaboradores_Result();
            List<SP_C_MostarNominas_Result> lstPayrollsDB = null;
            List<SP_C_MostrarPuestos_Result> lstPositionsDB = null;

            string position = "";
            string payroll = "";

            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                objCDB = db.SP_C_MostrarColaboradores(id, "one").FirstOrDefault();
                lstPayrollsDB = db.SP_C_MostarNominas("tod", 0).ToList();
                lstPositionsDB = db.SP_C_MostrarPuestos().ToList();

                objC = GetCollaborator(objCDB.Identificacion);
            }

            foreach (var pa in lstPayrollsDB)
            {
                if(objCDB.IdNomina == pa.IdNomina)
                {
                    payroll = pa.Descripcion;
                }
               
            }

            foreach (var po in lstPositionsDB)
            {
                if (objCDB.IdPuesto == po.IdPuesto)
                {
                    position = po.Descripcion;
                }

            }



            var objCJS = new
            {
                objC.Name,
                objC.Gender,
                objC.Active,
                objC.State,
                objC.Address,
                objC.Telephone1,
                objC.Telephone2,
                objC.Identification,
                objC.Nationality,
                DateBirth = objC.DateBirth.ToString("dd MMMM, yyyy"),
                DateEntry = objC.DateEntry.ToString("dd MMMM, yyyy"),
                DateDeparture = objC.DateDeparture.ToString("dd MMMM, yyyy"),
                IdNomina = payroll,
                objC.CivilStatus,
                objC.VacationBalance,
                LastVacationCalc = objC.LastVacationCalc.ToString("dd/MM/yyyy H:mm:ss"),
                IdPuesto = position,
                objC.FormPayment,
                objC.BankAccount,
                objC.Bank,
                objC.Email,
                objC.EmergencyContact,
                objC.TelephoneContact,
                objC.ReferenceSalary
            };


            return Json(objCJS, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ModificarEstado(string idCol)
        {
            ObjectParameter resultado = new ObjectParameter("resultado", false);
            ObjectParameter mensaje = new ObjectParameter("mensaje", "");

            string message = "";
            bool result = false;

            using (SICOAdminEntities db = new SICOAdminEntities())
            {
                db.SP_P_CambiarEstadoColaborador(idCol, resultado, mensaje);
                result = Convert.ToBoolean(resultado.Value);
            }
            if (result)
            {
                message = "1";
            }
            else
            {
                message = "2";
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }
    }
}