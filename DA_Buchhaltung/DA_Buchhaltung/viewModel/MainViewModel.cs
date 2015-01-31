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
        public KundeViewModel KundeViewModel;
        public KreditorViewModel KreditorenViewModel;
        public EinstellungenViewModel EinstellungViewModel;
        public RechnungViewModel RechnungsViewModel;
        public ErfolgsrechnungViewModel ErfolgsrechnungsViewModel;
        public AuftragViewModel AuftragsViewModel;


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

        private SimpleCommand _beendenCommand;
        public SimpleCommand BeendenCommand
        {
            get { return _beendenCommand ?? (_beendenCommand = new SimpleCommand(Beenden)); }
        }

        private SimpleCommand _zeigeKundenCommand;
        public SimpleCommand ZeigeKundenCommand
        {
            get { return _zeigeKundenCommand ?? (_zeigeKundenCommand = new SimpleCommand(ZeigeKunden)); }
        }

        private SimpleCommand _zeigeKreditorCommand;
        public SimpleCommand ZeigeKreditorCommand
        {
            get { return _zeigeKreditorCommand ?? (_zeigeKreditorCommand = new SimpleCommand(ZeigeKreditor)); }
        }

        private SimpleCommand _zeigeEinstellungenCommand;
        public SimpleCommand ZeigeEinstellungenCommand
        {
            get { return _zeigeEinstellungenCommand ?? (_zeigeEinstellungenCommand = new SimpleCommand(ZeigeEinstellungen)); }
        }

        private SimpleCommand _zeigeRechnungenCommand;
        public SimpleCommand ZeigeRechnungenCommand
        {
            get { return _zeigeRechnungenCommand ?? (_zeigeRechnungenCommand = new SimpleCommand(ZeigeRechnungen)); }
        }

        private SimpleCommand _zeigeRueckzahlungenCommand;
        public SimpleCommand ZeigeRueckzahlungenCommand
        {
            get { return _zeigeRueckzahlungenCommand ?? (_zeigeRueckzahlungenCommand = new SimpleCommand(ZeigeRueckzahlungen)); }
        }

        private SimpleCommand _zeigeErfolgsrechnungCommand;
        public SimpleCommand ZeigeErfolgsrechnungCommand
        {
            get { return _zeigeErfolgsrechnungCommand ?? (_zeigeErfolgsrechnungCommand = new SimpleCommand(ZeigeErfolgsrechnung)); }
        }

        private SimpleCommand _zeigeAuftragCommand;
        public SimpleCommand ZeigeAuftragCommand
        {
            get { return _zeigeAuftragCommand ?? (_zeigeAuftragCommand = new SimpleCommand(ZeigeAuftrag)); }
        }


        //Commandhelper

        private void Beenden()
        {
            Application.Current.Shutdown();
        }

        private void ZeigeKunden()
        {
            KundeViewModel.IstKundenAktiv = true;
            KreditorenViewModel.IstKreditorAktiv = false;
            RechnungsViewModel.IstRechnungAktiv = false;
            RechnungsViewModel.IstRueckzahlungAktiv = false;
            EinstellungViewModel.IstEinstellungenAktiv = false;
            ErfolgsrechnungsViewModel.IstErfolgsrechnungAktiv = false;
            AuftragsViewModel.IstAuftragAktiv = false;
        }

        private void ZeigeKreditor()
        {
            KundeViewModel.IstKundenAktiv = false;
            KreditorenViewModel.IstKreditorAktiv = true;
            RechnungsViewModel.IstRechnungAktiv = false;
            RechnungsViewModel.IstRueckzahlungAktiv = false;
            EinstellungViewModel.IstEinstellungenAktiv = false;
            ErfolgsrechnungsViewModel.IstErfolgsrechnungAktiv = false;
            AuftragsViewModel.IstAuftragAktiv = false;
        }

        private void ZeigeEinstellungen()
        {
            KundeViewModel.IstKundenAktiv = false;
            KreditorenViewModel.IstKreditorAktiv = false;
            RechnungsViewModel.IstRechnungAktiv = false;
            RechnungsViewModel.IstRueckzahlungAktiv = false;
            EinstellungViewModel.IstEinstellungenAktiv = true;
            ErfolgsrechnungsViewModel.IstErfolgsrechnungAktiv = false;
            AuftragsViewModel.IstAuftragAktiv = false;
        }

        private void ZeigeRechnungen()
        {
            KundeViewModel.IstKundenAktiv = false;
            KreditorenViewModel.IstKreditorAktiv = true;
            RechnungsViewModel.IstRechnungAktiv = true;
            RechnungsViewModel.IstRueckzahlungAktiv = false;
            EinstellungViewModel.IstEinstellungenAktiv = false;
            ErfolgsrechnungsViewModel.IstErfolgsrechnungAktiv = false;
            AuftragsViewModel.IstAuftragAktiv = false;
        }
        private void ZeigeRueckzahlungen()
        {
            KundeViewModel.IstKundenAktiv = false;
            KreditorenViewModel.IstKreditorAktiv = true;
            RechnungsViewModel.IstRechnungAktiv = false;
            RechnungsViewModel.IstRueckzahlungAktiv = true;
            EinstellungViewModel.IstEinstellungenAktiv = false;
            ErfolgsrechnungsViewModel.IstErfolgsrechnungAktiv = false;
            AuftragsViewModel.IstAuftragAktiv = false;
        }
        private void ZeigeErfolgsrechnung()
        {
            KundeViewModel.IstKundenAktiv = false;
            KreditorenViewModel.IstKreditorAktiv = false;
            RechnungsViewModel.IstRechnungAktiv = false;
            RechnungsViewModel.IstRueckzahlungAktiv = false;
            EinstellungViewModel.IstEinstellungenAktiv = false;
            ErfolgsrechnungsViewModel.IstErfolgsrechnungAktiv = true;
            AuftragsViewModel.IstAuftragAktiv = false;
        }

        private void ZeigeAuftrag()
        {
            KundeViewModel.IstKundenAktiv = true;
            KreditorenViewModel.IstKreditorAktiv = false;
            RechnungsViewModel.IstRechnungAktiv = false;
            RechnungsViewModel.IstRueckzahlungAktiv = false;
            EinstellungViewModel.IstEinstellungenAktiv = false;
            ErfolgsrechnungsViewModel.IstErfolgsrechnungAktiv = false;
            AuftragsViewModel.IstAuftragAktiv = true;
        }

        public MainViewModel()
        {
            
        }



    }
}
