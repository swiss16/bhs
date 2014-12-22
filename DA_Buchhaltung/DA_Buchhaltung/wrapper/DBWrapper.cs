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
        private bool connectstate = false;
        private List<Kunde> kundenliste = new List<Kunde>();
        private bhs_DBEntities db = new bhs_DBEntities();


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
                connectstate = false;
            }
            
            return connectstate;
        }


        //Public Methoden
        public List<Kunde> LadeKunden()
        {
            if (checkConnection() == false)
            {
                Logger.append("Error: Fehler beim Verbindungsaufbau zur Datenbank. Überprüfen sie die Internetverbindung oder die Konfiguration!",1);
                throw new Exception("Fehler beim Datenbankzugriff. Weitere Informationen stehen im Logfile.");
            }
            else
            {
                
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
                }
            
            return kundenliste;
        }
    }
}