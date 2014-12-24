using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public class Kreditor : Person
    {
        public int KreditorID { get; set; }
        public string Firma { get; set; }


        public Kreditor()
        {
            this.KreditorID = -1;
            this.Firma = string.Empty;
        }
    }
}
