using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media.Effects;
using System.Windows.Media;
using wasty.ViewModels;
using System.Windows.Shell;
using System.Windows.Threading;

namespace wasty
{
    public partial class MainWindow : Window
    {
        private IntPtr hwnd;

        // Constantes de mensajes de Windows para interacción personalizada
        private const int WM_NCHITTEST = 0x0084;
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int WM_NCLBUTTONUP = 0x00A2;

        // Constantes para zonas de la ventana
        private const int HTMAXBUTTON = 9;
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;

        private HwndSource _hwndSource;
        private bool _maxButtonPressed = false;
        private bool _hoveringMaximize = false; // Bandera para controlar hover manual

        // Límite del botón maximizar para detectar interacción del sistema
        private Rect _btnMaximizeBoundsScreen = Rect.Empty;
        private bool _isMaximized = false;
        private bool _restoreBoundsSet = false;
        private double _restoreWidth, _restoreHeight, _restoreLeft, _restoreTop;
        private const uint MONITOR_DEFAULTTONEAREST = 2;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int left, top, right, bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public int dwFlags;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);

        // Importaciones para llamadas a API de Windows
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref uint attrValue, int attrSize);

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).Services.GetService(typeof(MainWindowViewModel));

            // Configuraciones y eventos de ventana
            Loaded += MainWindow_Loaded;
            LocationChanged += (_, __) => ActualizarBoundsBotonMaximize();
            SizeChanged += (_, __) =>
            {
                Dispatcher.BeginInvoke(() => ActualizarBoundsBotonMaximize(), DispatcherPriority.Render);
            };
            BtnMaximize.Loaded += (_, __) => ActualizarBoundsBotonMaximize();

            // Lógica para desactivar estado maximizado forzado tras Snap Layout
            StateChanged += (s, e) =>
            {
                Dispatcher.BeginInvoke(() =>
                {
                    ActualizarBoundsBotonMaximize();
                }, DispatcherPriority.Render);
            };


        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            hwnd = new WindowInteropHelper(this).Handle;
            _hwndSource = HwndSource.FromHwnd(hwnd);
            _hwndSource.AddHook(WndProc);
            BtnMaximize.LayoutUpdated += (_, __) => ActualizarBoundsBotonMaximize();
        }

        private void ActualizarBoundsBotonMaximize()
        {
            // Calcula la posición absoluta del botón maximizar para detectar eventos del sistema
            if (BtnMaximize != null && BtnMaximize.IsLoaded)
            {
                Point topLeft = BtnMaximize.PointToScreen(new Point(0, 0));
                _btnMaximizeBoundsScreen = new Rect(topLeft, new Size(BtnMaximize.ActualWidth, BtnMaximize.ActualHeight));
            }
        }

        private void Barra_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Soporta doble clic para maximizar/restaurar y clic simple para mover la ventana
            if (e.ClickCount == 2)
            {
                MaximizarRestaurar_Click(sender, e);
            }
            else
            {
                DragMove();
            }
        }

        private void ReaplicarWindowChrome()
        {
            WindowChrome.SetWindowChrome(this, null);
            WindowChrome.SetWindowChrome(this, new WindowChrome
            {
                CaptionHeight = 40,
                ResizeBorderThickness = new Thickness(8),
                CornerRadius = new CornerRadius(12),
                GlassFrameThickness = new Thickness(0),
                UseAeroCaptionButtons = false
            });
            InvalidateVisual();
        }

        private void FSWindowChrome()
        {
            WindowChrome.SetWindowChrome(this, null);
            WindowChrome.SetWindowChrome(this, new WindowChrome
            {
                CaptionHeight = 40,
                ResizeBorderThickness = new Thickness(0),
                CornerRadius = new CornerRadius(0),
                GlassFrameThickness = new Thickness(0),
                UseAeroCaptionButtons = false
            });
            InvalidateVisual();
        }

        private void Minimizar_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizarRestaurar_Click(object sender, RoutedEventArgs e)
        {
            if (_isMaximized)
            {
                // Restaurar ventana
                Left = _restoreLeft;
                Top = _restoreTop;
                Width = _restoreWidth;
                Height = _restoreHeight;

                MaxRestoreIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
                MainBorder.Effect = new DropShadowEffect
                {
                    BlurRadius = 20,
                    Color = Colors.Black,
                    Opacity = 0.3,
                    ShadowDepth = 0
                };

                _isMaximized = false;
                ReaplicarWindowChrome();

                Dispatcher.BeginInvoke(() =>
                {
                    BtnMaximize.InvalidateVisual();
                    BtnMaximize.UpdateLayout();
                    ActualizarBoundsBotonMaximize(); // <-- REIMPORTANTE
                }, DispatcherPriority.Render);
            }
            else
            {
                if (!_restoreBoundsSet)
                {
                    _restoreLeft = Left - 1;
                    _restoreTop = Top - 1;
                    _restoreWidth = Width - 1;
                    _restoreHeight = Height - 1;
                    _restoreBoundsSet = true;
                }

                // Obtenemos área de trabajo del monitor actual (sin usar Forms)
                IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);
                MONITORINFO monitorInfo = new MONITORINFO();
                monitorInfo.cbSize = Marshal.SizeOf(typeof(MONITORINFO));
                GetMonitorInfo(monitor, ref monitorInfo);
                RECT workArea = monitorInfo.rcWork;

                Left = workArea.left;
                Top = workArea.top;
                Width = workArea.right - workArea.left;
                Height = workArea.bottom - workArea.top;

                MaxRestoreIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
                MainBorder.Effect = null;

                _isMaximized = true;
                FSWindowChrome();
                ActualizarBoundsBotonMaximize();
            }
        }


        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Lógica para cambiar el cursor cuando se pasa por los bordes para redimensionar
            base.OnMouseMove(e);

            Point position = e.GetPosition(this);
            int margin = 10;

            Cursor = Cursors.Arrow;

            bool inTopCornersOrTop = (position.X <= margin && position.Y <= margin) ||
                                      (position.X >= ActualWidth - margin && position.Y <= margin) ||
                                      (position.Y <= margin);

            if (!inTopCornersOrTop)
            {
                if (position.X <= margin && position.Y >= ActualHeight - margin)
                    Cursor = Cursors.SizeNESW;
                else if (position.X >= ActualWidth - margin && position.Y >= ActualHeight - margin)
                    Cursor = Cursors.SizeNWSE;
                else if (position.X <= margin)
                    Cursor = Cursors.SizeWE;
                else if (position.X >= ActualWidth - margin)
                    Cursor = Cursors.SizeWE;
                else if (position.Y >= ActualHeight - margin)
                    Cursor = Cursors.SizeNS;
            }

            if (e.LeftButton == MouseButtonState.Pressed && !inTopCornersOrTop)
            {
                // Lógica para permitir redimensionado desde los bordes
                if (position.X <= margin && position.Y >= ActualHeight - margin)
                    SendMessage(hwnd, WM_NCLBUTTONDOWN, (IntPtr)HTBOTTOMLEFT, IntPtr.Zero);
                else if (position.X >= ActualWidth - margin && position.Y >= ActualHeight - margin)
                    SendMessage(hwnd, WM_NCLBUTTONDOWN, (IntPtr)HTBOTTOMRIGHT, IntPtr.Zero);
                else if (position.X <= margin)
                    SendMessage(hwnd, WM_NCLBUTTONDOWN, (IntPtr)HTLEFT, IntPtr.Zero);
                else if (position.X >= ActualWidth - margin)
                    SendMessage(hwnd, WM_NCLBUTTONDOWN, (IntPtr)HTRIGHT, IntPtr.Zero);
                else if (position.Y >= ActualHeight - margin)
                    SendMessage(hwnd, WM_NCLBUTTONDOWN, (IntPtr)HTBOTTOM, IntPtr.Zero);
            }
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            // Captura mensajes de Windows para personalizar la interacción
            int x = (short)(lParam.ToInt64() & 0xFFFF);
            int y = (short)((lParam.ToInt64() >> 16) & 0xFFFF);
            Point mouse = new Point(x, y);

            if (msg == WM_NCHITTEST)
            {
                // Si el puntero está sobre el botón maximizar, activa hover visual y devuelve zona personalizada
                if (_btnMaximizeBoundsScreen.Contains(mouse))
                {
                    if (!_hoveringMaximize)
                    {
                        BtnMaximize.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333333"));
                        _hoveringMaximize = true;
                    }
                    handled = true;
                    return new IntPtr(HTMAXBUTTON);
                }
                else if (_hoveringMaximize)
                {
                    BtnMaximize.Background = Brushes.Transparent;
                    _hoveringMaximize = false;
                }
            }
            else if (msg == WM_NCLBUTTONDOWN)
            {
                // Al presionar el botón del sistema, marcamos bandera
                if (_btnMaximizeBoundsScreen.Contains(mouse))
                {
                    _maxButtonPressed = true;
                    handled = true;
                }
            }
            else if (msg == WM_NCLBUTTONUP)
            {
                // Al soltar el botón, si sigue dentro del área, ejecutamos el evento
                if (_maxButtonPressed && _btnMaximizeBoundsScreen.Contains(mouse))
                {
                    _maxButtonPressed = false;
                    handled = true;
                    BtnMaximize.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                }
                else
                {
                    _maxButtonPressed = false;
                }
            }

            return IntPtr.Zero;
        }
    }
}