using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace tjukica_zadaca_1
{
    class Program
    {
        Regex regex = new Regex("-(\\w) (\\w+[.]\\w+)|-(\\w) ([0-9-:]+ [0-9:]+)");

        static string dokumentVozila = null;
        static string dokumentLokacije = null;
        static string dokumentCjenik = null;
        static string dokumentVozilaKapacitet = null;
        static string dokumentOsobe = null;
        static string dokumentAktivnosti = null;
        static DateTime virtualnoVrijeme;

        

        static void Main(string[] args)
        {                                  
            if (args.Length != 14 && args.Length != 12)
            {
                Console.WriteLine("Neispravan broj argumenata!");
                return;
            }

            UnesiDokumente(args);
            UcitajDokument(TipDatoteke.osobe);
            UcitajDokument(TipDatoteke.lokacije);
            UcitajDokument(TipDatoteke.vozila);
            UcitajDokument(TipDatoteke.cjenik);
            Console.WriteLine(Cjenik.cjenik.Count);
            Console.ReadLine();
            
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
                        dokumentVozilaKapacitet = args[i + 1];
                        break;
                    case "-o":
                        dokumentOsobe = args[i + 1];
                        break;
                    case "-t":
                        virtualnoVrijeme = new DateTime();
                        virtualnoVrijeme = DateTime.Parse(args[i + 1]);
                        break;
                    case "-s":
                        dokumentAktivnosti = args[i + 1];
                        break;
                }
            }
        }

        static void UcitajDokument(TipDatoteke tip)
        {
            System.IO.StreamReader file = null;
            string line;
            int brojac;
            bool postoji;
            switch (tip)
            {
                case TipDatoteke.osobe:
                    file = new System.IO.StreamReader(dokumentOsobe);
                    brojac = 1;
                    while((line = file.ReadLine()) != null)
                    {
                        string[] atributi = Array.ConvertAll(line.Split(";"), p => p.Trim());
                        if (brojac > 1) //Preskacemo prvu liniju u datoteci
                        {
                            if (atributi.Length != 2)
                            {
                                Console.WriteLine("Pogrešna sintaksa u liniji: " + brojac + " - " + tip);
                            }
                            else
                            {
                                try
                                {
                                    Korisnik noviKorisnik = new Korisnik(int.Parse(atributi[0]), atributi[1]);
                                    Korisnik.korisnici.Add(noviKorisnik);
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Greška prilikom učitavanja " + tip + "!");
                                }
                            }                            
                        }
                        brojac++;
                    } 
                    break;
                case TipDatoteke.lokacije:
                    file = new System.IO.StreamReader(dokumentLokacije);
                    brojac = 1;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] atributi = Array.ConvertAll(line.Split(";"), p => p.Trim());
                        if (brojac > 1) //Preskacemo prvu liniju u datoteci
                        {
                            if (atributi.Length != 4)
                            {
                                Console.WriteLine("Pogrešna sintaksa u liniji: " + brojac + " - " + tip);
                            }
                            else
                            {
                                try
                                {
                                    Lokacija novaLokacija = new Lokacija(int.Parse(atributi[0]), atributi[1], atributi[2], atributi[3]);
                                    Lokacija.lokacije.Add(novaLokacija);
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Greška prilikom učitavanja " + tip + "!");
                                }
                            }
                        }
                        brojac++;
                    }
                    break;
                case TipDatoteke.vozila:
                    file = new System.IO.StreamReader(dokumentVozila);
                    brojac = 1;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] atributi = Array.ConvertAll(line.Split(";"), p => p.Trim());
                        if (brojac > 1) //Preskacemo prvu liniju u datoteci
                        {
                            if (atributi.Length != 4)
                            {
                                Console.WriteLine("Pogrešna sintaksa u liniji: " + brojac + " - " + tip);
                            }
                            else
                            {
                                try
                                {
                                    Vozilo novoVozilo = new Vozilo(int.Parse(atributi[0]), atributi[1], int.Parse(atributi[2]), int.Parse(atributi[3]));
                                    Vozilo.vozila.Add(novoVozilo);
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Greška prilikom učitavanja " + tip + "!");
                                }
                            }
                        }
                        brojac++;
                    }
                    break;
                case TipDatoteke.cjenik:
                    file = new System.IO.StreamReader(dokumentCjenik);
                    brojac = 1;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] atributi = Array.ConvertAll(line.Split(";"), p => p.Trim());
                        if (brojac > 1) //Preskacemo prvu liniju u datoteci
                        {
                            if (atributi.Length != 4)
                            {
                                Console.WriteLine("Pogrešna sintaksa u liniji: " + brojac + " - " + tip);
                            }
                            else
                            {
                                postoji = false;
                                foreach (Vozilo vozilo in Vozilo.vozila)
                                {
                                    if(vozilo.id == int.Parse(atributi[0]))
                                    {
                                        try
                                        {
                                            Cjenik noviCjenik = new Cjenik(int.Parse(atributi[0]), int.Parse(atributi[1]), int.Parse(atributi[2]), int.Parse(atributi[3]));
                                            Cjenik.cjenik.Add(noviCjenik);
                                        }
                                        catch (Exception)
                                        {
                                            Console.WriteLine("Greška prilikom učitavanja " + tip + "!");
                                        }
                                        postoji = true;
                                    }
                                }
                                if (!postoji)
                                {
                                    Console.WriteLine("Greška u kreiranju cjenika! Ne postoji vozilo ID-jem " + int.Parse(atributi[0]) + ".");
                                }
                            }
                        }
                        brojac++;
                    }
                    break;
            }

        }

        enum TipDatoteke
        {
            osobe,
            vozila,
            lokacije,
            cjenik,
            lokacije_kapacitet,
            aktivnosti
        }


    }
}
