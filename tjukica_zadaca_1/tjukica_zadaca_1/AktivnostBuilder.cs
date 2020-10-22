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
            this.korisnik = idKorisnik;
            this.lokacija = idLokacija;
            this.vozilo = idVozilo;
            return this;
        }

        public AktivnostBuilder setKorisnik(Korisnik korisnik)
        {
            this.korisnik = korisnik;
            return this;
        }
        public AktivnostBuilder setLokacija(Lokacija lokacija)
        {
            this.lokacija = lokacija;
            return this;
        }
        public AktivnostBuilder setVozilo(Vozilo vozilo)
        {
            this.vozilo = vozilo;
            return this;
        }
        public AktivnostBuilder setBrojKm(int brojKm)
        {
            this.brojKm = brojKm;
            return this;
        }

        public Aktivnost build()
        {
            return new Aktivnost(this);
        }

    }
}
