using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    class AktivnostBuilder
    {
        //Glavne
        private int idAktivnosti;
        private DateTime vrijeme;

        //Opcionalne
        private int idKorisnik;
        private int idLokacija;
        private int idVozilo;
        private int brojKm;
        public int IdKorisnik { get => idKorisnik; private set => idKorisnik = value; }
        public int IdLokacija { get => idLokacija; private set => idLokacija = value; }
        public int IdVozilo { get => idVozilo; private set => idVozilo = value; }
        public int BrojKm { get => brojKm; private set => brojKm = value; }
        public int IdAktivnosti { get => idAktivnosti; private set => idAktivnosti = value; }
        public DateTime Vrijeme { get => vrijeme; private set => vrijeme = value; }

        public AktivnostBuilder(int idAktivnosti, DateTime vrijeme)
        {
            this.idAktivnosti = idAktivnosti;
            this.vrijeme = vrijeme;
        }

        public AktivnostBuilder setPodatci(int idKorisnik, int idLokacija, int idVozilo)
        {
            this.idKorisnik = idKorisnik;
            this.idLokacija = idLokacija;
            this.idVozilo = idVozilo;
            return this;
        }

        public AktivnostBuilder setIdKorisnik(int id)
        {
            this.idKorisnik = id;
            return this;
        }
        public AktivnostBuilder setIdLokacija(int id)
        {
            this.idLokacija = id;
            return this;
        }
        public AktivnostBuilder setIdVozilo(int id)
        {
            this.idVozilo = id;
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
