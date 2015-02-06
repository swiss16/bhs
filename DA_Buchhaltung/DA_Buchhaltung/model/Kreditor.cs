/*
 * Klasse: Kreditor.cs
 * Author: Martin Osterwalder
 * Objektklasse um mit "Kreditoren" zu arbeiten. Leitet von Personen ab und erbt Personenbezogene Properties.
 * Der DBWrapper wrappt diese Klasse an die entsprechende Entity des Entity Models.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    /// <summary>
    /// Diese Klasse stellt Kreditorenspezifische Attribute und Methoden (DB unabhängig) bereit.
    /// Abgeleitet von der Basisklasse Person
    /// </summary>
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

        public void GetDefault()
        {
            this.ID = -1;
            this.KreditorID = -1;
            this.Firma = string.Empty;
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
