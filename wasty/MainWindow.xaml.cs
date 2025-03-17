using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using wasty.ViewModels;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Net.Http;
using System.Windows.Controls;

namespace wasty
{
    public partial class MainWindow : Window
    {
        private const int DWMWA_CAPTION_COLOR = 35; // Cambia el color de la barra de título
        private const int DWMWA_TEXT_COLOR = 34; // Cambia el color del borde de la ventana
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20; // Forzar modo oscuro en Windows 11s

        private IntPtr hwnd;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref uint attrValue, int attrSize);
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        
        public MainWindow()
        {
            InitializeComponent();
            // Establecer el DataContext al MainWindowViewModel
            DataContext = ((App)Application.Current).Services.GetService(typeof(MainWindowViewModel));
            Loaded += MainWindow_Loaded;

        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;

            uint titleBarColor = 0xFF000000; // Negro (ARGB: AARRGGBB)
            uint textColor = 0xFFFFFFFF; // Blanco (ARGB: AARRGGBB)

            // Cambiar el color de la barra de título
            DwmSetWindowAttribute(hwnd, DWMWA_CAPTION_COLOR, ref titleBarColor, sizeof(uint));

            // Cambiar el color del texto en la barra de título
            DwmSetWindowAttribute(hwnd, DWMWA_TEXT_COLOR, ref textColor, sizeof(uint));

            int useDarkMode = 1; // IMPORTANTE: Esto es INT, no UINT
            DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDarkMode, sizeof(int));
        }
    }
}


