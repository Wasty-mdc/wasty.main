using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace wasty.Models
{
    public class Residuo
    {
        [JsonPropertyName("codResiduo")]
        public int Codigo { get; set; }
        [JsonPropertyName("nombre")]
        public string Tipo { get; set; }
        public double Peso { get; set; }
        public string Estado { get; set; }
        public string Ubicacion { get; set; }
        public DateTime FechaRecoleccion { get; set; }
        public string EmpresaGestora { get; set; }
        public string Destino { get; set; }
    }
}