using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SICOAdmin1._0.Models
{
    public class Pagina
    {
        public string Usuario { set; get; }
        public int NumPagina { set; get; }
        public int CantRegistros { set; get; }
        public string palabraBuscar { set; get; }

        public bool estaBuscando { set; get; }

        public int totalPaginas { set; get; }

        // S -> Pagina Siguiete  |  A -> Pagina Anterior

        public char accion { set; get; }

        //Constructor
        public Pagina() {
            estaBuscando = true;
            CantRegistros = 1;
            accion = 'A';

        }
    }
}