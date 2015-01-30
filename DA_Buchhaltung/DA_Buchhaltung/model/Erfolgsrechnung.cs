using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA_Buchhaltung.common.config;
using DA_Buchhaltung.common.log;

namespace DA_Buchhaltung.model
{
    public class Erfolgsrechnung
    {
        public int ID { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EndDatum { get; set; }
        public List<Betraege> Einnahmen { get; set; }
        public List<Betraege> Ausgaben { get; set; }
        public decimal SubtotalEinnahmen { get; set; }
        public decimal SubtotalAusgaben { get; set; }
        public decimal Gewinn { get; set; }
        public bool IstJahresabrechnung { get; set; }

        private string _firma;
        private string _nameVorname;
        private string _adresse;
        private string _plzUndOrt;

        public Erfolgsrechnung()
        {
            this.ID = -1;
            this.StartDatum = DateTime.Now;
            this.EndDatum = DateTime.Now.AddDays(1);
            this.Einnahmen = new List<Betraege>();
            this.Ausgaben = new List<Betraege>();
            this.SubtotalEinnahmen = 0.00m;
            this.SubtotalAusgaben = 0.00m;
            this.Gewinn = 0.00m;
            this.IstJahresabrechnung = false;

            _firma = ConfigWrapper.Firma;
            _nameVorname = ConfigWrapper.NameVorname;
            _adresse = ConfigWrapper.StrasseUndNummer;
            _plzUndOrt = ConfigWrapper.PlzUndOrt;
        }

        /// <summary>
        /// Erstellt eine .html Datei, gemäss diesem Erfolgsrechnung-Objekt. Gibt True zurück, wenn es erfolgreich war. Die Datei wird anschliessend geöffnet.
        /// </summary>
        /// <returns></returns>
        public bool Print()
        {
            Update();
            string html = getHtmlCode();
            string filename = string.Format("Erfolgsrechnung_{0}.html", DateTime.Now.ToFileTime());
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
                if (File.Exists(@"common\layout\layout.css"))
                {
                    File.Copy(@"common\layout\layout.css", Path.Combine(dir, "layout.css"), true);
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
                Logger.append("Fehler beim Schreiben: Original: "+e.ToString(),Logger.ERROR);
                throw new Exception("Fehler beim Schreiben! Originalmeldung steht im Logfile");
            }
            return true;
        }

        /// <summary>
        /// Berechnet gemäss den Ein- und Ausgabenlisten, die jeweiligen Subtotals und den Gewinn(negativ Verlust).
        /// </summary>
        public void Update()
        {
            if (Einnahmen == null)
            {
                Einnahmen = new List<Betraege>();
            }
            if (Ausgaben == null)
            {
                Ausgaben = new List<Betraege>();
            }
            this.SubtotalEinnahmen = berechneSubtotal(Einnahmen);
            this.SubtotalAusgaben = berechneSubtotal(Ausgaben);
            this.Gewinn = berechneGewinn(SubtotalEinnahmen, SubtotalAusgaben);
        }

        private decimal berechneSubtotal(List<Betraege> betraege )
        {
            decimal _tempPreis = 0.0m;
            foreach (var betr in betraege)
            {
                _tempPreis += betr.BetragInFranken;
            }
            return _tempPreis;
        }

        private decimal berechneGewinn(decimal subtotalEinnahmen, decimal subtotalAusgaben)
        {

            return subtotalEinnahmen - subtotalAusgaben;
        }

        private string getHtmlCode()
        {
            //A4 = 210x297 (170 x237) Je Seite 20mm Abstand
            //Beginn header
            string html = (@"
<!DOCTYPE html>
<html>

<head>
<meta content='text/html; charset=utf-8' http-equiv='Content-Type'>
<meta name='viewport' content='width=device-width, initial-scale=1.0'/>
<meta name=“description” content='Erfolgsrechnung, generiert durch die Software Nailartist Financial Manager'>
<meta name='author' content='Martin Osterwalder'>
<title>Nailartist Financial Manager - Erfolgsrechnung</title>
<link rel='stylesheet' href='layout.css' type='text/css' media='screen' />
<link rel='stylesheet' href='layout.css' type='text/css' media='print' />
</head><body><div ID='a4'>
<!--Inhalt Beginnt -->

<header>
<img id='logo' src='logo.jpg' alt='Logo'>
<div id='title'>
");
            //Ende Head
            //Beginn Inhalt
            if (this.IstJahresabrechnung)
            {
                html = string.Concat(html, @"<h1 class='mitte'>Jahresabschluss</h1></div>");
            }
            else
            {
                html = string.Concat(html, @"<h1 class='mitte'>Zwischenbilanz</h1></div>");
            }
            html = string.Concat(html, @"
</header>
<div id='content'>
<h2>Informationen</h2>
<table class='tablehidden'>
");
            html = string.Concat(html, string.Format(@"<tr><th class='left'>Firma:</th><td>{0}</td></tr>
<tr><th class='left'>Name Vorname:</th><td>{1}</td></tr>
<tr><th class='left'>Strasse und Hausnr:</th><td>{2}</td></tr>
<tr><th class='left'>PLZ und Ort:</th><td>{3}</td></tr>
<tr><th class='left'></th><td></td></tr>
<tr><th class='left'>Anfang:</th><td>{4}</td></tr>
<tr><th class='left'>Ende:</th><td>{5}</td></tr>
",_firma,_nameVorname,_adresse,_plzUndOrt,StartDatum.ToShortDateString(),EndDatum.ToShortDateString()));

            html = string.Concat(html, @"</table>
<h2>Erfolgsrechnung</h2>
<div class='einnahmen'>
<h3>Einnahmen</h3>
<table class='table'>
<tr><th>Kategorie</th><th>Betrag in CHF</th></tr>
");
            foreach (var betraege in Einnahmen)
            {
                html = string.Concat(html, string.Format(@"<tr><td>{0}</td><td>{1} .-</td></tr>",betraege.Kategorie,betraege.BetragInFranken));
            }

            html = string.Concat(html, string.Format(@"<tr><td></td><td></td></tr>
<tr><th>Subtotal Einnahmen</th><th>{0} .-</th></tr></table></div>
<div class='ausgaben'><h3>Ausgaben</h3><table class='table'><tr><th>Kategorie</th><th>Betrag in CHF</th></tr>
",SubtotalEinnahmen));

            foreach (var betraege in Ausgaben)
            {
                html = string.Concat(html, string.Format(@"<tr><td>{0}</td><td>{1} .-</td></tr>", betraege.Kategorie, betraege.BetragInFranken));
            }

            html = string.Concat(html, string.Format(@"<tr><td></td><td></td></tr>
<tr><th>Subtotal Ausgaben</th><th>{0} .-</th></tr>
</table></div><div class='total'><h2>Endresultat</h2><table class='table'>
<tr><td>Einnahmen:</td><td>{1} .-</td></tr><tr><td>Ausgaben:</td><td>-{2} .-</td></tr>
",SubtotalAusgaben,SubtotalEinnahmen,SubtotalAusgaben));

            if (SubtotalEinnahmen>=SubtotalAusgaben)
            {
                html = string.Concat(html,
                    string.Format(@"<tr class='gewinn'><th>Gewinn: </th><th class='double'> {0} .- CHF</th></tr>",Gewinn));
            }
            else
            {
                html = string.Concat(html,
                    string.Format(@"<tr class='verlust'><th>Verlust: </th><th class='double'>-{0} .- CHF</th></tr>", Gewinn));
            }

            html = string.Concat(html, string.Format(@"</table></div><div id='signatur'><table class='tablehidden'>
<tr><th>Diese Abrechnung wurde erstellt von</th><td>{0}</td></tr>
<tr><td colspan='2'>Hiermit erkläre ich die Richtigkeit der Angaben.</td></tr><tr class='abstand'><th></th><td></td></tr>
<tr><th>Unterschrift</th><td>___________________________</td></tr>
",_nameVorname));
            //Ende Inhalt
            //Abschluss des Html Codes
            html = string.Concat(html, @"</table></div></div><!--Inhalt Endet --></div></body></html>");

            return html;
        }

    }
}
