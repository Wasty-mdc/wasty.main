using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using wasty.Models;
using wasty.Utils;
using wasty.Services;
using System.Text.Json;
using System.IO;


namespace wasty.ViewModels
    {
    public class FormularioViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _grupoPrincipalSeleccionado;
        private bool _grupoSecundarioHabilitado;
        private ObservableCollection<string> _opcionesGrupoSecundario;

        public ObservableCollection<BloqueFormulario> Bloques { get; set; }
        public ObservableCollection<BloqueFormulario> BloquesPaginaActual { get; set; }

        public ICommand AgregarPaginaCommand { get; }
        public ICommand ExportarJsonCommand { get; }
        public ICommand SiguientePaginaCommand { get; }
        public ICommand AnteriorPaginaCommand { get; }

        private int _paginaActual = 1;
        public int PaginaActual
        {
            get => _paginaActual;
            set { _paginaActual = value; OnPropertyChanged(); ActualizarVistaPagina(); }
        }
        public int TotalPaginas => Bloques.Max(b => b.NumeroPagina);
        public List<string> OpcionesGrupoPrincipal { get; } = new()
        {
            "Talleres", "Planchas", "Gestores", "Industrias", "Talleres Aceite", "Sin asignar"
        };

        public ObservableCollection<string> OpcionesGrupoSecundario
        {
            get => _opcionesGrupoSecundario;
            set { _opcionesGrupoSecundario = value; OnPropertyChanged(); }
        }

        public string GrupoPrincipalSeleccionado
        {
            get => _grupoPrincipalSeleccionado;
            set { _grupoPrincipalSeleccionado = value; OnPropertyChanged(); ActualizarOpcionesGrupoSecundario(); }
        }

        public bool GrupoSecundarioHabilitado
        {
            get => _grupoSecundarioHabilitado;
            set { _grupoSecundarioHabilitado = value; OnPropertyChanged(); }
        }

        public ICommand SeleccionarGrupoCommand { get; }

        public FormularioViewModel()
        {
            Bloques = new ObservableCollection<BloqueFormulario>(ObtenerBloquesIniciales());
            BloquesPaginaActual = new ObservableCollection<BloqueFormulario>();

            AgregarPaginaCommand = new RelayCommand(_ => DuplicarUltimaPagina());
            ExportarJsonCommand = new RelayCommand(async _ => await ExportarFormularioAsync());
            SiguientePaginaCommand = new RelayCommand(_ => { if (PaginaActual < TotalPaginas) PaginaActual++; });
            AnteriorPaginaCommand = new RelayCommand(_ => { if (PaginaActual > 1) PaginaActual--; });

            OpcionesGrupoSecundario = new ObservableCollection<string>();
            SeleccionarGrupoCommand = new RelayCommand(_ => ActualizarOpcionesGrupoSecundario());

            ActualizarVistaPagina();
        }

        private void ActualizarVistaPagina()
        {
            var visibles = Bloques.Where(b => b.NumeroPagina == PaginaActual).ToList();
            BloquesPaginaActual.Clear();
            foreach (var b in visibles)
                BloquesPaginaActual.Add(b);
            OnPropertyChanged(nameof(BloquesPaginaActual));
            OnPropertyChanged(nameof(PaginaActual));
            OnPropertyChanged(nameof(TotalPaginas));
        }

        private void DuplicarUltimaPagina()
        {
            int nuevaPagina = TotalPaginas + 1;
            var ultimos = Bloques.Where(b => b.NumeroPagina == TotalPaginas).ToList();

            foreach (var original in ultimos)
            {
                var copia = new BloqueFormulario
                {
                    Nombre = original.Nombre + " (Copia)",
                    ColorBase = original.ColorBase,
                    NumeroPagina = nuevaPagina,
                    Campos = new ObservableCollection<CampoFormulario>(original.Campos.Select(c => new CampoFormulario
                    {
                        Nombre = c.Nombre,
                        Tipo = c.Tipo,
                        Valor = "",
                        EstaSeleccionado = false,
                        Opciones = c.Opciones?.ToList(),
                        TipoValidacion = c.TipoValidacion
                    }))
                };
                Bloques.Add(copia);
            }

            PaginaActual = nuevaPagina;
        }

        private async Task ExportarFormularioAsync()
        {
            try
            {
                var json = JsonSerializer.Serialize(Bloques, new JsonSerializerOptions { WriteIndented = true });
                var rutaArchivo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "FormularioExportado.json");
                await File.WriteAllTextAsync(rutaArchivo, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el archivo JSON: {ex.Message}");
            }
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            CommandManager.InvalidateRequerySuggested();
        }

        private void ActualizarOpcionesGrupoSecundario()
        {
            OpcionesGrupoSecundario.Clear();

            switch (GrupoPrincipalSeleccionado)
            {
                case "Talleres":
                    OpcionesGrupoSecundario = new ObservableCollection<string>
                    {
                        "Mecánico", "Aceite", "Camiones", "Lavadero", "Mecánico y Planchas",
                        "Motos", "Plancha y Pintura", "Plancha, Pintura y Mecánico", "Transportes"
                    };
                    GrupoSecundarioHabilitado = true;
                    break;
                case "Planchas":
                    GrupoSecundarioHabilitado = false;
                    break;
                case "Gestores":
                    OpcionesGrupoSecundario = new ObservableCollection<string>
                    {
                        "Aceite Gestor", "Cartón", "Lodos", "Metales", "Palets", "Plástico",
                        "Residuos Peligrosos", "Residuos Voluminosos", "Vidrio"
                    };
                    GrupoSecundarioHabilitado = true;
                    break;
                case "Industrias":
                    OpcionesGrupoSecundario = new ObservableCollection<string>
                    {
                        "Aceite Industrial", "Cartón", "Contenedores", "Curtido Pieles",
                        "Material Quirúrgico", "Metales", "Plástico", "Otros"
                    };
                    GrupoSecundarioHabilitado = true;
                    break;
                case "Talleres Aceite":
                    GrupoSecundarioHabilitado = false;
                    break;
                case "Sin asignar":
                    OpcionesGrupoSecundario = new ObservableCollection<string> { "Sin asignar" };
                    GrupoSecundarioHabilitado = true;
                    break;
            }
        }

        private List<BloqueFormulario> ObtenerBloquesIniciales()
        {
            return new List<BloqueFormulario>
            {
                new BloqueFormulario
                {
                    Nombre = "Datos Clientes",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Prioridad del Cliente", Tipo = "Picker", Opciones = new List<string> { "1", "2", "3", "4", "5" }, Valor = "" },
                        new CampoFormulario { Nombre = "NIF", Tipo = "Texto", Valor = "" , TipoValidacion = "NIF"},
                        new CampoFormulario { Nombre = "Nombre Fiscal", Tipo = "Texto", Valor = "", TipoValidacion="Vacio"},
                        new CampoFormulario { Nombre = "Nombre Comercial", Tipo = "Texto", Valor = "", TipoValidacion = "Vacio" },
                    },
                    IsFirst = true,
                    ColorBase = "Yellow",
                    NumeroPagina = 1
                },
                /*new BloqueFormulario
                {
                    Nombre = "Online",
                    ColorBase = "Yellow",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Contraseña Online", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Acceso Online", Tipo = "Checkbox", EstaSeleccionado = false },
                    },
                    NumeroPagina = 1
                },*/
                new BloqueFormulario
                {
                    Nombre = "Contacto",
                    ColorBase = "Yellow",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario{Nombre="Teléfono", Tipo="Texto", Valor = ""},
                        new CampoFormulario{Nombre="Teléfono Móvil", Tipo="Texto", Valor=""},
                        new CampoFormulario{Nombre="Email", Tipo="Texto", Valor="", TipoValidacion="Email"}
                    },
                    NumeroPagina = 1
                },
                new BloqueFormulario
                {
                    Nombre = "Comercial",
                    ColorBase = "Yellow",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Persona de Contacto:", Tipo = "Texto", Valor = ""},
                        new CampoFormulario { Nombre = "DNI:", Tipo = "Texto", Opciones = OpcionesGrupoPrincipal, Valor = "", TipoValidacion = "DNI" },
                        new CampoFormulario { Nombre = "Cargo:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Comercial:", Tipo = "Picker", Opciones = new List<string> { "1", "2", "3", "4", "5" }, Valor = "" }
                    },
                    NumeroPagina = 1
                },
                new BloqueFormulario
                {
                    Nombre = "Ubicación",
                    ColorBase = "Yellow",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Dirección Fiscal:", Tipo = "Texto", Valor = "" , TipoValidacion = "Vacio"},
                        new CampoFormulario { Nombre = "Dirección de Recogida:", Tipo = "Texto", Valor = "" , TipoValidacion = "Vacio"},
                        new CampoFormulario { Nombre = "Código Postal:", Tipo = "Número", Valor = "" , TipoValidacion = "Vacio"},
                        new CampoFormulario { Nombre = "Población:", Tipo = "Texto", Valor = "" , TipoValidacion = "Vacio"},
                        new CampoFormulario { Nombre = "Provincia:", Tipo = "Texto", Valor = "" , TipoValidacion = "Vacio"},
                        new CampoFormulario { Nombre = "País:", Tipo = "Texto", Valor = "" , TipoValidacion = "Vacio"}
                    },
                    NumeroPagina = 1
                },
                new BloqueFormulario
                {
                    Nombre = "Observaciones",
                    ColorBase = "#9A94BC",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Observaciones:", Tipo = "Texto", Valor = "" }
                    },
                    NumeroPagina = 1
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
                        new CampoFormulario { Nombre = "Importe:", Tipo = "Número", Valor = "", TipoValidacion="Vacio" },
                        new CampoFormulario { Nombre = "IVA:", Tipo = "Picker", Opciones = new List<string> { "0%", "4%", "8%", "10%", "18%", "21%" }, Valor = "" },
                        new CampoFormulario { Nombre = "Fecha de Primer Cobro:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Periodicidad:", Tipo = "Picker", Opciones = new List<string> { "Mensual", "Bimensual", "Trimestral", "Semestral", "Anual"}, Valor = "" },
                        new CampoFormulario { Nombre = "Cobro a Fin Periodo", Tipo = "Checkbox",  EstaSeleccionado = false},
                        new CampoFormulario { Nombre = "No Cobrar (Marca de Rojo)", Tipo = "Checkbox", EstaSeleccionado = false },
                    },
                    NumeroPagina = 2
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
                    },
                    NumeroPagina = 2
                },
                new BloqueFormulario
                {
                    Nombre = "NIMA",
                    ColorBase = "Green",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "NIMA:", Tipo = "Texto", Valor = "", TipoValidacion = "NIMA" },
                        new CampoFormulario { Nombre = "Cod. Peligrosos:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Nom. Peligrosos:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Cod. No Peligrosos:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Nom. No Peligrosos:",  Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Actividad Económica:", Tipo = "Texto", Valor = "", TipoValidacion = "INE" },
                        new CampoFormulario { Nombre = "Forma Jurídica:", Tipo = "Texto", Valor = "", TipoValidacion = "CNAE" },
                        new CampoFormulario { Nombre = "Código INE:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Gestor Habitual:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Comunidad Autónoma:", Tipo = "Texto", Valor = "" },
                    },
                    NumeroPagina = 3
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
                    },
                    NumeroPagina = 3
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
                    },
                    NumeroPagina = 3
                },
                new BloqueFormulario
                {
                    Nombre = "Datos Centro",
                    ColorBase = "Green",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Fecha de Alta:", Tipo = "Fecha", Valor = "", TipoValidacion = "Vacio" },
                        new CampoFormulario { Nombre = "Fecha de Baja:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Lista de Correo", Tipo = "Checkbox", EstaSeleccionado = false },
                        new CampoFormulario { Nombre = "Cliente Gestor", Tipo = "Checkbox", EstaSeleccionado = false },
                        new CampoFormulario { Nombre = "Fevauto", Tipo = "Checkbox", EstaSeleccionado = false },
                        new CampoFormulario { Nombre = "Cliente Esporádico", Tipo = "Checkbox", EstaSeleccionado = false }
                    },
                    NumeroPagina = 3
                },
                new BloqueFormulario
                {
                    Nombre = "Horario del Centro",
                    ColorBase = "Green",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Horario de Mañana (Desde - Hasta)", Tipo = "Número", Valor = ""},
                        new CampoFormulario { Nombre = "Horario de Tarde (Desde - Hasta)", Tipo = "Número", Valor = "" },
                    },
                    NumeroPagina = 3
                },
                new BloqueFormulario
                {
                    Nombre = "Contacto del Centro",
                    ColorBase = "Green",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Nombre del Centro:", Tipo = "Texto", Valor = ""},
                        new CampoFormulario { Nombre = "Direccion del Centro:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Código Postal:", Tipo = "Texto", Valor = ""},
                        new CampoFormulario { Nombre = "Población:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Provincia:", Tipo = "Texto", Valor = ""},
                        new CampoFormulario { Nombre = "Teléfono:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Email:", Tipo = "Texto", Valor = "", TipoValidacion="Email"}
                    },
                    NumeroPagina = 3
                },
                new BloqueFormulario
                {
                    Nombre = "Datos de Recogidas",
                    ColorBase = "Green",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Dirección de Recogida:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Código Postal de Recogida:", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Población de Recogida:", Tipo = "Texto", Valor = "" },
                    },
                    NumeroPagina = 3
                },
                new BloqueFormulario
                {
                    Nombre = "Horario de Recogida",
                    ColorBase = "Green",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Horario de Recogida (Desde - Hasta)", Tipo = "Número", Valor = ""}
                    },
                    NumeroPagina = 3
                },
                new BloqueFormulario
                {
                    Nombre = "Recogidas No Peligrosas",
                    ColorBase = "Green",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Fecha Última Retirada:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Dias Entre Cada Retirada:", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Alarma:", Tipo = "Checkbox", EstaSeleccionado = false },
                    },
                    NumeroPagina = 3
                },
                new BloqueFormulario
                {
                    Nombre = "Recogidas Peligrosas",
                    ColorBase = "Green",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Fecha Última Retirada:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Dias Entre Cada Retirada:", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Alarma:", Tipo = "Checkbox", EstaSeleccionado = false },
                    },
                    NumeroPagina = 3
                },
                new BloqueFormulario
                {
                    Nombre = "Recogidas Especiales",
                    ColorBase = "Green",
                    Campos = new ObservableCollection<CampoFormulario>
                    {
                        new CampoFormulario { Nombre = "Fecha Última Retirada:", Tipo = "Fecha", Valor = "" },
                        new CampoFormulario { Nombre = "Dias Entre Cada Retirada:", Tipo = "Texto", Valor = "" },
                        new CampoFormulario { Nombre = "Alarma:", Tipo = "Checkbox", EstaSeleccionado = false },
                        new CampoFormulario { Nombre = "Contedro Especial:", Tipo = "Picker", Opciones = new List<string> { "Bidón", "Cisterna", "Sin Aceite", "Fosa"},Valor = "" },
                        new CampoFormulario { Nombre = "Forma de Retirada:", Tipo = "Picker", Opciones = new List<string> { "Cambio", "---", "---", "---"},Valor = "" },
                        new CampoFormulario { Nombre = "Número de Contenedores:", Tipo = "Número", Valor = "" },
                        new CampoFormulario { Nombre = "Tamaño del Vehículo:", Tipo = "Picker", Opciones = new List<string> { "Furgoneta", "Camión Ligero", "Camión Pesado"}, Valor = "" },
                    },
                    NumeroPagina = 3
                }
            };
        }
    }

    }
