using System.Text.Json.Serialization;

namespace wasty.Models
{
    public class ClienteModel
    {
        // =================== DATOS DEL CLIENTE ===================

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

        [JsonPropertyName("direccionFiscal")]
        public string DireccionFiscal { get; set; }

        [JsonPropertyName("direccionRecogida")]
        public string DireccionRecogida { get; set; }

        [JsonPropertyName("codigoPostal")]
        public string CP { get; set; }

        [JsonPropertyName("poblacion")]
        public string Poblacion { get; set; }

        [JsonPropertyName("provincia")]
        public string Provincia { get; set; }

        [JsonPropertyName("pais")]
        public string Pais { get; set; }

        [JsonPropertyName("personaContacto")]
        public string PersonaContacto { get; set; }

        [JsonPropertyName("dniContacto")]
        public string DNIContacto { get; set; }

        [JsonPropertyName("cargoContacto")]
        public string CargoContacto { get; set; }

        [JsonPropertyName("comercial")]
        public string Comercial { get; set; }

        [JsonPropertyName("fechaAlta")]
        public DateTime? FechaAlta { get; set; }

        [JsonPropertyName("fechaBaja")]
        public DateTime? FechaBaja { get; set; }

        [JsonPropertyName("observaciones")]
        public string Observaciones { get; set; }

        [JsonPropertyName("estado")]
        public string Estado { get; set; } = "Activo";


        // =================== DATOS DEL CONTRATO ===================

        [JsonPropertyName("banco")]
        public string Banco { get; set; }

        [JsonPropertyName("numeroCuenta")]
        public string NumeroCuenta { get; set; }

        [JsonPropertyName("formaCobro")]
        public string FormaCobro { get; set; }

        [JsonPropertyName("formaPago")]
        public string FormaPago { get; set; }

        [JsonPropertyName("diaCobro")]
        public string DiaCobro { get; set; }

        [JsonPropertyName("importe")]
        public decimal? Importe { get; set; }

        [JsonPropertyName("iva")]
        public string IVA { get; set; }

        [JsonPropertyName("fechaPrimerCobro")]
        public DateTime? FechaPrimerCobro { get; set; }

        [JsonPropertyName("periodicidad")]
        public string Periodicidad { get; set; }

        [JsonPropertyName("cobroFinPeriodo")]
        public bool CobroFinPeriodo { get; set; }

        [JsonPropertyName("noCobrar")]
        public bool NoCobrar { get; set; }

        [JsonPropertyName("precioChatarreriaMecanica")]
        public decimal? PrecioChatMecanica { get; set; }

        [JsonPropertyName("precioChatarreriaPlancha")]
        public decimal? PrecioChatPlancha { get; set; }

        [JsonPropertyName("precioPlastica")]
        public decimal? PrecioPlastica { get; set; }

        [JsonPropertyName("precioPapelArchivo")]
        public decimal? PrecioPapelArchivo { get; set; }

        [JsonPropertyName("precioAluminio")]
        public decimal? PrecioAluminio { get; set; }

        [JsonPropertyName("precioBateriasUds")]
        public decimal? PrecioBateriasUds { get; set; }

        [JsonPropertyName("precioBateriasKg")]
        public decimal? PrecioBateriasKg { get; set; }

        [JsonPropertyName("precioAceite")]
        public decimal? PrecioAceite { get; set; }

        [JsonPropertyName("precioCarton")]
        public decimal? PrecioCarton { get; set; }

        [JsonPropertyName("precioTaras")]
        public decimal? PrecioTaras { get; set; }


        // =================== DATOS DEL CENTRO ===================

        [JsonPropertyName("horarioMananaDesde")]
        public string HorarioMananaDesde { get; set; }

        [JsonPropertyName("horarioMananaHasta")]
        public string HorarioMananaHasta { get; set; }

        [JsonPropertyName("horarioTardeDesde")]
        public string HorarioTardeDesde { get; set; }

        [JsonPropertyName("horarioTardeHasta")]
        public string HorarioTardeHasta { get; set; }

        [JsonPropertyName("horarioRecogidaDesde")]
        public string HorarioRecogidaDesde { get; set; }

        [JsonPropertyName("horarioRecogidaHasta")]
        public string HorarioRecogidaHasta { get; set; }

        [JsonPropertyName("horarioSabadoDesde")]
        public string HorarioSabadoDesde { get; set; }

        [JsonPropertyName("horarioSabadoHasta")]
        public string HorarioSabadoHasta { get; set; }

        [JsonPropertyName("horarioFestivoDesde")]
        public string HorarioFestivoDesde { get; set; }

        [JsonPropertyName("horarioFestivoHasta")]
        public string HorarioFestivoHasta { get; set; }

        [JsonPropertyName("personaRecogida")]
        public string PersonaRecogida { get; set; }

        [JsonPropertyName("dniRecogida")]
        public string DNIRecogida { get; set; }

        [JsonPropertyName("cargoRecogida")]
        public string CargoRecogida { get; set; }

        [JsonPropertyName("emailRecogida")]
        public string EmailRecogida { get; set; }

        [JsonPropertyName("telefonoRecogida")]
        public string TelefonoRecogida { get; set; }

        [JsonPropertyName("observacionesHorarios")]
        public string ObservacionesHorarios { get; set; }

        [JsonPropertyName("listaCorreo")]
        public bool ListaCorreo { get; set; }

        [JsonPropertyName("fevauto")]
        public bool Fevauto { get; set; }

        [JsonPropertyName("clienteGestor")]
        public bool ClienteGestor { get; set; }

        [JsonPropertyName("clienteEsporadico")]
        public bool ClienteEsporadico { get; set; }

        [JsonPropertyName("centros")]
        public List<CentroModel> Centros { get; set; }

        [JsonPropertyName("observacionesRecogidas")]
        public string ObservacionesRecogidas { get; set; }
    }
}
