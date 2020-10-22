using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    public class Baza
    {
        private List<Korisnik> korisnici = new List<Korisnik>();
        private List<Lokacija> lokacije = new List<Lokacija>();
        private List<Vozilo> vozila = new List<Vozilo>();
        private List<Cjenik> cjenik = new List<Cjenik>();
        private List<LokacijaKapacitet> lokacijaKapacitet = new List<LokacijaKapacitet>();
        private DateTime virtualnoVrijeme;

        private static Baza baza = null;

        private Baza(){} //konstruktor

        public static Baza getInstance()
        {
            if(baza == null)
            {
                baza = new Baza();
            }
            return baza;
        }

        public DateTime getVirtualnoVrijeme()
        {
            return baza.virtualnoVrijeme;
        }
        public void setVirtualnoVrijeme(DateTime vrijeme)
        {
            baza.virtualnoVrijeme = vrijeme;
        }



        public List<Korisnik> getKorisnici()
        {
            return baza.korisnici;
        }
        public List<Lokacija> getLokacije()
        {
            return baza.lokacije;
        }
        public List<Vozilo> getVozila()
        {
            return baza.vozila;
        }
        public List<Cjenik> getCjenik()
        {
            return baza.cjenik;
        }
        public List<LokacijaKapacitet> getLokacijaKapacitet()
        {
            return baza.lokacijaKapacitet;
        }
    }
}
