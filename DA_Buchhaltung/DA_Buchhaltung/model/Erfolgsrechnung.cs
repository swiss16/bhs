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

        public bool Print()
        {
            Update();
            //todo: Hier die ausgabe implementieren (erfolgsrechnung als csv oder html)
            return true;
        }

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
