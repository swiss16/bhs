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
    
    public partial class TBL_Gutschein
    {
        public int Gutschein_ID { get; set; }
        public decimal Gs_Rabatt { get; set; }
        public string Gs_GutscheinCode { get; set; }
        public System.DateTime Gs_AusstellungsDatum { get; set; }
        public int Gs_GueltigInTage { get; set; }
        public int Auftrag_ID { get; set; }
    
        public virtual TBL_Auftrag TBL_Auftrag { get; set; }
    }
}
