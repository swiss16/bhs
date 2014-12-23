using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public class Bild
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Pfad { get; set; }

        public Bild()
        {
            this.ID = -1;
        }
    }
}
