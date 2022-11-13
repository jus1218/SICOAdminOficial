using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using System.Linq;
using System.Web;

namespace SICOAdmin1._0.Models.Parameter
{
    public class Parameter
    {

        public int IdParameter { get; set; }
        public string MonthlyPayment { get; set; }

        public int IdConsecRecibo { get; set; }

        public string InterestBlackberry { get; set; }

        public string DocumentTypePayment { get; set; }
        public int PaymentCondition { get; set; }
        public string Status{ get; set; }

        public string UserCreacion { get; set; }
        public string UserModification { get; set; }
        public DateTime DateCreacion { get; set; }
        public DateTime DateModification { get; set; }

        
    }

}