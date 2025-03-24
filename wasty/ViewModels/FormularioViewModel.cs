using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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

        public ICommand GuardarCommand { get; }

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
                        new CampoFormulario { Nombre = "Nombre Fiscal", Tipo = "Texto", Valor = "", TipoValidacion="Vacio"},
                        new CampoFormulario { Nombre = "NIF", Tipo = "Texto", Valor = "" , TipoValidacion = "NIF"},
                        new CampoFormulario { Nombre = "Nombre Comercial", Tipo = "Texto", Valor = "", TipoValidacion = "Vacio" },
                        new CampoFormulario { Nombre = "Email:", Tipo="Texto", Valor="", TipoValidacion="Email"}
                    },
                    IsFirst = true,
                    ColorBase = "Yellow"
                },
                new BloqueFormulario
                {
                    Nombre = "Online",
                    ColorBase = "Yellow",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Contraseña Online", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Acceso Online", Tipo = "Checkbox", EstaSeleccionado = false },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Datos Generales",
                    ColorBase = "Yellow",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Fecha de Alta:", Tipo = "Fecha", Valor = "", TipoValidacion = "Vacio" },
                        new CampoFormulario { Nombre = "Fecha de Baja:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Lista de Correo", Tipo = "Checkbox", EstaSeleccionado = false },
                        new CampoFormulario { Nombre = "Cliente Gestor", Tipo = "Checkbox", EstaSeleccionado = false },
                        new CampoFormulario { Nombre = "Fevauto", Tipo = "Checkbox", EstaSeleccionado = false },
                        new CampoFormulario { Nombre = "Cliente Esporádico", Tipo = "Checkbox", EstaSeleccionado = false }
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Horario",
                    ColorBase = "Yellow",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Horario de Mañana (Desde - Hasta)", Tipo = "Número", Valor = ""},
                        new CampoFormulario { Nombre = "Horario de Tarde (Desde - Hasta)", Tipo = "Número", Valor = "" },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Ubicación",
                    ColorBase = "Yellow",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Dirección Fiscal:", Tipo = "Texto", Valor = "" , TipoValidacion = "Vacio"},
                        new CampoFormulario { Nombre = "Código Postal:", Tipo = "Número", Valor = "" , TipoValidacion = "Vacio"},
                        new CampoFormulario { Nombre = "Población:", Tipo = "Texto", Valor = "" , TipoValidacion = "Vacio"},
                        new CampoFormulario { Nombre = "Provincia:", Tipo = "Texto", Valor = "" , TipoValidacion = "Vacio"},
                        new CampoFormulario { Nombre = "País:", Tipo = "Texto", Valor = "" , TipoValidacion = "Vacio"}
                    }
                },
                new BloqueFormulario
                { 
                    Nombre = "Comercial",
                    ColorBase = "Blue",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Persona de Contacto:", Tipo = "Texto", Valor = ""},
                        new CampoFormulario { Nombre = "DNI:", Tipo = "Texto", Opciones = OpcionesGrupoPrincipal, Valor = "", TipoValidacion = "DNI" },
                        new CampoFormulario { Nombre = "Cargo:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Comercial:", Tipo = "Picker", Opciones = new List<string> { "1", "2", "3", "4", "5" }, Valor = "" }
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Observaciones",
                    ColorBase = "Orange",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Observaciones:", Tipo = "Texto", Valor = "" }
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Datos de Pagos",
                    ColorBase = "Red",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Banco:", Tipo = "Texto", Valor = "", TipoValidacion = "Vacio"},
                        new CampoFormulario { Nombre = "Numero de Cuenta:", Tipo = "Texto", Valor = "", TipoValidacion="NCuenta" },
                        new CampoFormulario { Nombre = "Forma de Cobro:", Tipo = "Picker", Opciones = new List<string> { "Al Contado", "Giro Bancario", "Cheque", "Compensación", "Transferencia", "Otros" }, Valor = "" },
                        new CampoFormulario { Nombre = "Forma de Pago:", Tipo = "Picker", Opciones = new List<string> {"Al Contado", "Pendiente de Pago", "Cheque", "Compensación", "Compensación + Efectivo", "Compensación + Tarjeta", "Tarjeta"},Valor = "",},
                        new CampoFormulario { Nombre = "Dia de Cobro:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "IVA:", Tipo = "Picker", Opciones = new List<string> { "0%", "4%", "8%", "10%", "18%", "21%" }, Valor = "" },
                        new CampoFormulario { Nombre = "Fecha de Primer Cobro:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Periodicidad:", Tipo = "Picker", Opciones = new List<string> { "Mensual", "Bimensual", "Trimestral", "Semestral", "Anual"}, Valor = "" },
                        new CampoFormulario { Nombre = "Cobro a Fin Periodo", Tipo = "Checkbox",  EstaSeleccionado = false},
                        new CampoFormulario { Nombre = "No Cobrar (Marca de Rojo)", Tipo = "Checkbox", EstaSeleccionado = false },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Datos de Pagos",
                    ColorBase = "Red",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Chatarra Mecánica:", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Chatarra Planca:", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Plástica:", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Papel Archivo:", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Aluminio:", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Baterías (Unidades):", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Baterías (Kilos):", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Aceite:", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Cartón:", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Taras:", Tipo = "Número", Valor = "" },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Datos de Recogidas",
                    ColorBase = "Pink",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Fecha Última Retirada:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Dias Entre Cada Retirada:", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Dirección de Recogida:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Código Postal de Recogida:", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Población de Recogida:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Observaciones para las Recogidas:", Tipo = "Texto", Valor = "" },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Recogidas No Peligrosas",
                    ColorBase = "Pink",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Fecha Última Retirada:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Dias Entre Cada Retirada:", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Alarma:", Tipo = "Checkbox", EstaSeleccionado = false },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Recogidas Peligrosas",
                    ColorBase = "Pink",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Fecha Última Retirada:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Dias Entre Cada Retirada:", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Alarma:", Tipo = "Checkbox", EstaSeleccionado = false },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Recogidas Aceite",
                    ColorBase = "Pink",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Fecha Última Retirada:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Dias Entre Cada Retirada:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Alarma:", Tipo = "Checkbox", EstaSeleccionado = false },
                        new CampoFormulario { Nombre = "Contenido del Aceite:", Tipo = "Picker", Opciones = new List<string> { "Bidón", "Cisterna", "Sin Aceite", "Fosa"},Valor = "" },
                        new CampoFormulario { Nombre = "Forma de Retirada:", Tipo = "Picker", Opciones = new List<string> { "Cambio", "---", "---", "---"},Valor = "" },
                        new CampoFormulario { Nombre = "Número de Contenedores:", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Tamaño del Vehículo:", Tipo = "Picker", Opciones = new List<string> { "Furgoneta", "Camión Ligero", "Camión Pesado"}, Valor = "" },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "NIMA",
                    ColorBase = "Green",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "NIMA:", Tipo = "Texto", Valor = "", TipoValidacion = "NIMA" },
                        new CampoFormulario { Nombre = "Inscripción (P01/P02):", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Inscripción (P03/P04):", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Actividad Economica:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Froma Jurídica:", Tipo = "Picker", Opciones = new List<string> { "Unipersonal", "Sociedad Colectiva", "Cooperativa", "Sociedad Comanditaria", "Sociedad Limitada", "Sociedad Anónima"}, Valor = "" },
                        new CampoFormulario { Nombre = "Codigo INE:", Tipo = "Texto", Valor = "", TipoValidacion = "INE" },
                        new CampoFormulario { Nombre = "CNAE:", Tipo = "Texto", Valor = "", TipoValidacion = "CNAE" },
                        new CampoFormulario { Nombre = "Gestor Habitual:", Tipo = "Texto", Valor = "Vacio" },
                        new CampoFormulario { Nombre = "Comunidad Autónoma:", Tipo = "Texto", Valor = "Vacio" },
                    }
                },
                new BloqueFormulario
                {
                    Nombre = "Medio Ambiente",
                    ColorBase = "Green",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Responsable:", Tipo = "Texto", Valor = "", TipoValidacion = "Vacio" },
                        new CampoFormulario { Nombre = "Cargo:", Tipo = "Texto", Valor = "", TipoValidacion = "Vacio" },
                        new CampoFormulario { Nombre = "Telefono:", Tipo = "Numero", Valor = "", TipoValidacion = "Telefono" },
                        new CampoFormulario { Nombre = "NIF:", Tipo = "Texto", Valor = "", TipoValidacion="NIF" }
                    }
                },
                                new BloqueFormulario
                {
                    Nombre = "Legalidad",
                    ColorBase = "Green",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Responsable:", Tipo = "Texto", Valor = "", TipoValidacion = "Vacio" },
                        new CampoFormulario { Nombre = "Cargo:", Tipo = "Texto", Valor = "", TipoValidacion = "Vacio" },
                        new CampoFormulario { Nombre = "Telefono:", Tipo = "Numero", Valor = "", TipoValidacion = "Telefono" },
                        new CampoFormulario { Nombre = "NIF:", Tipo = "Texto", Valor = "", TipoValidacion = "NIF" }
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

            // Notificar que el botón "Guardar" debe actualizarse
            CommandManager.InvalidateRequerySuggested();
        }
    }

    }
