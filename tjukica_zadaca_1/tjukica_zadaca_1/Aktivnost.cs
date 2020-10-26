using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    public class Aktivnost
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
        public Korisnik Korisnik { get => korisnik; set => korisnik = value; }
        public Lokacija Lokacija { get => lokacija; set => lokacija = value; }
        public Vozilo Vozilo { get => vozilo; set => vozilo = value; }
        public int BrojKm { get => brojKm; private set => brojKm = value; }

        private Aktivnost(Builder builder)
        {
            this.idAktivnosti = builder.IdAktivnosti;
            this.vrijeme = builder.Vrijeme;
            this.lokacija = builder.Lokacija;
            this.korisnik = builder.Korisnik;
            this.vozilo = builder.Vozilo;
            this.brojKm = builder.BrojKm;
        }
        public class Builder
        {
            //Glavne
            private int idAktivnosti;
            private DateTime vrijeme;

            //Opcionalne
            private Korisnik korisnik;
            private Lokacija lokacija;
            private Vozilo vozilo;
            private int brojKm;

            public int IdAktivnosti { get => idAktivnosti; set => idAktivnosti = value; }
            public DateTime Vrijeme { get => vrijeme; set => vrijeme = value; }
            public Korisnik Korisnik { get => korisnik; set => korisnik = value; }
            public Lokacija Lokacija { get => lokacija; set => lokacija = value; }
            public Vozilo Vozilo { get => vozilo; set => vozilo = value; }
            public int BrojKm { get => brojKm; set => brojKm = value; }

            public Builder(int idAktivnosti, DateTime vrijeme)
            {
                this.IdAktivnosti = idAktivnosti;
                this.Vrijeme = vrijeme;
            }

            public Builder setPodatci(Korisnik idKorisnik, Lokacija idLokacija, Vozilo idVozilo)
            {
                if (this.setKorisnik(idKorisnik) == null ||
                this.setLokacija(idLokacija) == null ||
                this.setVozilo(idVozilo) == null)
                {
                    return null;
                }
                else
                {
                    return this;
                }
            }

            public Builder setKorisnik(Korisnik korisnik)
            {
                if (korisnik == null)
                {
                    Console.WriteLine("Greška: Korisnik ne postoji.");
                    return null;
                }
                this.Korisnik = korisnik;
                return this;
            }
            public Builder setLokacija(Lokacija lokacija)
            {
                if (lokacija == null)
                {
                    Console.WriteLine("Greška: Lokacija ne postoji.");
                    return null;
                }
                this.Lokacija = lokacija;
                return this;
            }
            public Builder setVozilo(Vozilo vozilo)
            {
                if (vozilo == null)
                {
                    Console.WriteLine("Greška: Vozilo ne postoji.");
                    return null;
                }
                this.Vozilo = vozilo;
                return this;
            }
            public Builder setBrojKm(int brojKm)
            {
                if (brojKm < 0)
                {
                    Console.WriteLine("Broj kilometara ne može biti negativan!");
                    return null;
                }
                this.BrojKm = brojKm;
                return this;
            }

            public Aktivnost build()
            {
                Baza.getInstance().setVirtualnoVrijeme(this.Vrijeme);
                return new Aktivnost(this);
            }

        }

    }
}
