using System;
using System.Collections.Generic;
using System.Text;
using tjukica_zadaca_1.Composite.Iterator;
using tjukica_zadaca_1.Proxy;

namespace tjukica_zadaca_1.Composite
{
    public class OrgJedinica : TvrtkaComponent, IterableCollection
    {

        public string orgJedinicaNaziv { get; private set; }
        List<TvrtkaComponent> tvrtkaComponents = new List<TvrtkaComponent>();

        public OrgJedinica(int id, string naziv, TvrtkaComponent nadredena, List<TvrtkaComponent> lokacije): base(id, nadredena)
        {            
            orgJedinicaNaziv = naziv;
            nadredeni = nadredena;
            tvrtkaComponents = lokacije;
            if(nadredena != null) this.razina = nadredeni.razina + 1;

        }

        public Iterator.Iterator GetIterator()
        {
            return new ConcreteIterator(this);
        }

        public void Add(TvrtkaComponent newTvrtkaComponent)
        {
            tvrtkaComponents.Add(newTvrtkaComponent);
        }

        public void Remove(TvrtkaComponent tvrtkaComponent)
        {
            tvrtkaComponents.Remove(tvrtkaComponent);
        }

        public override List<TvrtkaComponent> getChildrenComponents()
        {
            return tvrtkaComponents;
        }

        public override TvrtkaComponent getComponent()
        {
            return this;
        }

        public override string getComponentName()
        {
            return orgJedinicaNaziv;
        }

        public override TvrtkaComponent getParentComponent()
        {
            if (nadredeni == null && baza.ishodisna == this)
            {
                return this;
            }
            return nadredeni;
        }

        public override int getRazina()
        {
            return razina;
        }

        public override int DajSlobodnaMjesta(TipVozila tipVozila)
        {
            int retVal = 0;
            foreach (var item in this.getChildrenComponents())
            {
                retVal += item.DajSlobodnaMjesta(tipVozila);
               
            }
            return retVal;
        }

        public override int DajSlobodnaVozila(TipVozila tipVozila)
        {
            int retVal = 0;
            foreach (var item in this.getChildrenComponents())
            {
                retVal += item.DajSlobodnaVozila(tipVozila);

            }
            return retVal;
        }

        public override int DajPokvarenaVozila(TipVozila tipVozila)
        {
            int retVal = 0;
            foreach (var item in this.getChildrenComponents())
            {
                retVal += item.DajPokvarenaVozila(tipVozila);

            }
            return retVal;
        }

        public override decimal DajZaradu(TipVozila tipVozila, DateTime datum1, DateTime datum2)
        {
            decimal retVal = 0;
            foreach (var item in tvrtkaComponents)
            {
                retVal += item.DajZaradu(tipVozila, datum1, datum2);
            }
            return retVal;
        }

        public override int DajNajmove(TipVozila tipVozila, DateTime datum1, DateTime datum2)
        {
            int retVal = 0;
            foreach (var item in tvrtkaComponents)
            {
                retVal += item.DajNajmove(tipVozila, datum1, datum2);
            }
            return retVal;
        }

        public override List<Racun> DajRacune(TipVozila tipVozila, DateTime datum1, DateTime datum2)
        {
            List<Racun> retVal = new List<Racun>();
            foreach (var item in tvrtkaComponents)
            {
                retVal.AddRange(item.DajRacune(tipVozila, datum1, datum2));
            }
            return retVal;
        }

        internal override int DajVremenaNajmova(TipVozila tipVozila, DateTime datum_1, DateTime datum_2)
        {
            int retVal = 0;
            foreach (var item in tvrtkaComponents)
            {
                retVal += item.DajVremenaNajmova(tipVozila, datum_1, datum_2);
            }
            return retVal;
        }
    }
}
