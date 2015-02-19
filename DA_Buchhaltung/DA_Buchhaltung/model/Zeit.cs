using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public class Zeit
    {
        public decimal AbSechsUhr { get; set; }
        public decimal AbElfUhr { get; set; }
        public decimal AbSechzehnUhr { get; set; }
        public Kunde AbSechsUhrKunde { get; set; }
        public Kunde AbElfUhrKunde { get; set; }
        public Kunde AbSechzehnUhrKunde { get; set; }
        public bool SechsUhrKundeAktiv { get; set; }
        public bool ElfUhrKundeAktiv { get; set; }
        public bool SechzehnUhrKundeAktiv { get; set; }

        public Zeit()
        {
            this.AbSechsUhr = 0.00m;
            this.AbElfUhr = 0.00m;
            this.AbSechzehnUhr = 0.00m;
            this.AbSechsUhrKunde = new Kunde();
            this.AbElfUhrKunde = new Kunde();
            this.AbSechzehnUhrKunde = new Kunde();
        }
    }
}
