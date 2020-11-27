using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    public class Punjenje
    {
        public Vozilo vozilo { get; private set; }
        public LokacijaKapacitet lokacija { get; private set; }
        private DateTime vrijemeAktivnosti {  get;  set; }
        public DateTime gotovoPunjenje { get; private set; }

        private Helpers.ConsoleWriter cw = Helpers.ConsoleWriter.getInstance();

        public Punjenje(Vozilo vozilo, LokacijaKapacitet lokacija, DateTime vrijeme)
        {
            this.vozilo = vozilo;
            this.lokacija = lokacija;
            this.vrijemeAktivnosti = vrijeme;
        }

        public void IzracunajVrijemePunjenja(int napravljeniKilometri)
        {
            int vrijemePunjenja = vozilo.vrijemePunjenja; // npr. 4 sata
            int domet = vozilo.domet;
            int postotakBaterije = IzracunajPostotakBaterije(domet, napravljeniKilometri);
            if(postotakBaterije == -1)
            {
                cw.Write("Postotak baterije ne može biti manji od 0%!");
            }
            vozilo.kilometri += napravljeniKilometri;
            int zaNapuniti = 100 - postotakBaterije;
            double jedanPosto = (double)vrijemePunjenja / (double)100;
            double satiPunjenja = zaNapuniti * jedanPosto;
            gotovoPunjenje = vrijemeAktivnosti.AddHours(satiPunjenja);

        }

        private int IzracunajPostotakBaterije(int domet, int kilometri)
        {
            if(kilometri > domet)
            {
                cw.Write("Prijedeni kilometri su veci od dometa vozila.");
                return -1;
            } else
            {
                double baterija = (double)(domet - kilometri) / (double)domet;
                baterija *= 100;
                return (int)baterija;
            }
        }
    }
}
