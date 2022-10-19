﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SICOAdmin1._0.Models.User
{
    public class User
    {
        [Required]
        [Display(Name = "Nombre de usuario")]
        public string userName { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string name { get; set; }

        [Required]
        [Display(Name = "Tipo usuario")]
        public TypesU type { get; set; }

        [Required]
        [Display(Name = "Activo")]
        public bool active { get; set; }

        [Required]
        [Display(Name = "Bloqueado")]
        public bool locked { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string password { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Correo Electrónico")]
        public string email { get; set; }

        [Required]
        [Display(Name = "Dias cambio contraseña")]
        public int daysChangePassword { get; set; }

        [Required]
        [Display(Name = "Intentos fallidos")]
        public int failesAttempts { get; set; }
        public DateTime lastChangedPassword { get; set; }
        public DateTime lastEntry { get; set; }
        public string userCreation { get; set; }
        public DateTime dateCreation { get; set; }
        public string userModification { get; set; }
        public DateTime dateModification { get; set; }
    }

    public enum TypesU
    {
        Administrador,
        Transaccional,
        Consulta
    }

}
