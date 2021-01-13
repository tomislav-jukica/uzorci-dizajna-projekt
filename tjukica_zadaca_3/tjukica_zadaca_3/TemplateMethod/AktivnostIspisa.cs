using System;
using System.Collections.Generic;
using System.Text;
using tjukica_zadaca_1.Composite.Iterator;
using tjukica_zadaca_1.Helpers;
using tjukica_zadaca_1.Composite;

namespace tjukica_zadaca_1.TemplateMethod
{
    public abstract class AktivnostIspisa
    {
        protected static Baza baza = Baza.getInstance();
        protected Iterator iterator = baza.ishodisna.GetIterator();
        protected ConsoleWriter cw = ConsoleWriter.getInstance();

        protected int idAktivnosti;
        protected DateTime datum_1 = new DateTime();
        protected DateTime datum_2 = new DateTime();
        protected string komanda_1 = "";
        protected int idOrgJedinice = baza.ishodisna.id;
        //iterator.DFS(new List<TvrtkaComponent>(iterator.Current().getChildrenComponents()))

        public AktivnostIspisa(int aktivnost, string komanda1, DateTime datum1, DateTime datum2)
        {
            this.idAktivnosti = aktivnost;
            this.komanda_1 = komanda1;
            this.datum_1 = datum1;
            this.datum_2 = datum2;
        }

        public void PrikaziStrukturu(int idOrgJedinice)
        {
            Iterator iterator = baza.ishodisna.GetIterator();
            if (baza.ishodisna.id == idOrgJedinice)
            {
                IspisStrukture(iterator.DFS());
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
                            IspisStrukture(iterator
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

        private void IspisStrukture(List<TvrtkaComponent> lista)
        {
            Console.WriteLine("");
            Console.WriteLine("{0,10}\n", "Naziv");
            for (int ctr = 0; ctr < lista.Count; ctr++)
            {
                string razinaIcon = "";
                for (int i = 0; i < lista[ctr].razina; i++)
                {
                    razinaIcon += "-";
                }

                Console.WriteLine("{0,-20}", razinaIcon + "  " + lista[ctr].getComponentName());
            }
            Console.WriteLine("");
        }

    }
}
