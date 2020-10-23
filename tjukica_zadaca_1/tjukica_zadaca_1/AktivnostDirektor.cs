using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    class AktivnostDirektor
    {
        public static Aktivnost PregledVozila(int idAktivnosti, DateTime vrijeme, Korisnik idKorisnik, Lokacija idLokacija, Vozilo idVozilo)
        {
            Aktivnost aktivnost = null;
            AktivnostBuilder builder = null;
            builder = new AktivnostBuilder(idAktivnosti, vrijeme).setPodatci(idKorisnik, idLokacija, idVozilo);
            if (builder != null)
            {
                aktivnost = builder.build();
                Console.WriteLine("Na lokaciji " + idLokacija.naziv +
                " trenutno se nalazi " + Baza.getInstance().getKapacitetLokacije(idLokacija, idVozilo).brojVozila +
                " vozila tipa " + idVozilo.naziv + ".");
                return aktivnost;
            }
            else
            {
                return null;
            }
        }
        public static Aktivnost Najam(int idAktivnosti, DateTime vrijeme, Korisnik idKorisnik, Lokacija idLokacija, Vozilo idVozilo)
        {
            Aktivnost aktivnost = null;
            AktivnostBuilder builder = null;
            builder = new AktivnostBuilder(idAktivnosti, vrijeme).setPodatci(idKorisnik, idLokacija, idVozilo);
            if (builder != null)
            {
                if (Baza.getInstance().getKapacitetLokacije(idLokacija, idVozilo).brojVozila < 1)
                {
                    Console.WriteLine("Na lokaciji " + idLokacija.naziv + " nema vozila tipa " + idVozilo.naziv + ".");
                    return null;
                }
                if (idKorisnik.najamVozila != null)
                {
                    Console.WriteLine("Korisnik " + idKorisnik.ime + " već ima u najmu vozilo tipa " + idKorisnik.najamVozila.naziv + ".");
                    return null;
                }
                aktivnost = builder.build();
                Baza.getInstance().getKapacitetLokacije(idLokacija, idVozilo).brojVozila--;
                idKorisnik.najamVozila = idVozilo;
                Console.WriteLine("Korisnik " + idKorisnik.ime + " unajmio je vozilo tipa " + idVozilo.naziv + " na lokaciji " + idLokacija.naziv + ".");
                return aktivnost;
            }
            else
            {
                return null;
            }
        }
        public static Aktivnost PregledMjesta(int idAktivnosti, DateTime vrijeme, Korisnik idKorisnik, Lokacija idLokacija, Vozilo idVozilo)
        {
            Aktivnost aktivnost = null;
            AktivnostBuilder builder = null;
            builder = new AktivnostBuilder(idAktivnosti, vrijeme).setPodatci(idKorisnik, idLokacija, idVozilo);
            if (builder != null)
            {
                aktivnost = builder.build();
                LokacijaKapacitet kapacitet = Baza.getInstance().getKapacitetLokacije(idLokacija, idVozilo);
                int slobodnaMjesta = kapacitet.brojMjesta - kapacitet.brojVozila;
                Console.WriteLine("Na lokaciji " + idLokacija.naziv + " ima " + slobodnaMjesta + " slobodnih mjesta za vozilo tipa " + idVozilo.naziv + ".");
                return aktivnost;
            }
            else
            {
                return null;
            }
        }
        public static Aktivnost Vracanje(int idAktivnosti, DateTime vrijeme, Korisnik idKorisnik, Lokacija idLokacija, Vozilo idVozilo, int brojKm)
        {
            Aktivnost aktivnost = null;
            AktivnostBuilder builder = null;
            builder = new AktivnostBuilder(idAktivnosti, vrijeme).setPodatci(idKorisnik, idLokacija, idVozilo).setBrojKm(brojKm);
            if (builder != null)
            {
                if (idKorisnik.najamVozila == null)
                {
                    Console.WriteLine("Korisnik " + idKorisnik.ime + " nema nijedno vozilo u najmu.");
                    return null;
                }
                else if (idKorisnik.najamVozila != idVozilo)
                {
                    Console.WriteLine("Korisnik " + idKorisnik.ime + " nema u najmu vozilo tipa " + idVozilo.naziv);
                    return null;
                }
                LokacijaKapacitet kapacitet = Baza.getInstance().getKapacitetLokacije(idLokacija, idVozilo);
                if (kapacitet.brojMjesta == kapacitet.brojVozila)
                {
                    Console.WriteLine("Na lokaciji " + idLokacija.naziv + " nema slobodnog mjesta za vozilo tipa " + idVozilo.naziv + ".");
                    return null;
                } else
                {
                    Console.WriteLine("Korisnik " + idKorisnik.ime + " vratio je vozilo tipa " + idKorisnik.najamVozila.naziv + " na lokaciju " + idLokacija.naziv + ".");
                    kapacitet.brojVozila++;
                    idKorisnik.najamVozila = null;
                    aktivnost = builder.build();
                    return aktivnost;
                }
                
            }
            else
            {
                return null;
            }
        }
        public static Aktivnost Kraj(int idAktivnosti, DateTime vrijeme)
        {
            return new AktivnostBuilder(idAktivnosti, vrijeme).build();
        }
    }
}
