using System;
using System.Collections.Generic;
using System.Linq;
using DA_Buchhaltung.data;
using DA_Buchhaltung.model;

namespace DA_Buchhaltung.wrapper
{
    public class DBWrapper
    {
        //Private Properties
        private bool connectstate;
        private List<Kunde> kundenliste = new List<Kunde>();


        //Private Methoden
        /// <summary>
        ///     Prüft die Verbindung zur Datenbank. Nach dem ersten Fehlversuch wird die Verbindung neu konfiguriert und nochmals
        ///     geprüft.
        ///     Ist es immernoch Fehlerhaft wird der connectstate auf False gesetzt.
        ///     Konnte die Verbindung nicht neu konfiguriert werden, ist der Rückgabewert ebenfalls auf False gesetzt.
        /// </summary>
        /// <returns>bool false, Fehler bei Konfiguration, ansonsten true</returns>
        private bool checkConnection()
        {
            //Versuch 1
            try
            {
                using (var testContext = new bhs_DBEntities())
                {
                    testContext.SaveChanges();
                }
                connectstate = true;
            }
            catch (Exception)
            {
                connectstate = false;
            }
            //Neu konfiguration und Versuch 2
            if (connectstate == false)
            {
                if (configConnection())
                {
                    try
                    {
                        using (var testContext = new bhs_DBEntities())
                        {
                            testContext.SaveChanges();
                        }
                        connectstate = true;
                    }
                    catch (Exception)
                    {
                        connectstate = false;
                    }
                }
                else
                {
                    return false; //Konfiguration fehlgeschlagen
                }
            }

            return true;
        }

        /// <summary>
        ///     Konfiguriert die Verbindung zur Datenbank. Ist die Konfiguration erfolgreich wird "True" zurückgegeben. Ansonsten
        ///     False.
        /// </summary>
        /// <returns>bool True, wenn Konfiguration erfolgreich war</returns>
        private bool configConnection()
        {
            //TODO: Konfiguration implementieren
            return true;
        }


        //Public Methoden
        public List<Kunde> LadeKunden()
        {
            if (checkConnection() == false)
            {
                throw new Exception("Fehler bei der Konfiguration");
            }
            if (connectstate)
            {
                using (var dbcontext = new bhs_DBEntities())
                {
                    IQueryable<Kunde> klist = from k in dbcontext.TBL_Kunde
                        join ort in dbcontext.TBL_Ort on k.PLZ equals ort.PLZ
                        where k.K_Geloescht == false
                        select new Kunde
                        {
                            ID = k.Kunde_ID,
                            Name = k.K_Name,
                            Vorname = k.K_Vorname,
                            Adresse = k.K_Adresse,
                            PLZ = k.PLZ,
                            Wohnort = ort.Ortschaft,
                            TelMobile = k.K_TelMobile,
                            TelPrivat = k.K_TelPrivat,
                            Email = k.K_Email,
                            ErfDatum = k.K_Erfassungsdatum,
                            Reminder = k.K_Erinnerung
                        };
                    kundenliste = klist.ToList();
                }
            }
            return kundenliste;
        }
    }
}