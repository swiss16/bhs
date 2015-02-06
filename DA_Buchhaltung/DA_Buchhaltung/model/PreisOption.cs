/*
 * Klasse: PreisOption.cs
 * Author: Martin Osterwalder
 * Objektklasse um mit "preisOptionen" zu arbeiten.
 * Der DBWrapper wrappt diese Klasse an die entsprechende Entity des Entity Models.
 * Diese Klasse ermöglicht es Einstellungen der Preise vorzunehmen.
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
    public enum PreisKatalog
    {
        Dienstleistung,
        Option
    }
    public class PreisOption : ViewModelBase
    {
        public int ID { get; set; }
        public PreisKatalog PreisKatalog { get; set; }
        [Required(ErrorMessage = "Dieses Feld darf nicht leer sein")]
        [MaxLength(50, ErrorMessage = "Maximal 50 Stellen erlaubt")]
        [MinLength(3, ErrorMessage = "Es muss mindestens 3 Stellen haben")]
        public string Name
        {
            get { return GetValue(() => Name); }
            set { SetValue(() => Name, value); }
        }
        [Required(ErrorMessage = "Dieses Feld darf nicht leer sein")]
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Das Format muss zB: 5.20 sein und nicht negativ")]
        [Range(0, 10000, ErrorMessage = "Preis muss zwischen 0 und 10000 sein")]
        public decimal Preis
        {
            get { return GetValue(() => Preis); }
            set { SetValue(() => Preis, value); }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }


        public PreisOption()
        {
            this.ID = -1;
            this.Name = "Kein Element";
            this.Preis = 0.0m;
        }
    }
}
