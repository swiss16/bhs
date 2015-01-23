using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
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
        private List<Option> optionenListe = new List<Option>(); 


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
                    KundeID = a.Person_ID,
                    
                };
            
                foreach (var auftrag in list)
                {
                    foreach (var bild in db.TBL_Bild.Where(i => i.Auftrag_ID == auftrag.ID))
                    {
                        Bild tempBild = new Bild();
                        tempBild.ID = bild.Bild_ID;
                        tempBild.Name = bild.BildName;
                        tempBild.Pfad = bild.Pfad;
                        auftrag.Bilder.Add(tempBild);
                    }
                    foreach (var optAuftr in db.TBL_Opt_Auftr.Where(i => i.Auftrag_ID == auftrag.ID))
                    {
                        Option opt = new Option();
                        var optInDb = db.TBL_Option.Where(i => i.Option_ID == optAuftr.Option_ID).First();
                        opt.ID = optAuftr.Option_ID;
                        opt.PreisInFranken = optAuftr.GesammtPreis;
                        opt.Einheitspreis = optInDb.Einheitspreis;
                        opt.Anzahl = optAuftr.Anzahl;
                        opt.Name = optInDb.Name;
                        opt.Konfigurierbar = optInDb.Konfigurierbar;
                        opt.BereitsVorhanden = true;
                        auftrag.Positionen.Add(opt);

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
                                             KreditorID = r.Person_ID,
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
                                            KreditorID = r.Person_ID,
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
            //DateTime date = new DateTime();
            DateTime currentDate = DateTime.Now;
            IQueryable<PreisOption> list = from r in db.TBL_Option
                                           where r.Konfigurierbar == true
                                           select new PreisOption
                                           {
                                               ID = r.Option_ID,
                                               Name = r.Name,
                                               Preis = r.Einheitspreis,
                                               PreisKatalog = PreisKatalog.Option,
                                               StartDate = r.PreisStartDatum,
                                               EndDate = r.PreisEndDatum
                                           };

            IQueryable<PreisOption> list2 = from r in db.TBL_Dienstleistung
                                           select new PreisOption
                                           {
                                               ID = r.Dienstleistung_ID,
                                               Name = r.Name,
                                               Preis = r.Preis,
                                               PreisKatalog = PreisKatalog.Dienstleistung,
                                               StartDate = r.PreisStartDatum,
                                               EndDate = r.PreisEndDatum
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
            preisOptionsListe = preisOptionsListe.Where(i => (i.StartDate <= currentDate)&&(i.EndDate>=currentDate)).ToList();
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
        /// Gibt eine Liste zurück mit allen Optionen, welche konfigurierbar und aktuell aktiv sind.
        /// </summary>
        /// <returns></returns>
        public List<Option> LadeOptionen()
        {
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            IQueryable<Option> list = from r in db.TBL_Option
                                         where r.Konfigurierbar == true && r.PreisEndDatum.Date>=DateTime.Now.Date
                                         select new Option
                                         {
                                             ID = r.Option_ID,
                                             Anzahl = 1,
                                             BereitsVorhanden = false,
                                             Einheitspreis = r.Einheitspreis,
                                             Konfigurierbar = true,
                                             Name = r.Name,
                                             PreisInFranken = r.Einheitspreis,
                                             WurdeGeloescht = false
                                         };
            optionenListe = list.ToList();

            return optionenListe;
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
                    dlg.PreisEndDatum = DateTime.Now;
                    var newDlg = new TBL_Dienstleistung();
                    newDlg.Name = preisOption.Name;
                    newDlg.Preis = preisOption.Preis;
                    newDlg.PreisStartDatum = DateTime.Now.AddSeconds(1);
                    newDlg.PreisEndDatum = newDlg.PreisStartDatum.AddYears(100);
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
                    opt.PreisEndDatum = DateTime.Now;
                    var newOpt = new TBL_Option();
                    newOpt.Einheitspreis = preisOption.Preis;
                    newOpt.Konfigurierbar = true;
                    newOpt.Name = preisOption.Name;
                    newOpt.PreisStartDatum = DateTime.Now.AddSeconds(1);
                    newOpt.PreisEndDatum = newOpt.PreisStartDatum.AddYears(100);
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
            var firmaList = db.TBL_Kreditor.Where(i => i.Firma.ToLower() == kreditor.Firma.ToLower());
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

        /// <summary>
        /// Speichert eine bestehende oder erstellt eine neue Rechnung. Gibt die ID der Rechnung zurück, ausser bei einem Fehler wird -1 zurück gegeben.
        /// </summary>
        /// <param name="rechnung">Rechnungs Objekt</param>
        /// <returns>ID</returns>
        public int SpeicherenRechnung(Rechnung rechnung)
        {
            int returnValue = -1;
            int kat = -1;
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            try
            {
                kat = db.TBL_Kategorie.First(i => i.Name.ToLower() == rechnung.Kategorie.ToLower()).Kategorie_ID;
            }
            catch (Exception)
            {

                Logger.append("Fehler beim Neuerstellen, schreiben in die Datenbank nicht möglich", Logger.ERROR);
                return -1;
            }
            
            var bestRechnung = db.TBL_Rechnung.Find(rechnung.ID);
            if (bestRechnung == null)
            {
                

                var newRechnung = new TBL_Rechnung
                {
                    Erfassungsdatum = rechnung.Datum,
                    Beschreibung = rechnung.Beschreibung,
                    Kategorie_ID = kat,
                    Person_ID = rechnung.KreditorID,
                    Preis = rechnung.Betrag
                    
                };
                try
                {
                    db.TBL_Rechnung.Add(newRechnung);
                    db.SaveChanges();
                    db.Entry(newRechnung);
                    returnValue = newRechnung.Rechnung_ID;
                }
                catch (Exception)
                {

                    Logger.append("Fehler beim Neuerstellen, schreiben in die Datenbank nicht möglich", Logger.ERROR);
                    return -1;
                }
            }
            else
            {
                bestRechnung.Beschreibung = rechnung.Beschreibung;
                bestRechnung.Kategorie_ID = kat;
                bestRechnung.Person_ID = rechnung.KreditorID;
                bestRechnung.Preis = rechnung.Betrag;
                try
                {
                    db.SaveChanges();
                    db.Entry(bestRechnung);
                    returnValue = bestRechnung.Rechnung_ID;
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
        /// Speichert ein bestehende oder erstellt eine neue Rückerstattung gemäss dem Rechnungs Objekt. Gibt die ID des Datensatzes zurück, ausser bei einem Fehler wird -1 zurück gegeben.
        /// </summary>
        /// <param name="rechnung">Rechnungs objekt</param>
        /// <returns>ID</returns>
        public int SpeicherenRueckzahlung(Rechnung rechnung)
        {
            int returnValue = -1;
            int kat = -1;
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            try
            {
                kat = db.TBL_Kategorie.First(i => i.Name.ToLower() == rechnung.Kategorie.ToLower()).Kategorie_ID;
            }
            catch (Exception)
            {

                Logger.append("Fehler beim Neuerstellen, schreiben in die Datenbank nicht möglich", Logger.ERROR);
                return -1;
            }

            var bestRueckzahlung = db.TBL_Rueckerstattung.Find(rechnung.ID);
            if (bestRueckzahlung == null)
            {
                var newRueckzahlung = new TBL_Rueckerstattung
                {
                    Erfassungsdatum = rechnung.Datum,
                    Beschreibung = rechnung.Beschreibung,
                    Kategorie_ID = kat,
                    Person_ID = rechnung.KreditorID,
                    Total = rechnung.Betrag

                };
                try
                {
                    db.TBL_Rueckerstattung.Add(newRueckzahlung);
                    db.SaveChanges();
                    db.Entry(newRueckzahlung);
                    returnValue = newRueckzahlung.Rueckerstattung_ID;
                }
                catch (Exception)
                {

                    Logger.append("Fehler beim Neuerstellen, schreiben in die Datenbank nicht möglich", Logger.ERROR);
                    return -1;
                }
            }
            else
            {
                bestRueckzahlung.Beschreibung = rechnung.Beschreibung;
                bestRueckzahlung.Kategorie_ID = kat;
                bestRueckzahlung.Person_ID = rechnung.KreditorID;
                bestRueckzahlung.Total = rechnung.Betrag;
                try
                {
                    db.SaveChanges();
                    db.Entry(bestRueckzahlung);
                    returnValue = bestRueckzahlung.Rueckerstattung_ID;
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
        /// Speichert einen bestehenden oder erstellt einen neuen Auftrag inklusive der Positionen und gibt die ID des Auftrages zurück. Wenn es fehlschlägt, wird -1 zurückgegeben.
        /// </summary>
        /// <param name="auftrag">Auftrag Objekt</param>
        /// <returns>ID</returns>
        public int SpeicherenAuftrag(Auftrag auftrag)
        {
            List<Option> _tempOptions = new List<Option>();
            List<Option> _tempRemoveOptions = new List<Option>();
            int returnValue = -1;
            if (checkConnection() == false)
            {
                Logger.append(
                    "Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",
                    1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }

            #region OptionenSpeichern

            foreach (var option in auftrag.Positionen)
            {
                if (option.WurdeGeloescht == false)
                {
                    if (option.Konfigurierbar == false)
                    {
                        if (option.BereitsVorhanden == false)
                        {
                            var newOpt = new TBL_Option
                            {
                                Name = option.Name,
                                Einheitspreis = option.Einheitspreis,
                                Konfigurierbar = false,
                                PreisEndDatum = DateTime.Now.AddDays(1).Date,
                                PreisStartDatum = DateTime.Now.Date
                            };
                            try
                            {
                                db.TBL_Option.Add(newOpt);
                                db.SaveChanges();
                                db.Entry(newOpt);
                                _tempOptions.Add(new Option { Anzahl = option.Anzahl, PreisInFranken = option.PreisInFranken, ID = newOpt.Option_ID, BereitsVorhanden = false });
                            }
                            catch (Exception)
                            {

                                Logger.append("Fehler beim Neuerstellen, schreiben in die Datenbank nicht möglich", Logger.ERROR);
                                return -1;
                            }
                        }
                        else
                        {
                            //Bereits Vorhanden nur ändeern
                            var bestOpt = db.TBL_Option.Find(option.ID);
                            bestOpt.Name = option.Name;
                            bestOpt.Einheitspreis = option.Einheitspreis;
                            try
                            {
                                db.SaveChanges();
                                _tempOptions.Add(new Option { Anzahl = option.Anzahl, PreisInFranken = option.PreisInFranken, ID = bestOpt.Option_ID, BereitsVorhanden = true });
                            }
                            catch (Exception)
                            {

                                Logger.append("Fehler beim Neuerstellen, schreiben in die Datenbank nicht möglich", Logger.ERROR);
                                return -1;
                            }
                        }

                    }
                    else
                    {
                        _tempOptions.Add(new Option { Anzahl = option.Anzahl, PreisInFranken = option.PreisInFranken, ID = option.ID, BereitsVorhanden = option.BereitsVorhanden });
                    }
                }
                else
                {
                    //Wurden gelöscht
                    if (option.Konfigurierbar==false && option.BereitsVorhanden == true)
                    {
                        
                        try
                        {
                            //Kreutabellen Inhalte werden durch Cascading gelöscht
                            var deleteOpt = db.TBL_Option.Find(option.ID);
                            db.TBL_Option.Remove(deleteOpt);
                            db.SaveChanges();
                        }
                        catch (Exception)
                        {

                            Logger.append("Fehler beim Löschen einer Option", Logger.INFO);
                            
                        }
                    }
                    if (option.Konfigurierbar == true && option.BereitsVorhanden == true)
                    {
                        _tempRemoveOptions.Add(option);
                    }
                }
                
            }

            #endregion

            #region AuftragSpeichern

            if (auftrag.ID == -1)
            {
                var newAuftrag = new TBL_Auftrag
                {
                    Dienstleistung_ID = auftrag.Dienstleistung.ID,
                    KundenGespraech = auftrag.KundenGespraech,
                    Person_ID = auftrag.KundeID,
                    Rabatt = auftrag.Rabatt,
                    RabattInProzent = auftrag.RabattInProzent,
                    Total = auftrag.Total
                };
                try
                {
                    db.TBL_Auftrag.Add(newAuftrag);
                    db.SaveChanges();
                    db.Entry(newAuftrag);
                    returnValue = newAuftrag.Auftrag_ID;
                }
                catch (Exception)
                {

                    Logger.append("Fehler beim Neuerstellen, schreiben in die Datenbank nicht möglich", Logger.ERROR);
                    return -1;
                }
            }
            else
            {
                var bestAuftrag = db.TBL_Auftrag.Find(auftrag.ID);
                if (bestAuftrag==null)
                {
                    Logger.append("Auftrag konnte nicht gefunden werden", Logger.ERROR);
                    return -1;
                }
                bestAuftrag.Dienstleistung_ID = auftrag.Dienstleistung.ID;
                bestAuftrag.KundenGespraech = auftrag.KundenGespraech;
                bestAuftrag.Person_ID = auftrag.KundeID;
                bestAuftrag.Rabatt = auftrag.Rabatt;
                bestAuftrag.RabattInProzent = auftrag.RabattInProzent;
                bestAuftrag.Total = auftrag.Total;
                try
                {
                    db.SaveChanges();
                    db.Entry(bestAuftrag);
                    returnValue = bestAuftrag.Auftrag_ID;
                }
                catch (Exception)
                {

                    Logger.append("Fehler beim Neuerstellen, schreiben in die Datenbank nicht möglich", Logger.ERROR);
                    return -1;
                }
            }

            #endregion

            #region KReuztabelleOptAuftragSpeichern

            foreach (var tempOption in _tempOptions)
            {
                if (tempOption.BereitsVorhanden == false)
                {
                    var newOptAuftr = new TBL_Opt_Auftr
                    {
                        Auftrag_ID = returnValue,
                        Option_ID = tempOption.ID,
                        Anzahl = tempOption.Anzahl,
                        GesammtPreis = tempOption.PreisInFranken
                    };
                    try
                    {
                        db.TBL_Opt_Auftr.Add(newOptAuftr);
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {

                        Logger.append("Fehler beim Neuerstellen, schreiben in die Datenbank nicht möglich", Logger.ERROR);
                        return -1;
                    }
                }
                else
                {
                    var bestOptAuftr =
                        db.TBL_Opt_Auftr.First(i => (i.Auftrag_ID == returnValue) && (i.Option_ID == tempOption.ID));
                    bestOptAuftr.Anzahl = tempOption.Anzahl;
                    bestOptAuftr.GesammtPreis = tempOption.PreisInFranken;
                    try
                    {
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {

                        Logger.append("Fehler beim Neuerstellen, schreiben in die Datenbank nicht möglich", Logger.ERROR);
                        return -1;
                    }
                }
                
            }

            #endregion

            #region KreuztabelleOptAuftragLoeschen

            foreach (var removeOption in _tempRemoveOptions)
            {
                try
                {
                    var removeOptAuftr =
                    db.TBL_Opt_Auftr.First(i => (i.Auftrag_ID == returnValue) && (i.Option_ID == removeOption.ID));
                    db.TBL_Opt_Auftr.Remove(removeOptAuftr);
                    db.SaveChanges();
                }
                catch (Exception)
                {

                    Logger.append("Fehler beim Löschen einer Option", Logger.INFO);

                }
                

            }
            #endregion

            return returnValue;
        }
    }

}