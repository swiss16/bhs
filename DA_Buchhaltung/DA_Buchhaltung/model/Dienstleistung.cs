﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    public class Dienstleistung
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Preis { get; set; }

        public Dienstleistung()
        {
            this.ID = -1;
            this.Name = string.Empty;
            this.Preis = 0.0m;
        }
    }
}