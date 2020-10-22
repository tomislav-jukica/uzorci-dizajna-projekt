using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    class Aktivnost
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
       
        public Aktivnost(AktivnostBuilder builder)
        {
            this.idAktivnosti = builder.IdAktivnosti;
            this.vrijeme = builder.Vrijeme;
            this.idLokacija = builder.IdLokacija;
            this.idKorisnik = builder.IdKorisnik;
            this.idVozilo = builder.IdVozilo;
            this.brojKm = builder.BrojKm;
        }

        
    }
}
