using System.Text.Json.Serialization;

namespace wasty.Models
{
    public class Cliente
    {
        [JsonPropertyName("id")]
        public int Codigo { get; set; }
        [JsonPropertyName("nombreComercial")]
        public string NombreComercial { get; set; }
        [JsonPropertyName("nombreFiscal")]
        public string NombreFiscal { get; set; }
        [JsonPropertyName("nif")]
        public string NIF { get; set; }
        [JsonPropertyName("telefono")]
        public string Telefono { get; set; }
        [JsonPropertyName("nima")]
        public string Nima { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}