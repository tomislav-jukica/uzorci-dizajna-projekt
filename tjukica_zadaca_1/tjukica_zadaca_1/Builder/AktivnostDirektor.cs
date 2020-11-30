﻿using System;
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
                Vozilo v = Baza.getInstance().getKapacitetLokacije(idLokacija, idVozilo).dajVoziloUNajam();
                idKorisnik.IznajmiVozilo(v);
                Baza.getInstance().getIznajmljenaVozila().Add(v);
                cw.Write("Korisnik " + idKorisnik.ime + " unajmio je vozilo tipa " + idVozilo.naziv + " na lokaciji " + idLokacija.naziv + ".", false);
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
        public Aktivnost Vracanje(Korisnik idKorisnik, Lokacija idLokacija, TipVozila tipVozila, int brojKm, string opisProblema = null)
        {
            Aktivnost aktivnost = null;

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
            if (brojKm < idKorisnik.getVoziloUNajmu(tipVozila).kilometri)
            {
                cw.Write("Vrijednost kilometara ne može biti manja od prethodne vrijednosti!");
                return null;
            }
            if (brojKm > tipVozila.domet)
            {
                cw.Write("Vrijednost kilometara ne može biti veća od dometa vozila!");
                return null;
            }

            builder = builder.setPodatci(idKorisnik, idLokacija, tipVozila).setBrojKm(brojKm);
            if (builder != null)
            {
                ProvjeriPunjenje(builder.Vrijeme);
                if (idKorisnik.getVoziloUNajmu(tipVozila) == null)
                {
                    cw.Write("Korisnik " + idKorisnik.ime + " nema u najmu vozilo tipa " + tipVozila.naziv);
                    return null;
                }
                LokacijaKapacitet kapacitet = Baza.getInstance().getKapacitetLokacije(idLokacija, tipVozila);
                if (kapacitet.brojMjesta == kapacitet.trenutnaVozila.Count)
                {
                    cw.Write("Na lokaciji " + idLokacija.naziv + " nema slobodnog mjesta za vozilo tipa " + tipVozila.naziv + ".");
                    return null;
                }
                else
                {
                    Aktivnost stariNajam = null;
                    foreach (Aktivnost a in Baza.getInstance().getAktivnosti())
                    {
                        if (a.IdAktivnosti == 2 && a.Korisnik == idKorisnik && a.Vozilo == tipVozila)
                        {
                            stariNajam = a;
                        }
                    }

                    cw.Write("Korisnik " + idKorisnik.ime + " vratio je vozilo tipa " + tipVozila.naziv + " na lokaciju " + idLokacija.naziv + ".", false);
                    kapacitet.VratiVozilo(idKorisnik.getVoziloUNajmu(tipVozila), builder.Vrijeme, brojKm);


                    DateTime vrijemeNajma = stariNajam.Vrijeme;
                    long razlikaVremena = builder.Vrijeme.Subtract(vrijemeNajma).Ticks;
                    double sati = TimeSpan.FromTicks(razlikaVremena).TotalHours;

                    RacunovodstvoProxy rac = new RacunovodstvoProxy(Racunovodstvo.getInstance());
                    rac.IzdajRacun(stariNajam.Lokacija.id, idLokacija.id, tipVozila.id, sati, brojKm, builder.Vrijeme, idKorisnik.id);

                    if(opisProblema != null)
                    {
                        idKorisnik.brojNeispravnihVracanja++;
                        idKorisnik.getVoziloUNajmu(tipVozila).state.VratiPokvareno();
                        builder = builder.setOpis(opisProblema);
                    }
                    idKorisnik.VratiVozilo(idKorisnik.getVoziloUNajmu(tipVozila));
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
