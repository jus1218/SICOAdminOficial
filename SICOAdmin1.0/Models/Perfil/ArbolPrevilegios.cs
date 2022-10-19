using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SICOAdmin1._0.Models.Perfil
{
    public class ArbolPrevilegios
    {
        public int idAccion { get; set; }
        public int Padre { get; set; }
        public int NumeroHermano { get; set; }
        public string text { get; set; }
        public bool @checked { get; set; }
        public string UsuarioModificacion { get; set; }
        public bool hasChildren { get; set; }
        public virtual List<ArbolPrevilegios> children { get; set; }

    }
}