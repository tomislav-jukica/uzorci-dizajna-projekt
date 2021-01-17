using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using tjukica_zadaca_1.Composite;
using tjukica_zadaca_1.Proxy;
using System.Linq;

namespace tjukica_zadaca_1.Helpers
{
    public class ConsoleWriter
    {
        private static ConsoleWriter cw = null;
        private static Baza baza = Baza.getInstance();

        private ConsoleWriter() { } //konstruktor
        private System.IO.StreamWriter file = null;

        public static ConsoleWriter getInstance()
        {
            if (cw == null)
            {
                cw = new ConsoleWriter();
            }
            return cw;
        }

        public void Write(string msg, bool error = true)
        {
            if (Baza.getInstance().nazivDatotekeIzlaz != null)
            {

                string path = @".\" + Baza.getInstance().nazivDatotekeIzlaz;

                try
                {
                    if (file == null)
                    {
                        file = new System.IO.StreamWriter(path);
                    }
                    file.WriteLine(msg);
                }
                catch (Exception e)
                {
                    Console.WriteLine("GREŠKA: " + e);
                }
                if (error)
                {
                    for (int i = 0; i < msg.Length; i++)
                    {
                        file.Write("x");
                    }
                }
                file.Write("\n");
            }
            else
            {
                if (error) Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(msg);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void HorizontalLine(int length = 60)
        {
            if (Baza.getInstance().nazivDatotekeIzlaz != null)
            {

            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    Console.Write("-");
                }
                Console.Write("\n");
            }
        }

        public void zatvoriFile()
        {
            if (file != null)
            {
                file.Close();
            }
        }

        public void ispisStrukture(List<TvrtkaComponent> lista)
        {
            if (Baza.getInstance().nazivDatotekeIzlaz != null)
            {
                file.WriteLine(" ");
                file.WriteLine("{0,10}\n", "Naziv");
                for (int ctr = 0; ctr < lista.Count; ctr++)
                {
                    string razinaIcon = "";
                    for (int i = 0; i < lista[ctr].razina; i++)
                    {
                        razinaIcon += "-";
                    }

                    file.WriteLine("{0,-20}", razinaIcon + "  " + lista[ctr].getComponentName());
                }

            }
            else
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

        public void ispisiRacune(List<TvrtkaComponent> lista, DateTime datum_1, DateTime datum_2)
        {
            if (Baza.getInstance().nazivDatotekeIzlaz != null)
            {
                file.WriteLine("");
                file.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, -" + baza.dt + "} {4, -" + baza.dt + "}" +
                    "{5, " + baza.dc + "} {6, -" + baza.dt + "} {7, " + baza.dc + "} {8, -" + baza.dt + "}" +
                    "{9, " + (baza.dc + baza.dd + 1) + "} {10, " + (baza.dc + baza.dd + 1) + "} {11, " + (baza.dc + baza.dd + 1) + "} {12, " + baza.dc + "}\n",
                    "Naziv", "Vozilo", "Rb.", "Vrijeme", "Korisnik", "ID", "L. Najma", "ID", "L. Vracanja", "C. Najma", "C. Km", "C. Sat", "Ukupno");
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
                            file.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, -" + baza.dt + "} {4, -" + baza.dt + "}" +
                    "{5, " + baza.dc + "} {6, -" + baza.dt + "} {7, " + baza.dc + "} {8, -" + baza.dt + "}" +
                    "{9, " + (baza.dc + baza.dd + 1) + "} {10, " + (baza.dc + baza.dd + 1) + "} {11, " + (baza.dc + baza.dd + 1) + "} {12, " + baza.dc + "}",
                            name,
                            voziloName,
                            x.id,
                            x.datumIzdavanja,
                            baza.getKorisnik(x.idKorisnik).ime,
                            x.idLokacijeNajma,
                            baza.getLokacija(x.idLokacijeNajma).naziv,
                            x.idLokacijeVracanja,
                            baza.getLokacija(x.idLokacijeVracanja).naziv,
                            Math.Round(baza.getCjenikZaVozilo(tipVozila).najam, baza.dd, MidpointRounding.AwayFromZero),
                            Math.Round(baza.getCjenikZaVozilo(tipVozila).cijenaKm * x.brojKm, baza.dd, MidpointRounding.AwayFromZero),
                            Math.Round(baza.getCjenikZaVozilo(tipVozila).cijenaSat * Math.Ceiling(x.brojSati), baza.dd, MidpointRounding.AwayFromZero),
                            x.ukupno);

                        }

                    }
                }
                file.WriteLine("");
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, -" + baza.dt + "} {4, -" + baza.dt + "}" +
                    "{5, " + baza.dc + "} {6, -" + baza.dt + "} {7, " + baza.dc + "} {8, -" + baza.dt + "}" +
                    "{9, " + (baza.dc + baza.dd + 1) + "} {10, " + (baza.dc + baza.dd + 1) + "} {11, " + (baza.dc + baza.dd + 1) + "} {12, " + baza.dc + "}\n",
                    "Naziv", "Vozilo", "Rb.", "Vrijeme", "Korisnik", "ID", "L. Najma", "ID", "L. Vracanja", "C. Najma", "C. Km", "C. Sat", "Ukupno");
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
                    "{9, " + (baza.dc + baza.dd + 1) + "} {10, " + (baza.dc + baza.dd + 1) + "} {11, " + (baza.dc + baza.dd + 1) + "} {12, " + baza.dc + "}",
                            name,
                            voziloName,
                            x.id,
                            x.datumIzdavanja,
                            baza.getKorisnik(x.idKorisnik).ime,
                            x.idLokacijeNajma,
                            baza.getLokacija(x.idLokacijeNajma).naziv,
                            x.idLokacijeVracanja,
                            baza.getLokacija(x.idLokacijeVracanja).naziv,
                            Math.Round(baza.getCjenikZaVozilo(tipVozila).najam, baza.dd, MidpointRounding.AwayFromZero),
                            Math.Round(baza.getCjenikZaVozilo(tipVozila).cijenaKm * x.brojKm, baza.dd, MidpointRounding.AwayFromZero),
                            Math.Round(baza.getCjenikZaVozilo(tipVozila).cijenaSat * Math.Ceiling(x.brojSati), baza.dd, MidpointRounding.AwayFromZero),
                            x.ukupno);

                        }

                    }
                }
                Console.WriteLine("");
            }
        }

        public void ispisNajma(List<TvrtkaComponent> lista, DateTime datum_1, DateTime datum_2)
        {
            if (baza.nazivDatotekeIzlaz != null)
            {
                file.WriteLine("");
                file.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "}\n", "Naziv", "Vozilo", "Najam");
                for (int ctr = 0; ctr < lista.Count; ctr++)
                {
                    foreach (var tipVozila in baza.getTipoviVozila())
                    {
                        string name = lista[ctr].getComponentName();
                        string voziloName = tipVozila.naziv;

                        if (name.Length > baza.dt) name = name.Substring(0, baza.dt);
                        if (voziloName.Length > baza.dt) voziloName = voziloName.Substring(0, baza.dt);

                        file.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "}",
                            name,
                            voziloName,
                            lista[ctr].DajNajmove(tipVozila, datum_1, datum_2));
                    }
                }
                file.WriteLine("");
            }
            else
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
        }

        public void ispisPodataka(List<TvrtkaComponent> lista, DateTime datum_1, DateTime datum_2)
        {
            if (baza.nazivDatotekeIzlaz != null)
            {
                file.WriteLine("");
                file.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, " + baza.dc + "} {4, " + baza.dt + "}\n", "Naziv", "Vozilo", "Najam", "Zarada", "Trajanje(Min)");
                for (int ctr = 0; ctr < lista.Count; ctr++)
                {
                    foreach (var tipVozila in baza.getTipoviVozila())
                    {
                        string name = lista[ctr].getComponentName();
                        string voziloName = tipVozila.naziv;

                        if (name.Length > baza.dt) name = name.Substring(0, baza.dt);
                        if (voziloName.Length > baza.dt) voziloName = voziloName.Substring(0, baza.dt);

                        file.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, " + baza.dc + "} {4, " + baza.dt + "}",
                            name,
                            voziloName,
                            lista[ctr].DajNajmove(tipVozila, datum_1, datum_2),
                            lista[ctr].DajZaradu(tipVozila, datum_1, datum_2),
                            lista[ctr].DajVremenaNajmova(tipVozila, datum_1, datum_2));
                    }
                }
                file.WriteLine("");
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, " + baza.dc + "} {4, " + baza.dt + "}\n", "Naziv", "Vozilo", "Najam", "Zarada", "Trajanje(Min)");
                for (int ctr = 0; ctr < lista.Count; ctr++)
                {
                    foreach (var tipVozila in baza.getTipoviVozila())
                    {
                        string name = lista[ctr].getComponentName();
                        string voziloName = tipVozila.naziv;

                        if (name.Length > baza.dt) name = name.Substring(0, baza.dt);
                        if (voziloName.Length > baza.dt) voziloName = voziloName.Substring(0, baza.dt);

                        Console.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, " + baza.dc + "} {4, " + baza.dt + "}",
                            name,
                            voziloName,
                            lista[ctr].DajNajmove(tipVozila, datum_1, datum_2),
                            lista[ctr].DajZaradu(tipVozila, datum_1, datum_2),
                            lista[ctr].DajVremenaNajmova(tipVozila, datum_1, datum_2));
                    }
                }
                Console.WriteLine("");
            }
        }

        public void ispisZarade(List<TvrtkaComponent> lista, DateTime datum_1, DateTime datum_2)
        {
            if (baza.nazivDatotekeIzlaz != null)
            {
                file.WriteLine("");
                file.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "}\n", "Naziv", "Vozilo", "Zarada");
                for (int ctr = 0; ctr < lista.Count; ctr++)
                {
                    foreach (var tipVozila in baza.getTipoviVozila())
                    {
                        string name = lista[ctr].getComponentName();
                        string voziloName = tipVozila.naziv;

                        if (name.Length > baza.dt) name = name.Substring(0, baza.dt);
                        if (voziloName.Length > baza.dt) voziloName = voziloName.Substring(0, baza.dt);

                        file.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "}",
                            name,
                            voziloName,
                            lista[ctr].DajZaradu(tipVozila, datum_1, datum_2));
                    }
                }
                file.WriteLine("");
            }
            else
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
        }

        public void ispisiStrukturuStanja(List<TvrtkaComponent> lista)
        {
            if (baza.nazivDatotekeIzlaz != null)
            {
                file.WriteLine("");
                file.WriteLine("{0,10}\n", "Naziv");
                for (int ctr = 0; ctr < lista.Count; ctr++)
                {
                    string razinaIcon = "";
                    for (int i = 0; i < lista[ctr].razina; i++)
                    {
                        razinaIcon += "-";
                    }

                    file.WriteLine("{0,-20}", razinaIcon + "  " + lista[ctr].getComponentName());
                }
                file.WriteLine("");
            }
            else
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

        public void ispisiPodatkeStanja(List<TvrtkaComponent> lista)
        {
            if (baza.nazivDatotekeIzlaz != null)
            {
                file.WriteLine("");
                file.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, " + baza.dc + "} {4, " + baza.dc + "}  \n", "Naziv", "Vozilo", "SM", "SV", "NV");
                for (int ctr = 0; ctr < lista.Count; ctr++)
                {
                    foreach (var tipVozila in baza.getTipoviVozila())
                    {
                        string name = lista[ctr].getComponentName();
                        string voziloName = tipVozila.naziv;

                        if (name.Length > baza.dt) name = name.Substring(0, baza.dt);
                        if (voziloName.Length > baza.dt) voziloName = voziloName.Substring(0, baza.dt);

                        file.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, " + baza.dc + "} {4, " + baza.dc + "} ",
                            name,
                            voziloName,
                            lista[ctr].DajSlobodnaMjesta(tipVozila),
                            lista[ctr].DajSlobodnaVozila(tipVozila),
                            lista[ctr].DajPokvarenaVozila(tipVozila));
                    }
                }
                file.WriteLine("");
            }
            else
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
        }



        public void ispisiSveStanja(List<TvrtkaComponent> lista)
        {
            if (baza.nazivDatotekeIzlaz != null)
            {
                file.WriteLine("");
                file.WriteLine("{0, 10} {1, -" + baza.dt + "} {2, -" + baza.dt + "} {3, " + baza.dc + "} {4, " + baza.dc + "} {5, " + baza.dc + "}  \n", "Razina", "Naziv", "Vozilo", "SM", "SV", "NV");
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
                        file.WriteLine("{0,10} {1, -" + baza.dt + "} {2, -" + baza.dt + "} {3, " + baza.dc + "} {4, " + baza.dc + "} {5, " + baza.dc + "} ",
                            razinaIcon,
                            name,
                            voziloName,
                            lista[ctr].DajSlobodnaMjesta(tipVozila),
                            lista[ctr].DajSlobodnaVozila(tipVozila),
                            lista[ctr].DajPokvarenaVozila(tipVozila)); ;
                    }
                }
                file.WriteLine("");
            }
            else
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
        }

        public void ispisFinancijskogStanja()
        {
            List<Korisnik> lista = baza.getKorisnici();
            if (baza.nazivDatotekeIzlaz != null)
            {
                file.WriteLine("");
                file.WriteLine("{0, " + baza.dc + "} {1, -" + baza.dt + "} {2, -" + baza.dc + "} {3, " + baza.dt + "} \n", "ID", "Ime", "Dug", "Vrijeme");
                foreach (Korisnik k in lista)
                {
                    if (k.unajmioJeVozilo)
                    {
                        file.WriteLine("{0, " + baza.dc + "} {1, -" + baza.dt + "} {2, -" + baza.dc + "} {3, " + baza.dt + "} ",
                        k.id,
                        k.ime,
                        k.dugovanje,
                        k.zadnjiNajamVozila);
                    }
                }
                file.WriteLine("");
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("{0, " + baza.dc + "} {1, -" + baza.dt + "} {2, -" + baza.dc + "} {3, " + baza.dt + "} \n", "ID", "Ime", "Dug", "Vrijeme");
                foreach (Korisnik k in lista)
                {
                    if (k.unajmioJeVozilo)
                    {
                        Console.WriteLine("{0, " + baza.dc + "} {1, -" + baza.dt + "} {2, -" + baza.dc + "} {3, " + baza.dt + "} ",
                        k.id,
                        k.ime,
                        k.dugovanje,
                        k.zadnjiNajamVozila);
                    }

                }
                Console.WriteLine("");
            }
        }

        public void AktivnostDeset(int korisnikId, DateTime datum_1, DateTime datum_2)
        {
            RacunovodstvoProxy rp = new RacunovodstvoProxy(Racunovodstvo.getInstance());
            List<Racun> listaRacuna = rp.GetRacuni();
            if (baza.nazivDatotekeIzlaz != null)
            {
                file.WriteLine("");
                file.WriteLine("{0, " + baza.dc + "} {1, -" + baza.dc + "} {2, " + baza.dt + "} {3, -" + baza.dt + "} {4, -" + baza.dt + "} {5, -" + baza.dt + "} \n", "Broj", "Iznos", "Datum", "Status", "Vozilo", "Lokacija");
                List<Racun> sortiranaLista = new List<Racun>();
                listaRacuna.OrderBy(e => e.placen).ThenBy(e => e.datumIzdavanja);
                foreach (Racun r in listaRacuna)
                {
                    if (r.idKorisnik != korisnikId) continue;
                    if (datum_1.CompareTo(r.datumIzdavanja) < 0 && datum_2.CompareTo(r.datumIzdavanja) > 0)
                    {
                        string placen = r.placen ? "Placen" : "Nije placen";
                        string vozilo = baza.getVozilo(r.idVoziloTip).naziv;
                        string lokacija = baza.getLokacija(r.idLokacijeNajma).naziv;

                        file.WriteLine("{0, " + baza.dc + "} {1, " + baza.dc + "} {2, " + baza.dt + "} {3, -" + baza.dt + "} {4, -" + baza.dt + "} {5, -" + baza.dt + "} ",
                        r.id,
                        r.ukupno,
                        r.datumIzdavanja,
                        placen,
                        vozilo,
                        lokacija);
                    }
                }
                file.WriteLine("");
            }
            else
            {
                Console.WriteLine("");
                Console.WriteLine("{0, " + baza.dc + "} {1, -" + baza.dc + "} {2, " + baza.dt + "} {3, -" + baza.dt + "} {4, -" + baza.dt + "} {5, -" + baza.dt + "} \n", "Broj", "Iznos", "Datum", "Status", "Vozilo", "Lokacija");
                List<Racun> sortiranaLista = new List<Racun>();
                listaRacuna.OrderBy(e => e.placen).ThenBy(e => e.datumIzdavanja);
                foreach (Racun r in listaRacuna)
                {
                    if (r.idKorisnik != korisnikId) continue;
                    if (datum_1.CompareTo(r.datumIzdavanja) < 0 && datum_2.CompareTo(r.datumIzdavanja) > 0)
                    {
                        string placen = r.placen ? "Placen" : "Nije placen";
                        string vozilo = baza.getVozilo(r.idVoziloTip).naziv;
                        string lokacija = baza.getLokacija(r.idLokacijeNajma).naziv;

                        Console.WriteLine("{0, " + baza.dc + "} {1, " + baza.dc + "} {2, " + baza.dt + "} {3, -" + baza.dt + "} {4, -" + baza.dt + "} {5, -" + baza.dt + "} ",
                        r.id,
                        r.ukupno,
                        r.datumIzdavanja,
                        placen,
                        vozilo,
                        lokacija);
                    }
                }
                Console.WriteLine("");
            }
        }

        public void AktivnostEleven(int korisnikId, decimal iznos)
        {
            RacunovodstvoProxy rp = new RacunovodstvoProxy(Racunovodstvo.getInstance());
            List<Racun> listaRacuna = rp.GetRacuniKorisnika(korisnikId);
            Korisnik korisnik = baza.getKorisnik(korisnikId);
            if (baza.nazivDatotekeIzlaz != null)
            {
                file.WriteLine("Placeni racuni:");
                file.WriteLine("{0, " + baza.dc + "} {1, " + baza.dt + "} {2, " + baza.dc + "} \n", "Broj", "Datum", "Iznos");

                foreach (Racun r in listaRacuna)
                {
                    if (!r.placen)
                    {
                        if (r.ukupno < iznos)
                        {
                            iznos -= r.ukupno;
                            r.placen = true;
                            korisnik.dugovanje -= r.ukupno;
                            file.WriteLine("{0, " + baza.dc + "} {1, " + baza.dt + "} {2, " + baza.dc + "}  ",
                            r.id,
                            r.datumIzdavanja,
                            r.ukupno);
                        }
                    }

                }
                file.WriteLine("Korisniku je vraćeno " + iznos + " kuna.");
                file.WriteLine("");
            }
            else
            {
                Console.WriteLine("Placeni racuni:");
                Console.WriteLine("{0, " + baza.dc + "} {1, " + baza.dt + "} {2, " + baza.dc + "} \n", "Broj", "Datum", "Iznos");

                foreach (Racun r in listaRacuna)
                {
                    if (r.ukupno < iznos)
                    {
                        iznos -= r.ukupno;
                        r.placen = true;
                        korisnik.dugovanje -= r.ukupno;
                        Console.WriteLine("{0, " + baza.dc + "} {1, " + baza.dt + "} {2, " + baza.dc + "}  ",
                        r.id,
                        r.datumIzdavanja,
                        r.ukupno);
                    }

                }
                Console.WriteLine("Korisniku je vraćeno " + iznos + " kuna.");
                Console.WriteLine("");
            }
        }
    }
}
