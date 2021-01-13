using System;
using System.Collections.Generic;
using System.Text;
using tjukica_zadaca_1;
using tjukica_zadaca_1.Proxy;


namespace tjukica_zadaca_1.State
{
    class IznajmljenoState : VoziloState
    {
        public override void Iznajmi(Korisnik korisnik, LokacijaKapacitet lokacija)
        {
            cw.Write("Nije moguće iznajmiti već iznajmljeno vozilo.");
        }

        public override void Napuni()
        {
            cw.Write("Nije moguće puniti bateriju dok je vozilo iznajmljeno.");
        }

        public override void Vrati(Aktivnost.Builder builder)
        {
            Korisnik idKorisnik = builder.Korisnik;
            Composite.Lokacija idLokacija = builder.Lokacija;
            TipVozila tipVozila = builder.Vozilo;
            LokacijaKapacitet kapacitet = Baza.getInstance().getKapacitetLokacije(idLokacija, tipVozila);
            int brojKm = builder.BrojKm;
            string opisProblema = builder.OpisProblema;
            bool greska = false;
            if (idKorisnik.getVoziloUNajmu(tipVozila) == null)
            {
                cw.Write("Korisnik " + idKorisnik.ime + " nema u najmu vozilo tipa " + tipVozila.naziv);
                greska = true;
            }
            /*
            if (kapacitet.brojMjesta == kapacitet.trenutnaVozila.Count)
            {
                cw.Write("Na lokaciji " + idLokacija.naziv + " nema slobodnog mjesta za vozilo tipa " + tipVozila.naziv + ".");
                greska = true;
            }*/
            if(!greska)
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
                //kapacitet.VratiVozilo(idKorisnik.getVoziloUNajmu(tipVozila), builder.Vrijeme, brojKm);

                DateTime vrijemeNajma = stariNajam.Vrijeme;
                long razlikaVremena = builder.Vrijeme.Subtract(vrijemeNajma).Ticks;
                double sati = TimeSpan.FromTicks(razlikaVremena).TotalHours;

                RacunovodstvoProxy rac = new RacunovodstvoProxy(Racunovodstvo.getInstance());
                int prijedeniKilometri = brojKm - builder.Korisnik.getVoziloUNajmu(builder.Vozilo).kilometri; 
                rac.IzdajRacun(stariNajam.Lokacija.id, idLokacija.id, tipVozila.id, sati, prijedeniKilometri, builder.Vrijeme, idKorisnik.id);

                if (opisProblema != null)
                {
                    idKorisnik.brojNeispravnihVracanja++;
                    idKorisnik.getVoziloUNajmu(tipVozila).state.VratiPokvareno();
                    builder.setOpis(opisProblema);
                }
                idKorisnik.VratiVozilo(idKorisnik.getVoziloUNajmu(tipVozila));                
            }
            //TODO provjeri prijedene kilometre
            NaPunjenjuState nps = new NaPunjenjuState(this.vozilo, kapacitet, builder.Vrijeme, builder.BrojKm);

            nps.IzracunajVrijemePunjenja(builder.BrojKm);
            Baza.getInstance().getVozilaNaPunjenju().Add(nps);
            this.vozilo.TransitionTo(nps);

        }

        public override void VratiPokvareno()
        {
            this.vozilo.TransitionTo(new PokvarenoState());
        }
    }
}
