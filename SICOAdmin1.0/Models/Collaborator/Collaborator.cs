using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SICOAdmin1._0.Models.Collaborator
{
    public class Collaborator
    {
        public int Id { get; set; } //Autoincremental en la DB
        public string Name { get; set; }
        public string Gender { get; set; }
        public bool Active { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Identification { get; set; }
        public string Nationality { get; set; }
        public DateTime DateBirth { get; set; }
        public DateTime DateEntry { get; set; }
        public DateTime DateDeparture { get; set; }
        public int IdNomina { get; set; }
        public string CivilStatus { get; set; }
        public decimal VacationBalance { get; set; }// en 0
        public DateTime LastVacationCalc { get; set; }// en 0
        public int IdPuesto { get; set; }
        public string FormPayment { get; set; }//(Efectivo, transferencia, cheque)
        public string BankAccount { get; set; }
        public string Bank { get; set; }
        public string Email { get; set; }
        public string EmergencyContact { get; set; }
        public string TelephoneContact { get; set; }
        public decimal ReferenceSalary { get; set; }
        public string UserCreation { get; set; }
        public DateTime DateCreation { get; set; }
        public string UserModification { get; set; }
        public DateTime DateModification { get; set; }
    }

    public enum CivilStatus
    {
        Soltero,
        Casado
    }
}