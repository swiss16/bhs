using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using DA_Buchhaltung.common.log;
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
        private Erfolgsrechnung erfolgsrechnung;

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
                                (i.Name.ToLower().Contains(filterString)) || (i.Vorname.ToLower().Contains(filterString)) ||
                                (i.Adresse.ToLower().Contains(filterString)) || (i.Wohnort.ToLower().Contains(filterString))).ToList();
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
                                (i.Name.ToLower().Contains(filterString)) || (i.Firma.ToLower().Contains(filterString)) ||
                                (i.Adresse.ToLower().Contains(filterString)) || (i.Wohnort.ToLower().Contains(filterString))).ToList();
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
                                (i.Dienstleistung.Name.ToLower().Contains(filterString)) || (i.Total.ToString().Contains(filterString))).ToList();
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
                                (i.Beschreibung.ToLower().Contains(filterString)) || (i.Kategorie.ToLower().Contains(filterString)) || (i.Betrag.ToString().Contains(filterString))).ToList();
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
                                (i.Beschreibung.ToLower().Contains(filterString)) || (i.Kategorie.ToLower().Contains(filterString)) || (i.Betrag.ToString().Contains(filterString))).ToList();
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
        /// <summary>
        /// Erstellt eine Erfolgsrechnung, gemäss dem angebenen Jahr (int). Gibt das Erfolgsrechnungobjekt zurück.
        /// </summary>
        /// <param name="jahr"></param>
        /// <returns></returns>
        public Erfolgsrechnung ErstelleErfolgsrechnung(int jahr)
        {
            if (jahr < 2014)
            {
                erfolgsrechnung = new Erfolgsrechnung();
                Logger.append("Falsche Jahresangabe. Muss mindestens ab 2014 sein!", Logger.INFO);
                MessageBox.Show("Fehler beim erstellen der Erfolgsrechnung. Mehr Informationen sind im Logfile",
                    "Erfolgsrechnung fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                DateTime dts = new DateTime(jahr,1,1,00,00,00);
                DateTime dte = new DateTime(jahr, 12, 31, 23, 59, 59);
                erfolgsrechnung = ErstelleErfolgsrechnung(dts, dte, true);
                
            }
            return erfolgsrechnung;
        }
        /// <summary>
        /// Erstellt eine Erfolgsrechnung, gemäss dem angegebenen Start und Enddatum. Gibt ein Erfolgsrechnungsobjekt zurück.
        /// </summary>
        /// <param name="startDatum"></param>
        /// <param name="endDatum"></param>
        /// <returns></returns>
        public Erfolgsrechnung ErstelleErfolgsrechnung(DateTime startDatum, DateTime endDatum)
        {
            erfolgsrechnung = ErstelleErfolgsrechnung(startDatum,endDatum, false);           
            return erfolgsrechnung;
        }
        /// <summary>
        /// Speichert die Erfolgsrechnung, gemäss dem Erfolgsrechnungsobjekt im Dateisystem ab. Gibt True zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="aktuelleErfolgsrechnung"></param>
        /// <returns></returns>
        public bool SpeichereErfolgsrechnung(Erfolgsrechnung aktuelleErfolgsrechnung)
        {
            bool isSuccessfull = false;
            if (aktuelleErfolgsrechnung != null)
            {
                try
                {
                    isSuccessfull = aktuelleErfolgsrechnung.Print(); 
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString(), "Erfolgsrechnungs Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
                
            }
            if (!isSuccessfull)
            {
                MessageBox.Show("Es ist ein Fehler beim speichern der Erfolgsrechnung aufgetreten. Mehr Informationen stehen im Logfile", "Erfolgsrechnung speichern Fehlgeschlagen", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return isSuccessfull;
        }
        
        

        //Private Methoden
        /// <summary>
        /// Erstellt eine Erfolgsrechnung, gemäss dem angegebenen Start und Enddatum. Gibt ein Erfolgsrechnungsobjekt zurück.
        /// </summary>
        /// <param name="startDatum"></param>
        /// <param name="endDatum"></param>
        /// <param name="istJahresabrechnung"></param>
        /// <returns></returns>
        private Erfolgsrechnung ErstelleErfolgsrechnung(DateTime startDatum, DateTime endDatum, bool istJahresabrechnung)
        {
            if (startDatum.Year < 2014 || endDatum <= startDatum)
            {
                erfolgsrechnung = new Erfolgsrechnung();
                Logger.append("Falsche Jahresangabe. Muss mindestens ab 2014 sein und das Enddatum muss später als das Startdatum sein!", Logger.INFO);
                MessageBox.Show("Fehler beim erstellen der Erfolgsrechnung. Mehr Informationen sind im Logfile",
                    "Erfolgsrechnung fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                erfolgsrechnung = new Erfolgsrechnung();
                erfolgsrechnung.ID = 1;
                if (istJahresabrechnung)
                {
                    erfolgsrechnung.IstJahresabrechnung = true;
                }
                erfolgsrechnung.StartDatum = startDatum;
                erfolgsrechnung.EndDatum = endDatum;
                LadeAuftraege();
                LadeRechnungen();
                LadeRueckzahlungen();

                #region Ein-undAusgabenListen
                //Einnahmen
                decimal _tempPreisAuftraege = 0.00m;

                foreach (var auftrag in auftragsListe.Where(i => (i.Datum.Date >= startDatum.Date) && (i.Datum.Date <= endDatum.Date)) ?? new List<Auftrag>())
                {
                    _tempPreisAuftraege += auftrag.Total;
                }
                erfolgsrechnung.Einnahmen.Add(new Betraege { BetragInFranken = _tempPreisAuftraege, Kategorie = "Dienstleistung" });
                foreach (var rueckerstattung in rueckzahlungsListe.Where(i => (i.Datum.Date >= startDatum.Date) && (i.Datum.Date <= endDatum.Date)) ?? new List<Rechnung>())
                {
                    if (erfolgsrechnung.Einnahmen.Any(i => i.Kategorie.Contains(rueckerstattung.Kategorie)))
                    {
                        erfolgsrechnung.Einnahmen.First(i => i.Kategorie.Contains(rueckerstattung.Kategorie))
                            .BetragInFranken += rueckerstattung.Betrag;
                    }
                    else
                    {
                        erfolgsrechnung.Einnahmen.Add(new Betraege { BetragInFranken = rueckerstattung.Betrag, Kategorie = string.Format("Rückerstattung {0}", rueckerstattung.Kategorie) });
                    }                
                }
                //Hinweis für Kannziel: Gutschein Verkauf gleich wie oben implementieren, aber als Kategorie "Verkauf" angeben

                //Ausgaben
                foreach (var rechnung in rechnungsListe.Where(i => (i.Datum.Date >= startDatum.Date) && (i.Datum.Date <= endDatum.Date)) ?? new List<Rechnung>())
                {
                    if (erfolgsrechnung.Ausgaben.Any(i=>i.Kategorie.Contains(rechnung.Kategorie)))
                    {
                        erfolgsrechnung.Ausgaben.First(i => i.Kategorie.Contains(rechnung.Kategorie)).BetragInFranken
                            += rechnung.Betrag;
                    }
                    else
                    {
                        erfolgsrechnung.Ausgaben.Add(new Betraege { BetragInFranken = rechnung.Betrag, Kategorie = rechnung.Kategorie });
                    }
                } 
                #endregion

                erfolgsrechnung.Update();
            }

            return erfolgsrechnung;
        }

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
                suchtext = suchtext.Trim().ToLower();
                filterStrings = suchtext.Split(filter, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            
            

            return filterStrings;
        } 
    }
}
