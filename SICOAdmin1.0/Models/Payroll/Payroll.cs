using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SICOAdmin1._0.Models.Payroll
{
    public class Payroll
    {
        public int IdNomina { get; set; }
        [Required]
        [Display(Name = "Descripcion")]
        [StringLength(50)]
        public string Descripcion { get; set; }
        [Required]        
        [Display(Name = "Frecuencia")]
        [StringLength(20)]
        public string Frecuencia { get; set; }
        [Required]
        [Display(Name = "Consecutivo")]
        public int IdConsecutivo { get; set; }
        public string UsuarioCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }
}