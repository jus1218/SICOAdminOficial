using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SICOAdmin1._0.Models
{
    public class ControlAsistencia
    {

        public int IdAsistencia { set; get; }


        public int IdColaborador { set; get; }

        public string NomColaborador { set; get; }

        public DateTime FechaHoraIngreso  { get; set; }


        public DateTime FechaHoraSalida{ set; get; }

        public String TipoJornada { set; get; }

        public String HorasRegulares { set; get; }

        public String HorasExtras { set; get; }

        public String HoraDoble { set; get; }

        public String HorasExtrasDobles { set; get; }
        
        public String UsuarioCreacion { set; get; }
        public String UsuarioModificacion { set; get; }





    }
}