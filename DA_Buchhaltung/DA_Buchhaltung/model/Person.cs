using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public class Person
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50,MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Vorname { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Adresse { get; set; }
        [Required]
        [Range(1000,9999)]
        public int PLZ { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
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
            this.Name = string.Empty;
            this.Vorname = string.Empty;
            this.Adresse = string.Empty;
            this.Wohnort = string.Empty;
            this.Email = string.Empty;
            this.TelFirma = string.Empty;
            this.TelMobile = string.Empty;
            this.TelPrivat = string.Empty;
            this.Fax = string.Empty;
            this.PLZ = 0;
            
        }
    }
}
