﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class bhs_DBEntities : DbContext
    {
        public bhs_DBEntities()
            : base("name=bhs_DBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TBL_Auftrag> TBL_Auftrag { get; set; }
        public virtual DbSet<TBL_Bild> TBL_Bild { get; set; }
        public virtual DbSet<TBL_Dienstleistung> TBL_Dienstleistung { get; set; }
        public virtual DbSet<TBL_Gutschein> TBL_Gutschein { get; set; }
        public virtual DbSet<TBL_Kategorie> TBL_Kategorie { get; set; }
        public virtual DbSet<TBL_Kreditor> TBL_Kreditor { get; set; }
        public virtual DbSet<TBL_Kunde> TBL_Kunde { get; set; }
        public virtual DbSet<TBL_Opt_Auftr> TBL_Opt_Auftr { get; set; }
        public virtual DbSet<TBL_Option> TBL_Option { get; set; }
        public virtual DbSet<TBL_Ort> TBL_Ort { get; set; }
        public virtual DbSet<TBL_Rechnung> TBL_Rechnung { get; set; }
        public virtual DbSet<TBL_Rueckerstattung> TBL_Rueckerstattung { get; set; }
        public virtual DbSet<TBL_Termin> TBL_Termin { get; set; }
    }
}
