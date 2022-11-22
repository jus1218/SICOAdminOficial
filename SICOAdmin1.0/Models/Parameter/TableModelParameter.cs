using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SICOAdmin1._0.Models.Parameter
{
    public class TableModelParameter
    {
        public int IdParameter { get; set; }
        public string UserCreacion { get; set; }
        public string UserModification { get; set; }
        public string DateCreacion { get; set; }
        public string DateModification { get; set; }

        public string AccionRealizada { get; set; }
    }
}