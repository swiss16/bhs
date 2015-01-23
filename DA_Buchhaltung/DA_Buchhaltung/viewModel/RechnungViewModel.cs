using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DA_Buchhaltung.common.commands;
using DA_Buchhaltung.model;

namespace DA_Buchhaltung.viewModel
{
    public class RechnungViewModel : ViewModelBase
    {
        //NavigationsProperties
        Model model = new Model();

        //Statische Felder
        public static int RechnungErrors { get; set; }
        public static int RueckzahlungErrors { get; set; }

        //Properties
        private bool _keineNeueRechnungAktiv = false;
        public bool KeineNeueRechnungAktiv
        {
            get { return _keineNeueRechnungAktiv; }
            set
            {
                if (value != _keineNeueRechnungAktiv)
                {
                    _keineNeueRechnungAktiv = value;
                    OnPropertyChanged("KeineNeueRechnungAktiv");
                }
            }
        }

        private bool _geloeschteKategorieAktiv = false;
        public bool GeloeschteKategorieAktiv
        {
            get { return _geloeschteKategorieAktiv; }
            set
            {
                if (value != _geloeschteKategorieAktiv)
                {
                    _geloeschteKategorieAktiv = value;
                    OnPropertyChanged("GeloeschteKategorieAktiv");
                }
            }
        }

        private Rechnung _aktuelleRechnung = new Rechnung();
        public Rechnung AktuelleRechnung
        {
            get { return _aktuelleRechnung; }
            set
            {
                if (value != null)
                {
                    if (value.ID != _aktuelleRechnung.ID)
                    {
                        _aktuelleRechnung = value;
                        OnPropertyChanged("AktuelleRechnung");
                        UpdateKategorie(value.Kategorie);
                        if (value.ID == -1)
                        {
                            KeineNeueRechnungAktiv = false;
                        }
                        else
                        {
                            KeineNeueRechnungAktiv = true;
                        }
                    }
                }
                else
                {
                    AktuelleRechnung = new Rechnung();
                }


            }
        }

        private Kategorie _aktuelleKategorie = new Kategorie();
        public Kategorie AktuelleKategorie
        {
            get { return _aktuelleKategorie; }
            set
            {
                if (value != null)
                {
                    if (value.ID != _aktuelleKategorie.ID)
                    {
                        _aktuelleKategorie = value;
                        OnPropertyChanged("AktuelleKategorie");
                        AktuelleRechnung.Kategorie = value.Name;
                    }
                }
                else
                {
                    AktuelleKategorie = new Kategorie();
                }


            }
        }


        // Sichtbarkeitsproperties
        private bool _istRechnungAktiv = false;
        public bool IstRechnungAktiv
        {
            get { return _istRechnungAktiv; }
            set
            {
                if (value != _istRechnungAktiv)
                {
                    _istRechnungAktiv = value;
                    OnPropertyChanged("IstRechnungAktiv");
                    LadeRechnungen();
                }
            }
        }

        private bool _istRueckzahlungAktiv = false;
        public bool IstRueckzahlungAktiv
        {
            get { return _istRueckzahlungAktiv; }
            set
            {
                if (value != _istRueckzahlungAktiv)
                {
                    _istRueckzahlungAktiv = value;
                    OnPropertyChanged("IstRueckzahlungAktiv");
                    LadeRechnungen();
                }
            }
        }

        //Listen
        private readonly ObservableCollection<Rechnung> _rechnungsListe = new ObservableCollection<Rechnung>();
        public ObservableCollection<Rechnung> RechnungsListe
        {
            get { return _rechnungsListe; }
        }

        private readonly ObservableCollection<Kategorie> _kategorienListe = new ObservableCollection<Kategorie>();
        public ObservableCollection<Kategorie> KategorienListe
        {
            get { return _kategorienListe; }
        }


        //Commands
        private SimpleCommand _neueRechnungCommand;
        public SimpleCommand NeueRechnungCommand
        {
            get { return _neueRechnungCommand ?? (_neueRechnungCommand = new SimpleCommand(NeueRechnung)); }
        }

        private SimpleCommand _neueRueckzahlungCommand;
        public SimpleCommand NeueRueckzahlungCommand
        {
            get { return _neueRueckzahlungCommand ?? (_neueRueckzahlungCommand = new SimpleCommand(NeueRechnung)); }
        }

        private SimpleCommand _loescheRechnungCommand;
        public SimpleCommand LoescheRechnungCommand
        {
            get { return _loescheRechnungCommand ?? (_loescheRechnungCommand = new SimpleCommand(LoescheRechnung)); }
        }

        private SimpleCommand _loescheRueckzahlungCommand;
        public SimpleCommand LoescheRueckzahlungCommand
        {
            get { return _loescheRueckzahlungCommand ?? (_loescheRueckzahlungCommand = new SimpleCommand(LoescheRueckzahlung)); }
        }

        private SimpleCommand _speichereRechnungCommand;
        public SimpleCommand SpeichereRechnungCommand
        {
            get { return _speichereRechnungCommand ?? (_speichereRechnungCommand = new SimpleCommand(SpeichereRechnung)); }
        }

        private SimpleCommand _speichereRueckzahlungCommand;
        public SimpleCommand SpeichereRueckzahlungCommand
        {
            get { return _speichereRueckzahlungCommand ?? (_speichereRueckzahlungCommand = new SimpleCommand(SpeichereRueckzahlung)); }
        }


        //Commandhelpers
        private void NeueRechnung()
        {
            AktuelleRechnung = new Rechnung();
            if (KategorienListe.Any())
            {
                AktuelleKategorie = KategorienListe.First();
            }
            
        }
        
        private void LoescheRechnung()
        {
            if (AktuelleRechnung == null || AktuelleRechnung.ID == -1)
            {
                MessageBox.Show("Rechnung wurde nicht gelöscht! Es wurde keine Rechnung angewählt.", "Löschen Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var result =
                MessageBox.Show(
                    string.Format("Möchtest du die Rechnung ({0}) wirklich löschen?", AktuelleRechnung.Beschreibung),
                    "Bist du sicher?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (!model.LoescheRechnung(AktuelleRechnung))
                {
                    return;
                }
                LadeRechnungen();
                MessageBox.Show("Rechnung gelöscht!", "Löschen erfolgreich", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            
        }
        private void LoescheRueckzahlung()
        {
            if (AktuelleRechnung == null || AktuelleRechnung.ID == -1)
            {
                MessageBox.Show("Rückzahlung wurde nicht gelöscht! Es wurde keine Rückzahlung angewählt.", "Löschen Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var result =
                MessageBox.Show(
                    string.Format("Möchtest du die Rückzahlung {0} wirklich löschen?", AktuelleRechnung.Beschreibung),
                    "Bist du sicher?", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                if (!model.LoescheRueckzahlung(AktuelleRechnung))
                {
                    return;
                }
                LadeRechnungen();
                MessageBox.Show("Rückzahlung gelöscht!", "Löschen erfolgreich", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
        }
        private void SpeichereRechnung()
        {
            if (AktuelleRechnung == null)
            {
                MessageBox.Show("Die Rechnung wurde nicht gespeichert! Es wurde keine Rechnung angewählt.", "Speichern Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (RechnungErrors != 0)
            {
                return;
            }

            int _tempRechnungId = model.SpeichereRechnung(AktuelleRechnung);

            if (_tempRechnungId == -1)
            {
                return;
            }
            LadeRechnungen();
            if (RechnungsListe.Any(i=>i.ID== _tempRechnungId))
            {
                AktuelleRechnung = RechnungsListe.First(i => i.ID == _tempRechnungId);
            }
            
            MessageBox.Show("Rechnung gespeichert!", "Speichern erfolgreich", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        private void SpeichereRueckzahlung()
        {
            if (AktuelleRechnung == null)
            {
                MessageBox.Show("Die Rückzahlung wurde nicht gespeichert! Es wurde keine Rückzahlung angewählt.", "Speichern Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (RueckzahlungErrors != 0)
            {
                return;
            }

            int _tempRechnungId = model.SpeichereRueckzahlung(AktuelleRechnung);

            if (_tempRechnungId == -1)
            {
                return;
            }
            LadeRechnungen();
            if (RechnungsListe.Any(i => i.ID == _tempRechnungId))
            {
                AktuelleRechnung = RechnungsListe.First(i => i.ID == _tempRechnungId);
            }

            MessageBox.Show("Rückzahlung gespeichert!", "Speichern erfolgreich", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        //Wenn gelöschte Kategorie aktiv ist, wird ein Label zusätzlich mit dem alten Namen gezeigt (nicht mehr speicherbar)
        private void UpdateKategorie(string kat)
        {
            if (KategorienListe.Any(i=>i.Name == kat))
            {
                AktuelleKategorie = KategorienListe.First(i => i.Name == kat);
                GeloeschteKategorieAktiv = false;
            }
            else
            {
                GeloeschteKategorieAktiv = true;
            }

        }


        private void LadeRechnungen()
        {
            List<Kategorie> _tempKategorienListe = model.LadeKategorien();
            List<Rechnung> _tempRechnungsListe = new List<Rechnung>();

            KategorienListe.Clear();
            RechnungsListe.Clear();

            if (IstRechnungAktiv && !IstRueckzahlungAktiv)
            {
                _tempRechnungsListe = model.LadeRechnungen("");

            }
            if (!IstRechnungAktiv && IstRueckzahlungAktiv)
            {
                _tempRechnungsListe = model.LadeRueckzahlungen("");

            }

            if (_tempKategorienListe.Count != 0)
            {
                _tempKategorienListe.ForEach(kat => KategorienListe.Add(kat));
                AktuelleKategorie = KategorienListe.First();
            }

            if (_tempRechnungsListe.Count != 0)
            {
                _tempRechnungsListe.ForEach(re => RechnungsListe.Add(re));
                AktuelleRechnung = RechnungsListe.First();
            }
            
            
            
            
        }


        //Konstruktor
        public RechnungViewModel()
        {
            
        }
    }
}
