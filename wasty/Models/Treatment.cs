using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wasty.Models
{
    public class Treatment
    {
        public string Codigo { get; set; }
        public string Fecha { get; set; } 
        public string Denominacion { get; set; }
        public string LER { get; set; }
        public string OrigenDestino { get; set; }
        public string OperadorTraslados { get; set; }
        public string Activo { get; set; }
        public string Notificar { get; set; }
    }
}
