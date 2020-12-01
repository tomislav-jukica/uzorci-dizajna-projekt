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
            Console.WriteLine("");
            Console.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, -" + baza.dt + "} {4, -" + baza.dt + "}" +
                "{5, " + baza.dc + "} {6, -" + baza.dt + "} {7, " + baza.dc + "} {8, -" + baza.dt + "}" +
                "{9, " + (baza.dc + baza.dd + 1) + "} {10, " + (baza.dc + baza.dd + 1) + "} {11, " + (baza.dc + baza.dd + 1) + "}\n", 
                "Naziv", "Vozilo", "Rb.", "Vrijeme", "Korisnik","ID","L. Najma", "ID", "L. Vracanja", "C. Najma", "C. Km", "C. Sat");
            for (int ctr = 0; ctr < lista.Count; ctr++)
            {
                foreach (var tipVozila in baza.getTipoviVozila())
                {
                    string name = lista[ctr].getComponentName();
                    string voziloName = tipVozila.naziv;

                    if (name.Length > baza.dt) name = name.Substring(0, baza.dt);
                    if (voziloName.Length > baza.dt) voziloName = voziloName.Substring(0, baza.dt);

                    foreach (var x in lista[ctr].DajRacune(tipVozila, datum_1, datum_2))
                    {
                        Console.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, -" + baza.dt + "} {4, -" + baza.dt + "}" +
                "{5, " + baza.dc + "} {6, -" + baza.dt + "} {7, " + baza.dc + "} {8, -" + baza.dt + "}" +
                "{9, " + (baza.dc + baza.dd + 1) + "} {10, " + (baza.dc + baza.dd + 1) + "} {11, " + (baza.dc + baza.dd + 1) + "}",
                        name,
                        voziloName,
                        x.id,
                        x.datumIzdavanja,
                        baza.getKorisnik(x.idKorisnik).ime,
                        x.idLokacijeNajma, baza.getLokacija(x.idLokacijeNajma).naziv,
                        x.idLokacijeVracanja, baza.getLokacija(x.idLokacijeVracanja).naziv,
                        Math.Round(baza.getCjenikZaVozilo(tipVozila).najam, baza.dd, MidpointRounding.AwayFromZero),
                        Math.Round(baza.getCjenikZaVozilo(tipVozila).cijenaKm * x.brojKm, baza.dd, MidpointRounding.AwayFromZero),
                        Math.Round(baza.getCjenikZaVozilo(tipVozila).cijenaSat * x.brojSati, baza.dd, MidpointRounding.AwayFromZero),
                        lista[ctr].DajNajmove(tipVozila, datum_1, datum_2),
                        lista[ctr].DajZaradu(tipVozila, datum_1, datum_2));
                    }
                    
                }
            }
            Console.WriteLine("");
        }

    }
}
