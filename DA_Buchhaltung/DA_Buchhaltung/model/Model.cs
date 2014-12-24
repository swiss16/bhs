using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DA_Buchhaltung.wrapper;

namespace DA_Buchhaltung.model
{
    public class Model
    {
        //Private Variablen
        DBWrapper dbWrapper = new DBWrapper();
        private List<Kunde> kundenListe;
        private List<Kreditor> kreditorenListe;
        private List<Auftrag> auftragsListe ;
        private List<Rechnung> rechnungsListe ;
        private List<Rechnung> rueckzahlungsListe ;
        private List<PreisOption> preisOptionsListe ;
        private List<Kategorie> kategorienListe ;
        private List<Option> optionenListe ;

        //public Methoden
        /// <summary>
        /// Gibt die Liste aller nicht gelöschten Kunden aus und filtert gemäss suchtext string (Name,Vorname,Adresse, Wohnort). No Exceptions
        /// </summary>
        /// <param name="suchText"></param>
        /// <returns></returns>
        public List<Kunde> LadeKunden(string suchText = "")
        {
            try
            {
                kundenListe = dbWrapper.LadeKunden();
                foreach (var filterString in getFilterStrings(suchText))
                {
                    kundenListe =
                        kundenListe.Where(
                            i =>
                                (i.Name.Contains(filterString)) || (i.Vorname.Contains(filterString)) ||
                                (i.Adresse.Contains(filterString)) || (i.Wohnort.Contains(filterString))).ToList();
                }
            }
            catch (Exception e)
            {
                kundenListe = new List<Kunde>();
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return kundenListe;
        }
        /// <summary>
        /// Gibt die Liste aller nicht gelöschten Kreditoren aus und werden gefiltert gemäss dem Suchtext string (Name,Vorname,Adresse, Wohnort). No Exception
        /// </summary>
        /// <param name="suchText"></param>
        /// <returns></returns>
        public List<Kreditor> LadeKreditoren(string suchText = "")
        {
            try
            {
                kreditorenListe = dbWrapper.LadeKreditoren();
                foreach (var filterString in getFilterStrings(suchText))
                {
                    kreditorenListe =
                        kreditorenListe.Where(
                            i =>
                                (i.Name.Contains(filterString)) || (i.Vorname.Contains(filterString)) ||
                                (i.Adresse.Contains(filterString)) || (i.Wohnort.Contains(filterString))).ToList();
                }

            }
            catch (Exception e)
            {
                kreditorenListe = new List<Kreditor>();
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return kreditorenListe;
        }
        /// <summary>
        /// Gibt die Liste aller Aufträge aus und werden gefiltert gemäss dem Suchtext string (Dienstleistung und Total). No Exception
        /// </summary>
        /// <param name="suchText"></param>
        /// <returns></returns>
        public List<Auftrag> LadeAuftraege(string suchText = "")
        {
            try
            {
                auftragsListe = dbWrapper.LadeAuftraege();
                foreach (var filterString in getFilterStrings(suchText))
                {
                    auftragsListe =
                        auftragsListe.Where(
                            i =>
                                (i.Dienstleistung.Name.Contains(filterString)) || (i.Total.ToString().Contains(filterString))).ToList();
                }

            }
            catch (Exception e)
            {
                auftragsListe = new List<Auftrag>();
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return auftragsListe;
        }
        /// <summary>
        /// Gibt die Liste aller Rechnungen aus und werden gefiltert gemäss dem Suchtext string (Beschreibung und Betrag). No Exception
        /// </summary>
        /// <param name="suchText"></param>
        /// <returns></returns>
        public List<Rechnung> LadeRechnungen(string suchText = "")
        {
            try
            {
                rechnungsListe = dbWrapper.LadeRechnungen();
                foreach (var filterString in getFilterStrings(suchText))
                {
                    rechnungsListe =
                        rechnungsListe.Where(
                            i =>
                                (i.Beschreibung.Contains(filterString))||(i.Kategorie.Contains(filterString)) || (i.Betrag.ToString().Contains(filterString))).ToList();
                }

            }
            catch (Exception e)
            {
                rechnungsListe = new List<Rechnung>();
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return rechnungsListe;
        }
        /// <summary>
        /// Gibt die Liste aller Rückzahlungen aus und werden gefiltert gemäss dem Suchtext string (Beschreibung und Betrag). No Exception
        /// </summary>
        /// <param name="suchText"></param>
        /// <returns></returns>
        public List<Rechnung> LadeRueckzahlungen(string suchText = "")
        {
            try
            {
                rueckzahlungsListe = dbWrapper.LadeRueckzahlungen();
                foreach (var filterString in getFilterStrings(suchText))
                {
                    rueckzahlungsListe =
                        rueckzahlungsListe.Where(
                            i =>
                                (i.Beschreibung.Contains(filterString)) || (i.Kategorie.Contains(filterString)) || (i.Betrag.ToString().Contains(filterString))).ToList();
                }

            }
            catch (Exception e)
            {
                rueckzahlungsListe = new List<Rechnung>();
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return rueckzahlungsListe;
        }
        /// <summary>
        /// Gibt die Liste aller PreisOptionen (Dienstleistung und Optionen) aus, welche konfigurierbar sind. No Exception
        /// </summary>
        /// <returns></returns>
        public List<PreisOption> LadePreisOptionen()
        {
            try
            {
                preisOptionsListe = dbWrapper.LadePreisOptionen();
                

            }
            catch (Exception e)
            {
                preisOptionsListe = new List<PreisOption>();
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return preisOptionsListe;
        }
        /// <summary>
        /// Gibt die Liste aller Optionen aus, welche konfigurierbar und aktuell aktiv sind. No Exception
        /// </summary>
        /// <returns></returns>
        public List<Option> LadeOptionen()
        {
            try
            {
                optionenListe = dbWrapper.LadeOptionen();

            }
            catch (Exception e)
            {
                optionenListe = new List<Option>();
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return optionenListe;
        }
        /// <summary>
        /// Gibt eine Liste aller Kategorien zurück, welche nicht als gelöscht markiert sind. No Exception
        /// </summary>
        /// <returns></returns>
        public List<Kategorie> LadeKategorien()
        {
            try
            {
                kategorienListe = dbWrapper.LadeKategorien();
                
            }
            catch (Exception e)
            {
                kategorienListe = new List<Kategorie>();
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return kategorienListe;
        } 

        

        //Private Methoden
        /// <summary>
        /// Gibt alle durch Leerzeichen getrennte Teilstrings als Liste vom typ string zurück. Leerzeichen am Anfang und Ende werden entfernt.
        /// </summary>
        /// <param name="suchtext">zu filtender string</param>
        /// <returns>List von strings</returns>
        private List<string> getFilterStrings(string suchtext)
        {
            List<string> filterStrings = new List<string>();
            string[] filter = new string[2] { ",", " " };
            if (suchtext != "")
            {
                suchtext = suchtext.Trim();
                filterStrings = suchtext.Split(filter, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            
            

            return filterStrings;
        } 
    }
}
