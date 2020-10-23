﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace tjukica_zadaca_1
{
    class Program
    {
        Regex regex = new Regex("-(\\w) (\\w+[.]\\w+)|-(\\w) ([0-9-:]+ [0-9:]+)");
        private static Regex REGEX_KRAJ = new Regex("(\\d); .(\\d+-\\d+-\\d+ \\d+:\\d+:\\d+).");
        private static Regex REGEX_PODATCI = new Regex("(\\d); .(\\d+-\\d+-\\d+ \\d+:\\d+:\\d+).; (\\d+); (\\d+); (\\d+)");
        private static Regex REGEX_VRACANJE = new Regex("(\\d); .(\\d+-\\d+-\\d+ \\d+:\\d+:\\d+).; (\\d+); (\\d+); (\\d+); (\\d+)");

        static string dokumentVozila = null;
        static string dokumentLokacije = null;
        static string dokumentCjenik = null;
        static string dokumentLokacijeKapacitet = null;
        static string dokumentOsobe = null;
        static string dokumentAktivnosti = null;

        private static bool radi = true;
        static Baza baza = Baza.getInstance();

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
            Debug.WriteLine(baza.getVozilaZaNajam().Count);
            if (args.Length == 12)//interaktivni način
            {
                while (radi)
                {
                    Console.WriteLine("Unesite komandu: ");
                    string komanda = Console.ReadLine();
                    CitajKomandu(komanda);
                }
            }
            else if (args.Length == 14)//skupni način
            {

            }
            

        }

        static void CitajKomandu(string komanda)
        {
            MatchCollection matchPodatci = REGEX_PODATCI.Matches(komanda);
            MatchCollection matchVracanje = REGEX_VRACANJE.Matches(komanda);
            MatchCollection matchKraj = REGEX_KRAJ.Matches(komanda);

            if (matchVracanje.Count != 0)
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
                        Console.WriteLine("Pogrešna sintaksa komande!");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Pogrešan format datuma!");
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
                            Console.WriteLine("Pogrešna sintaksa komande!");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Pogrešan format datuma!");
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
                        Console.WriteLine("Pogrešna sintaksa komande!");
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Pogrešan format datuma!");
                }
            }
            else
            {
                Console.WriteLine("Pogrešna sintaksa komande!");
            }
        }

        

        private static void AktivnostPregledVozila(int idAktivnosti, DateTime vrijeme, string korisnik, string lokacija, string vozilo)
        {
            if (baza.UsporediVrijeme(vrijeme))
            {
                Aktivnost aktivnost = AktivnostDirektor.PregledVozila(idAktivnosti, vrijeme,
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
                Console.WriteLine("Vrijeme aktivnosti je manje od virtualnog vremena.");
            }
        }
        private static void AktivnostNajam(int idAktivnosti, DateTime vrijeme, string korisnik, string lokacija, string vozilo)
        {
            if (baza.UsporediVrijeme(vrijeme))
            {
                Aktivnost aktivnost = AktivnostDirektor.Najam(idAktivnosti, vrijeme,
                    baza.getKorisnik(int.Parse(korisnik)),
                    baza.getLokacija(int.Parse(lokacija)),
                    baza.getVozilo(int.Parse(vozilo)));
                if(aktivnost != null)
                {
                    baza.getAktivnosti().Add(aktivnost);
                }                
            }
            else
            {
                Console.WriteLine("Vrijeme aktivnosti je manje od virtualnog vremena.");
            }
        }
        private static void AktivnostPregledMjesta(int idAktivnosti, DateTime vrijeme, string korisnik, string lokacija, string vozilo)
        {
            if (baza.UsporediVrijeme(vrijeme))
            {
                Aktivnost aktivnost = AktivnostDirektor.PregledMjesta(idAktivnosti, vrijeme,
                    baza.getKorisnik(int.Parse(korisnik)),
                    baza.getLokacija(int.Parse(lokacija)),
                    baza.getVozilo(int.Parse(vozilo)));
                if(aktivnost != null)
                {
                    baza.getAktivnosti().Add(aktivnost);
                }                
            }
            else
            {
                Console.WriteLine("Vrijeme aktivnosti je manje od virtualnog vremena.");
            }
        }
        private static void AktivnostVracanje(int idAktivnosti, DateTime vrijeme, string korisnik, string lokacija, string vozilo, string brojKm)
        {
            if (baza.UsporediVrijeme(vrijeme))
            {
                Aktivnost aktivnost = AktivnostDirektor.Vracanje(idAktivnosti, vrijeme,
                    baza.getKorisnik(int.Parse(korisnik)),
                    baza.getLokacija(int.Parse(lokacija)),
                    baza.getNajamVozila(baza.getVozilo(int.Parse(vozilo))),
                    int.Parse(brojKm));
                if(aktivnost != null)
                {
                    baza.getAktivnosti().Add(aktivnost);
                }
                
            }
            else
            {
                Console.WriteLine("Vrijeme aktivnosti je manje od virtualnog vremena.");
            }
        }
        private static void AktivnostKraj(int idAktivnosti, DateTime vrijeme)
        {
            if (baza.UsporediVrijeme(vrijeme))
            {
                Aktivnost aktivnost = AktivnostDirektor.Kraj(idAktivnosti, vrijeme);
                Console.WriteLine("U " + vrijeme + " program završava s radom.");
                radi = false;
            }
            else
            {
                Console.WriteLine("Vrijeme aktivnosti je manje od virtualnog vremena.");
            }

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
                        DateTime virtualnoVrijeme = new DateTime();
                        virtualnoVrijeme = DateTime.Parse(args[i + 1]);
                        baza.setVirtualnoVrijeme(virtualnoVrijeme);
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
            if (UcitajDokument(TipDatoteke.osobe) &&
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
                    while ((line = file.ReadLine()) != null)
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
                                    baza.getKorisnici().Add(noviKorisnik);
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
                                    baza.getLokacije().Add(novaLokacija);
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
                                    baza.getVozila().Add(novoVozilo);
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
                                foreach (Vozilo vozilo in baza.getVozila())
                                {
                                    if (vozilo.id == int.Parse(atributi[0]))
                                    {
                                        try
                                        {
                                            Cjenik noviCjenik = new Cjenik(vozilo, int.Parse(atributi[1]), int.Parse(atributi[2]), int.Parse(atributi[3]));
                                            baza.getCjenik().Add(noviCjenik);
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
                                    Console.WriteLine("Linija: " + brojac + " - Greška u kreiranju kapaciteta lokacija! Ne postoji lokacija s ID-jem " + int.Parse(atributi[0]) + ".");
                                }
                                if (postoji)
                                {
                                    postoji = false;
                                    foreach (Vozilo vozilo in baza.getVozila())
                                    {
                                        if (vozilo.id == int.Parse(atributi[1]))
                                        {
                                            postoji = true;
                                            LokacijaKapacitet novaLokacijaKapacitet = new LokacijaKapacitet(lokacijaUnos, vozilo, int.Parse(atributi[2]), int.Parse(atributi[3]));
                                            baza.getLokacijaKapacitet().Add(novaLokacijaKapacitet);
                                            for (int i = 0; i < novaLokacijaKapacitet.brojVozila; i++)
                                            {
                                                NajamVozila najamVozila = new NajamVozila(vozilo.id, vozilo.naziv, vozilo.vrijemePunjenja, vozilo.domet);
                                                baza.getVozilaZaNajam().Add(najamVozila);
                                            }
                                            
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
