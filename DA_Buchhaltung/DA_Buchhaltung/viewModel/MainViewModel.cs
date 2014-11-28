using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA_Buchhaltung.common.commands;
using DA_Buchhaltung.model;

namespace DA_Buchhaltung.viewModel
{
    public class MainViewModel : ViewModelBase
    {
        //NavigationsProperties
        Model model = new Model();

        //Properties
        public Kunde AktuellerKunde { get; set; }
        public List<Kunde> KundenListe { get; set; }
        public int SelectedKundenIndex { get; set; }


        //Commands
        private RelayCommand<bool> _ladeKundenCommand;
        public RelayCommand<bool> LadeKundenCommand
        {
            get { return _ladeKundenCommand ?? (_ladeKundenCommand = new RelayCommand<bool>(ladeKunden)); }
        }



        //Commandhelper
        private void ladeKunden(bool isTrue)
        {
            KundenListe = model.LadeKunden();
        }





    }
}
