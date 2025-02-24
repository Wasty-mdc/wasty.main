using System.Text.Json.Serialization;

namespace wasty.Models
{
    public class Cliente
    {
        [JsonPropertyName("codCliente")]
        public int Codigo { get; set; }
        [JsonPropertyName("nombreComercial")]
        public string NombreComercial { get; set; }
        [JsonPropertyName("nombreFiscal")]
        public string NombreFiscal { get; set; }
        [JsonPropertyName("nif")]
        public string NIF { get; set; }
        [JsonPropertyName("telefono")]
        public string Telefono { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}