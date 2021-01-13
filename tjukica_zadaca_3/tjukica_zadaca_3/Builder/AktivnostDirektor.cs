using System;
using System.Collections.Generic;
using System.Text;
using tjukica_zadaca_1.Composite;
using tjukica_zadaca_1.State;
using tjukica_zadaca_1.Proxy;

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
                " vozila tipa " + idVozilo.naziv + ".", false);
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
                if (idKorisnik.ImaUNajmu(idVozilo))
                {
                    cw.Write("Korisnik " + idKorisnik.ime + " već ima u najmu vozilo tipa " + idVozilo.naziv + ".");
                    return null;
                }
                aktivnost = builder.build();
                Baza.getInstance().getKapacitetLokacije(idLokacija, idVozilo).dajVoziloUNajam(idKorisnik);
                return aktivnost;
            }
            else
            {
                return null;
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
        public Aktivnost Vracanje(Korisnik idKorisnik, Lokacija idLokacija, TipVozila tipVozila, int ukupniKilometri, string opisProblema = null)
        {
            Aktivnost aktivnost = null;

             //prijedeni kilometri 
            if (idKorisnik.najmovi.Count == 0)
            {
                cw.Write("Korisnik " + idKorisnik.ime + " nema nijedno vozilo u najmu.");
                return null;
            }
            if (idKorisnik.getVoziloUNajmu(tipVozila) == null)
            {
                cw.Write("Korisnik " + idKorisnik.ime + " nema u najmu vozilo tipa " + tipVozila.naziv + ".");
                return null;
            }
            if (ukupniKilometri < idKorisnik.getVoziloUNajmu(tipVozila).kilometri)
            {
                cw.Write("Vrijednost kilometara ne može biti manja od prethodne vrijednosti!");
                return null;
            }
            int brojKm = ukupniKilometri - idKorisnik.getVoziloUNajmu(tipVozila).kilometri;
            if (brojKm > tipVozila.domet)
            {
                cw.Write("Vrijednost kilometara ne može biti veća od dometa vozila!");
                return null;
            }
            builder = builder.setPodatci(idKorisnik, idLokacija, tipVozila).setBrojKm(ukupniKilometri);
            if (builder != null)
            {
                ProvjeriPunjenje(builder.Vrijeme);
                LokacijaKapacitet kapacitet = Baza.getInstance().getKapacitetLokacije(idLokacija, tipVozila);
                kapacitet.VratiVozilo(builder);
                aktivnost = builder.build();
                return aktivnost;

                

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

        private static void ProvjeriPunjenje(DateTime vrijeme)
        {
            try
            {
                List<NaPunjenjuState> temp = new List<NaPunjenjuState>();
                foreach (NaPunjenjuState p in Baza.getInstance().getVozilaNaPunjenju())
                {
                    if (p.gotovoPunjenje.CompareTo(vrijeme) < 0)
                    {
                        //p.vozilo.TransitionTo(new SlobodnoState());
                        p.vozilo.state.Napuni();
                        temp.Add(p);
                    }
                }
                foreach (NaPunjenjuState x in temp)
                {
                    Baza.getInstance().getVozilaNaPunjenju().Remove(x);
                }

            }
            catch (InvalidOperationException)
            {
            }
        }
    }
}
