//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SICOAdmin1._0.Models
{
    using System;
    
    public partial class SP_C_BuscarUsuario_Result
    {
        public string Usuario { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public bool Activo { get; set; }
        public bool Bloqueado { get; set; }
        public string Contrasena { get; set; }
        public string CorreoElectronico { get; set; }
        public int DiasCambioContrasena { get; set; }
        public byte IntentosFallidos { get; set; }
        public Nullable<System.DateTime> UltCambioContrasena { get; set; }
        public System.DateTime UltIngreso { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
    }
}
