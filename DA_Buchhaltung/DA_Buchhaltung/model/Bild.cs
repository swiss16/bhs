/*
 * Klasse: Bild.cs
 * Author: Martin Osterwalder
 * Objektklasse um mit "Bilder" zu arbeiten.
 * Dies gehört zu den Kannzielen um Bilder zu speichern. Wird derzeit noch nicht benötigt.
 * Der DBWrapper wrappt diese Klasse an die entsprechende Entity des Entity Models.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA_Buchhaltung.model
{
    /// <summary>
    /// Diese Klasse stellt Bilderspezifische Attribute und Methoden (DB unabhängig) bereit.
    /// </summary>
    public class Bild
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Pfad { get; set; }

        public Bild()
        {
            this.ID = -1;
            this.Name = string.Empty;
            this.Pfad = string.Empty;
        }
    }
}
