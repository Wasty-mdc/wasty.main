using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using wasty.Models;


    namespace wasty.ViewModels
    {
        public class FormularioViewModel : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private string _grupoPrincipalSeleccionado;
            private bool _grupoSecundarioHabilitado;
            private ObservableCollection<string> _opcionesGrupoSecundario;

            public ObservableCollection<BloqueFormulario> Bloques { get; set; }

            public List<string> OpcionesGrupoPrincipal { get; } = new()
        {
            "Talleres", "Planchas", "Gestores", "Industrias", "Talleres Aceite", "Sin asignar"
        };

            public ObservableCollection<string> OpcionesGrupoSecundario
            {
                get => _opcionesGrupoSecundario;
                set
                {
                    _opcionesGrupoSecundario = value;
                    OnPropertyChanged();
                }
            }

            public string GrupoPrincipalSeleccionado
            {
                get => _grupoPrincipalSeleccionado;
                set
                {
                    _grupoPrincipalSeleccionado = value;
                    OnPropertyChanged();
                    ActualizarOpcionesGrupoSecundario();
                }
            }

            public bool GrupoSecundarioHabilitado
            {
                get => _grupoSecundarioHabilitado;
                set
                {
                    _grupoSecundarioHabilitado = value;
                    OnPropertyChanged();
                }
            }

            public ICommand SeleccionarGrupoCommand { get; }

            public FormularioViewModel()
            {
                Bloques = new ObservableCollection<BloqueFormulario>
            {
                new BloqueFormulario
                {
                    Nombre = "Datos Clientes",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Prioridad del Cliente", Tipo = "Picker", Opciones = new List<string> { "1", "2", "3", "4", "5" }, Valor = "" },
                        new CampoFormulario { Nombre = "Nombre Fiscal", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "NIF", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Nombre Comercial:", Tipo = "Texto", Valor = "" }
                    },
                    IsFirst = true
                },
                new BloqueFormulario
                {
                    Nombre = "Online",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Contraseña Online", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Acceso Online", Tipo = "Checkbox", EstaSeleccionado = false },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Datos Generales",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Fecha de Alta:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Fecha de Baja:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Lista de Correo", Tipo = "Checkbox", EstaSeleccionado = false },
                        new CampoFormulario { Nombre = "Cliente Gestor", Tipo = "Checkbox", EstaSeleccionado = false },
                        new CampoFormulario { Nombre = "Fevauto", Tipo = "Checkbox", EstaSeleccionado = false },
                        new CampoFormulario { Nombre = "Cliente Esporádico", Tipo = "Checkbox", EstaSeleccionado = false }
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Contacto",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Horario de Mañana (Desde - Hasta)", Tipo = "Numero", Valor = "" },
                        new CampoFormulario { Nombre = "", Tipo = "Numero", Valor = "" },
                        new CampoFormulario { Nombre = "Horario de Tarde (Desde - Hasta)", Tipo = "Numero", Valor = "" },
                        new CampoFormulario { Nombre = "", Tipo = "Numero", Valor = "" },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Ubicación",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Dirección Fiscal:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Dirección de Recogida:", Tipo = "Texto", Opciones = OpcionesGrupoPrincipal, Valor = "Sin Asignar" },
                        new CampoFormulario { Nombre = "Código Postal:", Tipo = "Numero", Valor = "" },
                        new CampoFormulario { Nombre = "Población:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Provincia:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "País:", Tipo = "Texto", Valor = "" }
                    }
                },
                new BloqueFormulario
                { 
                    Nombre = "Comercial",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Persona de Contacto:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "DNI:", Tipo = "Texto", Opciones = OpcionesGrupoPrincipal, Valor = "Sin Asignar" },
                        new CampoFormulario { Nombre = "Cargo:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Comercial:", Tipo = "Picker", Opciones = new List<string> { "1", "2", "3", "4", "5" }, Valor = "" }
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Observaciones",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Observaciones:", Tipo = "Texto", Valor = "" }
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Datos de Pagos",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Banco:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Numero de Cuenta:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Forma de Cobro:", Tipo = "Picker", Opciones = new List<string> { "Al Contado", "Giro Bancario", "Cheque", "Compensación", "Transferencia", "Otros" }, Valor = "" },
                        new CampoFormulario { Nombre = "Forma de Pago:", Tipo = "Picker", Opciones = new List<string> {"Al Contado", "Pendiente de Pago", "Cheque", "Compensación", "Compensación + Efectivo", "Compensación + Tarjeta", "Tarjeta"},Valor = "",},
                        new CampoFormulario { Nombre = "Dia de Cobro:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "IVA", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Fecha de Primer Cobro:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Periodicidad:", Tipo = "Picker", Opciones = new List<string> { "Mensual", "Bimensual", "Trimestral", "Semestral", "Anual"}, Valor = "" },
                        new CampoFormulario { Nombre = "Cobro a Fin Periodo", Tipo = "Checkbox",  EstaSeleccionado = false},
                        new CampoFormulario { Nombre = "No Cobrar (Marca de Rojo)", Tipo = "Checkbox", EstaSeleccionado = false },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Datos de Pagos",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Chatarra Mecánica:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Chatarra Planca:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Plástica:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Papel Archivo:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Aluminio:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Baterías (Unidades):", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Baterías (Kilos):", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Aceite:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Cartón:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Taras:", Tipo = "Texto", Valor = "" },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Datos de Recogidas",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Fecha Última Retirada:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Dias Entre Cada Retirada:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Dirección de Recogida:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Código Postal de Recogida:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Población de Recogida:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Observaciones para las Recogidas:", Tipo = "Texto", Valor = "" },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Recogidas No Peligrosas",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Fecha Última Retirada:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Dias Entre Cada Retirada:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Alarma:", Tipo = "Checkbox", EstaSeleccionado = false },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Recogidas Peligrosas",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Fecha Última Retirada:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Dias Entre Cada Retirada:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Alarma:", Tipo = "Checkbox", EstaSeleccionado = false },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Recogidas Aceite",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Fecha Última Retirada:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Dias Entre Cada Retirada:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Alarma:", Tipo = "Checkbox", EstaSeleccionado = false },
                        new CampoFormulario { Nombre = "Contenido del Aceite:", Tipo = "Picker", Opciones = new List<string> { "Bidón", "Cisterna", "Sin Aceite", "Fosa"},Valor = "" },
                        new CampoFormulario { Nombre = "Forma de Retirada:", Tipo = "Picker", Opciones = new List<string> { "Cambio", "---", "---", "---"},Valor = "" },
                        new CampoFormulario { Nombre = "Número de Contenedores:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Tamaño del Vehículo:", Tipo = "Texto", Opciones = new List<string> { "Furgoneta", "Camión Ligero", "Camión Pesado"}, Valor = "" },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "NIMA",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "NIMA:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Inscripción (P01/P02):", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Inscripción (P03/P04):", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Actividad Economica:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Forma Juridica:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Codigo INE):", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "CNAE:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Gestor Habitual:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Comunidad Autónoma:", Tipo = "Texto", Valor = "" },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Medio Ambiente",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Responsable:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Cargo:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Telefono:", Tipo = "Numero", Valor = "" },
                        new CampoFormulario { Nombre = "NIF:", Tipo = "Texto", Valor = "" }
                    }
                },
                                new BloqueFormulario
                {
                    Nombre = "Legalidad",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Responsable:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Cargo:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Telefono:", Tipo = "Numero", Valor = "" },
                        new CampoFormulario { Nombre = "NIF:", Tipo = "Texto", Valor = "" }
                    }
                },
            };

                OpcionesGrupoSecundario = new ObservableCollection<string>();
                SeleccionarGrupoCommand = new RelayCommand(_ => ActualizarOpcionesGrupoSecundario());
            }

            private void ActualizarOpcionesGrupoSecundario()
            {
                OpcionesGrupoSecundario.Clear();

                switch (GrupoPrincipalSeleccionado)
                {
                    case "Talleres":
                        OpcionesGrupoSecundario.Add("Mecánico");
                        OpcionesGrupoSecundario.Add("Aceite");
                        OpcionesGrupoSecundario.Add("Camiones");
                        OpcionesGrupoSecundario.Add("Lavadero");
                        OpcionesGrupoSecundario.Add("Mecánico y Planchas");
                        OpcionesGrupoSecundario.Add("Motos");
                        OpcionesGrupoSecundario.Add("Plancha y Pintura");
                        OpcionesGrupoSecundario.Add("Plancha, Pintura y Mecánico");
                        OpcionesGrupoSecundario.Add("Transportes");
                        GrupoSecundarioHabilitado = true;
                        break;
                    case "Planchas":
                        GrupoSecundarioHabilitado = false;
                        break;
                    case "Gestores":
                        OpcionesGrupoSecundario.Add("Aceite Gestor");
                        OpcionesGrupoSecundario.Add("Cartón");
                        OpcionesGrupoSecundario.Add("Lodos");
                        OpcionesGrupoSecundario.Add("Metales");
                        OpcionesGrupoSecundario.Add("Palets");
                        OpcionesGrupoSecundario.Add("Plástico");
                        OpcionesGrupoSecundario.Add("Residuos Peligrosos");
                        OpcionesGrupoSecundario.Add("Residuos Voluminosos");
                        OpcionesGrupoSecundario.Add("Vidrio");
                        GrupoSecundarioHabilitado = true;
                        break;
                    case "Industrias":
                        OpcionesGrupoSecundario.Add("Aceite Industrial");
                        OpcionesGrupoSecundario.Add("Cartón");
                        OpcionesGrupoSecundario.Add("Contenedores");
                        OpcionesGrupoSecundario.Add("Curtido Pieles");
                        OpcionesGrupoSecundario.Add("Material Quirúrgico");
                        OpcionesGrupoSecundario.Add("Metales");
                        OpcionesGrupoSecundario.Add("Plástico");
                        OpcionesGrupoSecundario.Add("Otros");
                        GrupoSecundarioHabilitado = true;
                        break;
                    case "Talleres Aceite":
                        GrupoSecundarioHabilitado = false;
                        break;
                    case "Sin asignar":
                        OpcionesGrupoSecundario.Add("Sin asignar");
                        GrupoSecundarioHabilitado = true;
                        break;
                }
            }

            private void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
