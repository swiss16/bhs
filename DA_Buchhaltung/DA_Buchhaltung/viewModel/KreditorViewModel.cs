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
    public class KreditorViewModel:ViewModelBase
    {
        //NavigationsProperties
        Model model = new Model();

        //Statische Felder
        public static int Errors { get; set; }

        //Properties
        private bool _keinNeuerKreditorAktiv = false;
        public bool KeinNeuerKreditorAktiv
        {
            get { return _keinNeuerKreditorAktiv; }
            set
            {
                if (value != _keinNeuerKreditorAktiv)
                {
                    _keinNeuerKreditorAktiv = value;
                    OnPropertyChanged("KeinNeuerKreditorAktiv");
                }
            }
        }
        private Kreditor _aktuellerKreditor = new Kreditor();
        public Kreditor AktuellerKreditor
        {
            get { return _aktuellerKreditor; }
            set
            {
                if (value != null)
                {
                    if (value.ID != _aktuellerKreditor.ID)
                    {
                        _aktuellerKreditor = value;
                        OnPropertyChanged("AktuellerKreditor");
                        if (value.ID == -1)
                        {
                            KeinNeuerKreditorAktiv = false;
                        }
                        else
                        {
                            KeinNeuerKreditorAktiv = true;
                        }
                    }
                }
                else
                {
                    AktuellerKreditor = new Kreditor();
                }
                
                
            }
        }

        private bool _kreditorenListeIstNichtLeer;
        public bool KreditorenListeIstNichtLeer
        {
            get { return _kreditorenListeIstNichtLeer; }
            set
            {
                if (value != _kreditorenListeIstNichtLeer)
                {
                    _kreditorenListeIstNichtLeer = value;
                    OnPropertyChanged("KreditorenListeIstNichtLeer");
                }
            }
        }

        private int _selectedKreditorenIndex;
        public int SelectedKreditorenIndex
        {
            get { return _selectedKreditorenIndex; }
            set
            {
                if (value != _selectedKreditorenIndex)
                {
                    _selectedKreditorenIndex = value;
                    OnPropertyChanged("SelectedKreditorenIndex");
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

        

        //Listen
        private readonly ObservableCollection<Kreditor> _kreditorenListe = new ObservableCollection<Kreditor>();
        public ObservableCollection<Kreditor> KreditorenListe
        {
            get { return _kreditorenListe; }
        }
        

        //Commands
        private RelayCommand<string> _ladeKreditorenCommand;
        public RelayCommand<string> LadeKreditorenCommand
        {
            get { return _ladeKreditorenCommand ?? (_ladeKreditorenCommand = new RelayCommand<string>(LadeKreditoren)); }
        }

        private RelayCommand<string> _erstelleKreditorCommand;
        public RelayCommand<string> ErstelleKreditorCommand
        {
            get { return _erstelleKreditorCommand ?? (_erstelleKreditorCommand = new RelayCommand<string>(ErstelleKreditor)); }
        }

        private RelayCommand<int> _speichernKreditorCommand;
        public RelayCommand<int> SpeichernKreditorCommand
        {
            get { return _speichernKreditorCommand ?? (_speichernKreditorCommand = new RelayCommand<int>(SpeichernKreditor)); }
        }

        private RelayCommand<int> _loeschenKreditorCommand;
        public RelayCommand<int> LoeschenKreditorCommand
        {
            get { return _loeschenKreditorCommand ?? (_loeschenKreditorCommand = new RelayCommand<int>(LoeschenKreditor)); }
        }

        


        //Commandhelper
        private void LadeKreditoren(string suchText)
        {
            List<Kreditor> _tempKreditorenListe = model.LadeKreditoren(suchText);

            KreditorenListeIstNichtLeer = false;

            KreditorenListe.Clear();

            if (_tempKreditorenListe.Count == 0)
            {
                return;
            }
            _tempKreditorenListe.ForEach(customer => KreditorenListe.Add(customer));


            SelectedKreditorenIndex = -1;
            KreditorenListeIstNichtLeer = true;

        }
        private void ErstelleKreditor(string nix)
        {
            AktuellerKreditor = new Kreditor();
        }
        private void SpeichernKreditor(int id)
        {
            
            if (AktuellerKreditor == null)
            {
                MessageBox.Show("Kreditor wurde nicht gespeichert! Es wurde kein Kreditor angewählt.", "Speichern Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            if (Errors!=0)
            {
                return; // Speichern abbrechen, da Validierung Fehler meldet
            }

            int _kreditorIndex = model.SpeichereKreditor(AktuellerKreditor);

            if (_kreditorIndex == -1)
            {
                return;
            }
            LadeKreditoren("");
            AktuellerKreditor = KreditorenListe.FirstOrDefault(i => i.ID == _kreditorIndex);
            MessageBox.Show("Kreditor gespeichert!", "Speichern erfolgreich", MessageBoxButton.OK,
                MessageBoxImage.Information);
            

        }
        private void LoeschenKreditor(int id)
        {
            if (AktuellerKreditor == null || AktuellerKreditor.ID == -1)
            {
                MessageBox.Show("Kreditor wurde nicht gelöscht! Es wurde kein Kreditor angewählt.", "Löschen Abgebrochen",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }


            if (!model.LoescheKreditor(AktuellerKreditor))
            {
                return;
            }
            SelectedKreditorenIndex = -1;
            LadeKreditoren("");
            MessageBox.Show("Kreditor gelöscht!", "Löschen erfolgreich", MessageBoxButton.OK,
                MessageBoxImage.Information);


        }

        //Konstruktor
        public KreditorViewModel()
        {
            LadeKreditoren("");
        }

    }
}
