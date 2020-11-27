using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    class AktivnostDirektor
    {
        private Aktivnost.Builder builder;

        Helpers.ConsoleWriter cw = Helpers.ConsoleWriter.getInstance();
        public AktivnostDirektor(Aktivnost.Builder builder)
        {
            this.builder = builder;
        }

        public Aktivnost PregledVozila(Korisnik idKorisnik, Lokacija idLokacija, TipVozila idVozilo)
        {
            Aktivnost aktivnost = null;
            builder = builder.setPodatci(idKorisnik, idLokacija, idVozilo);
            if (builder != null)
            {
                ProvjeriPunjenje(builder.Vrijeme);
                aktivnost = builder.build();
                LokacijaKapacitet kapacitet = Baza.getInstance().getKapacitetLokacije(idLokacija, idVozilo);
                int brojVozila = kapacitet.dajBrojSlobodnihVozila();
                cw.Write("Na lokaciji " + idLokacija.naziv +
                " trenutno se nalazi " + brojVozila +
                " vozila tipa " + idVozilo.naziv + ".",false);
                return aktivnost;
            }
            else
            {
                return null;
            }
        }
        public Aktivnost Najam(Korisnik idKorisnik, Lokacija idLokacija, TipVozila idVozilo)
        {
            Aktivnost aktivnost = null;
            builder = builder.setPodatci(idKorisnik, idLokacija, idVozilo);
            if (builder != null)
            {
                ProvjeriPunjenje(builder.Vrijeme);
                if (Baza.getInstance().getKapacitetLokacije(idLokacija, idVozilo).dajBrojSlobodnihVozila() < 1)
                {
                    cw.Write("Na lokaciji " + idLokacija.naziv + " nema slobodnih vozila tipa " + idVozilo.naziv + ".");
                    return null;
                }
                if (idKorisnik.najamVozila != null) //TODO najam više vozila
                {
                    cw.Write("Korisnik " + idKorisnik.ime + " već ima u najmu vozilo tipa " + idKorisnik.najamVozila.naziv + ".");
                    return null;
                }
                aktivnost = builder.build();
                idKorisnik.najamVozila = Baza.getInstance().getKapacitetLokacije(idLokacija, idVozilo).dajVoziloUNajam();
                Baza.getInstance().getIznajmljenaVozila().Add(idKorisnik.najamVozila);
                cw.Write("Korisnik " + idKorisnik.ime + " unajmio je vozilo tipa " + idVozilo.naziv + " na lokaciji " + idLokacija.naziv + ".", false);
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
            }
        }

        public Aktivnost PregledMjesta(Korisnik idKorisnik, Lokacija idLokacija, TipVozila idVozilo)
        {
            Aktivnost aktivnost = null;
            builder = builder.setPodatci(idKorisnik, idLokacija, idVozilo);
            if (builder != null)
            {
                ProvjeriPunjenje(builder.Vrijeme);
                aktivnost = builder.build();
                LokacijaKapacitet kapacitet = Baza.getInstance().getKapacitetLokacije(idLokacija, idVozilo);
                int slobodnaMjesta = kapacitet.dajBrojSlobodnihMjesta();
                cw.Write("Na lokaciji " + idLokacija.naziv + " ima " + slobodnaMjesta + " slobodnih mjesta za vozilo tipa " + idVozilo.naziv + ".", false);
                return aktivnost;
            }
            else
            {
                return null;
            }
        }
        public Aktivnost Vracanje(Korisnik idKorisnik, Lokacija idLokacija, int brojKm)
        {
            Aktivnost aktivnost = null;
            
            if (idKorisnik.najamVozila == null) //TODO promijeni 
            {
                cw.Write("Korisnik " + idKorisnik.ime + " nema nijedno vozilo u najmu.");
                return null;
            }
            TipVozila idVozilo = Baza.getInstance().getVozilo(idKorisnik.najamVozila.id);
            if (brojKm < idKorisnik.najamVozila.kilometri)
            {
                cw.Write("Vrijednost kilometara ne može biti manja od prethodne vrijednosti!");
                return null;
            }
            if(brojKm > idVozilo.domet)
            {
                cw.Write("Vrijednost kilometara ne može biti veća od dometa vozila!");
                return null;
            }
            
            builder = builder.setPodatci(idKorisnik, idLokacija, idVozilo).setBrojKm(brojKm);
            if (builder != null)
            {
                ProvjeriPunjenje(builder.Vrijeme);
                if (idKorisnik.najamVozila.id != idVozilo.id)//TODO promijeni
                {
                    cw.Write("Korisnik " + idKorisnik.ime + " nema u najmu vozilo tipa " + idVozilo.naziv);
                    return null;
                }
                LokacijaKapacitet kapacitet = Baza.getInstance().getKapacitetLokacije(idLokacija, idVozilo);
                if (kapacitet.brojMjesta == kapacitet.trenutnaVozila.Count)
                {
                    cw.Write("Na lokaciji " + idLokacija.naziv + " nema slobodnog mjesta za vozilo tipa " + idVozilo.naziv + ".");
                    return null;
                } else
                {
                    Aktivnost stariNajam = null;
                    foreach (Aktivnost a in Baza.getInstance().getAktivnosti())
                    {
                        if (a.IdAktivnosti == 2 && a.Korisnik == idKorisnik && a.Vozilo == idVozilo)
                        {
                            stariNajam = a;
                        }
                    }

                    cw.Write("Korisnik " + idKorisnik.ime + " vratio je vozilo tipa " + idKorisnik.najamVozila.naziv + " na lokaciju " + idLokacija.naziv + ".", false);
                    kapacitet.VratiVozilo(idKorisnik.najamVozila);
                    Punjenje punjenje = new Punjenje(idKorisnik.najamVozila, kapacitet, builder.Vrijeme);



                    int prijedeniKilometri = brojKm - idKorisnik.najamVozila.kilometri;
                    punjenje.IzracunajVrijemePunjenja(prijedeniKilometri);
                    Baza.getInstance().getVozilaNaPunjenju().Add(punjenje);

                    float cijenaNajma = Baza.getInstance().getCjenikZaVozilo(idVozilo).najam;
                    DateTime vrijemeNajma = stariNajam.Vrijeme;                 
                    
                    long razlikaVremena = builder.Vrijeme.Subtract(vrijemeNajma).Ticks;
                    double sati = TimeSpan.FromTicks(razlikaVremena).TotalHours;
                    float cijenaSati = Baza.getInstance().getCjenikZaVozilo(idVozilo).cijenaSat * (int)Math.Ceiling(sati);
                    float cijenaKilometri = Baza.getInstance().getCjenikZaVozilo(idVozilo).cijenaKm * prijedeniKilometri;
                    //TODO napravi račun
                    Console.WriteLine("Račun:");
                    Console.WriteLine("1. Najam - " + cijenaNajma + " KN");
                    Console.WriteLine("2. Po satu - " + (int)Math.Ceiling(sati) + " sata = " + cijenaSati + " KN");
                    Console.WriteLine("3. Po km - " + prijedeniKilometri + " kilometara  = " + cijenaKilometri + " KN");
                    Console.WriteLine("UKUPNO: " + (cijenaNajma + cijenaSati + cijenaKilometri) + " KN");
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
        public Aktivnost Kraj()
        {
            return builder.build();
        }
    }
}
