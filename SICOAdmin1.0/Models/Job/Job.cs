using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SICOAdmin1._0.Models
{
    public class Job
    {
        public int IdPuesto { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        [StringLength(100)]
        public string Descripcion { get; set; }


        [Required]
        [Display(Name = "Salario Minimo")]

        public decimal SalarioMinimo { get; set; }


        [Required]
        [Display(Name = "Salario Promedio")]
        public decimal SalarioPromedio { get; set; }


        [Required]
        [Display(Name = "Salario Maximo")]
        public decimal SalarioMaximo { get; set; }


        [Required]
        [Display(Name = "Codigo CCSS")]
        [StringLength(20)]
        public string CodigoCCSS { get; set; }


        [Required]
        [Display(Name = "Codigo INS")]
        [StringLength(20)]
        public string CodigoINS { get; set; }


        [Required]
        [Display(Name = "Usuario Modificacion")]
        [StringLength(70)]
        public string UsuarioCreacion { get; set; }


        [Required]
        [Display(Name = "Usuario Modificacion")]
        [StringLength(70)]
        public string UsuarioModificacion { get; set; }

    }
}