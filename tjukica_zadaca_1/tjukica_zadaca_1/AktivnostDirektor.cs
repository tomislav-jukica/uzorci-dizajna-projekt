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
                ProvjeriPunjenje(vrijeme);
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
                ProvjeriPunjenje(vrijeme);
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

                //NajamVozila najam = new NajamVozila(idVozilo.id, idVozilo.naziv, idVozilo.vrijemePunjenja, idVozilo.domet);
                NajamVozila najam = Baza.getInstance().getNajamVozila(idVozilo);
                najam.iznajmljen = true;
                idKorisnik.najamVozila = najam;
                Baza.getInstance().getIznajmljenaVozila().Add(idKorisnik.najamVozila);
                //Baza.getInstance().getVozilaZaNajam().Add(najam);
                Console.WriteLine("Korisnik " + idKorisnik.ime + " unajmio je vozilo tipa " + idVozilo.naziv + " na lokaciji " + idLokacija.naziv + ".");
                return aktivnost;
            }
            else
            {
                return null;
            }
        }

        private static void ProvjeriPunjenje(DateTime vrijeme)
        {
            foreach (Punjenje p in Baza.getInstance().getVozilaNaPunjenju())
            {
                if (p.gotovoPunjenje.CompareTo(vrijeme) > 0)
                {
                    p.lokacija.brojVozila++;
                    Baza.getInstance().getVozilaNaPunjenju().Remove(p);
                }
            }
        }

        public static Aktivnost PregledMjesta(int idAktivnosti, DateTime vrijeme, Korisnik idKorisnik, Lokacija idLokacija, Vozilo idVozilo)
        {
            Aktivnost aktivnost = null;
            AktivnostBuilder builder = null;
            builder = new AktivnostBuilder(idAktivnosti, vrijeme).setPodatci(idKorisnik, idLokacija, idVozilo);
            if (builder != null)
            {
                ProvjeriPunjenje(vrijeme);
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
        public static Aktivnost Vracanje(int idAktivnosti, DateTime vrijeme, Korisnik idKorisnik, Lokacija idLokacija, NajamVozila najam, int brojKm)
        {
            Aktivnost aktivnost = null;
            AktivnostBuilder builder = null;
            Vozilo idVozilo = Baza.getInstance().getVozilo(najam.id);
            builder = new AktivnostBuilder(idAktivnosti, vrijeme).setPodatci(idKorisnik, idLokacija, idVozilo).setBrojKm(brojKm);
            if (builder != null)
            {
                ProvjeriPunjenje(vrijeme);
                if (idKorisnik.najamVozila == null)
                {
                    Console.WriteLine("Korisnik " + idKorisnik.ime + " nema nijedno vozilo u najmu.");
                    return null;
                }
                else if (idKorisnik.najamVozila.id != idVozilo.id)
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

                    Punjenje punjenje = new Punjenje(najam, kapacitet, vrijeme);
                    Baza.getInstance().getVozilaNaPunjenju().Add(punjenje);
                    //Baza.getInstance().getVozilaZaNajam().Remove(idKorisnik.najamVozila);
                    idKorisnik.najamVozila = null;                    
                    aktivnost = builder.build();
                    return aktivnost;
                    //TODO
                    /*
                    kapacitet.brojVozila++;
                    idKorisnik.najamVozila = null;
                    aktivnost = builder.build();
                    return aktivnost;
                    */
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
