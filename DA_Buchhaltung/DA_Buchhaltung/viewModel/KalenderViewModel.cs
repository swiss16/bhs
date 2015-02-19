using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DA_Buchhaltung.model;

namespace DA_Buchhaltung.viewModel
{
    public class KalenderViewModel : ViewModelBase
    {
        //Navigations Properties
        Model model = new Model();

        //Properties
        private DataGridCellInfo _aktuelleZelle;

        public DataGridCellInfo AktuelleZelle
        {
            get { return _aktuelleZelle; }
            set
            {
                _aktuelleZelle = value;
                OnPropertyChanged("AktuelleZelle");
            }
        }

        private decimal _aktuelleZeit = 0.00m;
        public decimal AktuelleZeit
        {
            get { return _aktuelleZeit; }
            set
            {
                _aktuelleZeit = value;
                OnPropertyChanged("AktuelleZeit");  
            }
        }

        private Kunde _aktuellerKunde = new Kunde();
        public Kunde AktuellerKunde
        {
            get { return _aktuellerKunde; }
            set
            {
                _aktuellerKunde = value;
                OnPropertyChanged("AktuellerKunde");
            }
        }

        private DateTime _aktuellesDatum = DateTime.Now;
        public DateTime AktuellesDatum
        {
            get { return _aktuellesDatum; }
            set
            {
                _aktuellesDatum = value;
                LadeZeiten(value.Date);
                OnPropertyChanged("AktuellesDatum");
            }
        }

        //Sichtbarkeits Properties
        private bool _istKalenderUebersichtAktiv = false;
        public bool IstKalenderUebersichtAktiv
        {
            get { return _istKalenderUebersichtAktiv; }
            set
            {
                if (value != _istKalenderUebersichtAktiv)
                {
                    _istKalenderUebersichtAktiv = value;
                    OnPropertyChanged("IstKalenderUebersichtAktiv");
                }
            }
        }

        //Listen
        private readonly ObservableCollection<Zeit> _zeitListe = new ObservableCollection<Zeit>();
        public ObservableCollection<Zeit> ZeitListe
        {
            get { return _zeitListe; }
        }




        //Private Methoden und Commandhelper
        //Lädt alle Zeitfelder des Terminkalenders
        private List<Zeit> LadeZeiten()
        {
            List<Zeit> _tempZeitlist = new List<Zeit>();
            decimal stundenCount = 0.00m;
            decimal minutenCount = 0.00m;
            //6 11 16
            for (int i = 0; i < 20; i++)
            {
                if (i%4 == 0 && i !=0)
                {
                    stundenCount += 1.00m;
                    minutenCount = 0.00m;
                }
                Zeit zeit = new Zeit();
                if (minutenCount == 0.00m)
                {
                    zeit.AbSechsUhr = 6.00m + stundenCount;
                    zeit.AbElfUhr = 11.00m + stundenCount;
                    zeit.AbSechzehnUhr = 16.00m + stundenCount;
                }
                else
                {
                    zeit.AbSechsUhr = 6.00m + stundenCount + minutenCount;
                    zeit.AbElfUhr = 11.00m + stundenCount + minutenCount;
                    zeit.AbSechzehnUhr = 16.00m + stundenCount + minutenCount;
                }
                _tempZeitlist.Add(zeit);
                minutenCount += 0.15m;
            }
            return _tempZeitlist;
        }
        //Füllt die Kunden in die Zeitfelder
        private void LadeZeiten(DateTime datum)
        {
            ZeitListe.Clear();
            List<Zeit> _tempZeitlist = LadeZeiten();
            //Bestehende Termine Laden und einfüllen
            List<Termin> terminListe = model.LadeTermine();

            if (terminListe.Count != 0)
            {
                terminListe = terminListe.Where(i => i.StartZeit.Date == datum.Date).ToList();
                foreach (var termin in terminListe)
                {
                    decimal _startZeit = 0.00m;
                    decimal _endZeit = 0.00m;
                    _startZeit += termin.StartZeit.Hour;
                    _endZeit += termin.EndZeit.Hour;
                    if (termin.StartZeit.Minute != 0)
                    {
                        _startZeit += termin.StartZeit.Minute / 100;
                    }
                    if (termin.EndZeit.Minute != 0)
                    {
                        _startZeit += termin.EndZeit.Minute / 100;
                    }

                    
                    foreach (var zeit in _tempZeitlist)
                    {
                        if (zeit.AbSechsUhr >= _startZeit && zeit.AbSechsUhr <= _endZeit)
                        {
                            zeit.AbSechsUhrKunde = termin.Kunde;
                            zeit.SechsUhrKundeAktiv = true;
                        }
                        if (zeit.AbElfUhr >= _startZeit && zeit.AbElfUhr <= _endZeit)
                        {
                            zeit.AbElfUhrKunde = termin.Kunde;
                            zeit.ElfUhrKundeAktiv = true;
                        }
                        if (zeit.AbSechzehnUhr >= _startZeit && zeit.AbSechzehnUhr <= _endZeit)
                        {
                            zeit.AbSechzehnUhrKunde = termin.Kunde;
                            zeit.SechzehnUhrKundeAktiv = true;
                        }
                    }
                }

            }
            if (_tempZeitlist.Count !=0)
            {
                _tempZeitlist.ForEach(i => ZeitListe.Add(i));
            }  
        }

        public KalenderViewModel()
        {
            LadeZeiten(DateTime.Now.Date);
        }
    }
}
