using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DA_Buchhaltung.wrapper;

namespace DA_Buchhaltung.model
{
    public class Model
    {
        //Private Variablen
        DBWrapper dbWrapper = new DBWrapper();
        private List<Kunde> kundenListe;


        public List<Kunde> LadeKunden()
        {
            try
            {
                kundenListe = dbWrapper.LadeKunden();
            }
            catch (Exception e)
            {
                kundenListe = new List<Kunde>();
                MessageBox.Show(e.ToString(), "Datenbank Error",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            return kundenListe;
        } 




    }
}
