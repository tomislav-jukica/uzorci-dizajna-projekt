using System;
using System.Collections.Generic;
using System.Text;
using tjukica_zadaca_1.Proxy;
using tjukica_zadaca_1.State;

namespace tjukica_zadaca_1.Composite
{
    public class Lokacija : TvrtkaComponent
    {
        public string naziv { get; private set; }
        private string adresa;
        private double geoSirina;
        private double geoDuzina;
        public double zarada = 0;

        public Lokacija(int id, string naziv, string adresa, string koordinata, TvrtkaComponent nadredeni) : base(id, nadredeni)
        {
            string[] temp = Array.ConvertAll(koordinata.Split(","), p => p.Trim());
            this.naziv = naziv;
            this.adresa = adresa;
            this.geoSirina = double.Parse(temp[0], System.Globalization.CultureInfo.InvariantCulture);
            this.geoDuzina = double.Parse(temp[1], System.Globalization.CultureInfo.InvariantCulture);
            this.nadredeni = nadredeni;
            //this.razina = nadredeni.razina + 1;
        }

        public override TvrtkaComponent getComponent()
        {
            return this;
        }

        public override string getComponentName()
        {
            return naziv;
        }

        public override TvrtkaComponent getParentComponent()
        {
            return nadredeni;
        }

        public override List<TvrtkaComponent> getChildrenComponents()
        {
            return new List<TvrtkaComponent>();
        }

        public override int getRazina()
        {
            return razina;
        }

        public override int DajSlobodnaMjesta(TipVozila tipVozila)
        {
            return baza.getKapacitetLokacije(this, tipVozila).dajBrojSlobodnihMjesta();
        }

        public override int DajSlobodnaVozila(TipVozila tipVozila)
        {
            return baza.getKapacitetLokacije(this, tipVozila).dajBrojSlobodnihVozila();
        }

        public override int DajPokvarenaVozila(TipVozila tipVozila)
        {
            return baza.getKapacitetLokacije(this, tipVozila).dajBrojPokvarenihVozila();
        }

        public override decimal DajZaradu(TipVozila tipVozila, DateTime datum1, DateTime datum2)
        {
            decimal retVal = 0;
            RacunovodstvoProxy rac = new RacunovodstvoProxy(Racunovodstvo.getInstance());
            List<Racun> racuni = rac.GetRacuni();
            foreach (Racun racun in racuni)
            {
                if (racun.idLokacijeNajma == this.id)
                {
                    if (racun.idVoziloTip == tipVozila.id)
                    {
                        if (datum1.CompareTo(racun.datumIzdavanja) < 0 && datum2.CompareTo(racun.datumIzdavanja) > 0)
                        {
                            retVal += racun.ukupno;
                        }  
                    }
                }
            }
            return retVal;
        }

        public override int DajNajmove(TipVozila tipVozila, DateTime datum1, DateTime datum2)
        {
            int retVal = 0;
            RacunovodstvoProxy rac = new RacunovodstvoProxy(Racunovodstvo.getInstance());
            List<Racun> racuni = rac.GetRacuni();

            foreach (Racun racun in racuni)
            {
                if (racun.idLokacijeNajma == this.id)
                {
                    if (racun.idVoziloTip == tipVozila.id)
                    {
                        if (datum1.CompareTo(racun.datumIzdavanja) < 0 && datum2.CompareTo(racun.datumIzdavanja) > 0)
                        {
                            retVal += 1;
                        }
                    }
                }
            }
            return retVal;
        }

        public override List<Racun> DajRacune(TipVozila tipVozila, DateTime datum1, DateTime datum2)
        {
            List<Racun> retVal = new List<Racun>();
            RacunovodstvoProxy rac = new RacunovodstvoProxy(Racunovodstvo.getInstance());
            List<Racun> racuni = rac.GetRacuni();

            foreach (Racun racun in racuni)
            {
                if (racun.idLokacijeNajma == this.id)
                {
                    if (racun.idVoziloTip == tipVozila.id)
                    {
                        if (datum1.CompareTo(racun.datumIzdavanja) < 0 && datum2.CompareTo(racun.datumIzdavanja) > 0)
                        {
                            retVal.Add(racun);
                        }
                    }
                }
            }
            return retVal;
        }

        internal override int DajVremenaNajmova(TipVozila tipVozila, DateTime datum_1, DateTime datum_2)
        {
            int retVal = 0;
            RacunovodstvoProxy rac = new RacunovodstvoProxy(Racunovodstvo.getInstance());
            List<Racun> racuni = rac.GetRacuni();            

            foreach (Racun racun in racuni)
            {
                if (racun.idLokacijeNajma == this.id)
                {
                    if (racun.idVoziloTip == tipVozila.id)
                    {
                        if (datum_1.CompareTo(racun.datumIzdavanja) < 0 && datum_2.CompareTo(racun.datumIzdavanja) > 0)
                        {
                            double vrijeme = racun.brojSati;
                            //int t =(int) Math.Ceiling(vrijeme.TotalMinutes);
                            retVal += (int)Math.Ceiling(vrijeme);
                        }
                    }
                }
            }
            return retVal;
        }
    }
}
