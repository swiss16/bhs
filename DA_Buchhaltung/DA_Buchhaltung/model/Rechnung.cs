/*
 * Klasse: Rechnung.cs
 * Author: Martin Osterwalder
 * Objektklasse um mit "Rechnungen" oder "Rückzahlungen" zu arbeiten.
 * Der DBWrapper wrappt diese Klasse an die entsprechende Entity des Entity Models.
 * Da Rechnungen und Rückzahlungen so gut wie identisch sind, wurde dafür weder eine Vererbung noch eine seperate Klasse erstellt.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA_Buchhaltung.viewModel;

namespace DA_Buchhaltung.model
{
    /// <summary>
    /// Diese Klasse stellt Rechnungsspezifische Attribute und Methoden (DB unabhängig) bereit.
    /// </summary>
    public class Rechnung : ViewModelBase
    {
        public int ID { get; set; }
        public int KreditorID { get; set; }
        [Required(ErrorMessage = "Dieses Feld darf nicht leer sein")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Das Format muss zB: 5.20 sein und nicht negativ")]
        [Range(0, 10000, ErrorMessage = "Preis muss zwischen 0 und 10000 sein")]
        public decimal Betrag
        {
            get { return GetValue(() => Betrag); }
            set { SetValue(() => Betrag, value); }
        }
        [Required(ErrorMessage = "Dieses Feld darf nicht leer sein")]
        [MaxLength(500, ErrorMessage = "Maximal 500 Stellen erlaubt")]
        [MinLength(3, ErrorMessage = "Es muss mindestens 3 Stellen haben")]
        public string Beschreibung
        {
            get { return GetValue(() => Beschreibung); }
            set { SetValue(() => Beschreibung, value); }
        }
        [Required(ErrorMessage = "Dieses Feld darf nicht leer sein")]
        [MaxLength(50, ErrorMessage = "Maximal 50 Stellen erlaubt")]
        [MinLength(3, ErrorMessage = "Es muss mindestens 3 Stellen haben")]
        public string Kategorie
        {
            get { return GetValue(() => Kategorie); }
            set { SetValue(() => Kategorie, value); }
        }
        [Required(ErrorMessage = "Dieses Feld darf nicht leer sein")]
        public DateTime Datum
        {
            get { return GetValue(() => Datum); }
            set { SetValue(() => Datum, value); }
        }
        public bool AlsRueckzahlung { get; set; }

        public Rechnung()
        {
            this.ID = -1;
            this.AlsRueckzahlung = false;
            this.Datum = DateTime.Now;
            this.KreditorID = -1;
            this.Betrag = 0.0m;
            this.Beschreibung = string.Empty;
            this.Kategorie = string.Empty;

        }
    }
}
