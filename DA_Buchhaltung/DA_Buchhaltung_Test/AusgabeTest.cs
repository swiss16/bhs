using System;
using System.Collections.Generic;
using System.Linq;
using DA_Buchhaltung.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DA_Buchhaltung_Test
{
    [TestClass]
    public class AusgabeTest
    {
        /// <summary>
        /// Unittests für die 3 Ausgabetests ( UT 4,5,6) und den Rückerstattungstest (UT 11)
        /// </summary>

        public int _id = 0;
        [TestMethod]
        public void AusgabeErfassenTest_KorrektesErfassen()
        {
            //Arrange           
            Model model = new Model();
            int kId = model.LadeKreditoren("").First().ID;
            Kategorie kat = model.LadeKategorien().First();
            Rechnung sollRechnung = new Rechnung
            {
                Beschreibung = "Testrechnung",
                Betrag = 100m,
                Kategorie = kat.Name,
                KreditorID = kId
            };

            //Act
            this._id = model.SpeichereRechnung(sollRechnung);
            List<Rechnung> rlist = model.LadeRechnungen("");
            Rechnung istRechnung = rlist.First(i => i.ID == this._id);


            //Assert
            Assert.IsNotNull(istRechnung);
            Assert.IsTrue(istRechnung.Beschreibung == "Testrechnung");
        }

        [TestMethod]
        public void AusgabeAendernTest_KorrektesSpeichern()
        {
            //Arrange
            Model model = new Model();
            List<Rechnung> rlist = model.LadeRechnungen("");
            Rechnung istRechnung = rlist.First(i => i.Beschreibung == "Testrechnung");
            istRechnung.Betrag = 555m;


            //Act
            this._id = model.SpeichereRechnung(istRechnung);
            rlist = model.LadeRechnungen("");
            istRechnung = rlist.First(i => i.Beschreibung == "Testrechnung");


            //Assert
            Assert.IsNotNull(istRechnung);
            Assert.IsTrue(istRechnung.Betrag == 555m);
        }

        [TestMethod]
        public void AusgabeLoeschenTest_KorrektesLoeschen()
        {
            //Arrange
            Model model = new Model();
            List<Rechnung> rlist = model.LadeRechnungen("");
            Rechnung istRechnung = rlist.First(i => i.Beschreibung == "Testrechnung");


            //Act
            bool successfull = model.LoescheRechnung(istRechnung);
            rlist = model.LadeRechnungen("");
            bool nochVorhanden = rlist.Any(i => i.Beschreibung == "Testrechnung");


            //Assert
            Assert.IsTrue(successfull);
            Assert.IsFalse(nochVorhanden);
        }

        [TestMethod]
        public void RueckerstattungErfassenTest_KorrektesErfassen()
        {
            //Arrange           
            Model model = new Model();
            int kId = model.LadeKreditoren("").First().ID;
            Kategorie kat = model.LadeKategorien().First();
            Rechnung sollRueckerstattung = new Rechnung
            {
                Beschreibung = "Testrueckerstattung",
                Betrag = 100m,
                Kategorie = kat.Name,
                KreditorID = kId,
                AlsRueckzahlung = true
            };

            //Act
            this._id = model.SpeichereRueckzahlung(sollRueckerstattung);
            List<Rechnung> rlist = model.LadeRueckzahlungen("");
            Rechnung istRueckzahlung = rlist.First(i => i.ID == this._id);


            //Assert
            Assert.IsNotNull(istRueckzahlung);
            Assert.IsTrue(istRueckzahlung.Beschreibung == "Testrueckerstattung");
            Assert.IsTrue(model.LoescheRueckzahlung(istRueckzahlung));
        }
    }
}
