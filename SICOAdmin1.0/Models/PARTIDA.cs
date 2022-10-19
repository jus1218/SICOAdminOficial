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
    using System.Collections.Generic;
    
    public partial class PARTIDA
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PARTIDA()
        {
            this.AUXILIAR_CXC = new HashSet<AUXILIAR_CXC>();
            this.AUXILIAR_CXP = new HashSet<AUXILIAR_CXP>();
        }
    
        public int IdPartida { get; set; }
        public string Alias { get; set; }
        public string Descripcion { get; set; }
        public string Tipo { get; set; }
        public bool Activo { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public Nullable<System.DateTime> FechaModificacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AUXILIAR_CXC> AUXILIAR_CXC { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AUXILIAR_CXP> AUXILIAR_CXP { get; set; }
    }
}
