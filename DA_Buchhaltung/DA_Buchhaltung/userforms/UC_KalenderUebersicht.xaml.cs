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
using DA_Buchhaltung.viewModel;

namespace DA_Buchhaltung.userforms
{
    /// <summary>
    /// Interaktionslogik für UC_KalenderUebersicht.xaml
    /// </summary>
    public partial class UC_KalenderUebersicht : UserControl
    {
        private readonly KalenderViewModel _viewModel = new KalenderViewModel();

        public KalenderViewModel ViewModel
        {
            get { return _viewModel; }

        }

        public UC_KalenderUebersicht()
        {
            InitializeComponent();
        }
    }
}
