using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace wasty.Models
{
    public class ResiduoModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("nombre")]
        public string Denominacion { get; set; }
        [JsonPropertyName("referencia")]
        public string Referencia { get; set; }
        [JsonPropertyName("codigoLER")]
        public string LER { get; set; }
        [JsonPropertyName("numeroONU")]
        public string NumeroONU { get; set; }
        [JsonPropertyName("codigoTratamientoR")]
        public string TratamientoR { get; set; }
        [JsonPropertyName("codigoTratamientoD")]
        public string TratamientoD { get; set; }
    }
}