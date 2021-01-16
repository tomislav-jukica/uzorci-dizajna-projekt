using System;
using System.Collections.Generic;
using System.Text;
using tjukica_zadaca_1.Proxy;

namespace tjukica_zadaca_1.Composite
{
    public abstract class TvrtkaComponent
    {
        public int id;
        public TvrtkaComponent nadredeni;
        public int razina = 0;
        public bool orgJedinica = false;
        protected Baza baza = Baza.getInstance();
        public TvrtkaComponent(int componentId, TvrtkaComponent nadred)
        {
            id = componentId;
            nadredeni = nadred;
        }
        public abstract TvrtkaComponent getParentComponent();
        public abstract TvrtkaComponent getComponent();
        public abstract List<TvrtkaComponent> getChildrenComponents();
        public abstract string getComponentName();
        public abstract int getRazina();
        public abstract int DajSlobodnaMjesta(TipVozila tipVozila);
        public abstract int DajSlobodnaVozila(TipVozila tipVozila);
        public abstract int DajPokvarenaVozila(TipVozila tipVozila);
        public abstract decimal DajZaradu(TipVozila tipVozila, DateTime datum1, DateTime datum2);
        public abstract int DajNajmove(TipVozila tipVozila, DateTime datum1, DateTime datum2);
        public abstract List<Racun> DajRacune(TipVozila tipVozila, DateTime datum1, DateTime datum2);
    }
}
