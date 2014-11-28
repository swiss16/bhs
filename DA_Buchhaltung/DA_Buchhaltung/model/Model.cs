using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA_Buchhaltung.wrapper;

namespace DA_Buchhaltung.model
{
    public class Model
    {
        //Private Properties
        DBWrapper dbWrapper = new DBWrapper();
        private List<Kunde> kundenListe;


        public List<Kunde> LadeKunde()
        {
            kundenListe = dbWrapper.LadeKunden();


            return kundenListe;
        } 




    }
}
