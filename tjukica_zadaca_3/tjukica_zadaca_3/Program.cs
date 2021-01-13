using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using tjukica_zadaca_1.Composite;
using tjukica_zadaca_1.Composite.Iterator;
using tjukica_zadaca_1.State;
using tjukica_zadaca_1.TemplateMethod;

namespace tjukica_zadaca_1
{
    class Program
    {
        private static Regex REGEX_VRIJEME = new Regex("(.(\\d+-\\d+-\\d+ \\d+:\\d+:\\d+))");
        private static Regex REGEX_KRAJ = new Regex("(\\d); .(\\d+-\\d+-\\d+ \\d+:\\d+:\\d+).");
        private static Regex REGEX_PODATCI = new Regex("(\\d); .(\\d+-\\d+-\\d+ \\d+:\\d+:\\d+).; (\\d+); (\\d+); (\\d+)");
        private static Regex REGEX_VRACANJE = new Regex("(\\d); .(\\d+-\\d+-\\d+ \\d+:\\d+:\\d+).; (\\d+); (\\d+); (\\d+); (\\d+)");
        private static Regex REGEX_VRACANJE_NEISPRAVNO = new Regex("(\\d); .(\\d+-\\d+-\\d+ \\d+:\\d+:\\d+).; (\\d+); (\\d+); (\\d+); (\\d+); (.+)");
        private static Regex REGEX_SKUPNI = new Regex("(5); (.+\\.txt)");
        private static Regex REGEX_ISPIS_STANJE = new Regex("(\\d); (struktura|stanje)? (struktura|stanje)?\\s?(\\d*)");
        private static Regex REGEX_AKTIVNOSTI_ISPISA = new Regex("(\\d); ([\\w\\s]+) (\\d{2}.\\d{2}.\\d{4}) (\\d{2}.\\d{2}.\\d{4})( \\d+)?");

        static string dokumentVozila = null;
        static string dokumentLokacije = null;
        static string dokumentCjenik = null;
        static string dokumentLokacijeKapacitet = null;
        static string dokumentOsobe = null;
        static string dokumentAktivnosti = null;
        static string dokumentTvrtka = null;
        static string dokumentKonfig = null;

        private static bool radi = true;
        static Baza baza = Baza.getInstance();
        static Helpers.ConsoleWriter cw = Helpers.ConsoleWriter.getInstance();
        private static bool skupni = false;
        static string pocetnaKomanda = "";
        static void Main(string[] args)
        {

            foreach (var x in args)
            {
                pocetnaKomanda += " " + x;
            }
            string[] strSplit = Array.ConvertAll(pocetnaKomanda.Split(" "), p => p.Trim());

            if (strSplit.Length == 2)
            {


                pocetnaKomanda = "";
                dokumentKonfig = strSplit[1];
                //UcitajDatoteku(TipDatoteke.konfig);
                UcitajDokument(TipDatoteke.konfig);

                Console.WriteLine(dokumentOsobe);
                foreach (var x in args)
                {
                    pocetnaKomanda += " " + x;
                }
                strSplit = Array.ConvertAll(pocetnaKomanda.Split(" "), p => p.Trim());
            }

            UnesiDokumente(strSplit);
            if (!UcitajSveDokumente())
            {
                cw.Write("Neuspjelo ucitavanje dokumenata. Zatvaram program...");
                return;
            }
            PostaviRoditeljeLokacijama();

            if (skupni)
            {
                PokreniSkupniNacinRada();
            }

            if (!skupni)
            {
                while (radi)
                {
                    cw.Write("INTERAKTIVNI NAČIN IZVOĐENJA", false);
                    cw.Write("Unesite komandu: ", false);
                    string komanda = Console.ReadLine();
                    CitajKomandu(komanda);
                }
            }
            /*
            List<TvrtkaComponent> temp = baza.ishodisna.GetIterator().DFS();
            foreach (var item in temp)
            {
                for (int i = 0; i < item.razina; i++)
                {
                    Console.Write("-");
                }
                cw.Write(item.getComponentName());                
            }*/
        }

        private static void PokreniSkupniNacinRada()
        {
            cw.Write("SKUPNI NAČIN IZVOĐENJA", false);
            UcitajDokumentAktivnosti();
            skupni = false;
        }
        private static void PostaviRoditeljeLokacijama()
        {

            Iterator iterator = baza.ishodisna.GetIterator();
            List<TvrtkaComponent> kolekcija = iterator.DFS();
            for (int i = 0; i < kolekcija.Count; i++)
            {
                List<TvrtkaComponent> children = kolekcija[i].getChildrenComponents();
                foreach (TvrtkaComponent c in children)
                {
                    c.razina = kolekcija[i].razina + 1;
                    c.nadredeni = kolekcija[i];
                }
            }
            List<Lokacija> temp = new List<Lokacija>();
            foreach (Lokacija l in baza.getLokacije())
            {
                if (l.nadredeni == null)
                {
                    temp.Add(l);
                    cw.Write("Greška prilikom unošenja lokacija. Lokacija " + l.naziv + " nema organizacijsku jedinicu.");
                }
            }
            foreach (var item in temp)
            {
                baza.getLokacije().Remove(item);
            }

        }

        static void CitajKomandu(string komanda)
        {
            MatchCollection matchPodatci = REGEX_PODATCI.Matches(komanda);
            MatchCollection matchVracanje = REGEX_VRACANJE.Matches(komanda);
            MatchCollection matchVracanjeNeispravno = REGEX_VRACANJE_NEISPRAVNO.Matches(komanda);
            MatchCollection matchKraj = REGEX_KRAJ.Matches(komanda);
            MatchCollection matchSkupni = REGEX_SKUPNI.Matches(komanda);
            MatchCollection matchIspisStanje = REGEX_ISPIS_STANJE.Matches(komanda);
            MatchCollection matchAktivnostiIspisa = REGEX_AKTIVNOSTI_ISPISA.Matches(komanda);
            cw.Write(komanda, false);
            if (matchVracanjeNeispravno.Count != 0)
            {
                try
                {
                    int aktivnost = int.Parse(matchVracanjeNeispravno[0].Groups[1].Value);
                    DateTime vrijeme = DateTime.Parse(matchVracanjeNeispravno[0].Groups[2].Value);
                    Match podatci = matchVracanjeNeispravno[0];
                    if (aktivnost == 4)
                    {
                        AktivnostVracanje(aktivnost, vrijeme, podatci.Groups[3].Value, podatci.Groups[4].Value, podatci.Groups[5].Value, podatci.Groups[6].Value, podatci.Groups[7].Value);
                    }
                    else
                    {
                        cw.Write("Pogrešna sintaksa komande! - Aktivnost: " + aktivnost);
                    }
                }
                catch (FormatException)
                {
                    cw.Write("Pogrešan format datuma!");
                }
            }
            else if (matchVracanje.Count != 0)
            {

                try
                {
                    int aktivnost = int.Parse(matchVracanje[0].Groups[1].Value);
                    DateTime vrijeme = DateTime.Parse(matchVracanje[0].Groups[2].Value);
                    Match podatci = matchVracanje[0];
                    if (aktivnost == 4)
                    {
                        AktivnostVracanje(aktivnost, vrijeme, podatci.Groups[3].Value, podatci.Groups[4].Value, podatci.Groups[5].Value, podatci.Groups[6].Value);
                    }
                    else
                    {
                        cw.Write("Pogrešna sintaksa komande! - Aktivnost: " + aktivnost);
                    }
                }
                catch (FormatException)
                {
                    cw.Write("Pogrešan format datuma!");
                }
            }
            else if (matchPodatci.Count != 0)
            {
                try
                {
                    int aktivnost = int.Parse(matchPodatci[0].Groups[1].Value);
                    DateTime vrijeme = DateTime.Parse(matchPodatci[0].Groups[2].Value);
                    Match podatci = matchPodatci[0];
                    switch (aktivnost)
                    {
                        case 1:
                            AktivnostPregledVozila(aktivnost, vrijeme, podatci.Groups[3].Value, podatci.Groups[4].Value, podatci.Groups[5].Value);
                            break;
                        case 2:
                            AktivnostNajam(aktivnost, vrijeme, podatci.Groups[3].Value, podatci.Groups[4].Value, podatci.Groups[5].Value);
                            break;
                        case 3:
                            AktivnostPregledMjesta(aktivnost, vrijeme, podatci.Groups[3].Value, podatci.Groups[4].Value, podatci.Groups[5].Value);
                            break;
                        default:
                            cw.Write("Pogrešna sintaksa komande! - Aktivnost: " + aktivnost);
                            break;
                    }
                }
                catch (FormatException)
                {
                    cw.Write("Pogrešan format datuma!");
                }
            }
            else if (matchSkupni.Count != 0)
            {
                try
                {
                    int aktivnost = int.Parse(matchSkupni[0].Groups[1].Value);
                    if (aktivnost == 5)
                    {
                        dokumentAktivnosti = matchSkupni[0].Groups[2].Value;
                        PokreniSkupniNacinRada();
                    }
                    else
                    {
                        cw.Write("Pogrešna sintaksa komande! - Aktivnost: " + aktivnost);
                    }
                }
                catch (FormatException)
                {
                    cw.Write("Pogrešan format datuma!");
                }
            }
            else if (matchAktivnostiIspisa.Count != 0)
            {
                int aktivnost = int.Parse(matchAktivnostiIspisa[0].Groups[1].Value);
                string komande = matchAktivnostiIspisa[0].Groups[2].Value;
                string[] komandeSplit = Array.ConvertAll(komande.Split(" "), p => p.Trim());
                DateTime datum_1 = DateTime.Parse(matchAktivnostiIspisa[0].Groups[3].Value);
                DateTime datum_2 = DateTime.Parse(matchAktivnostiIspisa[0].Groups[4].Value);
                int idOrgJedinice = baza.ishodisna.id;

                bool isNumber = int.TryParse(matchAktivnostiIspisa[0].Groups[matchAktivnostiIspisa[0].Groups.Count - 1].Value, out int n);
                if (isNumber) idOrgJedinice = n;


                if (aktivnost == 7)
                {
                    if (komandeSplit.Length > 0 && komandeSplit.Length < 4)
                    {
                        if (komandeSplit.Length == 1)
                        {
                            if (komandeSplit[0] == "struktura")
                            {
                                AktivnostIspisZarade aiz = new AktivnostIspisZarade(aktivnost, "struktura", datum_1, datum_2);
                                aiz.PrikaziStrukturu(idOrgJedinice);
                            }
                            else if (komandeSplit[0] == "zarada")
                            {
                                AktivnostIspisZarade aiz = new AktivnostIspisZarade(aktivnost, "zarada", datum_1, datum_2);
                                aiz.PrikaziZarada(idOrgJedinice);
                            }
                            else if (komandeSplit[0] == "najam")
                            {
                                AktivnostIspisZarade aiz = new AktivnostIspisZarade(aktivnost, "najam", datum_1, datum_2);
                                aiz.PrikaziNajam(idOrgJedinice);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < komandeSplit.Length; i++)
                            {
                                if (komandeSplit.Length == 2)
                                {
                                    if ((komandeSplit[0] == "zarada" || komandeSplit[0] == "najam") &&
                                        (komandeSplit[1] == "zarada" || komandeSplit[2] == "najam"))
                                    {
                                        AktivnostIspisZarade aiz = new AktivnostIspisZarade(aktivnost, "", datum_1, datum_2);
                                        aiz.PrikaziPodatke(idOrgJedinice);
                                    }
                                    else
                                    {
                                        AktivnostIspisZarade aiz = new AktivnostIspisZarade(aktivnost, "", datum_1, datum_2);
                                        aiz.PrikaziStrukturu(idOrgJedinice);
                                        if (komandeSplit[0] == "zarada" || komandeSplit[1] == "zarada")
                                        {
                                            aiz.PrikaziZarada(idOrgJedinice);
                                        }
                                        if (komandeSplit[0] == "najam" || komandeSplit[1] == "najam")
                                        {
                                            aiz.PrikaziNajam(idOrgJedinice);
                                        }
                                    }
                                }
                                else
                                {
                                    AktivnostIspisZarade aiz = new AktivnostIspisZarade(aktivnost, "", datum_1, datum_2);
                                    aiz.PrikaziStrukturu(idOrgJedinice);
                                    aiz.PrikaziPodatke(idOrgJedinice);
                                }
                            }
                        }

                    }
                    else
                    {
                        cw.Write("Pogresan broj komandi! - Aktivnost: " + aktivnost);
                    }

                }
                else if (aktivnost == 8)
                {
                    if (komandeSplit.Length > 0 && komandeSplit.Length < 4)
                    {
                        foreach (string k in komandeSplit)
                        {
                            if (k == "struktura")
                            {
                                AktivnostIspisRacuna air = new AktivnostIspisRacuna(aktivnost, k, datum_1, datum_2);
                                air.PrikaziStrukturu(idOrgJedinice);
                            }
                            else if (k == "racuni")
                            {
                                AktivnostIspisRacuna air = new AktivnostIspisRacuna(aktivnost, k, datum_1, datum_2);
                                air.PrikaziRacune(idOrgJedinice);
                            }
                        }
                    }
                    else
                    {
                        cw.Write("Pogresan broj komandi! - Aktivnost: " + aktivnost);
                    }
                }
                else
                {
                    cw.Write("Pogrešna sintaksa komande!");
                }
            }
            else if (matchIspisStanje.Count != 0)
            {
                int aktivnost = int.Parse(matchIspisStanje[0].Groups[1].Value);
                if (aktivnost == 6)
                {
                    AktivnostIspisStanja(matchIspisStanje[0]);
                }
                else
                {
                    cw.Write("Pogrešna sintaksa komande! - Aktivnost: " + aktivnost);
                }
            }
            else if (matchKraj.Count != 0)
            {
                try
                {
                    int aktivnost = int.Parse(matchKraj[0].Groups[1].Value);
                    DateTime vrijeme = DateTime.Parse(matchKraj[0].Groups[2].Value);
                    if (aktivnost == 0)
                    {
                        AktivnostKraj(aktivnost, vrijeme);
                    }
                    else
                    {
                        cw.Write("Pogrešna sintaksa komande! - Aktivnost: " + aktivnost);
                    }
                }
                catch (FormatException)
                {
                    cw.Write("Pogrešan format datuma!");
                }
            }
            else
            {
                if (komanda == "")
                {
                    cw.Write("Unjeli ste praznu komandu.");
                }
                else
                {
                    cw.Write("Pogrešna sintaksa komande! - Komanda: " + komanda);
                }

            }
        }



        private static void AktivnostPregledVozila(int idAktivnosti, DateTime vrijeme, string korisnik, string lokacija, string vozilo)
        {
            if (baza.UsporediVrijeme(vrijeme))
            {
                AktivnostDirektor direktor = new AktivnostDirektor(new Aktivnost.Builder(idAktivnosti, vrijeme));
                Aktivnost aktivnost = direktor.PregledVozila(
                    baza.getKorisnik(int.Parse(korisnik)),
                    baza.getLokacija(int.Parse(lokacija)),
                    baza.getVozilo(int.Parse(vozilo)));
                if (aktivnost != null)
                {
                    baza.getAktivnosti().Add(aktivnost);
                }
            }
            else
            {
                cw.Write("Vrijeme aktivnosti je manje od virtualnog vremena.");
            }
        }
        private static void AktivnostNajam(int idAktivnosti, DateTime vrijeme, string korisnik, string lokacija, string vozilo)
        {
            if (baza.UsporediVrijeme(vrijeme))
            {
                Aktivnost aktivnost = new AktivnostDirektor(new Aktivnost.Builder(idAktivnosti, vrijeme)).Najam(
                    baza.getKorisnik(int.Parse(korisnik)),
                    baza.getLokacija(int.Parse(lokacija)),
                    baza.getVozilo(int.Parse(vozilo)));
                if (aktivnost != null)
                {
                    baza.getAktivnosti().Add(aktivnost);
                }
            }
            else
            {
                cw.Write("Vrijeme aktivnosti je manje od virtualnog vremena.");
            }
        }
        private static void AktivnostPregledMjesta(int idAktivnosti, DateTime vrijeme, string korisnik, string lokacija, string vozilo)
        {
            if (baza.UsporediVrijeme(vrijeme))
            {
                Aktivnost aktivnost = new AktivnostDirektor(new Aktivnost.Builder(idAktivnosti, vrijeme)).PregledMjesta(
                    baza.getKorisnik(int.Parse(korisnik)),
                    baza.getLokacija(int.Parse(lokacija)),
                    baza.getVozilo(int.Parse(vozilo)));
                if (aktivnost != null)
                {
                    baza.getAktivnosti().Add(aktivnost);
                }
            }
            else
            {
                cw.Write("Vrijeme aktivnosti je manje od virtualnog vremena.");
            }
        }
        private static void AktivnostVracanje(int idAktivnosti, DateTime vrijeme, string korisnik, string lokacija, string vozilo, string brojKm, string opisProblema = null)
        {
            if (baza.UsporediVrijeme(vrijeme))
            {
                if (opisProblema != null)
                {
                    Aktivnost aktivnost = new AktivnostDirektor(new Aktivnost.Builder(idAktivnosti, vrijeme)).Vracanje(
                                baza.getKorisnik(int.Parse(korisnik)),
                                baza.getLokacija(int.Parse(lokacija)),
                                baza.getVozilo(int.Parse(vozilo)),
                                int.Parse(brojKm),
                                opisProblema);
                    if (aktivnost != null)
                    {
                        baza.getAktivnosti().Add(aktivnost);
                    }
                }
                else
                {
                    Aktivnost aktivnost = new AktivnostDirektor(new Aktivnost.Builder(idAktivnosti, vrijeme)).Vracanje(
                                baza.getKorisnik(int.Parse(korisnik)),
                                baza.getLokacija(int.Parse(lokacija)),
                                baza.getVozilo(int.Parse(vozilo)),
                                int.Parse(brojKm));
                    if (aktivnost != null)
                    {
                        baza.getAktivnosti().Add(aktivnost);
                    }
                }

            }
            else
            {
                cw.Write("Vrijeme aktivnosti je manje od virtualnog vremena.");
            }
        }
        private static void AktivnostKraj(int idAktivnosti, DateTime vrijeme)
        {
            if (baza.UsporediVrijeme(vrijeme))
            {
                Aktivnost aktivnost = new AktivnostDirektor(new Aktivnost.Builder(idAktivnosti, vrijeme)).Kraj();
                cw.Write("U " + vrijeme + " program završava s radom.", false);
                radi = false;
                Environment.Exit(0);
            }
            else
            {
                cw.Write("Vrijeme aktivnosti je manje od virtualnog vremena.");
            }

        }
        private static void AktivnostIspisStanja(Match match)
        {
            int idOrgJedinice = 1;
            string komanda1 = "";
            string komanda2 = "";

            if (match.Groups[2].Value != "") komanda1 = match.Groups[2].Value.Trim();
            if (match.Groups[3].Value != "") komanda2 = match.Groups[3].Value.Trim();
            if (match.Groups[4].Value != "") idOrgJedinice = int.Parse(match.Groups[4].Value);

            Iterator iterator = baza.ishodisna.GetIterator();
            if (komanda1 == "struktura")
            {
                if (baza.ishodisna.id == idOrgJedinice)
                {
                    if (komanda2 == "stanje")
                    {
                        IspisiSveStanja(iterator.DFS());
                    }
                    else
                    {
                        IspisiStrukturuStanja(iterator.DFS());
                    }

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
                                if (komanda2 == "stanje")
                                {
                                    IspisiSveStanja(iterator
                                        .DFS(new List<TvrtkaComponent>(iterator.Current().getChildrenComponents())));
                                }
                                else
                                {
                                    IspisiStrukturuStanja(iterator
                                        .DFS(new List<TvrtkaComponent>(iterator.Current().getChildrenComponents())));
                                }

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
            else if (komanda1 == "stanje")
            {
                if (baza.ishodisna.id == idOrgJedinice)
                {
                    if (komanda2 == "struktura")
                    {
                        IspisiSveStanja(iterator.DFS());
                    }
                    else
                    {
                        IspisiPodatkeStanja(iterator.DFS());
                    }
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
                                if (komanda2 == "struktura")
                                {
                                    IspisiSveStanja(iterator
                                        .DFS(new List<TvrtkaComponent>(iterator.Current().getChildrenComponents())));
                                }
                                else
                                {
                                    IspisiPodatkeStanja(iterator
                                        .DFS(new List<TvrtkaComponent>(iterator.Current().getChildrenComponents())));
                                }

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
        }

        private static void IspisiStrukturuStanja(List<TvrtkaComponent> lista)
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
        private static void IspisiPodatkeStanja(List<TvrtkaComponent> lista)
        {
            Console.WriteLine("");
            Console.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, " + baza.dc + "} {4, " + baza.dc + "}  \n", "Naziv", "Vozilo", "SM", "SV", "NV");
            for (int ctr = 0; ctr < lista.Count; ctr++)
            {
                foreach (var tipVozila in baza.getTipoviVozila())
                {
                    string name = lista[ctr].getComponentName();
                    string voziloName = tipVozila.naziv;

                    if (name.Length > baza.dt) name = name.Substring(0, baza.dt);
                    if (voziloName.Length > baza.dt) voziloName = voziloName.Substring(0, baza.dt);

                    Console.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, " + baza.dc + "} {4, " + baza.dc + "} ",
                        name,
                        voziloName,
                        lista[ctr].DajSlobodnaMjesta(tipVozila),
                        lista[ctr].DajSlobodnaVozila(tipVozila),
                        lista[ctr].DajPokvarenaVozila(tipVozila));
                }
            }
            Console.WriteLine("");
        }
        private static void IspisiSveStanja(List<TvrtkaComponent> lista)
        {
            Console.WriteLine("");
            Console.WriteLine("{0, 10} {1, -" + baza.dt + "} {2, -" + baza.dt + "} {3, " + baza.dc + "} {4, " + baza.dc + "} {5, " + baza.dc + "}  \n", "Razina", "Naziv", "Vozilo", "SM", "SV", "NV");
            for (int ctr = 0; ctr < lista.Count; ctr++)
            {
                foreach (var tipVozila in baza.getTipoviVozila())
                {
                    string name = lista[ctr].getComponentName();
                    string voziloName = tipVozila.naziv;

                    if (name.Length > baza.dt) name = name.Substring(0, baza.dt);
                    if (voziloName.Length > baza.dt) voziloName = voziloName.Substring(0, baza.dt);
                    string razinaIcon = "";
                    for (int i = 0; i < lista[ctr].razina; i++)
                    {
                        razinaIcon += "-";
                    }
                    Console.WriteLine("{0,10} {1, -" + baza.dt + "} {2, -" + baza.dt + "} {3, " + baza.dc + "} {4, " + baza.dc + "} {5, " + baza.dc + "} ",
                        razinaIcon,
                        name,
                        voziloName,
                        lista[ctr].DajSlobodnaMjesta(tipVozila),
                        lista[ctr].DajSlobodnaVozila(tipVozila),
                        lista[ctr].DajPokvarenaVozila(tipVozila)); ;
                }
            }
            Console.WriteLine("");
        }
        static void UnesiDokumente(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                String arg = args[i];
                switch (arg)
                {
                    case "-v":
                        dokumentVozila = args[i + 1];
                        break;
                    case "-l":
                        dokumentLokacije = args[i + 1];
                        break;
                    case "-c":
                        dokumentCjenik = args[i + 1];
                        break;
                    case "-k":
                        dokumentLokacijeKapacitet = args[i + 1];
                        break;
                    case "-o":
                        dokumentOsobe = args[i + 1];
                        break;
                    case "-t":
                        try
                        {
                            string vrijeme = args[i + 1] + " " + args[i + 2];
                            MatchCollection match = REGEX_VRIJEME.Matches(vrijeme);
                            DateTime virtualnoVrijeme = new DateTime();
                            virtualnoVrijeme = DateTime.Parse(match[0].Groups[2].Value);
                            baza.setVirtualnoVrijeme(virtualnoVrijeme);
                        }
                        catch (Exception)
                        {
                            cw.Write("Format vremena je u pogrešnom obliku!");
                            throw;
                        }
                        break;
                    case "-s":
                        skupni = true;
                        dokumentAktivnosti = args[i + 1];
                        break;
                    case "-os":
                        dokumentTvrtka = args[i + 1];
                        break;
                    case "-dt":
                        baza.dt = int.Parse(args[i + 1]);
                        break;
                    case "-dc":
                        baza.dc = int.Parse(args[i + 1]);
                        break;
                    case "-dd":
                        baza.dd = int.Parse(args[i + 1]);
                        break;
                }
            }
        }
        static bool UcitajSveDokumente()
        {
            bool retVal = false;
            if (UcitajDokument(TipDatoteke.osobe) &&
                UcitajDokument(TipDatoteke.lokacije) &&
                UcitajDokument(TipDatoteke.vozila) &&
                UcitajDokument(TipDatoteke.cjenik) &&
                UcitajDokument(TipDatoteke.lokacije_kapacitet) &&
                UcitajDokument(TipDatoteke.tvrtka))
            {
                retVal = true;
            }
            return retVal;
        }
        static bool UcitajDokument(TipDatoteke tip)
        {
            System.IO.StreamReader file = null;
            string line;
            int brojac;
            bool postoji;
            file = UcitajDatoteku(tip);
            if (file == null)
            {
                cw.Write("Ne postoji datoteka!");
                return false;
            }
            switch (tip)
            {
                case TipDatoteke.osobe:
                    brojac = 1;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] atributi = Array.ConvertAll(line.Split(";"), p => p.Trim());
                        if (brojac > 1) //Preskacemo prvu liniju u datoteci
                        {
                            if (atributi.Length != 3)
                            {
                                cw.Write("Pogrešan broj atributa u liniji: " + brojac + " - Datoteka: " + tip);
                            }
                            else
                            {
                                try
                                {
                                    Korisnik noviKorisnik = new Korisnik(int.Parse(atributi[0]), atributi[1], int.Parse(atributi[2]));
                                    baza.getKorisnici().Add(noviKorisnik);
                                }
                                catch (Exception)
                                {
                                    cw.Write("Linija: " + brojac + "Greška prilikom unosa novog korisnika! Datoteka: " + tip);
                                }
                            }
                        }
                        brojac++;
                    }
                    return true;
                case TipDatoteke.lokacije:
                    brojac = 1;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] atributi = Array.ConvertAll(line.Split(";"), p => p.Trim());
                        if (brojac > 1) //Preskacemo prvu liniju u datoteci
                        {
                            if (atributi.Length != 4)
                            {
                                cw.Write("Pogrešan broj atributa u liniji: " + brojac + " - Datoteka: " + tip);
                            }
                            else
                            {
                                try
                                {
                                    TvrtkaComponent tvrtkaComponent = null; //TODO
                                    Lokacija novaLokacija = new Lokacija(int.Parse(atributi[0]), atributi[1], atributi[2], atributi[3], tvrtkaComponent);
                                    baza.getLokacije().Add(novaLokacija);
                                }
                                catch (Exception)
                                {
                                    cw.Write("Linija: " + brojac + "Greška prilikom unosa nove lokacije! Datoteke: " + tip);
                                }
                            }
                        }
                        brojac++;
                    }
                    baza.lokacijeZaDodjelu = new List<Lokacija>(baza.getLokacije());
                    return true;
                case TipDatoteke.vozila:
                    brojac = 1;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] atributi = Array.ConvertAll(line.Split(";"), p => p.Trim());
                        if (brojac > 1) //Preskacemo prvu liniju u datoteci
                        {
                            if (atributi.Length != 4)
                            {
                                cw.Write("Pogrešan broj atributa u liniji: " + brojac + " - Datoteka: " + tip);
                            }
                            else
                            {
                                try
                                {
                                    TipVozila novoVozilo = new TipVozila(int.Parse(atributi[0]), atributi[1], int.Parse(atributi[2]), int.Parse(atributi[3]));
                                    baza.getTipoviVozila().Add(novoVozilo);
                                }
                                catch (Exception)
                                {
                                    cw.Write("Linija: " + brojac + "Greška prilikom unosa tipa vozila! - Datoteka: " + tip);
                                }
                            }
                        }
                        brojac++;
                    }
                    return true;
                case TipDatoteke.cjenik:
                    brojac = 1;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] atributi = Array.ConvertAll(line.Split(";"), p => p.Trim());
                        if (brojac > 1) //Preskacemo prvu liniju u datoteci
                        {
                            if (atributi.Length != 4)
                            {
                                cw.Write("Pogrešan broj atributa u liniji: " + brojac + " - Datoteka: " + tip);
                            }
                            else
                            {
                                postoji = false;
                                foreach (TipVozila vozilo in baza.getTipoviVozila())
                                {
                                    if (vozilo.id == int.Parse(atributi[0]))
                                    {
                                        try
                                        {
                                            Cjenik noviCjenik = new Cjenik(vozilo, float.Parse(atributi[1]), float.Parse(atributi[2]), float.Parse(atributi[3]));
                                            baza.getCjenik().Add(noviCjenik);
                                        }
                                        catch (Exception)
                                        {
                                            cw.Write("Linija: " + brojac + "Greška prilikom unosa novog cjenika! Datoteka: " + tip);
                                        }
                                        postoji = true;
                                    }
                                }
                                if (!postoji)
                                {
                                    cw.Write("Linija: " + brojac + " - Greška u kreiranju cjenika! Ne postoji tip vozila s ID-jem " + int.Parse(atributi[0]) + ".");
                                }
                            }
                        }
                        brojac++;
                    }
                    return true;
                case TipDatoteke.lokacije_kapacitet:
                    brojac = 1;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] atributi = Array.ConvertAll(line.Split(";"), p => p.Trim());
                        if (brojac > 1) //Preskacemo prvu liniju u datoteci
                        {
                            if (atributi.Length != 4)
                            {
                                cw.Write("Pogrešan broj atributa u liniji: " + brojac + " - Datoteka: " + tip);
                            }
                            else
                            {
                                postoji = false;
                                Lokacija lokacijaUnos = null;
                                foreach (Lokacija lokacija in baza.getLokacije())
                                {
                                    if (lokacija.id == int.Parse(atributi[0]))
                                    {
                                        postoji = true;
                                        lokacijaUnos = lokacija;
                                    }
                                }
                                if (!postoji)
                                {
                                    cw.Write("Linija: " + brojac + " - Greška u kreiranju kapaciteta lokacija! Ne postoji lokacija s ID-jem " + int.Parse(atributi[0]) + ".");
                                }
                                if (postoji)
                                {
                                    postoji = false;
                                    foreach (TipVozila vozilo in baza.getTipoviVozila())
                                    {

                                        if (vozilo.id == int.Parse(atributi[1]))
                                        {
                                            try
                                            {
                                                postoji = true;
                                                LokacijaKapacitet novaLokacijaKapacitet = new LokacijaKapacitet(lokacijaUnos, vozilo, int.Parse(atributi[2]), int.Parse(atributi[3]));
                                                if (novaLokacijaKapacitet.brojVozila > novaLokacijaKapacitet.brojMjesta)
                                                {
                                                    cw.Write("Linija: " + brojac + " - Greška u kreiranju kapaciteta lokacija! - Broj vozila ne moze biti veci od broja mjesta!");
                                                    novaLokacijaKapacitet.brojMjesta = 0;
                                                    novaLokacijaKapacitet.brojVozila = 0;
                                                }
                                                else
                                                {
                                                    baza.getLokacijaKapacitet().Add(novaLokacijaKapacitet);
                                                    for (int i = 0; i < novaLokacijaKapacitet.brojVozila; i++)
                                                    {
                                                        Vozilo najamVozila = new Vozilo(vozilo.id, vozilo.naziv, vozilo.vrijemePunjenja, vozilo.domet);
                                                        novaLokacijaKapacitet.trenutnaVozila.Add(najamVozila);
                                                        baza.getVozilaZaNajam().Add(najamVozila);
                                                    }
                                                }
                                            }
                                            catch (Exception)
                                            {
                                                cw.Write("Linija: " + brojac + " - Greška prilikom unosa kapaciteta lokacije! Datoteka: " + tip);
                                            }
                                        }
                                    }
                                    if (!postoji)
                                    {
                                        cw.Write("Linija: " + brojac + " - Greška u kreiranju kapaciteta lokacija! Ne postoji tip vozila s ID-jem " + int.Parse(atributi[1]) + ".");
                                    }
                                }

                            }
                        }
                        brojac++;
                    }
                    return true;
                case TipDatoteke.tvrtka:
                    int brojPrijelaza = 0;
                    TvrtkaComponent tvrtka = null;
                    List<TvrtkaComponent> temp = new List<TvrtkaComponent>();
                    while ((line = file.ReadLine()) != null)
                    {
                        brojPrijelaza++;
                    }
                    for (int i = 0; i < brojPrijelaza; i++)
                    {
                        file = UcitajDatoteku(tip);
                        brojac = 1;

                        while ((line = file.ReadLine()) != null)
                        {
                            string[] atributi = Array.ConvertAll(line.Split(";"), p => p.Trim());
                            if (brojac > 1) //Preskacemo prvu liniju u datoteci
                            {
                                if (!baza.PostojiOrgJedinica(int.Parse(atributi[0])))
                                {
                                    if (atributi.Length != 4)
                                    {
                                        cw.Write("Pogrešan broj atributa u liniji: " + brojac + " - Datoteka: " + tip);
                                    }
                                    else
                                    {
                                        string[] lokacije = Array.ConvertAll(atributi[3].Split(","), p => p.Trim());
                                        List<TvrtkaComponent> lokacijeComponents = new List<TvrtkaComponent>();

                                        List<int> greske = new List<int>();
                                        foreach (string x in lokacije)
                                        {

                                            foreach (var item in temp)
                                            {
                                                if (x == "") continue;
                                                if (int.Parse(x) == item.id)
                                                {
                                                    cw.Write("Linija: " + brojac + " - Lokacija sa ID-jem " + item.id + " već ima nadređenu organizacijsku jedinicu.");
                                                }
                                            }

                                            foreach (TvrtkaComponent t in baza.lokacijeZaDodjelu)
                                            {
                                                try
                                                {
                                                    if (x == "")
                                                    {
                                                        continue;
                                                    }
                                                    if (t.id == int.Parse(x))
                                                    {

                                                        lokacijeComponents.Add(t);
                                                        temp.Add(t);
                                                        break;
                                                    }
                                                }
                                                catch (Exception)
                                                {
                                                    if (!greske.Contains(brojac))
                                                    {
                                                        cw.Write("Linija: " + brojac + " - Pogrešno formatirana linija.");
                                                        greske.Add(brojac);
                                                    }
                                                }
                                            }

                                            foreach (var item in temp)
                                            {
                                                baza.lokacijeZaDodjelu.Remove((Lokacija)item);
                                            }
                                        }

                                        foreach (TvrtkaComponent t in baza.getSveOrgJedinice())
                                        {
                                            if (atributi[2] == "") continue;
                                            if (t.id == int.Parse(atributi[2]))
                                            {
                                                tvrtka = t;
                                            }
                                        }

                                        if (baza.ishodisna == null && atributi[2] == "")
                                        {
                                            OrgJedinica novaOrgJedinica = new OrgJedinica(int.Parse(atributi[0]), atributi[1], tvrtka, lokacijeComponents);
                                            novaOrgJedinica.orgJedinica = true;
                                            baza.ishodisna = novaOrgJedinica;
                                            baza.getSveOrgJedinice().Add(novaOrgJedinica);
                                        }
                                        else
                                        {
                                            if (tvrtka == null)
                                            {
                                                cw.Write("Linija: " + brojac + " - Ne postoji ta nadredena jedinica.");
                                                continue;
                                            }
                                            try
                                            {
                                                OrgJedinica novaOrgJedinica = new OrgJedinica(int.Parse(atributi[0]), atributi[1], tvrtka, lokacijeComponents);
                                                novaOrgJedinica.orgJedinica = true;
                                                baza.DodajDijeteRoditelju(novaOrgJedinica, tvrtka.id);
                                                baza.getSveOrgJedinice().Add(novaOrgJedinica);
                                            }
                                            catch (Exception)
                                            {
                                                //TODO
                                            }
                                        }

                                    }
                                }
                            }
                            brojac++;
                        }
                    }
                    return true;
                case TipDatoteke.konfig:
                    Regex regex = new Regex("([a-z]+)=(.+)");
                    while ((line = file.ReadLine()) != null)
                    {
                        MatchCollection matches = regex.Matches(line);
                        string kljuc = matches[0].Groups[1].Value;
                        string vrijednost = matches[0].Groups[2].Value;
                        switch (kljuc)
                        {
                            case "vozila":
                                pocetnaKomanda += " -v " + vrijednost;
                                break;
                            case "lokacije":
                                pocetnaKomanda += " -l " + vrijednost;
                                break;
                            case "cjenik":
                                pocetnaKomanda += " -c " + vrijednost;
                                break;
                            case "kapaciteti":
                                pocetnaKomanda += " -k " + vrijednost;
                                break;
                            case "osobe":
                                pocetnaKomanda += " -o " + vrijednost;
                                break;
                            case "vrijeme":
                                pocetnaKomanda += " -t „" + vrijednost;
                                break;
                            case "struktura":
                                pocetnaKomanda += " -os " + vrijednost;
                                break;
                            case "aktivnosti":
                                pocetnaKomanda += " -s " + vrijednost;
                                break;
                            case "tekst":
                                pocetnaKomanda += " -dt " + vrijednost;
                                break;
                            case "cijeli":
                                pocetnaKomanda += " -dc " + vrijednost;
                                break;
                            case "decimala":
                                pocetnaKomanda += " -dd " + vrijednost;
                                break;
                            case "dugovanje":
                                baza.dugovanje = float.Parse(vrijednost);
                                break;
                            case "izlaz":
                                baza.nazivDatotekeIzlaz = vrijednost;
                                break;
                            default:
                                cw.Write("Pogrešan ključ prilikom učitavanja datoteke konfiguracije!");
                                return false;
                        }
                    }
                    return true;
                default:
                    return false;
            }
        }
        static bool UcitajDokumentAktivnosti()
        {
            System.IO.StreamReader file = null;
            string line;
            int brojac;
            file = UcitajDatoteku(TipDatoteke.aktivnosti);
            if (file == null)
            {
                return false;
            }
            brojac = 1;
            while ((line = file.ReadLine()) != null)
            {
                string[] atributi = Array.ConvertAll(line.Split(";"), p => p.Trim());
                if (brojac > 1) //Preskacemo prvu liniju u datoteci
                {
                    try
                    {
                        CitajKomandu(line);
                    }
                    catch (Exception)
                    {
                        cw.Write("Greška prilikom učitavanja aktivnosti u liniji: " + brojac);
                    }
                }
                brojac++;
            }
            return true;
        }


        private static StreamReader UcitajDatoteku(TipDatoteke tip)
        {
            try
            {
                switch (tip)
                {
                    case TipDatoteke.osobe:
                        return new System.IO.StreamReader(dokumentOsobe);
                    case TipDatoteke.lokacije:
                        return new System.IO.StreamReader(dokumentLokacije);
                    case TipDatoteke.vozila:
                        return new System.IO.StreamReader(dokumentVozila);
                    case TipDatoteke.cjenik:
                        return new System.IO.StreamReader(dokumentCjenik);
                    case TipDatoteke.lokacije_kapacitet:
                        return new System.IO.StreamReader(dokumentLokacijeKapacitet);
                    case TipDatoteke.aktivnosti:
                        return new System.IO.StreamReader(dokumentAktivnosti);
                    case TipDatoteke.tvrtka:
                        return new System.IO.StreamReader(dokumentTvrtka);
                    case TipDatoteke.konfig:
                        return new System.IO.StreamReader(dokumentKonfig);
                }
            }
            catch (FileNotFoundException)
            {
                cw.Write("Greška prilikom unosa datoteke tipa: " + tip + " - Datoteka ne postoji!");
            }
            catch (DirectoryNotFoundException)
            {
                cw.Write("Greška prilikom unosa datoteke tipa: " + tip + " - Direktorij ne postoji!");
            }
            catch (ArgumentNullException)
            {
                cw.Write("Greška prilikom unosa datoteke tipa: " + tip + " - Ne postoji taj argument!");
            }
            return null;
        }

        enum TipDatoteke
        {
            osobe,
            vozila,
            lokacije,
            cjenik,
            lokacije_kapacitet,
            aktivnosti,
            tvrtka,
            konfig
        }


    }
}
