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

namespace DA_Buchhaltung.userforms
{
    /// <summary>
    /// Interaktionslogik für UC_Rechnung.xaml
    /// </summary>
    public partial class UC_Rechnung : UserControl
    {
        private readonly RechnungViewModel _viewModel = new RechnungViewModel();

        public RechnungViewModel ViewModel
        {
            get { return _viewModel; }
            
        }

        public UC_Rechnung()
        {
            InitializeComponent();
        }

        private void Validation_Rechnung_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) RechnungViewModel.RechnungErrors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) RechnungViewModel.RechnungErrors -= 1;

        }
        private void Validation_Rueckzahlung_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) RechnungViewModel.RueckzahlungErrors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) RechnungViewModel.RueckzahlungErrors -= 1;

        }
    }
}
