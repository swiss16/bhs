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
    /// Interaktionslogik für UC_Erfolgsrechnung.xaml
    /// </summary>
    public partial class UC_Erfolgsrechnung : UserControl
    {

        private readonly ErfolgsrechnungViewModel _viewModel = new ErfolgsrechnungViewModel();

        public ErfolgsrechnungViewModel ViewModel
        {
            get { return _viewModel; }

        }

        public UC_Erfolgsrechnung()
        {
            InitializeComponent();
        }
    }
}
