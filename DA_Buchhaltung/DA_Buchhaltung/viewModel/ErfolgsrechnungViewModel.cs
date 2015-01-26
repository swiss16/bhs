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
    public class ErfolgsrechnungViewModel : ViewModelBase
    {

        //NavigationsProperties
        Model model = new Model();

        //Statische Felder
        public static int Errors { get; set; }

        //Properties
        
        private Erfolgsrechnung _aktuelleErfolgsrechnung = new Erfolgsrechnung();
        public Erfolgsrechnung AktuelleErfolgsrechnung
        {
            get { return _aktuelleErfolgsrechnung; }
            set
            {
                if (value != null)
                {
                    if (value.ID != _aktuelleErfolgsrechnung.ID)
                    {
                        _aktuelleErfolgsrechnung = value;
                        OnPropertyChanged("AktuelleErfolgsrechnung");
                        UpdateLists();
                    }
                }
                else
                {
                    AktuelleErfolgsrechnung = new Erfolgsrechnung();
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
            if (Errors != 0)
            {
                return;
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
            
        }
    }
}
