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
        Model model = new Model();

        //Properties
        private string _currentDate = string.Format("Datum: {0}", DateTime.Now.ToShortDateString());
        public string CurrentDate
        {
            get { return _currentDate; }
            set
            {
                if (CurrentDate != _currentDate)
                {
                    _currentDate = value;
                    OnPropertyChanged("CurrentDate");
                }
            }
        }

        private Kunde _aktuellerKunde = new Kunde();
        public Kunde AktuellerKunde
        {
            get { return _aktuellerKunde; }
            set
            {
                if (AktuellerKunde != _aktuellerKunde)
                {
                    _aktuellerKunde = value;
                    OnPropertyChanged("AktuellerKunde");
                }
            }
        }

        private bool _kundenListeIstNichtLeer;
        public bool KundenListeIstNichtLeer
        {
            get { return _kundenListeIstNichtLeer; }
            set
            {
                if (KundenListeIstNichtLeer != _kundenListeIstNichtLeer)
                {
                    _kundenListeIstNichtLeer = value;
                    OnPropertyChanged("KundenListeIstNichtLeer");
                }
            }
        }

        private int _selectedKundenIndex;
        public int SelectedKundenIndex
        {
            get { return _selectedKundenIndex; }
            set
            {
                if (SelectedKundenIndex != _selectedKundenIndex)
                {
                    _selectedKundenIndex = value;
                    OnPropertyChanged("SelectedKundenIndex");
                }
            }
        }

        private Erfolgsrechnung _aktuelleErfolgsrechnung = new Erfolgsrechnung();
        public Erfolgsrechnung AktuelleErfolgsrechnung
        {
            get { return _aktuelleErfolgsrechnung; }
            set
            {
                if (AktuelleErfolgsrechnung != _aktuelleErfolgsrechnung)
                {
                    _aktuelleErfolgsrechnung = value;
                    OnPropertyChanged("AktuelleErfolgsrechnung");
                }
            }
        }

        //Listen
        private readonly ObservableCollection<Kunde> _customerCollection = new ObservableCollection<Kunde>();
        public ObservableCollection<Kunde> CustomerCollection
        {
            get { return _customerCollection; }
        }

        //Sichtbarkeits Properties
        private bool _istKundenAktiv = true;
        public bool IstKundenAktiv
        {
            get { return _kundenListeIstNichtLeer; }
            set
            {
                if (IstKundenAktiv != _istKundenAktiv)
                {
                    _istKundenAktiv = value;
                    OnPropertyChanged("IstKundenAktiv");
                }
            }
        }

        //Commands
        private RelayCommand<bool> _ladeKundenCommand;
        public RelayCommand<bool> LadeKundenCommand
        {
            get { return _ladeKundenCommand ?? (_ladeKundenCommand = new RelayCommand<bool>(LadeKunden)); }
        }
        private RelayCommand<string> _beendenCommand;
        public RelayCommand<string> BeendenCommand
        {
            get { return _beendenCommand ?? (_beendenCommand = new RelayCommand<string>(Beenden)); }
        }



        //Commandhelper
        private void LadeKunden(bool isTrue)
        {
            List<Kunde> KundenListe = model.LadeKunden();

            KundenListeIstNichtLeer = false;

            CustomerCollection.Clear();

            
            List<Kunde> customers = model.LadeKunden();

            if (customers.Count == 0)
            {
                 return;
            }
            customers.ForEach(customer => CustomerCollection.Add(customer));

               
            SelectedKundenIndex = 0;
            KundenListeIstNichtLeer = true;


            AktuelleErfolgsrechnung = model.ErstelleErfolgsrechnung(2014);
            model.SpeichereErfolgsrechnung(AktuelleErfolgsrechnung);

        }

        private void Beenden(string x)
        {
            Application.Current.Shutdown();
        }

        public MainViewModel()
        {
            
        }



    }
}
