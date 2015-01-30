﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public class Position
    {
        public string Beschreibung { get; set; }
        public decimal Anzahl { get; set; }
        public decimal Einheitspreis { get; set; }
        public decimal Preis { get; set; }
        public bool IstDienstleistung { get; set; }
        public bool IstOption { get; set; }
        public bool IstRabatt { get; set; }

        public Position()
        {
            Beschreibung = string.Empty;
            Anzahl = 0.0m;
            Einheitspreis = 0.0m;
            Preis = 0.0m;
        }
    }
}