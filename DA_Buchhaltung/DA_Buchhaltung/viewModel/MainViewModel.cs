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
        Model model = new Model();

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
       

        //Sichtbarkeits Properties
        private bool _istKundenAktiv = true;
        public bool IstKundenAktiv
        {
            get { return _istKundenAktiv; }
            set
            {
                if (value != _istKundenAktiv)
                {
                    _istKundenAktiv = value;
                    OnPropertyChanged("IstKundenAktiv");
                }
            }
        }
        private bool _istKreditorAktiv = true;
        public bool IstKreditorAktiv
        {
            get { return _istKreditorAktiv; }
            set
            {
                if (value != _istKreditorAktiv)
                {
                    _istKreditorAktiv = value;
                    OnPropertyChanged("IstKreditorAktiv");
                }
            }
        }

        //Commands
       
        private RelayCommand<string> _beendenCommand;
        public RelayCommand<string> BeendenCommand
        {
            get { return _beendenCommand ?? (_beendenCommand = new RelayCommand<string>(Beenden)); }
        }



        //Commandhelper

        private void Beenden(string x)
        {
            Application.Current.Shutdown();
        }

        public MainViewModel()
        {
            
        }



    }
}
