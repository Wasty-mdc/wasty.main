using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using wasty.ViewModels;
using System.Windows.Media;

namespace wasty
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Establecer el DataContext al MainWindowViewModel
            DataContext = ((App)Application.Current).Services.GetService(typeof(MainWindowViewModel));
            ApplyModernTitleBar();
            ApplyAcrylicEffect();
        }

        private void ApplyModernTitleBar()
        {
            IntPtr hWnd = new WindowInteropHelper(this).Handle;
            int attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
            int useDarkMode = 1; // 1 = Oscuro, 0 = Claro

            DwmSetWindowAttribute(hWnd, attribute, ref useDarkMode, sizeof(int));
        }

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int dwAttribute, ref int pvAttribute, int cbAttribute);


        private void ApplyAcrylicEffect()
        {
            IntPtr hWnd = new WindowInteropHelper(this).Handle;
            int micaEffect = 1; // 1 = Activar, 0 = Desactivar
            DwmSetWindowAttribute(hWnd, 1029, ref micaEffect, sizeof(int)); // 1029 = DWM Mica
        }


    }
}
