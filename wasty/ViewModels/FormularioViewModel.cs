using System;

public class FormularioViewModel
{
	public FormularioViewModel()
	{
		Bloques = new ObservableCollection<BloqueFormulario>
		{
			new BloqueFormulario
			{
				Nombre = "NIF y Nombres",
				Campos = ObservableCollection<CampoFormulario>
				{
                    new CampoFormulario {Nombre = "NIF", Tipo = "Texto", Valor = ""}
                    new CampoFormulario {Nombre = "Nombre Fiscal", Tipo = "Texto", Valor = ""}
                    new CampoFormulario {Nombre = "Nombre Juridico", Tipo = "Texto", Valor = ""}
                }
			}

            new BloqueFormulario
            {
                Nombre = "Ubicación",
                Campos = ObservableCollection<CampoFormulario>

                {
                    new CampoFormulario {Nombre = "Domicilio", Tipo = "Texto", Valor = ""}
                    new CampoFormulario {Nombre = "Municipio", Tipo = "Texto", Valor = ""}
                    new CampoFormulario {Nombre = "Provincia", Tipo = "Texto", Valor = ""}
                    new CampoFormulario {Nombre = "Comunidad Autónoma", Tipo = "Texto", Valor = ""}
                    new CampoFormulario {Nombre = "Código Postal", Tipo = "Número", Valor = ""}
                }
            }


            new BloqueFormulario
            {
                Nombre = "Contacto",
                Campos = ObservableCollection<CampoFormulario>

                {
                    new CampoFormulario {Nombre = "Teléfono", Tipo = "Número", Valor = ""}
                    new CampoFormulario {Nombre = "Email", Tipo = "Texto", Valor = ""}
                }
            }
        }
	}
}
