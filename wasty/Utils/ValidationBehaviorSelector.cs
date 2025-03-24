using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using wasty.Utils.Behaviors;

namespace wasty.Utils
{
    public class ValidationBehaviorSelector : MarkupExtension
    {
        public string TipoValidacion { get; set; }

        public ValidationBehaviorSelector(string tipoValidacion)
        {
            TipoValidacion = tipoValidacion;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return TipoValidacion switch
            {
                "Vacio" => new VacioValidationBehavior(),
                "DNI" => new DniValidationBehavior(),
                "NIF" => new NifValidationBehavior(),
                "NCuenta" => new NCuentaValidationBehavior(),
                "Telefono" => new TelefonoValidationBehavior(),
                "NIMA" => new NimaValidationBehavior(),
                "CNAE" => new CnaeValidationBehavior(),
                "INE" => new IneValidationBehavior(),
                "Email" => new EmailValidationBehavior(),
                _ => null
            };
        }
    }
}
