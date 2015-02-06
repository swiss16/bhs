/*
 * Klasse: Person.cs
 * Author: Martin Osterwalder
 * Basisklasse um mit Personen zu Arbeiten.
 * Diese Klasse wird selbst nicht direkt verwendet, vererbt jedoch seine Properties.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using DA_Buchhaltung.viewModel;

namespace DA_Buchhaltung.model
{
    /// <summary>
    /// Diese Klasse stellt Grundattribute für Personenbezogene Objekte wie Kunden und Kreditoren dar.
    /// Dies ist die Basisklasse der Kunden und Kreditoren
    /// </summary>
    public class Person : ViewModelBase
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Dieses Feld darf nicht leer sein")]
        [MaxLength(50, ErrorMessage = "Maximal 50 Stellen erlaubt")]
        [MinLength(3, ErrorMessage = "Es muss mindestens 3 Stellen haben")]
        public string Name
        {
            get { return GetValue(() => Name); }
            set { SetValue(() => Name, value); }
        }
        [Required(ErrorMessage = "Dieses Feld darf nicht leer sein")]
        [MaxLength(50, ErrorMessage = "Maximal 50 Stellen erlaubt")]
        [MinLength(3, ErrorMessage = "Es muss mindestens 3 Stellen haben")]
        public string Vorname
        {
            get { return GetValue(() => Vorname); }
            set { SetValue(() => Vorname, value); }
        }
        [Required(ErrorMessage = "Dieses Feld darf nicht leer sein")]
        [MaxLength(50, ErrorMessage = "Maximal 50 Stellen erlaubt")]
        [MinLength(3, ErrorMessage = "Es muss mindestens 3 Stellen haben")]
        public string Adresse
        {
            get { return GetValue(() => Adresse); }
            set { SetValue(() => Adresse, value); }
        }
        [Required(ErrorMessage = "Dieses Feld darf nicht leer sein")]
        [Range(1000, 9999, ErrorMessage = "PLZ muss zwischen 1000 und 9999 sein")]
        public int PLZ
        {
            get { return GetValue(() => PLZ); }
            set { SetValue(() => PLZ, value); }
        }
        [Required(ErrorMessage = "Dieses Feld darf nicht leer sein")]
        [MaxLength(100, ErrorMessage = "Maximal 100 Stellen erlaubt")]
        [MinLength(3, ErrorMessage = "Es muss mindestens 3 Stellen haben")]
        public string Wohnort
        {
            get { return GetValue(() => Wohnort); }
            set { SetValue(() => Wohnort, value); }
        }
        [MaxLength(50, ErrorMessage = "Maximal 50 Stellen erlaubt")]
        public string TelPrivat
        {
            get { return GetValue(() => TelPrivat); }
            set { SetValue(() => TelPrivat, value); }
        }
        [MaxLength(50, ErrorMessage = "Maximal 50 Stellen erlaubt")]
        public string TelFirma
        {
            get { return GetValue(() => TelFirma); }
            set { SetValue(() => TelFirma, value); }
        }
        [MaxLength(50, ErrorMessage = "Maximal 50 Stellen erlaubt")]
        public string TelMobile
        {
            get { return GetValue(() => TelMobile); }
            set { SetValue(() => TelMobile, value); }
        }
        [MaxLength(50, ErrorMessage = "Maximal 50 Stellen erlaubt")]
        public string Fax
        {
            get { return GetValue(() => Fax); }
            set { SetValue(() => Fax, value); }
        }
        [EmailAddress]
        [MaxLength(100, ErrorMessage = "Maximal 100 Stellen erlaubt")]
        public string Email
        {
            get { return GetValue(() => Email); }
            set { SetValue(() => Email, value == string.Empty ? null : value); }
        }
        public DateTime ErfDatum { get; set; }

        public Person()
        {
            this.ID = -1;
            this.ErfDatum = DateTime.Now;
            this.Name = string.Empty;
            this.Vorname = string.Empty;
            this.Adresse = string.Empty;
            this.Wohnort = string.Empty;
            this.Email = null;
            this.TelFirma = string.Empty;
            this.TelMobile = string.Empty;
            this.TelPrivat = string.Empty;
            this.Fax = string.Empty;
            this.PLZ = 0;
            
        }
    }
}
