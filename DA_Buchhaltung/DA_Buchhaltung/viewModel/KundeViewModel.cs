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
    public class KundeViewModel : ViewModelBase
    {
        //NavigationsProperties
        Model model = new Model();

        //Properties

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

        private string _suchText = string.Empty;
        public string SuchText
        {
            get { return _suchText; }
            set
            {
                if (SuchText != _suchText)
                {
                    _suchText = value;
                    OnPropertyChanged("SuchText");
                }
            }
        }

        

        //Listen
        private readonly ObservableCollection<Kunde> _kundenListe = new ObservableCollection<Kunde>();
        public ObservableCollection<Kunde> KundenListe
        {
            get { return _kundenListe; }
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
        private RelayCommand<string> _ladeKundenCommand;
        public RelayCommand<string> LadeKundenCommand
        {
            get { return _ladeKundenCommand ?? (_ladeKundenCommand = new RelayCommand<string>(LadeKunden)); }
        }

        private RelayCommand<string> _erstelleKundenCommand;
        public RelayCommand<string> ErstelleKundenCommand
        {
            get { return _erstelleKundenCommand ?? (_erstelleKundenCommand = new RelayCommand<string>(ErstelleKunden)); }
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
        private void ErstelleKunden(string nix)
        {

            Kunde newKunde = new Kunde();
            KundenListe.Add(newKunde);
            AktuellerKunde = newKunde;
            if (KundenListe.Contains(newKunde))
            {
                SelectedKundenIndex = KundenListe.IndexOf(newKunde);
            }
            

        }
        private void SpeichernKunde(int id)
        {
            if (AktuellerKunde == null || (AktuellerKunde.ID == id))
            {
                MessageBox.Show("Kunde wurde nicht gespeichert! Es wurde kein Kunde angewählt.", "Speichern Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            SelectedKundenIndex = model.SpeichereKunde(AktuellerKunde);

            if (SelectedKundenIndex == -1)
            {
                return;
            }
            MessageBox.Show("Kunde gespeichert!", "Speichern erfolgreich", MessageBoxButton.OK,
                MessageBoxImage.Information);


        }
        private void LoeschenKunde(int id)
        {
            if (AktuellerKunde == null || (AktuellerKunde.ID == id))
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
    }
}
