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
    /// Interaktionslogik für UC_Auftrag.xaml
    /// </summary>
    public partial class UC_Auftrag : UserControl
    {

        private readonly AuftragViewModel _viewModel = new AuftragViewModel();

        public AuftragViewModel ViewModel
        {
            get { return _viewModel; }
            
        }

        public UC_Auftrag()
        {
            InitializeComponent();
        }

        private void Validation_Error(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Added) AuftragViewModel.Errors += 1;
            if (e.Action == ValidationErrorEventAction.Removed) AuftragViewModel.Errors -= 1;
            
        }
        
    }
}
