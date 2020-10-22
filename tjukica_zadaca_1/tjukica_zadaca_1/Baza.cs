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
        private List<LokacijaKapacitet> kapacitetiLokacija = new List<LokacijaKapacitet>();
        private DateTime virtualnoVrijeme;
        private List<Aktivnost> aktivnosti = new List<Aktivnost>();

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


        public bool UsporediVrijeme(DateTime vrijeme)
        {
            if(vrijeme.CompareTo(baza.virtualnoVrijeme) > 0)
            {
                return true;
            } else
            {
                return false;
            }
        }


        public DateTime getVirtualnoVrijeme()
        {
            return baza.virtualnoVrijeme;
        }
        public void setVirtualnoVrijeme(DateTime vrijeme)
        {
            baza.virtualnoVrijeme = vrijeme;
        }

        public Korisnik getKorisnik(int id)
        {
            Korisnik korisnik = null;
            foreach (Korisnik k in baza.korisnici)
            {
                if(k.id == id)
                {
                    korisnik = k;
                }
            }
            return korisnik;
        }
        public Lokacija getLokacija(int id)
        {
            Lokacija lokacija = null;
            foreach(Lokacija l in baza.lokacije)
            {
                if(l.id == id)
                {
                    lokacija = l;
                }
            }
            return lokacija;
        }
        public Vozilo getVozilo(int id)
        {
            Vozilo vozilo = null;
            foreach(Vozilo v in baza.vozila)
            {
                if(v.id == id)
                {
                    vozilo = v;
                }
            }
            return vozilo;
        }
        public LokacijaKapacitet getKapacitetLokacije(Lokacija lokacija, Vozilo vozilo)
        {
            LokacijaKapacitet retVal = null;
            foreach (LokacijaKapacitet x in baza.kapacitetiLokacija)
            {
                if(x.lokacija.id == lokacija.id && x.vozilo.id == vozilo.id)
                {
                    retVal = x;
                }
            }
            return retVal;
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
            return baza.kapacitetiLokacija;
        }

        public List<Aktivnost> getAktivnosti()
        {
            return baza.aktivnosti;
        }
    }
}
