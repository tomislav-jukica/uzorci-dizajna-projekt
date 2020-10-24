using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace tjukica_zadaca_1
{

    public class LokacijaKapacitet
    {
        public Lokacija lokacija { get; private set; }
        public Vozilo vozilo { get; private set; }
        public int brojMjesta { get; private set; }
        public int brojVozila { get; set; }

        public List<NajamVozila> trenutnaVozila = new List<NajamVozila>();

        public LokacijaKapacitet(Lokacija lokacija, Vozilo vozilo, int brojMjesta, int brojVozila)
        {
            this.lokacija = lokacija;
            this.vozilo = vozilo;
            this.brojMjesta = brojMjesta;
            this.brojVozila = brojVozila;
        }

        public int dajBrojSlobodnihVozila()
        {
            int retVal = trenutnaVozila.Count;
            Baza baza = Baza.getInstance();

            foreach (NajamVozila v in trenutnaVozila)
            {
                if (v.iznajmljen == true)
                {
                    retVal -= 1;
                }
            }
            /*
            foreach (Punjenje p in baza.getVozilaNaPunjenju())
            {
                foreach(NajamVozila v in trenutnaVozila)
                {
                    if(p.vozilo == v)
                    {
                        retVal -= 1;
                    }
                }
            }*/
            return retVal;
        }

        public int dajBrojSlobodnihMjesta()
        {
            return brojMjesta - trenutnaVozila.Count;
        }

        public NajamVozila dajVoziloUNajam()
        {
            NajamVozila vozilo = null;
            foreach (NajamVozila v in trenutnaVozila)
            {
                if (v.iznajmljen == false)
                {
                    vozilo = v;
                    v.iznajmljen = true;
                    break;
                }
            }
            trenutnaVozila.Remove(vozilo);
            return vozilo;
        }

        public void VratiVozilo(NajamVozila vozilo)
        {
            if (dajBrojSlobodnihMjesta() > 0)
            {
                trenutnaVozila.Add(vozilo);
            }
            else
            {
                Console.WriteLine("Nema slobodnih mjesta.");
            }
        }
    }
}
