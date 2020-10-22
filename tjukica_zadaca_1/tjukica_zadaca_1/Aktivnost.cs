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

        public Aktivnost(AktivnostBuilder builder)
        {
            this.idAktivnosti = builder.IdAktivnosti;
            this.vrijeme = builder.Vrijeme;
            this.lokacija = builder.Lokacija;
            this.korisnik = builder.Korisnik;
            this.vozilo = builder.Vozilo;
            this.brojKm = builder.BrojKm;
        }

        
    }
}
