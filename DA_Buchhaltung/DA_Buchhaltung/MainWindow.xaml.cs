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
    /// Verwaltet alle Viewmodels und deren Beziehungen (zB. Kunde -> Auftrag)
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

        private readonly ErfolgsrechnungViewModel _ucErfolgsrechnungViewModel = new ErfolgsrechnungViewModel();
        public ErfolgsrechnungViewModel UcErfolgsrechnungViewModel
        {
            get { return _ucErfolgsrechnungViewModel; }
        }

        private readonly AuftragViewModel _ucAuftragViewModel = new AuftragViewModel();
        public AuftragViewModel UcAuftragViewModel
        {
            get { return _ucAuftragViewModel; }
        }

        private readonly KalenderViewModel _ucKalenderViewModel = new KalenderViewModel();
        public KalenderViewModel UcKalenderViewModel
        {
            get { return _ucKalenderViewModel; }
        }

        public MainWindow()
        {
            InitializeComponent();
            //Eigene Naviagtionen
            ViewModel.KreditorenViewModel = UcKreditorViewModel;
            ViewModel.KundeViewModel = UcKundeViewModel;
            ViewModel.EinstellungViewModel = UcEinstellungenViewModel;
            ViewModel.RechnungsViewModel = UcRechnungViewModel;
            ViewModel.ErfolgsrechnungsViewModel = UcErfolgsrechnungViewModel;
            ViewModel.AuftragsViewModel = UcAuftragViewModel;
            ViewModel.KalenderViewmodel = UcKalenderViewModel;

            //Hilfsnavigationen für Childs
            ViewModel.KreditorenViewModel.RechnungViewModel = UcRechnungViewModel;
            ViewModel.KundeViewModel.AuftragViewModel = UcAuftragViewModel;

        }

    }
}
