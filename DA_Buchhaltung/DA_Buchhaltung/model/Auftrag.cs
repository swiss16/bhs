/*
 * Klasse: Auftrag.cs
 * Author: Martin Osterwalder
 * Objektklasse um mit "Aufträge" zu arbeiten.
 * Zusätzliche Print() Methode um Aufträge zu drucken.
 * Der DBWrapper wrappt diese Klasse an die entsprechende Entity des Entity Models.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA_Buchhaltung.common.config;
using DA_Buchhaltung.common.log;
using DA_Buchhaltung.viewModel;

namespace DA_Buchhaltung.model
{
    /// <summary>
    /// Diese Klasse stellt Auftragsspezifische Attribute und Methoden (DB unabhängig) bereit.
    /// </summary>
    public class Auftrag : ViewModelBase
    {
        public int ID { get; set; }
        public List<Option> Positionen { get; set; }
        public Dienstleistung Dienstleistung { get; set; }
        public int KundeID { get; set; }
        public Kunde Kunde { get; set; }
        public string KundenGespraech { get; set; }
        public List<Bild> Bilder { get; set; }
        public decimal Total { get; set; }
        public DateTime Datum { get; set; }
        public bool RabattInProzent { get; set; }
        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Das Format muss zB: 5.20 sein und nicht negativ")]
        [Range(0, 500, ErrorMessage = "Wert muss zwischen 0 und 500 sein")]
        public decimal Rabatt
        {
            get { return GetValue(() => Rabatt); }
            set { SetValue(() => Rabatt, value); }
        }

        private string _firma;
        private string _nameVorname;
        private string _adresse;
        private string _plzUndOrt;

        public Auftrag()
        {
            this.ID = -1;
            this.Datum = DateTime.Now;
            this.KundenGespraech = string.Empty;
            this.Positionen = new List<Option>();
            this.KundeID = -1;
            this.Bilder = new List<Bild>();
            this.Total = 0.0m;
            this.Rabatt = 0.0m;
            this.RabattInProzent = true;
            this.Dienstleistung = new Dienstleistung();

            _firma = ConfigWrapper.Firma;
            _nameVorname = ConfigWrapper.NameVorname;
            _adresse = ConfigWrapper.StrasseUndNummer;
            _plzUndOrt = ConfigWrapper.PlzUndOrt;
        }

        /// <summary>
        /// Stellt eine druckbare Htmlseite, gemäss dem Auftragobjekt zusammen. Gibt true zurück, wenn es erfolgreich war.
        /// </summary>
        /// <returns></returns>
        public bool Print(List<Position> positionList )
        {
            string html = getHtmlCode(positionList);
            string filename = string.Format("Rechnung_{0}.html", DateTime.Now.ToFileTime());
            string dir = Path.GetTempPath();

            //writer
            try
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                    DirectoryInfo info = new DirectoryInfo(dir);
                    info.Attributes = FileAttributes.Normal;
                }
                if (File.Exists(@"common\layout\logo.jpg"))
                {
                    File.Copy(@"common\layout\logo.jpg", Path.Combine(dir, "logo.jpg"), true);
                }
                if (File.Exists(@"common\layout\layout_rechnung.css"))
                {
                    File.Copy(@"common\layout\layout_rechnung.css", Path.Combine(dir, "layout_rechnung.css"), true);
                }

                using (FileStream fs = new FileStream(Path.Combine(dir, filename), FileMode.Create))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.Write(html);
                        sw.Flush();
                        fs.Flush();
                    }
                }
                Process.Start(Path.Combine(dir, filename));
            }
            catch (Exception e)
            {
                Logger.append("Fehler beim Schreiben: Original: " + e.ToString(), Logger.ERROR);
                throw new Exception("Fehler beim Schreiben! Originalmeldung steht im Logfile");
            }
            return true;
        }

        private string getHtmlCode(List<Position> positionList)
        {
            string nurOrt = string.Empty;
            if (_plzUndOrt.Length >5)
            {
                nurOrt = _plzUndOrt.Substring(4);
            }
            else
            {
                nurOrt = string.Empty;
            }
            
            //A4 = 210x297 (170 x237) Je Seite 20mm Abstand
            //Beginn header
            string html = (@"
<!DOCTYPE html>
<html>

<head>
<meta content='text/html; charset=utf-8' http-equiv='Content-Type'>
<meta name='viewport' content='width=device-width, initial-scale=1.0'/>
<meta name=“description” content='Rechnung, generiert durch die Software Nailartist Financial Manager'>
<meta name='author' content='Martin Osterwalder'>
<title>Nailartist Financial Manager - Rechnung</title>
<link rel='stylesheet' href='layout_rechnung.css' type='text/css' media='screen' />
<link rel='stylesheet' href='layout_rechnung.css' type='text/css' media='print' />
</head><body><div ID='a4'>
<!--Inhalt Beginnt -->

<header>
<img id='logo' src='logo.jpg' alt='Logo'>
<div id='title'>
<h1 class='mitte'>Quittung</h1></div>
</header>

<div id='kunde'>
<table class='tablehidden'>
");
            html = string.Concat(html, string.Format(@"<tr><th class='left'>Rechnungs Nr:</th><td>R-{12}</td></tr>
<tr><th class='left'></th><td></td></tr>
<tr><th class='left'>Kunden Nr:</th><td>K-{0}</td></tr>
<tr><th class='left'>Name Vorname:</th><td>{1} {2}</td></tr>
<tr><th class='left'>Strasse und Hausnr:</th><td>{3}</td></tr>
<tr><th class='left'>PLZ und Ort:</th><td>{4}, {5}</td></tr>
</table>
</div>
<div id='firma'>
<table class='tablehidden'>
<tr><th class='left'>Firma:</th><td>{6}</td></tr>
<tr><th class='left'>Name Vorname:</th><td>{7}</td></tr>
<tr><th class='left'>Strasse und Hausnr:</th><td>{8}</td></tr>
<tr><th class='left'>PLZ und Ort:</th><td>{9}</td></tr>
<tr><th class='left'></th><td></td></tr>
<tr><th class='left'>Datum, Ort:</th><td>{10}, {11}</td></tr>
</table>
</div>
<div id='content'>
<div class='ausgaben'>
<h2>Abrechnung</h2>
<table class='table'>
<tr><th>Produkt</th><th>Anzahl</th><th>Einzelpreis</th><th>Total</th></tr>
", Kunde.KundeID, Kunde.Name, Kunde.Vorname,Kunde.Adresse, Kunde.PLZ, Kunde.Wohnort,_firma,_nameVorname,_adresse,_plzUndOrt, DateTime.Now.ToShortDateString(), nurOrt, ID));

            foreach (var pos in positionList)
            {
                html = string.Concat(html, string.Format(@"<tr><td>{0}</td><td>{1}</td><td>Fr. {2} .-</td><td>Fr. {3} .-</td></tr>", pos.Beschreibung, pos.Anzahl, pos.Einheitspreis, pos.Preis));
            }

            html = string.Concat(html, string.Format(@"<tr><td></td><td></td></tr>
<tr><th>Total</th><td></td><td></td><th>Fr. {0} .-</th></tr>
</table>
</div>

<div id='signatur'>
<table class='tablehidden'>
<tr><td>Wir danken Ihnen für Ihr Vertrauen</td></tr>
<tr><th>Sie wurden bedient von</th><td>{1}</td></tr>
</table>
</div>


</div>
<!--Inhalt Endet -->
</div></body></html>
", Total, _nameVorname));


            return html;
        }
    }
}
