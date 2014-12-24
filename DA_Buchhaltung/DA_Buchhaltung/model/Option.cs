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
        public bool IstRabatt {get;set;}

        public Option()
        {
            this.ID = -1;
            this.IstRabatt = false;
            this.Name = string.Empty;
            this.Einheitspreis = 0.0m;
            this.PreisInFranken = 0.0m;
            this.Anzahl = 1;
        }

    }
}
