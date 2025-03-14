using System;
using System.Collections.ObjectModel;

namespace wasty.Models 
{ 
	public class BloqueFormulario
	{
		public string Nombre {  get; set; }
		public ObservableCollection<CampoFormulario> Campos { get; set; }
	}
}

