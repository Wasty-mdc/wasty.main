using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wasty.Models
{
    internal class TrasladoModel
    {
        public string Codigo { get; set; }
        public string FechaInicio { get; set; }
        public string FechaFin { get; set; }
        public string Transportista { get; set; }
        public string OrigenDestino { get; set; }
        public string Chofer { get; set; }
        public string Matricula { get; set; }
        public string StatusTrasla { get; set; } // para el botón de estado
    }
}
