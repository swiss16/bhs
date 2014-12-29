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
        /// <summary>
        /// Ändert die Preiseinstellung der Option oder Dienstleistung. Gibt true zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="prOpt"></param>
        /// <returns></returns>
        public bool AenderePreisOption(PreisOption prOpt)
        {
            bool success = false;

            try
            {
                success = dbWrapper.AenderungPreisOption(prOpt);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!success)
            {
                MessageBox.Show("Fehler beim konfigurieren der Option oder Dienstleistung. Mehr Informationen stehen im Logfile", "Konfiguration Fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return success;
        }
        /// <summary>
        /// Speichert ode erstellt einen Kunde, gemäss dem Kundenobjekt. Gibt die ID des Kunden zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="kunde"></param>
        /// <returns></returns>
        public int SpeichereKunde(Kunde kunde)
        {
            int returnValue = -1;
            try
            {
                returnValue = dbWrapper.SpeichernKunde(kunde);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
            if (returnValue == -1)
            {
                MessageBox.Show("Es ist ein Fehler beim Speichern aufgetreten. Mehr Informationen stehen im Logfile", "Speichern Fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return returnValue;
        }
        /// <summary>
        /// Speichert oder erstellt einen Kreditor, gemäss dem Kreditorobjekt. Gibt die ID des Kreditors zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="kreditor"></param>
        /// <returns></returns>
        public int SpeichereKreditor(Kreditor kreditor)
        {
            int returnValue = -1;
            try
            {
                returnValue = dbWrapper.SpeichernKreditor(kreditor);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
            if (returnValue == -1)
            {
                MessageBox.Show("Es ist ein Fehler beim Speichern aufgetreten. Mehr Informationen stehen im Logfile", "Speichern Fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return returnValue;
        }
        /// <summary>
        /// Speichert oder erstellt eine Kategorie, gemäss dem Kategorieobjekt. Gibt die ID der Kategorie zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="kategorie"></param>
        /// <returns></returns>
        public int SpeichereKategorie(Kategorie kategorie)
        {
            int returnValue = -1;
            try
            {
                returnValue = dbWrapper.SpeicherenKategorie(kategorie);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
            if (returnValue == -1)
            {
                MessageBox.Show("Es ist ein Fehler beim Speichern aufgetreten. Mehr Informationen stehen im Logfile", "Speichern Fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return returnValue;
        }
        /// <summary>
        /// Speichert oder erstellt einen Auftrag gemäss dem Auftragobjekt, der dazugehörende Kunde muss aber bereits existieren. Gibt die ID des Auftrages zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="auftrag"></param>
        /// <returns></returns>
        public int SpeichereAuftrag(Auftrag auftrag)
        {
            int returnValue = -1;
            bool kundeIstGespeichert = true;
            try
            {
                if (auftrag.KundeID == -1)
                {
                    kundeIstGespeichert = false;
                }
                else
                {
                    returnValue = dbWrapper.SpeicherenAuftrag(auftrag);
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
            if (kundeIstGespeichert)
            {
                if (returnValue == -1)
                {
                    MessageBox.Show("Es ist ein Fehler beim Speichern aufgetreten. Mehr Informationen stehen im Logfile", "Speichern Fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Der Neukunde muss zuerst gespeichert werden! Vorgang abgebrochen", "Speichern Fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            

            return returnValue;
        }
        /// <summary>
        /// Speichert oder erstellt eine Rechnung gemäss dem Rechnungsobjekt, der dazu gehörende Kreditor muss zuerst erfasst werden. Gibt die ID der Rechnung zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="rechnung"></param>
        /// <returns></returns>
        public int SpeichereRechnung(Rechnung rechnung)
        {
            int returnValue = -1;
            bool kreditorIstGespeichert = true;
            try
            {
                if (rechnung.KreditorID == -1)
                {
                    kreditorIstGespeichert = false;
                }
                else
                {
                    returnValue = dbWrapper.SpeicherenRechnung(rechnung);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
            if (kreditorIstGespeichert)
            {
                if (returnValue == -1)
                {
                    MessageBox.Show("Es ist ein Fehler beim Speichern aufgetreten. Mehr Informationen stehen im Logfile", "Speichern Fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Der Neue Kreditor muss zuerst gespeichert werden! Vorgang abgebrochen", "Speichern Fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            return returnValue;
        }
        /// <summary>
        /// Speichert oder erstellt eine Rückzahlung gemäss dem Rechnungsobjekt, der dazu gehörende Kreditor muss jedoch bereits erfasst sein. Gibt die ID der Rückerstattung zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="rueckzahlung"></param>
        /// <returns></returns>
        public int SpeichereRueckzahlung(Rechnung rueckzahlung)
        {
            int returnValue = -1;
            bool kreditorIstGespeichert = true;
            try
            {
                if (rueckzahlung.KreditorID == -1)
                {
                    kreditorIstGespeichert = false;
                }
                else
                {
                    returnValue = dbWrapper.SpeicherenRueckzahlung(rueckzahlung);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
            if (kreditorIstGespeichert)
            {
                if (returnValue == -1)
                {
                    MessageBox.Show("Es ist ein Fehler beim Speichern aufgetreten. Mehr Informationen stehen im Logfile", "Speichern Fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Der Neue Kreditor muss zuerst gespeichert werden! Vorgang abgebrochen", "Speichern Fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            return returnValue;
        }
        /// <summary>
        /// Löscht den Kunde, gemäss dem Kundenobjekt. Gibt True zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="kunde"></param>
        /// <returns></returns>
        public bool LoescheKunde(Kunde kunde)
        {
            bool isSuccessful = true;
            try
            {
                isSuccessful = dbWrapper.LoescheKunde(kunde);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!isSuccessful)
            {
                MessageBox.Show("Es ist ein Fehler beim Löschen aufgetreten. Mehr Informationen stehen im Logfile", "Löschen Fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            

            return isSuccessful;
        }
        /// <summary>
        /// Löscht den Kreditor, gemäss dem Kreditorenobjekt. Gibt True zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="kreditor"></param>
        /// <returns></returns>
        public bool LoescheKreditor(Kreditor kreditor)
        {
            bool isSuccessful = true;
            try
            {
                isSuccessful = dbWrapper.LoescheKreditor(kreditor);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!isSuccessful)
            {
                MessageBox.Show("Es ist ein Fehler beim Löschen aufgetreten. Mehr Informationen stehen im Logfile", "Löschen Fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            return isSuccessful;
        }
        /// <summary>
        /// Löscht die Kategorie, gemäss dem Kategorieobjekt. Gibt True zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="kategorie"></param>
        /// <returns></returns>
        public bool LoescheKategorie(Kategorie kategorie)
        {
            bool isSuccessful = true;
            try
            {
                isSuccessful = dbWrapper.LoescheKategorie(kategorie);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!isSuccessful)
            {
                MessageBox.Show("Es ist ein Fehler beim Löschen aufgetreten. Mehr Informationen stehen im Logfile", "Löschen Fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            return isSuccessful;
        }
        /// <summary>
        /// Löscht den Auftrag, gemäss dem Auftragobjekt. Gibt True zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="auftrag"></param>
        /// <returns></returns>
        public bool LoescheAuftrag(Auftrag auftrag)
        {
            bool isSuccessful = true;
            try
            {
                isSuccessful = dbWrapper.LoescheAuftrag(auftrag);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!isSuccessful)
            {
                MessageBox.Show("Es ist ein Fehler beim Löschen aufgetreten. Mehr Informationen stehen im Logfile", "Löschen Fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            return isSuccessful;
        }
        /// <summary>
        /// Löscht die Rechnung, gemäss dem Rechnungsobjekt. Gibt True zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="rechnung"></param>
        /// <returns></returns>
        public bool LoescheRechnung(Rechnung rechnung)
        {
            bool isSuccessful = true;
            try
            {
                isSuccessful = dbWrapper.LoescheRechnung(rechnung);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!isSuccessful)
            {
                MessageBox.Show("Es ist ein Fehler beim Löschen aufgetreten. Mehr Informationen stehen im Logfile", "Löschen Fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            return isSuccessful;
        }
        /// <summary>
        /// Löscht die Rückzahlung, gemäss dem Rechnungsobjekt. Gibt True zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="rueckzahlung"></param>
        /// <returns></returns>
        public bool LoescheRueckzahlung(Rechnung rueckzahlung)
        {
            bool isSuccessful = true;
            try
            {
                isSuccessful = dbWrapper.LoescheRueckzahlung(rueckzahlung);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Datenbank Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!isSuccessful)
            {
                MessageBox.Show("Es ist ein Fehler beim Löschen aufgetreten. Mehr Informationen stehen im Logfile", "Löschen Fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            return isSuccessful;
        }

        //TODO: Erfolgsrechnung generieren
        //TODO: Erfolgsrechnung ausgeben (zuerst neu generieren, dann Print() )
        

        //Private Methoden
        /// <summary>
        /// Gibt alle, durch Leerzeichen oder Komma, getrennte Teilstrings als Liste vom typ string zurück. Leerzeichen am Anfang und Ende werden entfernt.
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
