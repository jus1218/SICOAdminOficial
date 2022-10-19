using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SICOAdmin1._0.Models.Consecutivo
{
    public class Consecutivo
    {


        public int IdConsecutivo { get; set; }
        [Required]
        [Display(Name = "Alias")]
        [StringLength(10)]
        public string Alias { get; set; }
        [Required]
        [Display(Name = "Mascara")]
        [StringLength(30)]
        public string Mascara { get; set; }
        [Required]
        [Display(Name = "Valor Inicial")]
        [StringLength(30)]
        public string ProximoValor { get; set; }

        [Required]
        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        public string UsuarioCreacion { get; set; }

        public string UsuarioModificacion { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }


    }
}