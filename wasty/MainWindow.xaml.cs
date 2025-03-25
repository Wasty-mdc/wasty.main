using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using Microsoft.Win32;
using wasty.ViewModels;

namespace wasty
{
    public partial class MainWindow : Window
    {
        private const int DWMWA_CAPTION_COLOR = 35;
        private const int DWMWA_TEXT_COLOR = 34;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        private IntPtr hwnd;

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref uint attrValue, int attrSize);
        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        public MainWindow()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).Services.GetService(typeof(MainWindowViewModel));

            Loaded += MainWindow_Loaded;
            Unloaded += MainWindow_Unloaded;

            SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            hwnd = new WindowInteropHelper(this).Handle;
            ApplyWindowTheme();
        }

        private void MainWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;
        }

        private void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
        {
            if (e.Category == UserPreferenceCategory.General)
            {
                Dispatcher.Invoke(() => ApplyWindowTheme());
            }
        }

        private void ApplyWindowTheme()
        {
            bool isDark = ThemeHelper.IsDarkThemeEnabled();

            uint titleBarColor = isDark ? 0xFF000000 : 0xFFFFFFFF;
            uint textColor = isDark ? 0xFFFFFFFF : 0xFF000000;
            int useDarkMode = isDark ? 1 : 0;

            DwmSetWindowAttribute(hwnd, DWMWA_CAPTION_COLOR, ref titleBarColor, sizeof(uint));
            DwmSetWindowAttribute(hwnd, DWMWA_TEXT_COLOR, ref textColor, sizeof(uint));
            DwmSetWindowAttribute(hwnd, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDarkMode, sizeof(int));
        }
    }

    public static class ThemeHelper
    {
        public static bool IsDarkThemeEnabled()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    if (key != null)
                    {
                        object registryValueObject = key.GetValue("AppsUseLightTheme");
                        if (registryValueObject != null)
                        {
                            int registryValue = (int)registryValueObject;
                            return registryValue == 0; // 0 = oscuro, 1 = claro
                        }
                    }
                }
            }
            catch
            {
                // Si falla, asumimos tema claro
            }

            return false;
        }
    }
}
