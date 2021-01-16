using System;
using System.Collections.Generic;
using System.Text;
using tjukica_zadaca_1.Composite.Iterator;
using tjukica_zadaca_1.Composite;

namespace tjukica_zadaca_1.TemplateMethod
{
    class AktivnostIspisRacuna : AktivnostIspisa
    {
        string komanda_2;

        public AktivnostIspisRacuna(int aktivnost, string komanda1, DateTime datum1, DateTime datum2, string komanda2 = "") 
            : base(aktivnost, komanda1, datum1, datum2)
        {
            this.komanda_2 = komanda2;
        }

        public void PrikaziRacune(int idOrgJedinice)
        {
            Iterator iterator = baza.ishodisna.GetIterator();
            if (baza.ishodisna.id == idOrgJedinice)
            {
                IspisiRacune(iterator.DFS());
            }
            else
            {
                bool flag = true;
                iterator.DFS();
                while (flag)
                {
                    if (iterator.Current().orgJedinica)
                    {
                        if (iterator.Current().id == idOrgJedinice)
                        {
                            IspisiRacune(iterator
                                    .DFS(new List<TvrtkaComponent>(iterator.Current().getChildrenComponents())));
                            iterator.MoveNext();
                            flag = false;
                        }
                    }
                    iterator.MoveNext();
                    if (iterator.IsEnd())
                    {
                        cw.Write("Ne postoji organizacijska jedinica sa ID-jem: " + idOrgJedinice);
                        flag = false;
                    }
                }
            }
        }

        private void IspisiRacune(List<TvrtkaComponent> lista)
        {
            cw.ispisiRacune(lista, datum_1, datum_2);
        }

    }
}
