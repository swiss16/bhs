using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public class Termin
    {
        public int Id { get; set; }
        public DateTime StartZeit { get; set; }
        public DateTime EndZeit { get; set; }
        public Kunde Kunde { get; set; }
        public int KundeId { get; set; }


        public Termin()
        {
            this.Id = -1;
            this.StartZeit = DateTime.Now;
            this.EndZeit = DateTime.Now.AddMinutes(15);
            this.Kunde = new Kunde();
        }
    }
}
