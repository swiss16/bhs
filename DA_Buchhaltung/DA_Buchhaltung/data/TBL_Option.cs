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
    
    public partial class TBL_Option
    {
        public TBL_Option()
        {
            this.TBL_Opt_Auftr = new HashSet<TBL_Opt_Auftr>();
        }
    
        public int Option_ID { get; set; }
        public string Name { get; set; }
        public decimal Einheitspreis { get; set; }
        public System.DateTime PreisStartDatum { get; set; }
        public System.DateTime PreisEndDatum { get; set; }
        public bool Konfigurierbar { get; set; }
    
        public virtual ICollection<TBL_Opt_Auftr> TBL_Opt_Auftr { get; set; }
    }
}
