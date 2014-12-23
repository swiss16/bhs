using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public class Betraege
    {
        public int ID { get; set; }
        public string Kategorie { get; set; }
        public decimal BetragInFranken { get; set; }


        public Betraege()
        {
            this.ID = -1;
        }
    }
}
