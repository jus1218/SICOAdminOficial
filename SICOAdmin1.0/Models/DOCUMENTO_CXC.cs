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
    
    public partial class DOCUMENTO_CXC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DOCUMENTO_CXC()
        {
            this.AUXILIAR_CXC = new HashSet<AUXILIAR_CXC>();
            this.AUXILIAR_CXC1 = new HashSet<AUXILIAR_CXC>();
        }
    
        public string Documento { get; set; }
        public int IdCliente { get; set; }
        public string TipoDocumento { get; set; }
        public decimal Monto { get; set; }
        public decimal Saldo { get; set; }
        public string Estado { get; set; }
        public int CondicionPago { get; set; }
        public System.DateTime FechaDocumento { get; set; }
        public string Notas { get; set; }
        public string UsuarioCreacion { get; set; }
        public System.DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public System.DateTime FechaModificacion { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AUXILIAR_CXC> AUXILIAR_CXC { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AUXILIAR_CXC> AUXILIAR_CXC1 { get; set; }
        public virtual CLIENTE CLIENTE { get; set; }
    }
}
