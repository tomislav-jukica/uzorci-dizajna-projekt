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
            Console.WriteLine("");
            Console.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "}\n", "Naziv", "Vozilo", "Zarada");
            for (int ctr = 0; ctr < lista.Count; ctr++)
            {
                foreach (var tipVozila in baza.getTipoviVozila())
                {
                    string name = lista[ctr].getComponentName();
                    string voziloName = tipVozila.naziv;

                    if (name.Length > baza.dt) name = name.Substring(0, baza.dt);
                    if (voziloName.Length > baza.dt) voziloName = voziloName.Substring(0, baza.dt);

                    Console.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "}",
                        name,
                        voziloName,
                        lista[ctr].DajZaradu(tipVozila, datum_1, datum_2));
                }
            }
            Console.WriteLine("");
        }
        private void IspisNajma(List<TvrtkaComponent> lista)
        {
            Console.WriteLine("");
            Console.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "}\n", "Naziv", "Vozilo", "Najam");
            for (int ctr = 0; ctr < lista.Count; ctr++)
            {
                foreach (var tipVozila in baza.getTipoviVozila())
                {
                    string name = lista[ctr].getComponentName();
                    string voziloName = tipVozila.naziv;

                    if (name.Length > baza.dt) name = name.Substring(0, baza.dt);
                    if (voziloName.Length > baza.dt) voziloName = voziloName.Substring(0, baza.dt);

                    Console.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "}",
                        name,
                        voziloName,
                        lista[ctr].DajNajmove(tipVozila, datum_1, datum_2));
                }
            }
            Console.WriteLine("");
        }
        private void IspisPodataka(List<TvrtkaComponent> lista)
        {
            Console.WriteLine("");
            Console.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, " + baza.dc + "}\n", "Naziv", "Vozilo", "Najam", "Zarada");
            for (int ctr = 0; ctr < lista.Count; ctr++)
            {
                foreach (var tipVozila in baza.getTipoviVozila())
                {
                    string name = lista[ctr].getComponentName();
                    string voziloName = tipVozila.naziv;

                    if (name.Length > baza.dt) name = name.Substring(0, baza.dt);
                    if (voziloName.Length > baza.dt) voziloName = voziloName.Substring(0, baza.dt);

                    Console.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, " + baza.dc + "}",
                        name,
                        voziloName,
                        lista[ctr].DajNajmove(tipVozila, datum_1, datum_2),
                        lista[ctr].DajZaradu(tipVozila,datum_1, datum_2));
                }
            }
            Console.WriteLine("");
        }
    }
}
