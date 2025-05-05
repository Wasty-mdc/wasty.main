using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace wasty.Models
{
    public class CentroModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("nombre")]
        public string NombreCentro { get; set; }

        [JsonPropertyName("nima")]
        public string NIMA { get; set; }

        [JsonPropertyName("telefonoPrincipal")]
        public string TelefonoPrincipal { get; set; }

        [JsonPropertyName("telefonoSecundario")]
        public string TelefonoSecundario { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}