using System;
using System.Collections.Generic;
using System.Text;
using tjukica_zadaca_1.Composite;
using tjukica_zadaca_1.State;

namespace tjukica_zadaca_1
{
    public class Baza
    {
        private List<Korisnik> korisnici = new List<Korisnik>();
        private List<Lokacija> lokacije = new List<Lokacija>();
        private List<TipVozila> tipoviVozila = new List<TipVozila>();
        private List<Cjenik> cjenik = new List<Cjenik>();
        private List<LokacijaKapacitet> kapacitetiLokacija = new List<LokacijaKapacitet>();
        private DateTime virtualnoVrijeme;
        private List<Aktivnost> aktivnosti = new List<Aktivnost>();
        private List<Vozilo> vozilaZaNajam = new List<Vozilo>();
        private List<NaPunjenjuState> vozilaNaPunjenju = new List<NaPunjenjuState>();
        private List<Vozilo> iznajmljenaVozila = new List<Vozilo>();
        private List<TvrtkaComponent> sveOrgJedinice = new List<TvrtkaComponent>();
        public int dt = 30;
        public int dc = 5;
        public int dd = 2;
        public List<Lokacija> lokacijeZaDodjelu { get; set; } = new List<Lokacija>();

        public OrgJedinica ishodisna { get; set; } = null;
        private static Baza baza = null;


        private Baza(){} //konstruktor

        public static Baza getInstance()
        {
            if(baza == null)
            {
                baza = new Baza();
            }
            return baza;
        }


        public bool UsporediVrijeme(DateTime vrijeme)
        {
            if(vrijeme.CompareTo(baza.virtualnoVrijeme) > 0)
            {
                return true;
            } else
            {
                return false;
            }
        }


        public DateTime getVirtualnoVrijeme()
        {
            return baza.virtualnoVrijeme;
        }
        public void setVirtualnoVrijeme(DateTime vrijeme)
        {
            baza.virtualnoVrijeme = vrijeme;
        }

        public Korisnik getKorisnik(int id)
        {
            Korisnik korisnik = null;
            foreach (Korisnik k in baza.korisnici)
            {
                if(k.id == id)
                {
                    korisnik = k;
                }
            }
            return korisnik;
        }

        public Lokacija getLokacija(int id)
        {
            Lokacija lokacija = null;
            foreach(Lokacija l in baza.lokacije)
            {
                if(l.id == id)
                {
                    lokacija = l;
                }
            }
            return lokacija;
        }
        public TipVozila getVozilo(int id)
        {
            TipVozila vozilo = null;
            foreach(TipVozila v in baza.tipoviVozila)
            {
                if(v.id == id)
                {
                    vozilo = v;
                }
            }
            return vozilo;
        }
        public LokacijaKapacitet getKapacitetLokacije(Lokacija lokacija, TipVozila vozilo)
        {
            LokacijaKapacitet retVal = null;
            if (lokacija != null && vozilo != null)
            {
                foreach (LokacijaKapacitet x in baza.kapacitetiLokacija)
                {
                    if (x.lokacija.id == lokacija.id && x.tipVozila.id == vozilo.id)
                    {
                        retVal = x;
                    }
                } 
            }
            return retVal;
        }

        public List<Korisnik> getKorisnici()
        {
            return baza.korisnici;
        }
        public List<Lokacija> getLokacije()
        {
            return baza.lokacije;
        }
        public List<TipVozila> getTipoviVozila()
        {
            return baza.tipoviVozila;
        }
        public List<Cjenik> getCjenik()
        {
            return baza.cjenik;
        }
        public Cjenik getCjenikZaVozilo (TipVozila vozilo)
        {
            foreach(Cjenik c in cjenik)
            {
                if(c.tipVozila == vozilo)
                {
                    return c;
                }
            }
            return null;
        }
        public List<LokacijaKapacitet> getLokacijaKapacitet()
        {
            return baza.kapacitetiLokacija;
        }
        public List<Aktivnost> getAktivnosti()
        {
            return baza.aktivnosti;
        }
        public List<Vozilo> getVozilaZaNajam()
        {
            return baza.vozilaZaNajam;
        }
        public Vozilo getNajamVozila(TipVozila vozilo)
        {
            Vozilo najam = null;
            foreach (Vozilo n in baza.vozilaZaNajam)
            {
                if(vozilo.id == n.id && n.state.GetType() == new SlobodnoState().GetType())
                {
                    najam = n;
                }
            }
            return najam;
        }

        public List<NaPunjenjuState> getVozilaNaPunjenju()
        {
            return baza.vozilaNaPunjenju;
        }
        public List<Vozilo> getIznajmljenaVozila()
        {
            return baza.iznajmljenaVozila;
        }

        public int brojVozilaKojaSePune(LokacijaKapacitet lokacija)
        {
            int retVal = 0;
            foreach (NaPunjenjuState v in baza.vozilaNaPunjenju)
            {
                if(v.lokacija == lokacija)
                {
                    retVal++;
                }
            }
            return retVal;
        }

        public List<TvrtkaComponent> getSveOrgJedinice()
        {
            return sveOrgJedinice;
        }

        public void DodajDijeteRoditelju(OrgJedinica dijete, int idRoditelja)
        {
            foreach (OrgJedinica o in sveOrgJedinice)
            {
                if(o.id == idRoditelja)
                {
                    o.getChildrenComponents().Add(dijete);
                }
            }
        }

        public bool PostojiOrgJedinica(int idOrgJedinice)
        {
            foreach (var o in sveOrgJedinice)
            {
                if(o.id == idOrgJedinice)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
