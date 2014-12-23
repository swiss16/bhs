using System;
using System.Collections.Generic;
using System.Linq;
using DA_Buchhaltung.common.log;
using DA_Buchhaltung.data;
using DA_Buchhaltung.model;

namespace DA_Buchhaltung.wrapper
{
    public class DBWrapper
    {
        //Private Properties
        private bhs_DBEntities db = new bhs_DBEntities();
        private bool connectstate;
        private List<Kreditor> kreditorenliste = new List<Kreditor>();
        private List<Kunde> kundenliste = new List<Kunde>();
        private List<Auftrag> auftragsListe = new List<Auftrag>(); 
        private List<Rechnung> rechnungsListe = new List<Rechnung>();
        private List<Rechnung> rueckzahlungsListe = new List<Rechnung>(); 
        private List<PreisOption> preisOptionsListe = new List<PreisOption>();
        private List<Kategorie> kategorienListe = new List<Kategorie>();


        //Private Methoden
        /// <summary>
        ///     Prüft die Verbindung zur Datenbank. Gibt den Wert "false" zurück, wenn die Testverbindung fehlschlägt.
        /// </summary>
        /// <returns>bool true, wenn Verbindung OK ist</returns>
        private bool checkConnection()
        {
            //Verbindungsversuch
            try
            {
                db.SaveChanges();
                connectstate = true;
            }
            catch (Exception)
            {
                try
                {
                    //Reconnect und 2 Versuch
                    db = new bhs_DBEntities();
                    db.SaveChanges();
                    connectstate = true;

                }
                catch (Exception)
                {

                    connectstate = false;
                }
                
            }

            return connectstate;
        }


        //Public Methoden
        /// <summary>
        ///     Lädt alle Kunden aus der Datenbank und gibt diese als Liste von Kunden zurück.
        /// </summary>
        /// <exception>Datenbank Verbindung ist instabil</exception>
        /// <returns>List von Kunde</returns>
        public List<Kunde> LadeKunden()
        {
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            IQueryable<Kunde> klist = from k in db.TBL_Kunde
                join p in db.TBL_Person on k.Kunde_ID equals p.Kunde_ID
                where p.Geloescht == false
                select new Kunde
                {
                    ID = p.Person_ID,
                    KundeID = k.Kunde_ID,
                    Name = p.Name,
                    Vorname = p.Vorname,
                    Adresse = p.Adresse,
                    PLZ = p.PLZ,
                    Wohnort = p.Ortschaft,
                    TelMobile = p.TelMobile,
                    TelPrivat = p.TelPrivat,
                    Email = p.Email,
                    ErfDatum = p.Erfassungsdatum,
                    Reminder = k.Erinnerung
                };
            kundenliste = klist.ToList();

            return kundenliste;
        }

        /// <summary>
        ///     Lädt alle Aufträge aus der Datenbank und gibt diese als Liste von Aufträge zurück.
        /// </summary>
        /// <exception>Datenbank Verbindung ist instabil</exception>
        /// <returns>List von Auftrag</returns>
        public List<Auftrag> LadeAuftraege()
        {
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            IQueryable<Auftrag> list = from a in db.TBL_Auftrag
                join oa in db.TBL_Opt_Auftr on a.Auftrag_ID equals oa.Auftrag_ID
                join d in db.TBL_Dienstleistung on a.Dienstleistung_ID equals d.Dienstleistung_ID
                select new Auftrag
                {
                    ID = a.Auftrag_ID,
                    Total = a.Total,
                    Datum = a.ErfassungsDatum,
                    KundenGespraech = a.KundenGespraech,
                    Dienstleistung = new Dienstleistung
                    {
                        ID = d.Dienstleistung_ID,
                        Name = d.Name,
                        Preis = d.Preis
                    },
                    KundeID = a.Kunde_ID,
                    Positionen = new List<Option>(),
                    Bilder = new List<Bild>()
                };
            foreach (var auftrag in list)
            {
                foreach (var bild in db.TBL_Bild.Where(i=>i.Auftrag_ID == auftrag.ID))
                {
                    Bild tempBild = new Bild();
                    tempBild.ID = bild.Bild_ID;
                    tempBild.Name = bild.BildName;
                    tempBild.Pfad = bild.Pfad;
                    auftrag.Bilder.Add(tempBild);
                }
                foreach (var optAuftr in db.TBL_Opt_Auftr.Where(i=>i.Auftrag_ID == auftrag.ID))
                {
                    Option opt = new Option();
                    var optInDb = db.TBL_Option.Where(i => i.Option_ID == optAuftr.Option_ID).First();
                    opt.ID = optAuftr.Option_ID;
                    opt.PreisInFranken = optAuftr.GesammtPreis;
                    opt.Einheitspreis = optInDb.Einheitspreis;
                    opt.Anzahl = optAuftr.Anzahl;
                    opt.Name = optInDb.Name;
                    if (optAuftr.GesammtPreis <0)
                    {
                        opt.IstRabatt = true;
                    }
                    else
                    {
                        opt.IstRabatt = false;
                    }

                }
            }
            auftragsListe = list.ToList();

            return auftragsListe;
        }

        /// <summary>
        ///     Lädt alle Rechnungen aus der Datenbank und gibt diese als Liste von Rechnungen zurück.
        /// </summary>
        /// <exception>Datenbank Verbindung ist instabil</exception>
        /// <returns>List von Rechnung</returns>
        public List<Rechnung> LadeRechnungen()
        {
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            IQueryable<Rechnung> list = from r in db.TBL_Rechnung
                                         join k in db.TBL_Kategorie on r.Kategorie_ID equals k.Kategorie_ID
                                         select new Rechnung
                                         {
                                             ID = r.Rechnung_ID,
                                             KreditorID = r.Kreditor_ID,
                                             AlsRueckzahlung = false,
                                             Beschreibung = r.Beschreibung,
                                             Betrag = r.Preis,
                                             Datum = r.Erfassungsdatum,
                                             Kategorie = k.Name
                                         };
            rechnungsListe = list.ToList();

            return rechnungsListe;
        }

        /// <summary>
        ///     Lädt alle Kreditoren aus der Datenbank und gibt diese als Liste von Kreditoren zurück.
        /// </summary>
        /// <exception>Datenbank Verbindung ist instabil</exception>
        /// <returns>List von Kreditor</returns>
        public List<Kreditor> LadeKreditoren()
        {
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            IQueryable<Kreditor> klist = from k in db.TBL_Kreditor
                                         join p in db.TBL_Person on k.Kreditor_ID equals p.Kreditor_ID
                                         where p.Geloescht == false
                                         select new Kreditor
                                         {
                                             Firma = k.Firma,
                                             Name = p.Name,
                                             Vorname = p.Vorname,
                                             Adresse = p.Adresse,
                                             PLZ = p.PLZ,
                                             Wohnort = p.Ortschaft,
                                             Email = p.Email,
                                             ErfDatum = p.Erfassungsdatum,
                                             Fax = p.Fax,
                                             ID = p.Person_ID,
                                             KreditorID = k.Kreditor_ID,
                                             TelFirma = p.TelFirma,
                                             TelMobile = p.TelMobile,
                                             TelPrivat = p.TelPrivat
                                         };
            kreditorenliste = klist.ToList();

            return kreditorenliste;
        }

        /// <summary>
        ///     Lädt alle Rückzahlungen aus der Datenbank und gibt diese als Liste von Rechnungen zurück.
        /// </summary>
        /// <exception>Datenbank Verbindung ist instabil</exception>
        /// <returns>List von Rechnung</returns>
        public List<Rechnung> LadeRueckzahlungen()
        {
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            IQueryable<Rechnung> list = from r in db.TBL_Rueckerstattung
                                        join k in db.TBL_Kategorie on r.Kategorie_ID equals k.Kategorie_ID
                                        select new Rechnung
                                        {
                                            ID = r.Rueckerstattung_ID,
                                            KreditorID = r.Kreditor_ID,
                                            AlsRueckzahlung = true,
                                            Beschreibung = r.Beschreibung,
                                            Betrag = r.Total,
                                            Datum = r.Erfassungsdatum,
                                            Kategorie = k.Name
                                        };
            rueckzahlungsListe = list.ToList();

            return rueckzahlungsListe;
        }

        /// <summary>
        ///     Lädt alle Konfigurierbare Dienstleistungen und Optionen aus der Datenbank und gibt diese als Liste von PreisOptionen zurück.
        /// </summary>
        /// <exception>Datenbank Verbindung ist instabil</exception>
        /// <returns>List von PreisOption</returns>
        public List<PreisOption> LadePreisOptionen()
        {
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            IQueryable<PreisOption> list = from r in db.TBL_Option
                                           where r.Konfigurierbar == true && r.PreisEndDatum.Date < DateTime.Now.Date
                                           select new PreisOption
                                           {
                                               ID = r.Option_ID,
                                               Name = r.Name,
                                               Preis = r.Einheitspreis,
                                               PreisKatalog = PreisKatalog.Option
                                           };

            IQueryable<PreisOption> list2 = from r in db.TBL_Dienstleistung
                                           where r.PreisEndDatum.Date < DateTime.Now.Date
                                           select new PreisOption
                                           {
                                               ID = r.Dienstleistung_ID,
                                               Name = r.Name,
                                               Preis = r.Preis,
                                               PreisKatalog = PreisKatalog.Dienstleistung
                                           };
            preisOptionsListe = new List<PreisOption>();
            foreach (var dienstleistung in list2.ToList())
            {
                preisOptionsListe.Add(dienstleistung);
            }
            foreach (var option in list.ToList())
            {
                preisOptionsListe.Add(option);
            }

            return preisOptionsListe;
        }

        /// <summary>
        ///     Lädt alle Kategorien aus der Datenbank und gibt diese als Liste von Kategorien zurück.
        /// </summary>
        /// <exception>Datenbank Verbindung ist instabil</exception>
        /// <returns>List von Kategorie</returns>
        public List<Kategorie> LadeKategorien()
        {
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            IQueryable<Kategorie> list = from r in db.TBL_Kategorie
                                        where r.Geloescht == false
                                        select new Kategorie
                                        {
                                            ID = r.Kategorie_ID,
                                            Name = r.Name
                                        };
            kategorienListe = list.ToList();

            return kategorienListe;
        }

        /// <summary>
        /// Setzt den Gelöscht Status des Kunden auf true. Gibt True zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="kunde">Kunde Objekt</param>
        /// <returns>True wenn erfolgreich</returns>
        public bool LoescheKunde(Kunde kunde)
        {
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            var pers = db.TBL_Person.Find(kunde.ID);
            if (pers == null)
            {
                Logger.append("Der Kunde konnte nicht gefunden werden.", Logger.INFO);
                return false;
            }
            pers.Geloescht = true;
            db.SaveChanges();


            return true;
        }

        /// <summary>
        /// Setzt den Gelöscht Status des Kreditors auf true. Gibt True zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="kreditor">Objekt Kreditor</param>
        /// <returns>True wenn erfolgreich</returns>
        public bool LoescheKreditor(Kreditor kreditor)
        {
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            var pers = db.TBL_Person.Find(kreditor.ID);
            if (pers == null)
            {
                Logger.append("Der Kunde konnte nicht gefunden werden.", Logger.INFO);
                return false;
            }
            pers.Geloescht = true;
            db.SaveChanges();


            return true;
        }

        /// <summary>
        /// Löscht einen Auftrag vollkommen aus der Datenbank. Gibt true zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="auftrag">Auftrag Objekt</param>
        /// <returns>True wenn erfolgreich</returns>
        public bool LoescheAuftrag(Auftrag auftrag)
        {
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            var auftr = db.TBL_Auftrag.Find(auftrag.ID);
            if (auftr == null)
            {
                Logger.append("Der Auftrag konnte nicht gefunden werden.", Logger.INFO);
                return false;
            }
            try
            {
                db.TBL_Auftrag.Remove(auftr);
                db.SaveChanges();
            }
            catch (Exception)
            {
                
                Logger.append("Fehler beim Löschen",Logger.ERROR);
                return false;
            }
            


            return true;
        }

        /// <summary>
        /// Löscht eine Rechnung komplett aus der Datenbank. Gibt true zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="rechnung">Objekt Rechnung</param>
        /// <returns>True wenn erfolgreich</returns>
        public bool LoescheRechnung(Rechnung rechnung)
        {
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            var rech = db.TBL_Rechnung.Find(rechnung.ID);
            if (rech == null)
            {
                Logger.append("Die Rechnung konnte nicht gefunden werden.", Logger.INFO);
                return false;
            }
            try
            {
                db.TBL_Rechnung.Remove(rech);
                db.SaveChanges();
            }
            catch (Exception)
            {

                Logger.append("Fehler beim Löschen", Logger.ERROR);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Löscht eine Rückzahlung komplett aus der Datenbank. Gibt true zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="rueckzahlung">Objekt Rechnung als Rueckzahlung</param>
        /// <returns>true wenn erfolgreich</returns>
        public bool LoescheRueckzahlung(Rechnung rueckzahlung)
        {
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            var rech = db.TBL_Rueckerstattung.Find(rueckzahlung.ID);
            if (rech == null)
            {
                Logger.append("Die Rückerstattung konnte nicht gefunden werden.", Logger.INFO);
                return false;
            }
            try
            {
                db.TBL_Rueckerstattung.Remove(rech);
                db.SaveChanges();
            }
            catch (Exception)
            {

                Logger.append("Fehler beim Löschen", Logger.ERROR);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Lässt die alte PreisOption verfallen und erstellt eine neue gültige für den nächsten Tag. Gibt true zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="preisOption">PreisOption (Dienstleistung oder Option)</param>
        /// <returns>true wenn erfolreich</returns>
        public bool AenderungPreisOption(PreisOption preisOption)
        {
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            if (preisOption == null)
            {
                Logger.append("Die PreisOption wurde falsch übergeben", Logger.INFO);
                return false;
            }

            if (preisOption.PreisKatalog == PreisKatalog.Dienstleistung)
            {
                var dlg = db.TBL_Dienstleistung.Find(preisOption.ID);
                if (dlg == null)
                {
                    Logger.append("Die Dienstleistung konnte nicht gefunden werden", Logger.INFO);
                    return false;
                }
                try
                {
                    dlg.PreisEndDatum = DateTime.Now.Date;
                    var newDlg = new TBL_Dienstleistung();
                    newDlg.Name = preisOption.Name;
                    newDlg.Preis = preisOption.Preis;
                    newDlg.PreisStartDatum = DateTime.Now.AddDays(1).Date;
                    newDlg.PreisEndDatum = newDlg.PreisStartDatum.AddYears(100).Date;
                    db.TBL_Dienstleistung.Add(newDlg);
                }
                catch (Exception)
                {

                    Logger.append("Fehler beim Löschen", Logger.ERROR);
                    return false;
                }
                

            }
            else
            {
                var opt = db.TBL_Option.Find(preisOption.ID);
                if (opt == null)
                {
                    Logger.append("Die Option konnte nicht gefunden werden", Logger.INFO);
                    return false;
                }
                try
                {
                    opt.PreisEndDatum = DateTime.Now.Date;
                    var newOpt = new TBL_Option();
                    newOpt.Einheitspreis = preisOption.Preis;
                    newOpt.Konfigurierbar = true;
                    newOpt.Name = preisOption.Name;
                    newOpt.PreisStartDatum = DateTime.Now.AddDays(1).Date;
                    newOpt.PreisEndDatum = newOpt.PreisStartDatum.AddYears(100).Date;
                    db.TBL_Option.Add(newOpt);
                }
                catch (Exception)
                {

                    Logger.append("Fehler beim Löschen", Logger.ERROR);
                    return false;
                }
            }

            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {

                Logger.append("Fehler beim Löschen", Logger.ERROR);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Setzt den Geloescht Zustand der Kategorie auf true, Gibt true zurück, wenn es erfolgreich war.
        /// </summary>
        /// <param name="kategorie">Kategorie Objekt</param>
        /// <returns>true wenn erfolreich</returns>
        public bool LoescheKategorie(Kategorie kategorie)
        {
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            var kat = db.TBL_Kategorie.Find(kategorie.ID);
            if (kat == null)
            {
                Logger.append("Die Kategorie konnte nicht gefunden werden.", Logger.INFO);
                return false;
            }
            kat.Geloescht = true;
            db.SaveChanges();

            return true;
        }

        /// <summary>
        /// Speichert einen bestehenden oder erstellt einen neuen Kunden, mit dem angegebenen Kunden Objekt. Gibt die ID der Person zurück. Oder -1 wenn es fehlgeschlagen ist.
        /// </summary>
        /// <param name="kunde">Kunden Objekt</param>
        /// <returns>ID der Person</returns>
        public int SpeichernKunde(Kunde kunde)
        {
            int returnValue = -1;
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            int kid = 1;
            if (kunde.Reminder == true)
            {
                kid = 2;
            }
            if (kunde.ID == -1)
            {
                
                var newKunde = new TBL_Person()
                {
                    Kunde_ID = kid,
                    Adresse = kunde.Adresse,
                    Email = kunde.Email,
                    Erfassungsdatum = kunde.ErfDatum,
                    Fax = kunde.Fax,
                    Geloescht = false,
                    Land = "CH",
                    Kreditor_ID = null,
                    Name = kunde.Name,
                    Ortschaft = kunde.Wohnort,
                    TelFirma = kunde.TelFirma,
                    TelMobile = kunde.TelMobile,
                    TelPrivat = kunde.TelPrivat,
                    PLZ = kunde.PLZ,
                    Vorname = kunde.Vorname

                };
                try
                {
                    db.TBL_Person.Add(newKunde);
                    db.SaveChanges();
                    db.Entry(newKunde);
                    returnValue = newKunde.Person_ID;
                }
                catch (Exception)
                {

                    Logger.append("Fehler beim Neuerstellen, schreiben in die Datenbank nicht möglich", Logger.ERROR);
                    return -1;
                }

            }
            else
            {
                var bestKunde = db.TBL_Person.Find(kunde.ID);
                if (bestKunde == null)
                {
                    Logger.append("Der bestehende Kunde konnte nicht geladen werden", Logger.ERROR);
                    return -1;
                }
                    bestKunde.Kunde_ID = kid;
                    bestKunde.Adresse = kunde.Adresse;
                    bestKunde.Email = kunde.Email;
                    bestKunde.Erfassungsdatum = kunde.ErfDatum;
                    bestKunde.Fax = kunde.Fax;
                    bestKunde.Geloescht = false;
                    bestKunde.Land = "CH";
                    bestKunde.Kreditor_ID = null;
                    bestKunde.Name = kunde.Name;
                    bestKunde.Ortschaft = kunde.Wohnort;
                    bestKunde.TelFirma = kunde.TelFirma;
                    bestKunde.TelMobile = kunde.TelMobile;
                    bestKunde.TelPrivat = kunde.TelPrivat;
                    bestKunde.PLZ = kunde.PLZ;
                    bestKunde.Vorname = kunde.Vorname;


                try
                {
                    db.SaveChanges();
                    db.Entry(bestKunde);
                    returnValue = bestKunde.Person_ID;
                }
                catch (Exception)
                {

                    Logger.append("Fehler beim Neuerstellen, schreiben in die Datenbank nicht möglich", Logger.ERROR);
                    return -1;
                }
            }

            

            return returnValue;
        }

        /// <summary>
        /// Speichert einen bestehenden oder erstellt einen neuen Kreditor, gemäss dem Kreditoren Objekt. Gibt die ID der Person zurück, oder -1 wenn es fehlerhaft war.
        /// </summary>
        /// <param name="kreditor">Kreditor Objekt</param>
        /// <returns>ID</returns>
        public int SpeichernKreditor(Kreditor kreditor)
        {
            int returnValue = -1;
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }

            #region KreditorenTabelle
            var firmaList = db.TBL_Kreditor.Where(i => i.Firma == kreditor.Firma);
            TBL_Kreditor firma;
            if (firmaList.Any())
            {
                firma = firmaList.First();
            }
            else
            {
                try
                {
                    firma = new TBL_Kreditor { Firma = kreditor.Firma, KRD_Geloescht = false };
                    db.TBL_Kreditor.Add(firma);
                    db.SaveChanges();
                    db.Entry(firma);
                }
                catch (Exception)
                {

                    Logger.append("Fehler beim Neuerstellen, schreiben in die Datenbank nicht möglich", Logger.ERROR);
                    return -1;
                }

            } 
            #endregion
            if (kreditor.ID == -1)
            {
                
                var newKreditor = new TBL_Person()
                {
                    Kreditor_ID = firma.Kreditor_ID,
                    Adresse = kreditor.Adresse,
                    Email = kreditor.Email,
                    Erfassungsdatum = kreditor.ErfDatum,
                    Fax = kreditor.Fax,
                    Geloescht = false,
                    Land = "CH",
                    Kunde_ID = null,
                    Name = kreditor.Name,
                    Ortschaft = kreditor.Wohnort,
                    TelFirma = kreditor.TelFirma,
                    TelMobile = kreditor.TelMobile,
                    TelPrivat = kreditor.TelPrivat,
                    PLZ = kreditor.PLZ,
                    Vorname = kreditor.Vorname

                };
                try
                {
                    db.TBL_Person.Add(newKreditor);
                    db.SaveChanges();
                    db.Entry(newKreditor);
                    returnValue = newKreditor.Person_ID;
                }
                catch (Exception)
                {

                    Logger.append("Fehler beim Neuerstellen, schreiben in die Datenbank nicht möglich", Logger.ERROR);
                    return -1;
                }

            }
            else
            {
                var bestKreditor = db.TBL_Person.Find(kreditor.ID);
                if (bestKreditor == null)
                {
                    Logger.append("Der bestehende Kunde konnte nicht geladen werden", Logger.ERROR);
                    return -1;
                }
                bestKreditor.Kunde_ID = null;
                bestKreditor.Adresse = kreditor.Adresse;
                bestKreditor.Email = kreditor.Email;
                bestKreditor.Erfassungsdatum = kreditor.ErfDatum;
                bestKreditor.Fax = kreditor.Fax;
                bestKreditor.Geloescht = false;
                bestKreditor.Land = "CH";
                bestKreditor.Kreditor_ID = firma.Kreditor_ID;
                bestKreditor.Name = kreditor.Name;
                bestKreditor.Ortschaft = kreditor.Wohnort;
                bestKreditor.TelFirma = kreditor.TelFirma;
                bestKreditor.TelMobile = kreditor.TelMobile;
                bestKreditor.TelPrivat = kreditor.TelPrivat;
                bestKreditor.PLZ = kreditor.PLZ;
                bestKreditor.Vorname = kreditor.Vorname;


                try
                {
                    db.SaveChanges();
                    db.Entry(bestKreditor);
                    returnValue = bestKreditor.Person_ID;
                }
                catch (Exception)
                {

                    Logger.append("Fehler beim Neuerstellen, schreiben in die Datenbank nicht möglich", Logger.ERROR);
                    return -1;
                }
            }



            return returnValue;
        }

        /// <summary>
        /// Speichert eine bestehende oder erstellt eine neue Kategorie, gemäss dem Kategorieobjekt. Gibt die ID der kategorie zurück, -1 wenn es nicht erfolgreich war.
        /// </summary>
        /// <param name="kategorie">Kategorie Objekt</param>
        /// <returns>ID der Kategorie</returns>
        public int SpeicherenKategorie(Kategorie kategorie)
        {
            int returnValue = -1;
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            if (kategorie.ID == -1)
            {
                var newKat = new TBL_Kategorie
                {
                    Name = kategorie.Name,
                    Geloescht = false
                };

                try
                {
                    db.TBL_Kategorie.Add(newKat);
                    db.SaveChanges();
                    db.Entry(newKat);
                    returnValue = newKat.Kategorie_ID;
                }
                catch (Exception)
                {

                    Logger.append("Fehler beim Neuerstellen, schreiben in die Datenbank nicht möglich", Logger.ERROR);
                    return -1;
                }


            }
            else
            {
                var oldKat = db.TBL_Kategorie.Find(kategorie.ID);
                oldKat.Name = kategorie.Name;
                oldKat.Geloescht = false;

                try
                {
                    db.SaveChanges();
                    returnValue = oldKat.Kategorie_ID;
                }
                catch (Exception)
                {

                    Logger.append("Fehler beim Neuerstellen, schreiben in die Datenbank nicht möglich", Logger.ERROR);
                    return -1;
                }
            }


            return returnValue;
        }


        //Todo: Save methoden für Aufträge,Rechnungen, Rückzahlungen
    }

}