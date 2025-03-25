using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wasty.Models
{
    public class Residuo
    {
        public int Codigo { get; set; }
        public DateTime Fecha { get; set; }
        public string Denominacion { get; set; }
        public string LER { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string OperadorTraslado { get; set; }
        public bool Activo { get; set; }

        // Datos de residuo
        public string Referencia { get; set; }
        public string Tipo { get; set; }
        public string ProcesoD { get; set; }
        public string ProcesoR { get; set; }
        public string Desagregado { get; set; }
        public string Peligrosidad1 { get; set; }
        public string Peligrosidad2 { get; set; }
        public string LERRAE { get; set; }
        public string ONU { get; set; }
        public string ADR { get; set; }
        public string Acondicionamiento { get; set; }
        public string ParametrosEnvase { get; set; }
        public string CondicionesFisicoQuimicas { get; set; }

        // Datos contrato
        public bool ContratoActivo { get; set; }
        public bool VisibleSolicitudes { get; set; }
        public DateTime InicioTraslados { get; set; }
        public double KilosTotales { get; set; }
        public string E3LContrato { get; set; }
        public string Frecuencia { get; set; }
        public string TipoContrato { get; set; }
        public string NotasContrato { get; set; }

        // Notificación
        public bool Notificada { get; set; }
        public string CodigoE3L { get; set; }
        public DateTime FechaNotificacion { get; set; }
        public string Plataforma { get; set; }
        public bool PDFAlmacenado { get; set; }

        // Tratamientos
        public string Centro1 { get; set; }
        public string ProcesoDR1 { get; set; }
        public string Centro2 { get; set; }
        public string ProcesoDR2 { get; set; }

        public string EmpresaOrigen { get; set; }
        public string EmpresaDestino { get; set; }

        // Facturación
        public string ClienteFacturacion { get; set; }
        public string Producto { get; set; }
        public string Cantidad { get; set; }
        public string Precio { get; set; }
        public string PorcentajeDescuento { get; set; }

        // Integridad
        public string IntegridadMensaje { get; set; }
    }

}
