using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;

namespace tjukica_zadaca_1
{

    public class LokacijaKapacitet
    {
        public Lokacija lokacija { get; private set; }
        public TipVozila tipVozila { get; private set; }
        public int brojMjesta { get; set; }
        public int brojVozila { get; set; }

        public List<Vozilo> trenutnaVozila = new List<Vozilo>();

        private Helpers.ConsoleWriter cw = Helpers.ConsoleWriter.getInstance();

        public LokacijaKapacitet(Lokacija lokacija, TipVozila vozilo, int brojMjesta, int brojVozila)
        {
            this.lokacija = lokacija;
            this.tipVozila = vozilo;
            this.brojMjesta = brojMjesta;
            this.brojVozila = brojVozila;
        }

        public int dajBrojSlobodnihVozila()
        {
            int retVal = trenutnaVozila.Count;
            Baza baza = Baza.getInstance();

            foreach (Vozilo v in trenutnaVozila)
            {
                if (v.iznajmljen == true)
                {
                    retVal -= 1;
                }
            }
            return retVal;
        }

        public int dajBrojSlobodnihMjesta()
        {
            return brojMjesta - trenutnaVozila.Count;
        }

        public Vozilo dajVoziloUNajam()
        {
            Vozilo vozilo = null;
            foreach (Vozilo v in trenutnaVozila)
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

        public void VratiVozilo(Vozilo vozilo)
        {
            if (dajBrojSlobodnihMjesta() > 0)
            {
                trenutnaVozila.Add(vozilo);
            }
            else
            {
                cw.Write("Trenutno nema slobodnih mjesta na lokaciji " + lokacija.naziv + "!");
            }
        }
    }
}
