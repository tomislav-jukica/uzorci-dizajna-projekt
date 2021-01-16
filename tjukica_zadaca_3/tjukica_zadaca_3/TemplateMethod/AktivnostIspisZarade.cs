using System;
using System.Collections.Generic;
using System.Text;
using tjukica_zadaca_1.Composite.Iterator;
using tjukica_zadaca_1.Composite;

namespace tjukica_zadaca_1.TemplateMethod
{
    class AktivnostIspisZarade : AktivnostIspisa
    {
        string komanda_2;
        string komanda_3;
        public AktivnostIspisZarade(int aktivnost, string komanda1, DateTime datum1, DateTime datum2, string komanda2 = "", string komanda3 = "") : 
            base(aktivnost, komanda1, datum1, datum2)
        {
            this.komanda_2 = komanda2;
            this.komanda_3 = komanda3;
        }

        public void PrikaziNajam(int idOrgJedinice)
        {
            Iterator iterator = baza.ishodisna.GetIterator();
            if (baza.ishodisna.id == idOrgJedinice)
            {
                IspisNajma(iterator.DFS());
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
                            IspisNajma(iterator
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

        public void PrikaziZarada(int idOrgJedinice)
        {
            Iterator iterator = baza.ishodisna.GetIterator();
            if (baza.ishodisna.id == idOrgJedinice)
            {
                IspisZarade(iterator.DFS());
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
                            IspisZarade(iterator
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

        public void PrikaziPodatke(int idOrgJedinice)
        {
            Iterator iterator = baza.ishodisna.GetIterator();
            if (baza.ishodisna.id == idOrgJedinice)
            {
                IspisPodataka(iterator.DFS());
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
                            IspisPodataka(iterator
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

        private void IspisZarade(List<TvrtkaComponent> lista)
        {
            cw.ispisZarade(lista, datum_1, datum_2);
        }
        private void IspisNajma(List<TvrtkaComponent> lista)
        {
            cw.ispisNajma(lista, datum_1, datum_2);
        }
        private void IspisPodataka(List<TvrtkaComponent> lista)
        {
            cw.ispisPodataka(lista, datum_1, datum_2);
        }
    }
}
