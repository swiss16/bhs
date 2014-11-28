using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public class Kunde
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public string Adresse { get; set; }
        public int PLZ { get; set; }
        public string Wohnort { get; set; }
        public string TelPrivat { get; set; }
        public string TelMobile { get; set; }
        public string Email { get; set; }
        public bool Reminder { get; set; }
        public DateTime ErfDatum { get; set; }
    }
}
