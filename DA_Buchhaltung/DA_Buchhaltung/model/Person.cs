using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public string Adresse { get; set; }
        public int PLZ { get; set; }
        public string Wohnort { get; set; }
        public string TelPrivat { get; set; }
        public string TelFirma { get; set; }
        public string TelMobile { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        
        public DateTime ErfDatum { get; set; }

        public Person()
        {
            this.ID = -1;
            this.ErfDatum = DateTime.Now;
        }
    }
}
