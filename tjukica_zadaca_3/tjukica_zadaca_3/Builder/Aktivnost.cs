using System;
using System.Collections.Generic;
using System.Text;
using tjukica_zadaca_1.Composite;

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
        private TipVozila vozilo;
        private int brojKm;

        static Helpers.ConsoleWriter cw = Helpers.ConsoleWriter.getInstance();

        public int IdAktivnosti { get => idAktivnosti; private set => idAktivnosti = value; }
        public DateTime Vrijeme { get => vrijeme; private set => vrijeme = value; }
        public Korisnik Korisnik { get => korisnik; set => korisnik = value; }
        public Lokacija Lokacija { get => lokacija; set => lokacija = value; }
        public TipVozila Vozilo { get => vozilo; set => vozilo = value; }
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
            private TipVozila vozilo;
            private int brojKm;
            private string opis;

            public int IdAktivnosti { get => idAktivnosti; set => idAktivnosti = value; }
            public DateTime Vrijeme { get => vrijeme; set => vrijeme = value; }
            public Korisnik Korisnik { get => korisnik; set => korisnik = value; }
            public Lokacija Lokacija { get => lokacija; set => lokacija = value; }
            public TipVozila Vozilo { get => vozilo; set => vozilo = value; }
            public int BrojKm { get => brojKm; set => brojKm = value; }

            public Builder(int idAktivnosti, DateTime vrijeme)
            {
                this.IdAktivnosti = idAktivnosti;
                this.Vrijeme = vrijeme;
            }

            public Builder setPodatci(Korisnik idKorisnik, Lokacija idLokacija, TipVozila idVozilo)
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
                    cw.Write("Greška prilikom aktivnosti: " + Aktivnost.getNazivAktivnosti(idAktivnosti) + " - Korisnik ne postoji.");
                    return null;
                }
                this.Korisnik = korisnik;
                return this;
            }
            public Builder setLokacija(Lokacija lokacija)
            {
                if (lokacija == null)
                {
                    cw.Write("Greška prilikom aktivnosti: " + Aktivnost.getNazivAktivnosti(idAktivnosti) + " - Lokacija ne postoji.");
                    return null;
                }
                this.Lokacija = lokacija;
                return this;
            }
            public Builder setVozilo(TipVozila vozilo)
            {
                if (vozilo == null)
                {
                    cw.Write("Greška prilikom aktivnosti: " + Aktivnost.getNazivAktivnosti(idAktivnosti) + " - Tip vozila ne postoji.");
                    return null;
                }
                this.Vozilo = vozilo;
                return this;
            }
            public Builder setBrojKm(int brojKm)
            {
                if (brojKm < 0)
                {
                    cw.Write("Greška prilikom aktivnosti: " + Aktivnost.getNazivAktivnosti(idAktivnosti) + " - Broj kilometara ne može biti negativan.");
                    return null;
                }
                this.BrojKm = brojKm;
                return this;
            }
            public Builder setOpis(string opis)
            {
                if(opis == "")
                {
                    cw.Write("Greška prilikom vraćanja neispravnog vozila. Niste unjeli opis problema.");
                    return null;
                }
                this.opis = opis;
                return this;
            }

            public Aktivnost build()
            {
                Baza.getInstance().setVirtualnoVrijeme(this.Vrijeme);
                return new Aktivnost(this);
            }

        }

        public static TipAktivnosti getNazivAktivnosti(int idAktivnosti)
        {
            switch (idAktivnosti)
            {
                case 0:
                    return TipAktivnosti.kraj;
                case 1:
                    return TipAktivnosti.pregledVozila;
                case 2:
                    return TipAktivnosti.najam;
                case 3:
                    return TipAktivnosti.pregledMjesta;
                case 4:
                    return TipAktivnosti.vracanje;
                case 5:
                    return TipAktivnosti.prijelazUSkupni;
                case 6:
                    return TipAktivnosti.ispisStanja;
                case 7:
                    return TipAktivnosti.ispisPodatakaONajmu;
                case 8:
                    return TipAktivnosti.ispisPodatakaORacunima;
            }

            return TipAktivnosti.ERROR;
        }

        public enum TipAktivnosti
        {
            kraj,
            pregledVozila,
            najam,
            pregledMjesta,
            vracanje,
            prijelazUSkupni,
            ispisStanja,
            ispisPodatakaONajmu,
            ispisPodatakaORacunima,
            ERROR
        }
    }
}
