using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using wasty.ViewModels;

namespace wasty.Utils
{
    public partial class ImportDialog : Window
    {
        public List<string> DroppedFiles { get; private set; } = new List<string>();

        public ImportDialog()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).Services.GetService(typeof(ImportDialogViewModel));
        }

        private void Window_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                DroppedFiles.Clear();
                FilesListBox.Items.Clear();

                foreach (var file in files)
                {
                    DroppedFiles.Add(file);
                    FilesListBox.Items.Add(System.IO.Path.GetFileName(file));
                }

                // Mostrar lista y botón
                FilePanel.Visibility = Visibility.Visible;
            }
        }

        private void Window_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Copy;
            e.Handled = true;
        }

        private void AcceptButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
