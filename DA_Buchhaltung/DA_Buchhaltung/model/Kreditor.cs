using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public class Kreditor : Person
    {
        public int KreditorID { get; set; }
        [Required(ErrorMessage = "Dieses Feld darf nicht leer sein")]
        [MaxLength(50, ErrorMessage = "Maximal 50 Stellen erlaubt")]
        [MinLength(3, ErrorMessage = "Es muss mindestens 3 Stellen haben")]
        public string Firma
        {
            get { return GetValue(() => Firma); }
            set { SetValue(() => Firma, value); }
        }


        public Kreditor()
        {
            this.KreditorID = -1;
            this.Firma = string.Empty;
        }
    }
}
