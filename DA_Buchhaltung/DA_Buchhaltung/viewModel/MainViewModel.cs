using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DA_Buchhaltung.common.commands;
using DA_Buchhaltung.model;

namespace DA_Buchhaltung.viewModel
{
    public class MainViewModel : ViewModelBase
    {
        //NavigationsProperties
        private Model model = new Model();
        public KundeViewModel kundenViewModel;
        public KreditorViewModel kreditorViewModel;

        //Properties
        private string _currentDate = string.Format("Datum: {0}", DateTime.Now.ToShortDateString());
        public string CurrentDate
        {
            get { return _currentDate; }
            set
            {
                if (value != _currentDate)
                {
                    _currentDate = value;
                    OnPropertyChanged("CurrentDate");
                }
            }
        }
       


        //Commands
       
        private RelayCommand<string> _beendenCommand;
        public RelayCommand<string> BeendenCommand
        {
            get { return _beendenCommand ?? (_beendenCommand = new RelayCommand<string>(Beenden)); }
        }

        private RelayCommand<string> _zeigeKundenCommand;
        public RelayCommand<string> ZeigeKundenCommand
        {
            get { return _zeigeKundenCommand ?? (_zeigeKundenCommand = new RelayCommand<string>(ZeigeKunden)); }
        }

        private RelayCommand<string> _zeigeKreditorCommand;
        public RelayCommand<string> ZeigeKreditorCommand
        {
            get { return _zeigeKreditorCommand ?? (_zeigeKreditorCommand = new RelayCommand<string>(ZeigeKreditor)); }
        }


        //Commandhelper

        private void Beenden(string x)
        {
            Application.Current.Shutdown();
        }

        private void ZeigeKunden(string x)
        {
            kundenViewModel.IstKundenAktiv = true;
            kreditorViewModel.IstKreditorAktiv = false;
        }

        private void ZeigeKreditor(string x)
        {
            kundenViewModel.IstKundenAktiv = false;
            kreditorViewModel.IstKreditorAktiv = true;
        }
        public MainViewModel()
        {
            
        }



    }
}
