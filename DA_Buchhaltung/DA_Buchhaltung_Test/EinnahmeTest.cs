using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using DA_Buchhaltung.data;
using DA_Buchhaltung.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DA_Buchhaltung_Test
{
    /// <summary>
    /// Unittests für die 3 Einnahmetests ( UT 7,8,9)
    /// </summary>
    [TestClass]
    public class EinnahmeTest
    {
        public int _id = 0;
        [TestMethod]
        public void EinnahmeErfassenTest_KorrektesErfassen()
        {
            //Arrange           
            Model model = new Model();
            Dienstleistung dl = model.LadeDienstleistungen().First();
            int kId = model.LadeKunden("").First().ID;
            Auftrag sollAuftrag = new Auftrag
            {
                KundeID = kId,
                Dienstleistung = dl,
                Total = 140m
            };

            //Act
            this._id = model.SpeichereAuftrag(sollAuftrag);
            List<Auftrag> alist = model.LadeAuftraege("");
            Auftrag istAuftrag = alist.First(i => i.ID == this._id);


            //Assert
            Assert.IsNotNull(istAuftrag);
            Assert.IsTrue(istAuftrag.Total == sollAuftrag.Total);
        }

        [TestMethod]
        public void EinnahmeAendernTest_KorrektesSpeichern()
        {
            //Arrange
            Model model = new Model();
            List<Auftrag> alist = model.LadeAuftraege("");
            Auftrag istAuftrag = alist.Last();
            istAuftrag.Total = 222m;
            

            //Act
            this._id = model.SpeichereAuftrag(istAuftrag);
            alist = model.LadeAuftraege("");
            istAuftrag = alist.First(i => i.ID == this._id);


            //Assert
            Assert.IsNotNull(istAuftrag);
            Assert.IsTrue(istAuftrag.Total == 222m);
        }

        [TestMethod]
        public void EinnahmeLoeschenTest_KorrektesLoeschen()
        {
            //Arrange
            Model model = new Model();
            List<Auftrag> alist = model.LadeAuftraege("");
            Auftrag istAuftrag = alist.Last();


            //Act
            bool successfull = model.LoescheAuftrag(istAuftrag);
            alist = model.LadeAuftraege("");
            bool nochVorhanden = alist.Any(i => i.ID == this._id);


            //Assert
            Assert.IsTrue(successfull);
            Assert.IsFalse(nochVorhanden);
        }
    }
}
