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
                new HeaderColumn("NombreComercial", "Account", "#266DD3"),
                new HeaderColumn("NombreFiscal", "AccountBox", "#205AAC"),
                new HeaderColumn("NIMA", "RecycleVariant", "#A0C26C"),
                new HeaderColumn("NIF", "CardAccountDetails", "#FFD166"),
                new HeaderColumn("Telefono", "PhoneClassic", "#FF6B6B"),

            };
        }
    }
}
