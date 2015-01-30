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


        public Kunde()
        {
            this.KundeID = -1;
            this.Reminder = false;
        }

        public void GetDefault()
        {
            this.ID = -1;
            this.KundeID = -1;
            this.Reminder = false;
            this.Name = string.Empty;
            this.Vorname = string.Empty;
            this.Adresse = string.Empty;
            this.PLZ = 0;
            this.Wohnort = string.Empty;
            this.TelFirma = string.Empty;
            this.TelMobile = string.Empty;
            this.TelPrivat = string.Empty;
            this.Email = null;
            this.ErfDatum = DateTime.Now;
            this.Fax = string.Empty;
        }
    }
}
