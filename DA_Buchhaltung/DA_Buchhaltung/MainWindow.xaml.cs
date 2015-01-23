using System;
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

namespace DA_Buchhaltung
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel = new MainViewModel();
        public MainViewModel ViewModel
        {
            get { return _viewModel; }
        }

        private readonly KundeViewModel _ucKundeviewModel = new KundeViewModel();
        public KundeViewModel UcKundeViewModel
        {
            get { return _ucKundeviewModel; }
        }

        private readonly KreditorViewModel _ucKreditorviewModel = new KreditorViewModel();
        public KreditorViewModel UcKreditorViewModel
        {
            get { return _ucKreditorviewModel; }
        }

        private readonly RechnungViewModel _ucRechnungviewModel = new RechnungViewModel();
        public RechnungViewModel UcRechnungViewModel
        {
            get { return _ucRechnungviewModel; }
        }

        private readonly EinstellungenViewModel _ucEinstellungenviewModel = new EinstellungenViewModel();
        public EinstellungenViewModel UcEinstellungenViewModel
        {
            get { return _ucEinstellungenviewModel; }
        }

        public MainWindow()
        {
            InitializeComponent();
            //Eigene Naviagtionen
            ViewModel.KreditorViewModel = UcKreditorViewModel;
            ViewModel.KundenViewModel = UcKundeViewModel;
            ViewModel.EinstellungenViewModel = UcEinstellungenViewModel;
            ViewModel.RechnungViewModel = UcRechnungViewModel;

            //Hilfsnavigationen für Childs
            ViewModel.KreditorViewModel.RechnungViewModel = UcRechnungViewModel;

        }

    }
}
