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
                    ID = k.Kunde_ID,
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
                                           where r.Konfigurierbar == true && r.PreisEndDatum < DateTime.Now
                                           select new PreisOption
                                           {
                                               ID = r.Option_ID,
                                               Name = r.Name,
                                               Preis = r.Einheitspreis,
                                               PreisKatalog = PreisKatalog.Option
                                           };

            IQueryable<PreisOption> list2 = from r in db.TBL_Dienstleistung
                                           where r.PreisEndDatum < DateTime.Now
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


    }

}