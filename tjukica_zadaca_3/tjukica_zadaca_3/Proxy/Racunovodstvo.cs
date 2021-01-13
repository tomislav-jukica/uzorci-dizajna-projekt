using System;
using System.Collections.Generic;
using System.Text;
using tjukica_zadaca_1.Helpers;

namespace tjukica_zadaca_1.Proxy
{
    class Racunovodstvo : IzdavanjeRacuna
    {
        private int racunId = 1;
        private ConsoleWriter cw = ConsoleWriter.getInstance();
        private Baza baza = Baza.getInstance();
        private List<Racun> racuni = new List<Racun>();
        

        private static Racunovodstvo racunovodstvo = null;
        private Racunovodstvo() { } //konstruktor

        public static Racunovodstvo getInstance()
        {
            if (racunovodstvo == null)
            {
                racunovodstvo = new Racunovodstvo();
            }
            return racunovodstvo;
        }

        public void IzdajRacun(int lokacijaIdNajam, int lokacijaIdVracanje, int voziloId, double brojSati, int brojKm, DateTime vrijeme, int korisnikId)
        {
            TipVozila vozilo = baza.getVozilo(voziloId);
            Cjenik cjenik = baza.getCjenikZaVozilo(vozilo);

            double ukupno = cjenik.najam + cjenik.cijenaSat * (int)Math.Ceiling(brojSati) + cjenik.cijenaKm * brojKm;
            Racun noviRacun = new Racun(racunId++, lokacijaIdNajam, lokacijaIdVracanje, voziloId, brojSati, brojKm, korisnikId, vrijeme ,ukupno);
            racuni.Add(noviRacun);

            cw.HorizontalLine();
            cw.Write("Racun broj: " + noviRacun.id, false);
            cw.Write(vrijeme.ToString(), false);
            cw.Write("Kupac: " + baza.getKorisnik(korisnikId).ime,false);
            cw.Write("Vozilo unajmljeno na lokaciji: " + baza.getLokacija(lokacijaIdNajam).naziv, false);
            cw.Write("Vozilo vraceno na lokaciju: " + baza.getLokacija(lokacijaIdVracanje).naziv, false);
            cw.Write("");
            cw.Write("Stavke racuna:", false);
            cw.Write("1. Najam: " + cjenik.najam + " KN", false);
            cw.Write("2. Cijena po satu * sati u najmu: " 
                + cjenik.cijenaSat + " * " + (int)Math.Ceiling(brojSati) + " = " 
                + cjenik.cijenaSat * (int)Math.Ceiling(brojSati) + " KN", false);
            cw.Write("3. Cijena po km * prijedeni kilometri: "
                + cjenik.cijenaKm + " * " + brojKm + " = " + cjenik.cijenaKm * brojKm + " KN", false);
            cw.Write("UKUPNO: " + ukupno + " KN", false);
            cw.HorizontalLine();

            DodajZaraduLokaciji(lokacijaIdNajam, ukupno);
        }

        private void DodajZaraduLokaciji(int idLokacije, double zarada)
        {
            baza.getLokacija(idLokacije).zarada += zarada;
        }
        public List<Racun> GetRacuni()
        {
            return this.racuni;
        }

    }
}
