﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using wasty.Services;
using wasty.ViewModels;

namespace wasty.Views
{
    /// <summary>
    /// Lógica de interacción para TreatmentsView.xaml
    /// </summary>
    public partial class TreatmentsView : UserControl
    {
        public TreatmentsView()
        {
            InitializeComponent();
            DataContext = ((App)Application.Current).Services.GetService(typeof(TreatmentsViewModel));


        }
    }
}
