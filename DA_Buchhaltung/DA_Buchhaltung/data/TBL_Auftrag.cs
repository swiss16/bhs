//------------------------------------------------------------------------------
// <auto-generated>
//     Der Code wurde von einer Vorlage generiert.
//
//     Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten der Anwendung.
//     Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DA_Buchhaltung.data
{
    using System;
    using System.Collections.Generic;
    
    public partial class TBL_Auftrag
    {
        public TBL_Auftrag()
        {
            this.TBL_Bild = new HashSet<TBL_Bild>();
            this.TBL_Gutschein = new HashSet<TBL_Gutschein>();
            this.TBL_Opt_Auftr = new HashSet<TBL_Opt_Auftr>();
        }
    
        public int Auftrag_ID { get; set; }
        public System.DateTime ErfassungsDatum { get; set; }
        public decimal Total { get; set; }
        public bool RabattInProzent { get; set; }
        public Nullable<decimal> Rabatt { get; set; }
        public string KundenGespraech { get; set; }
        public int Dienstleistung_ID { get; set; }
        public int Kunde_ID { get; set; }
    
        public virtual ICollection<TBL_Bild> TBL_Bild { get; set; }
        public virtual ICollection<TBL_Gutschein> TBL_Gutschein { get; set; }
        public virtual ICollection<TBL_Opt_Auftr> TBL_Opt_Auftr { get; set; }
        public virtual TBL_Dienstleistung TBL_Dienstleistung { get; set; }
        public virtual TBL_Kunde TBL_Kunde { get; set; }
    }
}
