using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public enum PreisKatalog
    {
        Dienstleistung,
        Option
    }
    public class PreisOption
    {
        public int ID { get; set; }
        public PreisKatalog PreisKatalog { get; set; }
        public string Name { get; set; }
        public decimal Preis { get; set; }


        public PreisOption()
        {
            this.ID = -1;
            this.Name = string.Empty;
            this.Preis = 0.0m;
        }
    }
}
