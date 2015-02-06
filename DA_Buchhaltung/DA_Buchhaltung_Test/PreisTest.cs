using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DA_Buchhaltung.model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DA_Buchhaltung_Test
{
    [TestClass]
    public class PreisTest
    {
        /// <summary>
        /// Unittest für Konfigurieren von Preisoptionen (UT 10)
        /// </summary>

        [TestMethod]
        public void PreiseKonfigurieren_AenderePreisUndWiederRueckgaengig_Value145()
        {

            //Arrange           
            Model model = new Model();
            List<PreisOption> poList = model.LadePreisOptionen();
            PreisOption istPreisOption = poList.First();
            int oldId = istPreisOption.ID;
            string oldName = istPreisOption.Name;
            decimal oldPreis = istPreisOption.Preis;
            istPreisOption.Preis = 145m;

            //Act
            bool isSuccesfull = model.AenderePreisOption(istPreisOption);
            Thread.Sleep(2000);
            poList = model.LadePreisOptionen();
            istPreisOption = poList.First(I=> I.Preis == 145m && I.Name == oldName);


            //Assert
            Assert.IsNotNull(istPreisOption);
            Assert.IsTrue(istPreisOption.Preis == 145m); //neues Preisobjekt checken
            Assert.IsFalse(poList.Any(i=>i.ID == oldId)); //Altes Preisobjekt darf nicht mehr existieren

            //Rückgängig machen
            istPreisOption.Preis = oldPreis;
            Assert.IsTrue(model.AenderePreisOption(istPreisOption));
        }
    }
}
