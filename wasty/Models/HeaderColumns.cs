using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wasty.Models
{
    public class HeaderColumn
    {
        public string Nombre { get; }
        public string Icono { get; }
        public string Color { get; }

        public HeaderColumn(string nombre, string icono, string color)
        {
            Nombre = nombre;
            Icono = icono;
            Color = color;
        }

        public static List<HeaderColumn> ObtenerHeaders()
        {
            return new List<HeaderColumn>
            {
                new HeaderColumn("Default", "FileAlertOutline", "#9C9C9C"),
                new HeaderColumn("NombreComercial", "Account", "#f77f00"),
                new HeaderColumn("NombreFiscal", "AccountBox", "#fcbf49"),
                new HeaderColumn("NIMA", "RecycleVariant", "#A0C26C"),
                new HeaderColumn("NIF", "CardAccountDetails", "#FFD166"),
                new HeaderColumn("Telefono", "PhoneClassic", "#FF6B6B"),
                new HeaderColumn("Prioridad", "AlertCircle", "#EF476F"),      
                new HeaderColumn("Direccion", "MapMarker", "#90e0ef"),        
                new HeaderColumn("CodigoPostal", "Mailbox", "#48cae4"),      
                new HeaderColumn("Poblacion", "City", "#00b4d8"),             
                new HeaderColumn("Provincia", "Map", "#0096c7"),   
                new HeaderColumn("Pais", "Earth", "#0077b6"),
                new HeaderColumn("Codigo", "BarCode", "#e0e1dd"),

                };
        }
    }
}
