using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA_Buchhaltung.common.log;

namespace DA_Buchhaltung.model
{
    public class Erfolgsrechnung
    {
        public int ID { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EndDatum { get; set; }
        public List<Betraege> Einnahmen { get; set; }
        public List<Betraege> Ausgaben { get; set; }
        public decimal SubtotalEinnahmen { get; set; }
        public decimal SubtotalAusgaben { get; set; }
        public decimal Gewinn { get; set; }
        public bool IstJahresabrechnung { get; set; }

        public Erfolgsrechnung()
        {
            this.ID = -1;
            this.StartDatum = DateTime.Now;
            this.EndDatum = DateTime.Now;
            this.Einnahmen = new List<Betraege>();
            this.Ausgaben = new List<Betraege>();
            this.SubtotalEinnahmen = 0.00m;
            this.SubtotalAusgaben = 0.00m;
            this.Gewinn = 0.00m;
            this.IstJahresabrechnung = false;
        }

        /// <summary>
        /// Erstellt eine .csv oder .html Datei, gemäss diesem Erfolgsrechnung-Objekt. Gibt True zurück, wenn es erfolgreich war.
        /// </summary>
        /// <returns></returns>
        public bool Print()
        {
            Update();
            //todo: Hier die ausgabe implementieren (erfolgsrechnung als csv oder html) (Jenachdem die Summary umbeschreiben)
            return true;
        }

        /// <summary>
        /// Berechnet gemäss den Ein- und Ausgabenlisten, die jeweiligen Subtotals und den Gewinn(negativ Verlust).
        /// </summary>
        public void Update()
        {
            if (Einnahmen == null)
            {
                Einnahmen = new List<Betraege>();
            }
            if (Ausgaben == null)
            {
                Ausgaben = new List<Betraege>();
            }
            this.SubtotalEinnahmen = berechneSubtotal(Einnahmen);
            this.SubtotalAusgaben = berechneSubtotal(Ausgaben);
            this.Gewinn = berechneGewinn(SubtotalEinnahmen, SubtotalAusgaben);
        }

        private decimal berechneSubtotal(List<Betraege> betraege )
        {
            decimal _tempPreis = 0.0m;
            foreach (var betr in betraege)
            {
                _tempPreis += betr.BetragInFranken;
            }
            return _tempPreis;
        }

        private decimal berechneGewinn(decimal subtotalEinnahmen, decimal subtotalAusgaben)
        {

            return subtotalEinnahmen - subtotalAusgaben;
        }

    }
}
