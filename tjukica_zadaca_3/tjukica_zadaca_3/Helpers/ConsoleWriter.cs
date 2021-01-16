using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using tjukica_zadaca_1.Composite;

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
            file.Close();
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

            } else
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
            if(Baza.getInstance().nazivDatotekeIzlaz != null)
            {
                file.WriteLine("");
                file.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, -" + baza.dt + "} {4, -" + baza.dt + "}" +
                    "{5, " + baza.dc + "} {6, -" + baza.dt + "} {7, " + baza.dc + "} {8, -" + baza.dt + "}" +
                    "{9, " + (baza.dc + baza.dd + 1) + "} {10, " + (baza.dc + baza.dd + 1) + "} {11, " + (baza.dc + baza.dd + 1) + "}\n",
                    "Naziv", "Vozilo", "Rb.", "Vrijeme", "Korisnik", "ID", "L. Najma", "ID", "L. Vracanja", "C. Najma", "C. Km", "C. Sat");
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
                file.WriteLine("");
            } else
            {
                Console.WriteLine("");
                Console.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, -" + baza.dt + "} {4, -" + baza.dt + "}" +
                    "{5, " + baza.dc + "} {6, -" + baza.dt + "} {7, " + baza.dc + "} {8, -" + baza.dt + "}" +
                    "{9, " + (baza.dc + baza.dd + 1) + "} {10, " + (baza.dc + baza.dd + 1) + "} {11, " + (baza.dc + baza.dd + 1) + "}\n",
                    "Naziv", "Vozilo", "Rb.", "Vrijeme", "Korisnik", "ID", "L. Najma", "ID", "L. Vracanja", "C. Najma", "C. Km", "C. Sat");
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

        public void ispisNajma(List<TvrtkaComponent> lista, DateTime datum_1, DateTime datum_2)
        {
            if(baza.nazivDatotekeIzlaz != null)
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
            if(baza.nazivDatotekeIzlaz != null)
            {
                file.WriteLine("");
                file.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, " + baza.dc + "}\n", "Naziv", "Vozilo", "Najam", "Zarada");
                for (int ctr = 0; ctr < lista.Count; ctr++)
                {
                    foreach (var tipVozila in baza.getTipoviVozila())
                    {
                        string name = lista[ctr].getComponentName();
                        string voziloName = tipVozila.naziv;

                        if (name.Length > baza.dt) name = name.Substring(0, baza.dt);
                        if (voziloName.Length > baza.dt) voziloName = voziloName.Substring(0, baza.dt);

                        file.WriteLine("{0, -" + baza.dt + "} {1, -" + baza.dt + "} {2, " + baza.dc + "} {3, " + baza.dc + "}",
                            name,
                            voziloName,
                            lista[ctr].DajNajmove(tipVozila, datum_1, datum_2),
                            lista[ctr].DajZaradu(tipVozila, datum_1, datum_2));
                    }
                }
                file.WriteLine("");
            } else
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
                            lista[ctr].DajZaradu(tipVozila, datum_1, datum_2));
                    }
                }
                Console.WriteLine("");
            }
        }

        public void ispisZarade(List<TvrtkaComponent> lista, DateTime datum_1, DateTime datum_2)
        {
            if(baza.nazivDatotekeIzlaz != null)
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
            } else
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
            if(baza.nazivDatotekeIzlaz != null)
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
            } else
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
            if(baza.nazivDatotekeIzlaz != null)
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
            } else
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
            if(baza.nazivDatotekeIzlaz != null)
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
            } else
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
    }
}
