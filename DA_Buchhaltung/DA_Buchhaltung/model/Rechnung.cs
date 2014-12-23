using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public class Rechnung
    {
        public int ID { get; set; }
        public int KreditorID { get; set; }
        public decimal Betrag { get; set; }
        public string Beschreibung { get; set; }
        public string Kategorie { get; set; }
        public DateTime Datum { get; set; }
        public bool AlsRueckzahlung { get; set; }

        public Rechnung()
        {
            this.ID = -1;
            this.AlsRueckzahlung = false;
            this.Datum = DateTime.Now;

        }
    }
}
