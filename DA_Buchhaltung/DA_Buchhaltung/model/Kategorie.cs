using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public class Kategorie
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public Kategorie()
        {
            this.ID = -1;
        }
    }
}
