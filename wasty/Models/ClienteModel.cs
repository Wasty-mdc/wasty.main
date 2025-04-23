using System.Text.Json.Serialization;

namespace wasty.Models
{
    public class ClienteModel
    {
        [JsonPropertyName("id")]
        public int Codigo { get; set; }
        [JsonPropertyName("nombreComercial")]
        public string NombreComercial { get; set; }
        [JsonPropertyName("nombreFiscal")]
        public string NombreFiscal { get; set; }
        [JsonPropertyName("nif")]
        public string NIF { get; set; }
        [JsonPropertyName("telefonoPrincipal")]
        public string Telefono1 { get; set; }
        [JsonPropertyName("telefonoSecundario")]
        public string Telefono2 { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("nima")]
        public string Nima { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}