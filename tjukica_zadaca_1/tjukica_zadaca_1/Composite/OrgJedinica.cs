using System;
using System.Collections.Generic;
using System.Text;
using tjukica_zadaca_1.Composite.Iterator;

namespace tjukica_zadaca_1.Composite
{
    public class OrgJedinica : TvrtkaComponent, IterableCollection
    {

        public string orgJedinicaNaziv { get; private set; }
        List<TvrtkaComponent> tvrtkaComponents = new List<TvrtkaComponent>();
        Baza baza = Baza.getInstance();

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
    }
}
