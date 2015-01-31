﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows;
using DA_Buchhaltung.common.commands;
using DA_Buchhaltung.model;

namespace DA_Buchhaltung.viewModel
{
    public class AuftragViewModel : ViewModelBase
    {
        //NavigationsProperties
        Model model = new Model();

        //Private und Statische Felder
        private List<Option> _optionenListe =new List<Option>();
        private List<Dienstleistung> _dienstleistungsListe = new List<Dienstleistung>(); 
        public static int Errors { get; set; }

        //Allgemeine Properties
        private Auftrag _aktuellerAuftrag = new Auftrag();
        public Auftrag AktuellerAuftrag
        {
            get { return _aktuellerAuftrag; }
            set
            {
                if (value != null)
                {
                    
                    _aktuellerAuftrag = value;
                    OnPropertyChanged("AktuellerAuftrag");
                    if (value.ID == -1)
                    {
                        KeinNeuerAuftragAktiv = false;
                    }
                    else
                    {
                        KeinNeuerAuftragAktiv = true;
                    }
                    
                }
                else
                {
                    AktuellerAuftrag = new Auftrag();
                }


            }
        }

        public int AktuelleKundenId { get; set; }


        private bool _keinNeuerAuftragAktiv = false;
        public bool KeinNeuerAuftragAktiv
        {
            get { return _keinNeuerAuftragAktiv; }
            set
            {
                if (value != _keinNeuerAuftragAktiv)
                {
                    _keinNeuerAuftragAktiv = value;
                    OnPropertyChanged("KeinNeuerAuftragAktiv");
                }
            }
        }

        private bool _sonstigesAktiv = false;
        public bool SonstigesAktiv
        {
            get { return _sonstigesAktiv; }
            set
            {
                if (value != _sonstigesAktiv)
                {
                    _sonstigesAktiv = value;
                    OnPropertyChanged("SonstigesAktiv");
                }
            }
        }

        private bool _rabattInProzent = true;
        public bool RabattInProzent
        {
            get { return _rabattInProzent; }
            set
            {
                if (value != _rabattInProzent)
                {
                    _rabattInProzent = value;
                    OnPropertyChanged("RabattInProzent");
                    AktualisiereRabatt(AktuellerAuftrag.Rabatt.ToString());
                }
            }
        }

        //Dienstleistung Properties
        private bool _istNeuset = true;
        public bool IstNeuset
        {
            get { return _istNeuset; }
            set
            {
                _istNeuset = value;
                OnPropertyChanged("IstNeuset");
                if (value)
                {
                    IstAuffuellen = false;
                    IstGellack = false;
                    UpdateDienstleistung();
                }
            }
        }

        private bool _istAuffuellen = false;
        public bool IstAuffuellen
        {
            get { return _istAuffuellen; }
            set
            {
                _istAuffuellen = value;
                OnPropertyChanged("IstAuffuellen");
                if (value)
                {
                    IstNeuset = false;
                    IstGellack = false;
                    UpdateDienstleistung();
                }
            }
        }

        private bool _istGellack = false;
        public bool IstGellack
        {
            get { return _istGellack; }
            set
            {
                _istGellack = value;
                OnPropertyChanged("IstGellack");
                if (value)
                {
                    IstNeuset = false;
                    IstAuffuellen = false;
                    UpdateDienstleistung();
                }
            }
        }

        //Optionen Properties
        private int _reperatur = 0;
        public int Reperatur
        {
            get { return _reperatur; }
            set
            {
                _reperatur = value;
                OnPropertyChanged("Reperatur");
                    
                UpdatePosition(value, "Reperatur", true);
                
            }
        }

        [Range(0, 500, ErrorMessage = "Muss 0-500 sein")]
        private int _steinchen = 0;
        public int Steinchen
        {
            get { return GetValue(() => _steinchen); }
            set
            {
                SetValue(() => _steinchen, value);
                OnPropertyChanged("Steinchen");

                UpdatePosition(value, "Steinchen", true);
                
            }
        }

        [Range(0, 500, ErrorMessage = "Muss 0-500 sein")]
        private decimal _stamping = 0.0m;
        public decimal Stamping
        {
            get { return GetValue(() => _stamping); }
            set
            {
                SetValue(() => _stamping, value);
                OnPropertyChanged("Stamping");

                UpdatePosition(value, "Stamping", false);
                
            }
        }

        [Range(0,500, ErrorMessage = "Muss 0-500 sein")]
        private decimal _nailart = 0.0m;
        public decimal Nailart
        {
            get { return GetValue(() => _nailart); }
            set
            {
                
                 SetValue(() => _nailart, value);
                 OnPropertyChanged("Nailart");

                 UpdatePosition(value, "Nailart", false);
                
            }
        }

        [MaxLength(50, ErrorMessage = "Maximal 50 Stellen erlaubt")] 
        private string _sonstigesText = string.Empty;
        public string SonstigesText
        {
            get { return GetValue(() => _sonstigesText); }
            set
            {
                 SetValue(() => _sonstigesText, value);
                 OnPropertyChanged("SonstigesText");
                 if (value.Length == 0)
                 {
                     SonstigesAktiv = false;
                     SonstigesPreis = 0.0m;
                 }
                 else
                 {
                     SonstigesAktiv = true;
                 }
                    
                
            }
        }

        [Range(0, 500, ErrorMessage = "Muss 0-500 sein")]
        private decimal _sonstigesPreis = 0.0m;
        public decimal SonstigesPreis
        {
            get { return GetValue(() => _sonstigesPreis); }
            set
            {
                SetValue(() => _sonstigesPreis, value);
                OnPropertyChanged("SonstigesPreis");
                if (!string.IsNullOrEmpty(SonstigesText))
                {
                    UpdatePosition(value, SonstigesText, false);
                }
                else
                {
                    UpdatePosition(value, "Sonstiges", false);
                }
                    
                
            }
        }


        // Sichtbarkeitsproperties
        private bool _istAuftragAktiv = false;
        public bool IstAuftragAktiv
        {
            get { return _istAuftragAktiv; }
            set
            {
                if (value != _istAuftragAktiv)
                {
                    _istAuftragAktiv = value;
                    OnPropertyChanged("IstAuftragAktiv");
                    UpdateKunde(AktuelleKundenId);
                }
            }
        }

        //Listen
        private readonly ObservableCollection<Auftrag> _auftragsListe = new ObservableCollection<Auftrag>();
        public ObservableCollection<Auftrag> AuftragsListe
        {
            get { return _auftragsListe; }
        }

        private readonly ObservableCollection<Position> _positionsListe = new ObservableCollection<Position>();
        public ObservableCollection<Position> PositionsListe
        {
            get { return _positionsListe; }
        }

        private readonly ObservableCollection<int> _reperaturListe = new ObservableCollection<int>();
        public ObservableCollection<int> ReperaturListe
        {
            get { return _reperaturListe; }
        }


        //Commands
        private SimpleCommand _neuerAuftragCommand;
        public SimpleCommand NeuerAuftragCommand
        {
            get { return _neuerAuftragCommand ?? (_neuerAuftragCommand = new SimpleCommand(NeuerAuftrag)); }
        }

        private SimpleCommand _loescheAuftragCommand;
        public SimpleCommand LoescheAuftragCommand
        {
            get { return _loescheAuftragCommand ?? (_loescheAuftragCommand = new SimpleCommand(LoescheAuftrag)); }
        }

        private SimpleCommand _speichereAuftragCommand;
        public SimpleCommand SpeichereAuftragCommand
        {
            get { return _speichereAuftragCommand ?? (_speichereAuftragCommand = new SimpleCommand(SpeichereAuftrag)); }
        }

        private SimpleCommand _druckeAuftragCommand;
        public SimpleCommand DruckeAuftragCommand
        {
            get { return _druckeAuftragCommand ?? (_druckeAuftragCommand = new SimpleCommand(DruckeAuftrag)); }
        }

        private RelayCommand<string> _aktualisiereRabattCommand;
        public RelayCommand<string> AktualisiereRabattCommand
        {
            get { return _aktualisiereRabattCommand ?? (_aktualisiereRabattCommand = new RelayCommand<string>(AktualisiereRabatt)); }
        }


        //Commandhelpers und Private Methoden
        private void NeuerAuftrag()
        {
            AktuellerAuftrag = new Auftrag();
            AktuellerAuftrag.KundeID = AktuelleKundenId;
            AktuellerAuftrag = AktuellerAuftrag;
            KeinNeuerAuftragAktiv = false;
            IstNeuset = true;
        }
        
        private void LoescheAuftrag()
        {
            if (AktuellerAuftrag == null || AktuellerAuftrag.ID == -1)
            {
                MessageBox.Show("Auftrag wurde nicht gelöscht! Es wurde kein Auftrag angewählt.", "Löschen Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var result =
                MessageBox.Show(
                    string.Format("Möchtest du den Auftrag von ({0}) wirklich löschen?" ,AktuellerAuftrag.Datum),
                    "Bist du sicher?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (!model.LoescheAuftrag(AktuellerAuftrag))
                {
                    return;
                }
                LadeAuftraege();
                MessageBox.Show("Auftrag gelöscht!", "Löschen erfolgreich", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            
        }
        
        private void SpeichereAuftrag()
        {
            if (AktuellerAuftrag == null || AktuellerAuftrag.Dienstleistung.ID == -1)
            {
                MessageBox.Show("Der Auftrag wurde nicht gespeichert! Es wurde kein Auftrag angewählt.", "Speichern Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (Errors != 0)
            {
                return;
            }
            if (AktuelleKundenId == -1)
            {
                MessageBox.Show("Der Auftrag wurde nicht gespeichert! Es wurde kein Kunde angewählt.", "Speichern Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            AktuellerAuftrag.KundeID = AktuelleKundenId;
            
            int _tempAuftragId = model.SpeichereAuftrag(AktuellerAuftrag);

            if (_tempAuftragId == -1)
            {
                return;
            }
            LadeAuftraege();
            if (AuftragsListe.Any(i=>i.ID== _tempAuftragId))
            {
                AktuellerAuftrag = AuftragsListe.First(i => i.ID == _tempAuftragId);
            }
            
            MessageBox.Show("Auftrag gespeichert!", "Speichern erfolgreich", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
   
        private void LadeAuftraege()
        {
            List<Auftrag> _tempAuftragsListe = new List<Auftrag>();

            AuftragsListe.Clear();

            _tempAuftragsListe = model.LadeAuftraege("");

            _tempAuftragsListe = _tempAuftragsListe.Where(i => i.KundeID == AktuelleKundenId).ToList();

            if (_tempAuftragsListe.Count != 0)
            {
                _tempAuftragsListe.ForEach(a => AuftragsListe.Add(a));
                AktuellerAuftrag = AuftragsListe.First();
            }

            _optionenListe = model.LadeOptionen();
            
        }

        private void DruckeAuftrag()
        {
            if (AktuellerAuftrag == null || AktuellerAuftrag.ID == -1)
            {
                MessageBox.Show("Der Auftrag kann nicht gedruckt werden. Speichern sie zuerst den Auftrag!", "Drucken Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (AktuellerAuftrag.KundeID == -1)
            {
                MessageBox.Show("Der Auftrag kann nicht gedruckt werden. Der Kunde wurde nicht gefunden", "Drucken Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            AktuellerAuftrag.Print();
        }

        private void UpdatePosition(decimal value, string name, bool konfigurierbar)
        {
            if (konfigurierbar)
            {
                if (value > 0)
                {
                    //Gibt es schon in Liste, nur ändern
                    if (AktuellerAuftrag.Positionen.Any(i => (i.Name == name)&&(i.WurdeGeloescht == false)))
                    {
                        AktuellerAuftrag.Positionen.First(i => (i.Name == name) && (i.WurdeGeloescht == false)).Anzahl = value;
                        AktuellerAuftrag.Positionen.First(i => (i.Name == name) && (i.WurdeGeloescht == false)).PreisInFranken = value *
                                                                                                           _optionenListe
                                                                                                               .First(
                                                                                                                   i =>
                                                                                                                       i
                                                                                                                           .Name ==
                                                                                                                       name)
                                                                                                               .Einheitspreis;
                        
                        
                    }
                    else
                    {
                        //Neu hinzufügen
                        Option bestOpt = _optionenListe.First(i => i.Name == name);
                        Option opt = new Option{Anzahl = bestOpt.Anzahl, BereitsVorhanden = bestOpt.BereitsVorhanden, Einheitspreis = bestOpt.Einheitspreis, EndDate = bestOpt.EndDate, StartDate = bestOpt.StartDate, ID = bestOpt.ID, Konfigurierbar = bestOpt.Konfigurierbar, Name = bestOpt.Name, PreisInFranken = bestOpt.PreisInFranken, WurdeGeloescht = bestOpt.WurdeGeloescht};
                        opt.Anzahl = value;
                        opt.PreisInFranken = value * opt.Einheitspreis;
                        AktuellerAuftrag.Positionen.Add(opt);
                    }
                }
                else
                {
                    //Löschen falls in liste existiert
                    if (AktuellerAuftrag.Positionen.Any(i => (i.Name == name) && (i.WurdeGeloescht == false)))
                    {
                        AktuellerAuftrag.Positionen.First(i => (i.Name == name) && (i.WurdeGeloescht == false)).WurdeGeloescht = true;
                    }
                }
            }
            else
            {
                //Nicht konfigurierbar und nicht sonstiges
                if (name == "Stamping" || name == "Nailart")
                {
                    if (value > 0)
                    {
                        //Gibt es schon in Liste, nur ändern
                        if (AktuellerAuftrag.Positionen.Any(i => (i.Name == name) && (i.WurdeGeloescht == false)))
                        {
                            AktuellerAuftrag.Positionen.First(i => (i.Name == name) && (i.WurdeGeloescht == false)).Anzahl = 1;
                            AktuellerAuftrag.Positionen.First(i => (i.Name == name) && (i.WurdeGeloescht == false)).PreisInFranken = value;
                            AktuellerAuftrag.Positionen.First(i => (i.Name == name) && (i.WurdeGeloescht == false)).Einheitspreis = value;
                        }
                        else
                        {
                            Option opt = new Option();
                            opt.Anzahl = 1;
                            opt.Name = name;
                            opt.Einheitspreis = value;
                            opt.PreisInFranken = value;
                            AktuellerAuftrag.Positionen.Add(opt);
                        }
                    }
                    else
                    {
                        //Löschen der Option
                        if (AktuellerAuftrag.Positionen.Any(i => (i.Name == name) && (i.WurdeGeloescht == false)))
                        {
                            AktuellerAuftrag.Positionen.First(i => (i.Name == name) && (i.WurdeGeloescht == false)).WurdeGeloescht = true;
                        }
                    }
                }
                else
                {
                    //Sonstiges
                    if (value > 0)
                    {
                        if (AktuellerAuftrag.Positionen.Any(i => (i.Name != "Steinchen") && (i.Name != "Stamping") && (i.Name != "Nailart") && (i.Name != "Reperatur") && (i.WurdeGeloescht == false)))
                        {
                            AktuellerAuftrag.Positionen.First(
                                i =>
                                    (i.Name != "Steinchen") && (i.Name != "Stamping") && (i.Name != "Nailart") &&
                                    (i.Name != "Reperatur") && (i.WurdeGeloescht == false)).Name = name;
                            AktuellerAuftrag.Positionen.First(
                                i =>
                                    (i.Name != "Steinchen") && (i.Name != "Stamping") && (i.Name != "Nailart") &&
                                    (i.Name != "Reperatur") && (i.WurdeGeloescht == false)).Einheitspreis = value;
                            AktuellerAuftrag.Positionen.First(
                                i =>
                                    (i.Name != "Steinchen") && (i.Name != "Stamping") && (i.Name != "Nailart") &&
                                    (i.Name != "Reperatur") && (i.WurdeGeloescht == false)).PreisInFranken = value;
                        }
                        else
                        {
                            Option opt = new Option();
                            opt.Anzahl = 1;
                            opt.Name = name;
                            opt.Einheitspreis = value;
                            opt.PreisInFranken = value;
                            AktuellerAuftrag.Positionen.Add(opt);
                        }
                    }
                    else
                    {
                        if (AktuellerAuftrag.Positionen.Any(i => (i.Name != "Steinchen") && (i.Name != "Stamping") && (i.Name != "Nailart") && (i.Name != "Reperatur") && (i.WurdeGeloescht == false)))
                        {
                            AktuellerAuftrag.Positionen.First(
                                i =>
                                    (i.Name != "Steinchen") && (i.Name != "Stamping") && (i.Name != "Nailart") &&
                                    (i.Name != "Reperatur") && (i.WurdeGeloescht == false)).WurdeGeloescht = true;
                        }
                    }
                    
                    
                }
                
            }
            UpdatePositionList();

        }

        private void UpdatePositionList()
        {
            PositionsListe.Clear();
            //Dienstleistung
            Position dienstleistung = new Position
            {
                Anzahl = 1,
                Beschreibung = AktuellerAuftrag.Dienstleistung.Name,
                Einheitspreis = AktuellerAuftrag.Dienstleistung.Preis,
                Preis = AktuellerAuftrag.Dienstleistung.Preis,
                IstDienstleistung = true,
                IstOption = false,
                IstRabatt = false
            };
            PositionsListe.Add(dienstleistung);

            //Optionen
            foreach (var option in AktuellerAuftrag.Positionen)
            {
                if (option.WurdeGeloescht == false)
                {
                    Position opt = new Position
                    {
                        Anzahl = option.Anzahl,
                        Beschreibung = option.Name,
                        Einheitspreis = option.Einheitspreis,
                        Preis = option.PreisInFranken,
                        IstDienstleistung = false,
                        IstOption = true,
                        IstRabatt = false
                    };
                    PositionsListe.Add(opt);
                }
                
            }
            //Berechne Total vor Rabatt
            AktuellerAuftrag.Total = 0.0m;
            foreach (var position in PositionsListe)
            {
                AktuellerAuftrag.Total += position.Preis;
            }

            //Rabatt
            decimal _tempRabatt = 0.0m;
            if (AktuellerAuftrag.Rabatt>0)
            {
                if (AktuellerAuftrag.RabattInProzent)
                {

                    _tempRabatt = AktuellerAuftrag.Total * ((AktuellerAuftrag.Rabatt / 100));
                }
                else
                {
                    _tempRabatt = AktuellerAuftrag.Rabatt;
                }
                _tempRabatt= _tempRabatt*(-1);
                _tempRabatt = Math.Round(_tempRabatt * 20.0M, MidpointRounding.AwayFromZero) * 0.05M;
                Position rabatt = new Position
                {
                    Anzahl = 1,
                    Beschreibung = "Rabatt",
                    Einheitspreis = _tempRabatt,
                    Preis = _tempRabatt,
                    IstDienstleistung = false,
                    IstOption = false,
                    IstRabatt = true
                };
                PositionsListe.Add(rabatt);
                AktuellerAuftrag.Total += _tempRabatt;
            }
            else
            {
                AktuellerAuftrag.Rabatt = 0.0m;
            }
            AktuellerAuftrag = AktuellerAuftrag;

        }

        private void UpdateDienstleistung()
        {
            _dienstleistungsListe = model.LadeDienstleistungen();
            if (IstAuffuellen)
            {
                if (_dienstleistungsListe.Any(i => i.Name == "Auffüllen"))
                {
                    AktuellerAuftrag.Dienstleistung = _dienstleistungsListe.First(i => i.Name == "Auffüllen");
                }
                else
                {
                    AktuellerAuftrag.Dienstleistung = new Dienstleistung();
                    MessageBox.Show("Dienstleistung nicht gefunden", "Fehler beim Berechnen", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

            }

            if (IstNeuset)
            {
                if (_dienstleistungsListe.Any(i => i.Name == "Neuset"))
                {
                    AktuellerAuftrag.Dienstleistung = _dienstleistungsListe.First(i => i.Name == "Neuset");
                }
                else
                {
                    AktuellerAuftrag.Dienstleistung = new Dienstleistung();
                    MessageBox.Show("Dienstleistung nicht gefunden", "Fehler beim Berechnen", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

            }

            if (IstGellack)
            {
                if (_dienstleistungsListe.Any(i => i.Name == "Gellack"))
                {
                    AktuellerAuftrag.Dienstleistung = _dienstleistungsListe.First(i => i.Name == "Gellack");
                }
                else
                {
                    AktuellerAuftrag.Dienstleistung = new Dienstleistung();
                    MessageBox.Show("Dienstleistung nicht gefunden", "Fehler beim Berechnen", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                    return;
                }

            }

            UpdatePositionList();
        }

        private void AktualisiereRabatt(string rabattAsString)
        {
            decimal rabatt = 0.0m;
            if (decimal.TryParse(rabattAsString, out rabatt))
            {
                AktuellerAuftrag.Rabatt = rabatt;
                AktuellerAuftrag.RabattInProzent = RabattInProzent;
                AktuellerAuftrag = AktuellerAuftrag;
                UpdatePositionList();
            }             
        }

        //Public Methoden
        public void UpdateKunde(int id)
        {
            AktuelleKundenId = id;
            AktuellerAuftrag.KundeID = id;
            LadeAuftraege();
        }


        //Konstruktor
        public AuftragViewModel()
        {
            AktuelleKundenId = -1;
            _optionenListe = model.LadeOptionen();
            IstNeuset = true;
            ReperaturListe.Clear();
            for (int i = 0; i < 10; i++)
            {
                ReperaturListe.Add(i);
            }
        }
    }
}
