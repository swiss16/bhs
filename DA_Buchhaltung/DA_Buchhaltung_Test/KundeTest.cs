using System;
using System.Collections.Generic;
using System.Linq;
using DA_Buchhaltung.data;
using DA_Buchhaltung.model;
using DA_Buchhaltung.viewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DA_Buchhaltung_Test
{
    [TestClass]
    public class KundeTest
    {
        [TestMethod]
        public void KundeErfassenTest_KorrektesErfassen()
        {
            //Arrange
            Model model = new Model();
            Kunde sollKunde = new Kunde
            {
                Adresse = "Teststrasse",
                Email = "Test@test.ch",
                Name = "Tester",
                Vorname = "Test",
                PLZ = 8888,
                Wohnort = "Teststadt"
            };

            //Act
            int id = model.SpeichereKunde(sollKunde);
            List<Kunde> klist = model.LadeKunden("");
            Kunde istKunde = klist.First(i => i.ID == id);


            //Assert
            Assert.IsNotNull(istKunde);
            Assert.IsTrue(sollKunde.Adresse == istKunde.Adresse);
        }

        [TestMethod]
        public void KundeAendernTest_KorrektesSpeichern()
        {
            //Arrange
            Model model = new Model();
            List<Kunde> klist = model.LadeKunden("");
            Kunde istKunde = klist.First(i => i.Adresse == "Teststrasse");
            istKunde.Adresse = "Teststrasse2";
            Kunde sollKunde = new Kunde
            {
                Adresse = "Teststrasse2",
                Email = "Test@test.ch",
                Name = "Tester",
                Vorname = "Test",
                PLZ = 8888,
                Wohnort = "Teststadt"
            };

            //Act
            int id = model.SpeichereKunde(istKunde);
            klist = model.LadeKunden("");
            istKunde = klist.First(i => i.ID == id);


            //Assert
            Assert.IsNotNull(istKunde);
            Assert.IsTrue(sollKunde.Adresse == istKunde.Adresse);
        }

        [TestMethod]
        public void KundeLoeschenTest_KorrektesLoeschen()
        {
            //Arrange
            Model model = new Model();
            List<Kunde> klist = model.LadeKunden("");
            Kunde istKunde = klist.First(i => i.Adresse == "Teststrasse2");
            

            //Act
            bool successfull = model.LoescheKunde(istKunde);
            klist = model.LadeKunden("");
            bool nochVorhanden = klist.Any(i => i.Adresse == "Teststrasse2");


            //Assert
            Assert.IsTrue(successfull);
            Assert.IsFalse(nochVorhanden);
        }
    }
}
