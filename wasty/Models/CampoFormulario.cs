using System;

public class CampoFormulario
{
	public CampoFormulario()
	{
		public string Nombre { get; set; } //Nombre del campo
		public string Tipo { get; set; } //Si es texto, numero, etc...
		public string Valor {  get; set; } //Valor que tendra por defecto
	}
}
