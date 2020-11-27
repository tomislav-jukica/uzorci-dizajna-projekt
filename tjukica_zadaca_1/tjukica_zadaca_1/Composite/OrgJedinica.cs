using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1.Composite
{
    public class OrgJedinica : TvrtkaComponent
    {

        public string orgJedinicaNaziv { get; private set; }
        TvrtkaComponent nadredenaJedinica;
        List<TvrtkaComponent> tvrtkaComponents = new List<TvrtkaComponent>();
        Baza baza = Baza.getInstance();

        public OrgJedinica(int id, string naziv, TvrtkaComponent nadredena, List<TvrtkaComponent> lokacije): base(id, nadredena)
        {            
            orgJedinicaNaziv = naziv;
            nadredenaJedinica = nadredena;
            tvrtkaComponents = lokacije;
        }

        public void Add(TvrtkaComponent newTvrtkaComponent)
        {
            tvrtkaComponents.Add(newTvrtkaComponent);
        }

        public void Remove(TvrtkaComponent tvrtkaComponent)
        {
            tvrtkaComponents.Remove(tvrtkaComponent);
        }

        public List<TvrtkaComponent> getChildrenComponents()
        {
            return tvrtkaComponents;
        }

        public override TvrtkaComponent getComponent(int id)
        {
            throw new NotImplementedException();
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
            return nadredenaJedinica;
        }
    }
}
