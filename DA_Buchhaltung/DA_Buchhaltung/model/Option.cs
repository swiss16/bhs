/*
 * Klasse: Option.cs
 * Author: Martin Osterwalder
 * Objektklasse um mit "Optionen" zu arbeiten.
 * Der DBWrapper wrappt diese Klasse an die entsprechende Entity des Entity Models.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public class Option
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Einheitspreis { get; set; }
        public decimal PreisInFranken { get; set; }
        public decimal Anzahl { get; set; }
        public bool Konfigurierbar { get; set; }
        public bool BereitsVorhanden { get; set; }
        public bool WurdeGeloescht { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Option()
        {
            this.ID = -1;
            this.Name = string.Empty;
            this.Einheitspreis = 0.0m;
            this.PreisInFranken = 0.0m;
            this.Anzahl = 1;
            this.Konfigurierbar = false;
            this.BereitsVorhanden = false;
            this.WurdeGeloescht = false;
            this.StartDate = DateTime.Now;
            this.EndDate = DateTime.Now.AddSeconds(1);
        }

    }
}
