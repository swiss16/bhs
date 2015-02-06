/*
 * Klasse: ErfolgsrechnungViewModel.cs
 * Author: Martin Osterwalder
 * Stellt für die View die Commands und Properties bereit um die entsprechende Modelfunktionen aufzurufen.
 * Die View bindet die Steuerungselemente an diese Commands und Properties.
 * Zuständig für die weiterleitung jegliche Aufgaben um Erfolgsrechnung zu generieren und drucken.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DA_Buchhaltung.common.commands;
using DA_Buchhaltung.model;

namespace DA_Buchhaltung.viewModel
{
    public class ErfolgsrechnungViewModel : ViewModelBase
    {

        //NavigationsProperties
        Model model = new Model();

        //Properties
        
        private Erfolgsrechnung _aktuelleErfolgsrechnung = new Erfolgsrechnung();
        public Erfolgsrechnung AktuelleErfolgsrechnung
        {
            get { return _aktuelleErfolgsrechnung; }
            set
            {
                        _aktuelleErfolgsrechnung = value;
                        OnPropertyChanged("AktuelleErfolgsrechnung");
                        UpdateLists();
            }
        }

        private DateTime _startDatum = DateTime.Now.Date;
        public DateTime StartDatum
        {
            get { return _startDatum; }
            set
            {
                if (value != _startDatum)
                {
                    _startDatum = value;
                    OnPropertyChanged("StartDatum");
                    AktuelleErfolgsrechnung.StartDatum = value;

                }
            }
        }

        private DateTime _endDatum = DateTime.Now.AddDays(1).Date;
        public DateTime EndDatum
        {
            get { return _endDatum; }
            set
            {
                if (value != _endDatum)
                {
                    _endDatum = value;
                    OnPropertyChanged("EndDatum");
                    AktuelleErfolgsrechnung.EndDatum = value;

                }
            }
        }

        private decimal _total = 0.0m;
        public decimal Total
        {
            get { return _total; }
            set
            {
                if (value != _total)
                {
                    _total = value;
                    OnPropertyChanged("Total");
                    AktuelleErfolgsrechnung.Gewinn = value;
                    IstGewinn = value >=0;

                }
            }
        }

        private decimal _subtotalEinnahme = 0.0m;
        public decimal SubtotalEinnahme
        {
            get { return _subtotalEinnahme; }
            set
            {
                if (value != _subtotalEinnahme)
                {
                    _subtotalEinnahme = value;
                    OnPropertyChanged("SubtotalEinnahme");
                    AktuelleErfolgsrechnung.SubtotalEinnahmen = value;

                }
            }
        }

        private decimal _subtotalAusgabe = 0.0m;
        public decimal SubtotalAusgabe
        {
            get { return _subtotalAusgabe; }
            set
            {
                if (value != _subtotalAusgabe)
                {
                    _subtotalAusgabe = value;
                    OnPropertyChanged("SubtotalAusgabe");
                    AktuelleErfolgsrechnung.SubtotalAusgaben = value;

                }
            }
        }

        private bool _istGewinn = true;
        public bool IstGewinn
        {
            get { return _istGewinn; }
            set
            {
                if (value != _istGewinn)
                {
                    _istGewinn = value;
                    OnPropertyChanged("IstGewinn");
                    if (_istGewinn)
                    {
                        IstVerlust = false;
                    }
                    else
                    {
                        IstVerlust = true;
                    }
                }
            }
        }

        private bool _istVerlust = false;
        public bool IstVerlust
        {
            get { return _istVerlust; }
            set
            {
                if (value != _istVerlust)
                {
                    _istVerlust = value;
                    OnPropertyChanged("IstVerlust");
                }
            }
        }

        private bool _jahresrechnung = true;
        public bool Jahresrechnung
        {
            get { return _jahresrechnung; }
            set
            {
                if (value != _jahresrechnung)
                {
                    _jahresrechnung = value;
                    OnPropertyChanged("Jahresrechnung");
                    if (value)
                    {
                        AktuelleErfolgsrechnung.IstJahresabrechnung = true;
                        KeineJahresrechnung = false;
                    }
                    else
                    {
                        AktuelleErfolgsrechnung.IstJahresabrechnung = false;
                        KeineJahresrechnung = true;
                    }
                }
            }
        }

        private bool _keineJahresrechnung = false;
        public bool KeineJahresrechnung
        {
            get { return _keineJahresrechnung; }
            set
            {
                if (value != _keineJahresrechnung)
                {
                    _keineJahresrechnung = value;
                    OnPropertyChanged("KeineJahresrechnung");
                }
            }
        }

        private bool _wurdeGeneriert = false;
        public bool WurdeGeneriert
        {
            get { return _wurdeGeneriert; }
            set
            {
                if (value != _wurdeGeneriert)
                {
                    _wurdeGeneriert = value;
                    OnPropertyChanged("WurdeGeneriert");
                }
            }
        }


        //Sichtbarkeits Properties
        private bool _istErfolgsrechnungAktiv = false;
        public bool IstErfolgsrechnungAktiv
        {
            get { return _istErfolgsrechnungAktiv; }
            set
            {
                if (value != _istErfolgsrechnungAktiv)
                {
                    _istErfolgsrechnungAktiv = value;
                    OnPropertyChanged("IstErfolgsrechnungAktiv");
                }
            }
        }

        //Listen
        private readonly ObservableCollection<Betraege> _einnahmeListe = new ObservableCollection<Betraege>();
        public ObservableCollection<Betraege> EinnahmeListe
        {
            get { return _einnahmeListe; }
        }

        private readonly ObservableCollection<Betraege> _ausgabeListe = new ObservableCollection<Betraege>();
        public ObservableCollection<Betraege> AusgabeListe
        {
            get { return _ausgabeListe; }
        }

        //Commands
        private SimpleCommand _berechneErfolgsrechnungCommand;
        public SimpleCommand BerechneErfolgsrechnungCommand
        {
            get { return _berechneErfolgsrechnungCommand ?? (_berechneErfolgsrechnungCommand = new SimpleCommand(BerechneErfolgsrechnung)); }
        }

        private SimpleCommand _speichereErfolgsrechnungCommand;
        public SimpleCommand SpeichereErfolgsrechnungCommand
        {
            get { return _speichereErfolgsrechnungCommand ?? (_speichereErfolgsrechnungCommand = new SimpleCommand(SpeichereErfolgsrechnung)); }
        }

        

        //Command Helpers
        private void SpeichereErfolgsrechnung()
        {
            if (AktuelleErfolgsrechnung == null)
            {
                MessageBox.Show("Erfolgsrechnung wurde nicht gespeichert! Es wurde keine Erfolgsrechnung gefunden.", "Speichern Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (!model.SpeichereErfolgsrechnung(AktuelleErfolgsrechnung))
            {
                return;
            }
            
            
            MessageBox.Show("Erfolgsrechnung gespeichert!", "Speichern erfolgreich", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void BerechneErfolgsrechnung()
        {
            if (AktuelleErfolgsrechnung == null)
            {
                MessageBox.Show("Erfolgsrechnung konnte nicht generiert werden! Es wurde keine Erfolgsrechnung gefunden.", "Erstellen Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (AktuelleErfolgsrechnung.StartDatum >= AktuelleErfolgsrechnung.EndDatum)
            {
                if (!AktuelleErfolgsrechnung.IstJahresabrechnung)
                {
                    MessageBox.Show("Das Startdatum muss vor dem Enddatum liegen", "Erstellen Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                
            }
            if (AktuelleErfolgsrechnung.IstJahresabrechnung)
            {
                AktuelleErfolgsrechnung = model.ErstelleErfolgsrechnung(AktuelleErfolgsrechnung.StartDatum.Year);
            }
            else
            {
                AktuelleErfolgsrechnung = model.ErstelleErfolgsrechnung(AktuelleErfolgsrechnung.StartDatum,
                    AktuelleErfolgsrechnung.EndDatum);
            }
            Total = AktuelleErfolgsrechnung.Gewinn;
            StartDatum = AktuelleErfolgsrechnung.StartDatum;
            EndDatum = AktuelleErfolgsrechnung.EndDatum;
            SubtotalAusgabe = AktuelleErfolgsrechnung.SubtotalAusgaben;
            SubtotalEinnahme = AktuelleErfolgsrechnung.SubtotalEinnahmen;

            UpdateLists();
            IstGewinn = AktuelleErfolgsrechnung.Gewinn >= 0;
            WurdeGeneriert = true;

        }

        private void UpdateLists()
        {
            EinnahmeListe.Clear();
            AusgabeListe.Clear();

            if (AktuelleErfolgsrechnung.Einnahmen.Count !=0)
            {
                AktuelleErfolgsrechnung.Einnahmen.ForEach(i=>EinnahmeListe.Add(i));
            }

            if (AktuelleErfolgsrechnung.Ausgaben.Count != 0)
            {
                AktuelleErfolgsrechnung.Ausgaben.ForEach(i=>AusgabeListe.Add(i));
            }
        }
        

        


        //Konstruktor
        public ErfolgsrechnungViewModel()
        {
            AktuelleErfolgsrechnung.IstJahresabrechnung = true;
        }
    }
}
