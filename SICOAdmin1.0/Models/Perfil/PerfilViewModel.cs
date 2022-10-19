using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SICOAdmin1._0.Models.ViewModels
{
    public class PerfilViewModel
    {
    
            public int IdPerfil { get; set; }

            [Required]
            [Display(Name = "Nombre")]
            [StringLength(50)]
            public string Nombre { get; set; }

            [Required]
            [Display(Name = "Descripcion")]
            [StringLength(100)]
            public string Descripcion { get; set; }

            [Required]
            [Display(Name = "Activo")]
            public bool Activo { get; set; }

            [Required]
            [Display(Name = "Usuario Modificacion")]
            [StringLength(70)]
            public string UsuarioModificacion { get; set; }

        
    }
}