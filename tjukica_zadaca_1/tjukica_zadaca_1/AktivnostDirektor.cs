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
                LokacijaKapacitet kapacitet = Baza.getInstance().getKapacitetLokacije(idLokacija, idVozilo);
                int brojVozila = kapacitet.dajBrojSlobodnihVozila();
                Console.WriteLine("Na lokaciji " + idLokacija.naziv +
                " trenutno se nalazi " + brojVozila +
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
                if (Baza.getInstance().getKapacitetLokacije(idLokacija, idVozilo).dajBrojSlobodnihVozila() < 1)
                {
                    Console.WriteLine("Na lokaciji " + idLokacija.naziv + " nema slobodnih vozila tipa " + idVozilo.naziv + ".");
                    return null;
                }
                if (idKorisnik.najamVozila != null)
                {
                    Console.WriteLine("Korisnik " + idKorisnik.ime + " već ima u najmu vozilo tipa " + idKorisnik.najamVozila.naziv + ".");
                    return null;
                }
                aktivnost = builder.build();
                //Baza.getInstance().getKapacitetLokacije(idLokacija, idVozilo).brojVozila--;

                //NajamVozila najam = new NajamVozila(idVozilo.id, idVozilo.naziv, idVozilo.vrijemePunjenja, idVozilo.domet);
                idKorisnik.najamVozila = Baza.getInstance().getKapacitetLokacije(idLokacija, idVozilo).dajVoziloUNajam();
                Baza.getInstance().getIznajmljenaVozila().Add(idKorisnik.najamVozila);
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
            try
            {
                List<Punjenje> temp = new List<Punjenje>();
                foreach (Punjenje p in Baza.getInstance().getVozilaNaPunjenju())
                {
                    if (p.gotovoPunjenje.CompareTo(vrijeme) < 0)
                    {
                        //p.lokacija.brojVozila++; //TODO ??
                        p.vozilo.iznajmljen = false;
                        temp.Add(p);
                    }
                }
                foreach(Punjenje x in temp)
                {
                    Baza.getInstance().getVozilaNaPunjenju().Remove(x);
                }   
                
            }
            catch (InvalidOperationException)
            {
                //TODO hendlaj exception
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
                int slobodnaMjesta = kapacitet.dajBrojSlobodnihMjesta();
                Console.WriteLine("Na lokaciji " + idLokacija.naziv + " ima " + slobodnaMjesta + " slobodnih mjesta za vozilo tipa " + idVozilo.naziv + ".");
                return aktivnost;
            }
            else
            {
                return null;
            }
        }
        public static Aktivnost Vracanje(int idAktivnosti, DateTime vrijeme, Korisnik idKorisnik, Lokacija idLokacija, int brojKm)
        {
            Aktivnost aktivnost = null;
            AktivnostBuilder builder = null;            
            if (idKorisnik.najamVozila == null)
            {
                Console.WriteLine("Korisnik " + idKorisnik.ime + " nema nijedno vozilo u najmu.");
                return null;
            }
            Vozilo idVozilo = Baza.getInstance().getVozilo(idKorisnik.najamVozila.id);
            builder = new AktivnostBuilder(idAktivnosti, vrijeme).setPodatci(idKorisnik, idLokacija, idVozilo).setBrojKm(brojKm);
            if (builder != null)
            {
                ProvjeriPunjenje(vrijeme);
                if (idKorisnik.najamVozila.id != idVozilo.id)
                {
                    Console.WriteLine("Korisnik " + idKorisnik.ime + " nema u najmu vozilo tipa " + idVozilo.naziv);
                    return null;
                }
                LokacijaKapacitet kapacitet = Baza.getInstance().getKapacitetLokacije(idLokacija, idVozilo);
                if (kapacitet.brojMjesta == kapacitet.trenutnaVozila.Count)
                {
                    Console.WriteLine("Na lokaciji " + idLokacija.naziv + " nema slobodnog mjesta za vozilo tipa " + idVozilo.naziv + ".");
                    return null;
                } else
                {
                    Console.WriteLine("Korisnik " + idKorisnik.ime + " vratio je vozilo tipa " + idKorisnik.najamVozila.naziv + " na lokaciju " + idLokacija.naziv + ".");
                    kapacitet.VratiVozilo(idKorisnik.najamVozila);
                    Punjenje punjenje = new Punjenje(idKorisnik.najamVozila, kapacitet, vrijeme);
                    punjenje.IzracunajVrijemePunjenja(brojKm);
                    Baza.getInstance().getVozilaNaPunjenju().Add(punjenje);

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
