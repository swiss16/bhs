/*
 * Klasse: Betraege.cs
 * Author: Martin Osterwalder
 * Objektklasse um mit "Betraege" zu arbeiten.
 * Dies ist eine Hilfsklasse um die benötigten Daten für die Erfolgsrechnung zu ergänzen.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public class Betraege
    {
        public string Kategorie { get; set; }
        public decimal BetragInFranken { get; set; }


        public Betraege()
        {
            this.Kategorie = string.Empty;
            this.BetragInFranken = 0.0m;
        }
    }
}
