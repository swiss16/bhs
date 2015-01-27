﻿using System;
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
        public KundeViewModel KundenViewModel;
        public KreditorViewModel KreditorViewModel;
        public EinstellungenViewModel EinstellungenViewModel;
        public RechnungViewModel RechnungViewModel;
        public ErfolgsrechnungViewModel ErfolgsrechnungViewModel;


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


        //Commandhelper

        private void Beenden()
        {
            Application.Current.Shutdown();
        }

        private void ZeigeKunden()
        {
            KundenViewModel.IstKundenAktiv = true;
            KreditorViewModel.IstKreditorAktiv = false;
            RechnungViewModel.IstRechnungAktiv = false;
            RechnungViewModel.IstRueckzahlungAktiv = false;
            EinstellungenViewModel.IstEinstellungenAktiv = false;
            ErfolgsrechnungViewModel.IstErfolgsrechnungAktiv = false;
        }

        private void ZeigeKreditor()
        {
            KundenViewModel.IstKundenAktiv = false;
            KreditorViewModel.IstKreditorAktiv = true;
            RechnungViewModel.IstRechnungAktiv = false;
            RechnungViewModel.IstRueckzahlungAktiv = false;
            EinstellungenViewModel.IstEinstellungenAktiv = false;
            ErfolgsrechnungViewModel.IstErfolgsrechnungAktiv = false;
        }

        private void ZeigeEinstellungen()
        {
            KundenViewModel.IstKundenAktiv = false;
            KreditorViewModel.IstKreditorAktiv = false;
            RechnungViewModel.IstRechnungAktiv = false;
            RechnungViewModel.IstRueckzahlungAktiv = false;
            EinstellungenViewModel.IstEinstellungenAktiv = true;
            ErfolgsrechnungViewModel.IstErfolgsrechnungAktiv = false;
        }

        private void ZeigeRechnungen()
        {
            KundenViewModel.IstKundenAktiv = false;
            KreditorViewModel.IstKreditorAktiv = true;
            RechnungViewModel.IstRechnungAktiv = true;
            RechnungViewModel.IstRueckzahlungAktiv = false;
            EinstellungenViewModel.IstEinstellungenAktiv = false;
            ErfolgsrechnungViewModel.IstErfolgsrechnungAktiv = false;
        }
        private void ZeigeRueckzahlungen()
        {
            KundenViewModel.IstKundenAktiv = false;
            KreditorViewModel.IstKreditorAktiv = true;
            RechnungViewModel.IstRechnungAktiv = false;
            RechnungViewModel.IstRueckzahlungAktiv = true;
            EinstellungenViewModel.IstEinstellungenAktiv = false;
            ErfolgsrechnungViewModel.IstErfolgsrechnungAktiv = false;
        }
        private void ZeigeErfolgsrechnung()
        {
            KundenViewModel.IstKundenAktiv = false;
            KreditorViewModel.IstKreditorAktiv = false;
            RechnungViewModel.IstRechnungAktiv = false;
            RechnungViewModel.IstRueckzahlungAktiv = false;
            EinstellungenViewModel.IstEinstellungenAktiv = false;
            ErfolgsrechnungViewModel.IstErfolgsrechnungAktiv = true;
        }

        public MainViewModel()
        {
            
        }



    }
}
