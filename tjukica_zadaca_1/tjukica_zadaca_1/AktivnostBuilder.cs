using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    public class AktivnostBuilder
    {
        //Glavne
        private int idAktivnosti;
        private DateTime vrijeme;

        //Opcionalne
        private Korisnik korisnik;
        private Lokacija lokacija;
        private Vozilo vozilo;
        private int brojKm;
        
        public int IdAktivnosti { get => idAktivnosti; private set => idAktivnosti = value; }
        public DateTime Vrijeme { get => vrijeme; private set => vrijeme = value; }
        public Korisnik Korisnik { get => korisnik; private set => korisnik = value; }
        public Lokacija Lokacija { get => lokacija; private set => lokacija = value; }
        public Vozilo Vozilo { get => vozilo; private set => vozilo = value; }
        public int BrojKm { get => brojKm; private set => brojKm = value; }

        public AktivnostBuilder(int idAktivnosti, DateTime vrijeme)
        {
            this.idAktivnosti = idAktivnosti;
            this.vrijeme = vrijeme;
        }

        public AktivnostBuilder setPodatci(Korisnik idKorisnik, Lokacija idLokacija, Vozilo idVozilo)
        {
            if(this.setKorisnik(idKorisnik) == null ||
            this.setLokacija(idLokacija) == null ||
            this.setVozilo(idVozilo) == null)
            {
                return null;
            } else
            {
                return this;
            }            
        }

        public AktivnostBuilder setKorisnik(Korisnik korisnik)
        {
            if(korisnik == null)
            {
                Console.WriteLine("Greška: Korisnik ne postoji.");
                return null;
            }
            this.korisnik = korisnik;
            return this;
        }
        public AktivnostBuilder setLokacija(Lokacija lokacija)
        {
            if (lokacija == null)
            {
                Console.WriteLine("Greška: Lokacija ne postoji.");
                return null;
            }
            this.lokacija = lokacija;
            return this;
        }
        public AktivnostBuilder setVozilo(Vozilo vozilo)
        {
            if (vozilo == null)
            {
                Console.WriteLine("Greška: Vozilo ne postoji.");
                return null;
            }
            this.vozilo = vozilo;
            return this;
        }
        public AktivnostBuilder setBrojKm(int brojKm)
        {
            if(brojKm < 0)
            {
                Console.WriteLine("Broj kilometara ne može biti negativan!");
                return null;
            }
            this.brojKm = brojKm;
            return this;
        }

        public Aktivnost build()
        {            
            Baza.getInstance().setVirtualnoVrijeme(this.vrijeme);
            return new Aktivnost(this);
        }

    }
}
