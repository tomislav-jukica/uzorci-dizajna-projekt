using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace tjukica_zadaca_1
{
    class Program
    {
        Regex regex = new Regex("-(\\w) (\\w+[.]\\w+)|-(\\w) ([0-9-:]+ [0-9:]+)");
        private static Regex REGEX_KRAJ = new Regex("(\\d) .(\\d+-\\d+-\\d+ \\d+:\\d+:\\d+).");

        static string dokumentVozila = null;
        static string dokumentLokacije = null;
        static string dokumentCjenik = null;
        static string dokumentLokacijeKapacitet = null;
        static string dokumentOsobe = null;
        static string dokumentAktivnosti = null;
        static DateTime virtualnoVrijeme;

        private static bool radi = true;
        

        static void Main(string[] args)
        {                                  
            if (args.Length != 14 && args.Length != 12)
            {
                Console.WriteLine("Neispravan broj argumenata!");
                return;
            }

            UnesiDokumente(args);
            if (!UcitajSveDokumente())
            {
                Console.WriteLine("Neuspjelo ucitavanje dokumenata. Zatvaram program...");
                return;
            }

            if(args.Length == 12)//interaktivni način
            {
                while (radi)
                {
                    Console.WriteLine("Unesite komandu: ");
                    string komanda = Console.ReadLine();
                    CitajKomandu(komanda);
                }
            }
            else if(args.Length == 14)//skupni način
            {

            }
            

            
            
        }

        static void CitajKomandu(string komanda)
        {
            MatchCollection matches = REGEX_KRAJ.Matches(komanda);
            int aktivnost = int.Parse(matches[0].Groups[1].Value);
            DateTime vrijeme = DateTime.Parse(matches[0].Groups[2].Value);
            switch (aktivnost)
            {
                case 0:
                    AktivnostKraj(aktivnost, vrijeme);
                    break;
            }

        }

        private static void AktivnostKraj(int idAktivnosti, DateTime vrijeme)
        {
            Aktivnost aktivnost = AktivnostDirektor.Kraj(idAktivnosti, vrijeme);         
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
                        virtualnoVrijeme = new DateTime();
                        virtualnoVrijeme = DateTime.Parse(args[i + 1]);
                        break;
                    case "-s":
                        dokumentAktivnosti = args[i + 1];
                        break;
                }
            }
        }

        static bool UcitajSveDokumente()
        {
            bool retVal = false;
            if(UcitajDokument(TipDatoteke.osobe) &&
                UcitajDokument(TipDatoteke.lokacije) &&
                UcitajDokument(TipDatoteke.vozila) &&
                UcitajDokument(TipDatoteke.cjenik) &&
                UcitajDokument(TipDatoteke.lokacije_kapacitet))
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
            switch (tip)
            {
                case TipDatoteke.osobe:
                    try
                    {
                        file = new System.IO.StreamReader(dokumentOsobe);
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine("Greška prilikom unosa osoba. Datoteka ne postoji!");
                        return false;
                    }
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
                    return true;
                case TipDatoteke.lokacije:
                    try
                    {
                        file = new System.IO.StreamReader(dokumentLokacije);
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine("Greška prilikom unosa lokacija. Datoteka ne postoji!");
                        return false;
                    }
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
                    return true;
                case TipDatoteke.vozila:
                    try
                    {
                        file = new System.IO.StreamReader(dokumentVozila);
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine("Greška prilikom unosa vozila. Datoteka ne postoji!");
                        return false;                       
                    }
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
                    return true;
                case TipDatoteke.cjenik:
                    try
                    {
                        file = new System.IO.StreamReader(dokumentCjenik);
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine("Greška prilikom unosa cjenika. Datoteka ne postoji!");
                        return false;
                    }
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
                                            Cjenik noviCjenik = new Cjenik(vozilo, int.Parse(atributi[1]), int.Parse(atributi[2]), int.Parse(atributi[3]));
                                            Cjenik.cjenik.Add(noviCjenik);                                            
                                        }
                                        catch (Exception)
                                        {
                                            Console.WriteLine("Greška prilikom učitavanja " + tip + "a!");
                                        }
                                        postoji = true;
                                    }
                                }
                                if (!postoji)
                                {
                                    Console.WriteLine("Linija: " + brojac + " - Greška u kreiranju cjenika! Ne postoji vozilo s ID-jem " + int.Parse(atributi[0]) + ".");
                                }
                            }
                        }
                        brojac++;
                    }
                    return true;
                case TipDatoteke.lokacije_kapacitet:
                    try
                    {
                        file = new System.IO.StreamReader(dokumentLokacijeKapacitet);
                    }
                    catch (FileNotFoundException)
                    {
                        Console.WriteLine("Greška prilikom unosa kapaciteta lokacija. Datoteka ne postoji!");
                        return false;
                    }
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
                                Lokacija lokacijaUnos = null;
                                foreach (Lokacija lokacija in Lokacija.lokacije)
                                {
                                    if(lokacija.id == int.Parse(atributi[0])){
                                        postoji = true;
                                        lokacijaUnos = lokacija;
                                    }
                                }
                                if (!postoji)
                                {
                                    Console.WriteLine("Linija: " + brojac + " - Greška u kreiranju kapaciteta lokacija! Ne postoji lokacija s ID-jem " + int.Parse(atributi[0]) + ".");
                                }
                                if (postoji)
                                {
                                    postoji = false;
                                    foreach (Vozilo vozilo in Vozilo.vozila)
                                    {
                                        if (vozilo.id == int.Parse(atributi[1]))
                                        {
                                            postoji = true;
                                            LokacijaKapacitet novaLokacijaKapacitet = new LokacijaKapacitet(lokacijaUnos, vozilo, int.Parse(atributi[2]), int.Parse(atributi[3]));
                                            LokacijaKapacitet.kapacitetiLokacija.Add(novaLokacijaKapacitet);
                                        }
                                    }
                                    if (!postoji)
                                    {
                                        Console.WriteLine("Linija: " + brojac + " - Greška u kreiranju kapaciteta lokacija! Ne postoji vozilo s ID-jem " + int.Parse(atributi[1]) + ".");
                                    }
                                }
                                
                            }
                        }
                        brojac++;
                    }
                    return true;
                default:
                    return false;
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
