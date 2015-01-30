using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private List<Option> OptionenListe =new List<Option>();
        public static int Errors { get; set; }

        //Properties
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

        private Auftrag _aktuellerAuftrag = new Auftrag();
        public Auftrag AktuellerAuftrag
        {
            get { return _aktuellerAuftrag; }
            set
            {
                if (value != null)
                {
                    if (value.ID != _aktuellerAuftrag.ID)
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
                }
                else
                {
                    AktuellerAuftrag = new Auftrag();
                }


            }
        }

        private int _reperatur = 0;
        public int Reperatur
        {
            get { return _reperatur; }
            set
            {
                if (value != _reperatur)
                {
                    _reperatur = value;
                    OnPropertyChanged("Reperatur");
                    
                    UpdatePosition(value, "Reperatur", true);
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

        private RelayCommand<int> _entfernePositionCommand;
        public RelayCommand<int> EntfernePositionCommand
        {
            get { return _entfernePositionCommand ?? (_entfernePositionCommand = new RelayCommand<int>(EntfernePosition)); }
        }


        //Commandhelpers
        private void NeuerAuftrag()
        {
            AktuellerAuftrag = new Auftrag();
            AktuellerAuftrag.KundeID = AktuelleKundenId;          
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
            if (AktuellerAuftrag == null)
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

            OptionenListe = model.LadeOptionen();
            
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

        private void EntfernePosition(int pos)
        {
            if (PositionsListe[pos].IstRabatt)
            {
                
            }
        }

        private void UpdatePosition(decimal value, string name, bool konfigurierbar)
        {
            if (konfigurierbar)
            {
                if (value > 0)
                {
                    //Gibt es schon in Liste, nur ändern
                    if (AktuellerAuftrag.Positionen.Any(i => i.Name == name))
                    {
                        AktuellerAuftrag.Positionen.First(i => i.Name == name).Anzahl = value;
                        AktuellerAuftrag.Positionen.First(i => i.Name == name).PreisInFranken = value *
                                                                                                       OptionenListe
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
                        Option opt = OptionenListe.First(i => i.Name == name);
                        opt.Anzahl = value;
                        opt.PreisInFranken = value * opt.Einheitspreis;
                        AktuellerAuftrag.Positionen.Add(opt);
                    }
                }
                else
                {
                    //Löschen falls in liste existiert
                    if (AktuellerAuftrag.Positionen.Any(i => i.Name == name))
                    {
                        AktuellerAuftrag.Positionen.First(i => i.Name == name).WurdeGeloescht = true;
                    }
                }
            }
            else
            {
                //Nicht konfigurierbar
                if (value > 0)
                {
                    //Gibt es schon in Liste, nur ändern
                    if (AktuellerAuftrag.Positionen.Any(i => i.Name == name))
                    {
                        AktuellerAuftrag.Positionen.First(i => i.Name == name).Anzahl = 1;
                        AktuellerAuftrag.Positionen.First(i => i.Name == name).PreisInFranken = value;
                        AktuellerAuftrag.Positionen.First(i => i.Name == name).Einheitspreis = value;
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
                    if (AktuellerAuftrag.Positionen.Any(i => i.Name == name))
                    {
                        AktuellerAuftrag.Positionen.First(i => i.Name == name).WurdeGeloescht = true;
                    }
                }
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
            OptionenListe = model.LadeOptionen();
        }
    }
}
