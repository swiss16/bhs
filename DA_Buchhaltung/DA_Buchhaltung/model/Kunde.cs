using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public class Kunde:Person
    {
        public int KundeID { get; set; }
        public bool Reminder { get; set; }
    }
}
