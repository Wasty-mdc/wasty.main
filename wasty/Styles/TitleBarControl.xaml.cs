using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace wasty.Styles
{
    public partial class TitleBarControl : UserControl
    {
        private IntPtr _hwnd;
        private HwndSource _hwndSource;
        private Rect _btnMaximizeBoundsScreen = Rect.Empty;
        private bool _isMaximized = false;
        private bool _hoveringMaximize = false;
        private bool _maxButtonPressed = false;
        private bool _restoreBoundsSet = false;
        private double _restoreWidth, _restoreHeight, _restoreLeft, _restoreTop;
        private const int WM_NCHITTEST = 0x0084;
        private const int WM_NCLBUTTONDOWN = 0x00A1;
        private const int WM_NCLBUTTONUP = 0x00A2;
        private const int HTMAXBUTTON = 9;
        private const int HTLEFT = 10, HTRIGHT = 11, HTBOTTOM = 15, HTBOTTOMLEFT = 16, HTBOTTOMRIGHT = 17;
        private const uint MONITOR_DEFAULTTONEAREST = 2;

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT { public int left, top, right, bottom; }

        [StructLayout(LayoutKind.Sequential)]
        private struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public int dwFlags;
        }

        [DllImport("user32.dll")] static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);
        [DllImport("user32.dll", SetLastError = true)] static extern bool GetMonitorInfo(IntPtr hMonitor, ref MONITORINFO lpmi);
        [DllImport("user32.dll")] static extern IntPtr SendMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        public TitleBarControl()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                var window = Window.GetWindow(this);
                _hwnd = new WindowInteropHelper(window).Handle;
                _hwndSource = HwndSource.FromHwnd(_hwnd);
                _hwndSource.AddHook(WndProc);
                BtnMaximize.LayoutUpdated += (_, __) => ActualizarBoundsBotonMaximize();
                window.LocationChanged += (_, __) => ActualizarBoundsBotonMaximize();
                window.SizeChanged += (_, __) => ActualizarBoundsBotonMaximize();
                window.StateChanged += (_, __) => ActualizarBoundsBotonMaximize();
            };
        }

        private void ActualizarBoundsBotonMaximize()
        {
            if (BtnMaximize != null && BtnMaximize.IsLoaded)
            {
                Point topLeft = BtnMaximize.PointToScreen(new Point(0, 0));
                _btnMaximizeBoundsScreen = new Rect(topLeft, new Size(BtnMaximize.ActualWidth, BtnMaximize.ActualHeight));
            }
        }

        private void Minimizar_Click(object sender, RoutedEventArgs e) => Window.GetWindow(this).WindowState = WindowState.Minimized;

        private void MaximizarRestaurar_Click(object sender, RoutedEventArgs e)
        {
            var window = Window.GetWindow(this);

            if (_isMaximized)
            {
                window.Left = _restoreLeft;
                window.Top = _restoreTop;
                window.Width = _restoreWidth;
                window.Height = _restoreHeight;
                MaxRestoreIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowMaximize;
                (window.Content as Border).Effect = new DropShadowEffect { BlurRadius = 20, Color = Colors.Black, Opacity = 0.3, ShadowDepth = 0 };
                _isMaximized = false;
            }
            else
            {
                if (!_restoreBoundsSet)
                {
                    _restoreLeft = window.Left;
                    _restoreTop = window.Top;
                    _restoreWidth = window.Width;
                    _restoreHeight = window.Height;
                    _restoreBoundsSet = true;
                }
                IntPtr monitor = MonitorFromWindow(_hwnd, MONITOR_DEFAULTTONEAREST);
                MONITORINFO mi = new MONITORINFO { cbSize = Marshal.SizeOf(typeof(MONITORINFO)) };
                GetMonitorInfo(monitor, ref mi);
                RECT workArea = mi.rcWork;
                window.Left = workArea.left;
                window.Top = workArea.top;
                window.Width = workArea.right - workArea.left;
                window.Height = workArea.bottom - workArea.top;
                MaxRestoreIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.WindowRestore;
                (window.Content as Border).Effect = null;
                _isMaximized = true;
            }
            ActualizarBoundsBotonMaximize();
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e) => Window.GetWindow(this).Close();

        private void Barra_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2) MaximizarRestaurar_Click(sender, e);
            else Window.GetWindow(this).DragMove();
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            int x = (short)(lParam.ToInt64() & 0xFFFF);
            int y = (short)((lParam.ToInt64() >> 16) & 0xFFFF);
            Point mouse = new Point(x, y);

            if (msg == WM_NCHITTEST)
            {
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
                if (_btnMaximizeBoundsScreen.Contains(mouse))
                {
                    _maxButtonPressed = true;
                    handled = true;
                }
            }
            else if (msg == WM_NCLBUTTONUP)
            {
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
