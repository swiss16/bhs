using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public class Auftrag
    {
        public int ID { get; set; }
        public List<Option> Positionen { get; set; }
        public Dienstleistung Dienstleistung { get; set; }
        public int KundeID { get; set; }
        public string KundenGespraech { get; set; }
        public List<Bild> Bilder { get; set; }
        public decimal Total { get; set; }
        public DateTime Datum { get; set; }

        public Auftrag()
        {
            this.ID = -1;
            this.Datum = DateTime.Now;
        }
    }
}
