/*
 * Klasse: EinstellungenViewModel.cs
 * Author: Martin Osterwalder
 * Stellt für die View die Commands und Properties bereit um die entsprechende Modelfunktionen aufzurufen.
 * Die View bindet die Steuerungselemente an diese Commands und Properties.
 * Zuständig für die weiterleitung jeglichen Aufgaben um Einstellungen zu speichern.
 */
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
    public class EinstellungenViewModel : ViewModelBase
    {
        //NavigationsProperties
        Model model = new Model();

        //Statische Felder
        public static int Errors { get; set; }

        //Properties
        
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
                        if (value.ID == -1)
                        {
                            KeineNeueKategorieAktiv = false;
                        }
                        else
                        {
                            KeineNeueKategorieAktiv = true;
                        }
                    }
                }
                else
                {
                    AktuelleKategorie = new Kategorie();
                }


            }
        }

        private PreisOption _aktuellePreisOption = new PreisOption();
        public PreisOption AktuellePreisOption
        {
            get { return _aktuellePreisOption; }
            set
            {
                if (value != null)
                {
                    if (value.ID != _aktuellePreisOption.ID)
                    {
                        _aktuellePreisOption = value;
                        OnPropertyChanged("AktuellePreisOption");
                    }
                }
                else
                {
                    AktuellePreisOption = new PreisOption();
                }


            }
        }

        private bool _keineNeueKategorieAktiv = false;
        public bool KeineNeueKategorieAktiv
        {
            get { return _keineNeueKategorieAktiv; }
            set
            {
                if (value != _keineNeueKategorieAktiv)
                {
                    _keineNeueKategorieAktiv = value;
                    OnPropertyChanged("KeineNeueKategorieAktiv");
                }
            }
        }

        //Sichtbarkeits Properties
        private bool _istEinstellungenAktiv = false;
        public bool IstEinstellungenAktiv
        {
            get { return _istEinstellungenAktiv; }
            set
            {
                if (value != _istEinstellungenAktiv)
                {
                    _istEinstellungenAktiv = value;
                    OnPropertyChanged("IstEinstellungenAktiv");
                }
            }
        }

        //Listen
        private readonly ObservableCollection<Kategorie> _kategorieListe = new ObservableCollection<Kategorie>();
        public ObservableCollection<Kategorie> KategorieListe
        {
            get { return _kategorieListe; }
        }

        private readonly ObservableCollection<PreisOption> _preisOptionsListe = new ObservableCollection<PreisOption>();
        public ObservableCollection<PreisOption> PreisOptionsListe
        {
            get { return _preisOptionsListe; }
        }

        //Commands
        private SimpleCommand _neueKategorieCommand;
        public SimpleCommand NeueKategorieCommand
        {
            get { return _neueKategorieCommand ?? (_neueKategorieCommand = new SimpleCommand(NeueKategorie)); }
        }

        private SimpleCommand _speichereKategorieCommand;
        public SimpleCommand SpeichereKategorieCommand
        {
            get { return _speichereKategorieCommand ?? (_speichereKategorieCommand = new SimpleCommand(SpeichereKategorie)); }
        }

        private SimpleCommand _loescheKategorieCommand;
        public SimpleCommand LoescheKategorieCommand
        {
            get { return _loescheKategorieCommand ?? (_loescheKategorieCommand = new SimpleCommand(LoescheKategorie)); }
        }

        private SimpleCommand _speicherePreisOptionCommand;
        public SimpleCommand SpeicherePreisOptionCommand
        {
            get { return _speicherePreisOptionCommand ?? (_speicherePreisOptionCommand = new SimpleCommand(SpeicherePreisOption)); }
        }

        //Command Helpers
        private void SpeichereKategorie()
        {
            if (AktuelleKategorie == null)
            {
                MessageBox.Show("Kategorie wurde nicht gespeichert! Es wurde keine Kategorie angewählt.", "Speichern Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (Errors != 0)
            {
                return;
            }

            int _tempKategorieIndex = model.SpeichereKategorie(AktuelleKategorie);

            if (_tempKategorieIndex == -1)
            {
                return;
            }
            LadeDaten();
            AktuelleKategorie = KategorieListe.FirstOrDefault(i => i.ID == _tempKategorieIndex);
            MessageBox.Show("Kategorie gespeichert!", "Speichern erfolgreich", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void NeueKategorie()
        {
            AktuelleKategorie = new Kategorie();
        }

        private void LoescheKategorie()
        {
            if (AktuelleKategorie == null || AktuelleKategorie.ID == -1)
            {
                MessageBox.Show("Kategorie wurde nicht gelöscht! Es wurde keine Kategorie angewählt.", "Löschen Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


            if (!model.LoescheKategorie(AktuelleKategorie))
            {
                return;
            }
            LadeDaten();
            MessageBox.Show("Kategorie gelöscht!", "Löschen erfolgreich", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void SpeicherePreisOption()
        {
            if (AktuellePreisOption == null || AktuellePreisOption.ID == -1)
            {
                MessageBox.Show("Die Preiseinstellung wurde nicht gespeichert! Es wurde keine Preiseinstellung angewählt.", "Speichern Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (Errors != 0)
            {
                return;
            }

            bool _speichernErfolgreich = model.AenderePreisOption(AktuellePreisOption);

            if (!_speichernErfolgreich)
            {
                return;
            }
            Thread.Sleep(1500);
            LadeDaten();
            AktuellePreisOption = new PreisOption();
            MessageBox.Show("Preiseinstellung gespeichert!", "Speichern erfolgreich", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void LadeDaten()
        {
            List<Kategorie> _tempKategorienListe = model.LadeKategorien();
            List<PreisOption> _tempPreisOptionenListe = model.LadePreisOptionen();

            KategorieListe.Clear();
            PreisOptionsListe.Clear();

            if (_tempKategorienListe.Count != 0)
            {
                _tempKategorienListe.ForEach(kat => KategorieListe.Add(kat));
                AktuelleKategorie = KategorieListe.First();
            }
            
            if (_tempPreisOptionenListe.Count != 0)
            {
                _tempPreisOptionenListe.ForEach(propt => PreisOptionsListe.Add(propt));
                AktuellePreisOption = PreisOptionsListe.First();
            }

        }


        //Konstruktor
        public EinstellungenViewModel()
        {
            LadeDaten();
        }
    }
}
