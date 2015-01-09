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
    /// Interaktionslogik für UC_Kunde.xaml
    /// </summary>
    public partial class UC_Kunde : UserControl
    {

        private readonly KundeViewModel _viewModel = new KundeViewModel();

        public KundeViewModel ViewModel
        {
            get { return _viewModel; }
            
        }

        public UC_Kunde()
        {
            InitializeComponent();
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) KundeViewModel.Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) KundeViewModel.Errors -= 1;
            
        }
    }
}
