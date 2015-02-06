/*
 * Klasse: KundeViewModel.cs
 * Author: Martin Osterwalder
 * Stellt für die View die Commands und Properties bereit um die entsprechende Modelfunktionen aufzurufen.
 * Die View bindet die Steuerungselemente an diese Commands und Properties.
 * Zuständig für die weiterleitung jeglichen Aufgaben um die Kunden zu verwalten.
 */
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DA_Buchhaltung.common.commands;
using DA_Buchhaltung.model;

namespace DA_Buchhaltung.viewModel
{
    public class KundeViewModel : ViewModelBase
    {
        //NavigationsProperties
        Model model = new Model();
        public AuftragViewModel AuftragViewModel;

        //Statische Felder
        public static int Errors { get; set; }

        //Properties
        private bool _keinNeuerKundeAktiv = false;
        public bool KeinNeuerKundeAktiv
        {
            get { return _keinNeuerKundeAktiv; }
            set
            {
                if (value != _keinNeuerKundeAktiv)
                {
                    _keinNeuerKundeAktiv = value;
                    OnPropertyChanged("KeinNeuerKundeAktiv");
                }
            }
        }
        private Kunde _aktuellerKunde = new Kunde();
        public Kunde AktuellerKunde
        {
            get { return _aktuellerKunde; }
            set
            {
                if (value != null)
                {
                    if (value.ID != _aktuellerKunde.ID)
                    {
                        _aktuellerKunde = value;
                        OnPropertyChanged("AktuellerKunde");
                        AuftragViewModel.UpdateKunde(value.ID);
                        if (value.ID == -1)
                        {
                            KeinNeuerKundeAktiv = false;
                        }
                        else
                        {
                            KeinNeuerKundeAktiv = true;
                        }
                    }
                }
                else
                {
                    AktuellerKunde = new Kunde();
                }
                
                
            }
        }

        private bool _kundenListeIstNichtLeer;
        public bool KundenListeIstNichtLeer
        {
            get { return _kundenListeIstNichtLeer; }
            set
            {
                if (value != _kundenListeIstNichtLeer)
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
                if (value != _selectedKundenIndex)
                {
                    _selectedKundenIndex = value;
                    OnPropertyChanged("SelectedKundenIndex");
                }
            }
        }

        private string _suchText = string.Empty;
        public string SuchText
        {
            get { return _suchText; }
            set
            {
                if (value != _suchText)
                {
                    _suchText = value;
                    OnPropertyChanged("SuchText");

                }
            }
        }

        //Sichtbarkeits Properties
        private bool _istKundenAktiv = false;
        public bool IstKundenAktiv
        {
            get { return _istKundenAktiv; }
            set
            {
                if (value != _istKundenAktiv)
                {
                    _istKundenAktiv = value;
                    OnPropertyChanged("IstKundenAktiv");
                }
            }
        }

        //Listen
        private readonly ObservableCollection<Kunde> _kundenListe = new ObservableCollection<Kunde>();
        public ObservableCollection<Kunde> KundenListe
        {
            get { return _kundenListe; }
        }

        //Commands
        private RelayCommand<string> _ladeKundenCommand;
        public RelayCommand<string> LadeKundenCommand
        {
            get { return _ladeKundenCommand ?? (_ladeKundenCommand = new RelayCommand<string>(LadeKunden)); }
        }

        private RelayCommand<string> _erstelleKundenCommand;
        public RelayCommand<string> ErstelleKundenCommand
        {
            get { return _erstelleKundenCommand ?? (_erstelleKundenCommand = new RelayCommand<string>(ErstelleKunde)); }
        }

        private RelayCommand<int> _speichernKundeCommand;
        public RelayCommand<int> SpeichernKundeCommand
        {
            get { return _speichernKundeCommand ?? (_speichernKundeCommand = new RelayCommand<int>(SpeichernKunde)); }
        }

        private RelayCommand<int> _loeschenKundeCommand;
        public RelayCommand<int> LoeschenKundeCommand
        {
            get { return _loeschenKundeCommand ?? (_loeschenKundeCommand = new RelayCommand<int>(LoeschenKunde)); }
        }

        


        //Commandhelper
        private void LadeKunden(string suchText)
        {
            List<Kunde> _tempKundenListe = model.LadeKunden(suchText);

            KundenListeIstNichtLeer = false;

            KundenListe.Clear();

            if (_tempKundenListe.Count == 0)
            {
                return;
            }
            _tempKundenListe.ForEach(customer => KundenListe.Add(customer));


            SelectedKundenIndex = -1;
            KundenListeIstNichtLeer = true;

        }
        private void ErstelleKunde(string nix)
        {
            SelectedKundenIndex = -1;
            AktuellerKunde = new Kunde();
            AktuellerKunde.GetDefault();

        }
        private void SpeichernKunde(int id)
        {
            
            if (AktuellerKunde == null)
            {
                MessageBox.Show("Kunde wurde nicht gespeichert! Es wurde kein Kunde angewählt.", "Speichern Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (Errors!=0)
            {
                return;
            }

            int _tempKundeIndex = model.SpeichereKunde(AktuellerKunde);

            if (_tempKundeIndex == -1)
            {
                return;
            }
            LadeKunden("");
            AktuellerKunde = KundenListe.FirstOrDefault(i => i.ID == _tempKundeIndex);
            MessageBox.Show("Kunde gespeichert!", "Speichern erfolgreich", MessageBoxButton.OK,
                MessageBoxImage.Information);
            

        }
        private void LoeschenKunde(int id)
        {
            if (AktuellerKunde == null || AktuellerKunde.ID == -1)
            {
                MessageBox.Show("Kunde wurde nicht gelöscht! Es wurde kein Kunde angewählt.", "Löschen Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


            if (!model.LoescheKunde(AktuellerKunde))
            {
                return;
            }
            SelectedKundenIndex = -1;
            LadeKunden("");
            MessageBox.Show("Kunde gelöscht!", "Löschen erfolgreich", MessageBoxButton.OK,
                MessageBoxImage.Information);


        }

        //Konstruktor
        public KundeViewModel()
        {
            LadeKunden("");
        }

    }
}
