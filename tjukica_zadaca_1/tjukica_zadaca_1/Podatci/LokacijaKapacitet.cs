using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using tjukica_zadaca_1.Composite;
using tjukica_zadaca_1.State;

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
            //int retVal = trenutnaVozila.Count;
            int retVal = 0;
            Baza baza = Baza.getInstance();

            foreach (Vozilo v in trenutnaVozila)
            {
                if (v.state.GetType() == new SlobodnoState().GetType())
                {
                    //retVal -= 1;
                    retVal += 1;
                }
            }
            return retVal;
        }

        public int dajBrojSlobodnihMjesta()
        {
            return brojMjesta - trenutnaVozila.Count;
        }

        public int dajBrojPokvarenihVozila()
        {
            int retVal = 0;
            foreach (Vozilo v in trenutnaVozila)
            {
                if(v.state.GetType() == new PokvarenoState().GetType())
                {
                    retVal++;
                }
            }
            return retVal;
        }

        public Vozilo dajVoziloUNajam()
        {
            Vozilo vozilo = null;
            foreach (Vozilo v in trenutnaVozila)
            {
                if (v.state.GetType() == new SlobodnoState().GetType())
                {

                    vozilo = v;
                    //v.iznajmljen = true;
                    break;
                }
            }
            trenutnaVozila.Remove(vozilo);
            vozilo.state.Iznajmi();
            return vozilo;
        }

        public void VratiVozilo(Vozilo vozilo, DateTime vrijeme, int kilometri)
        {
            if (dajBrojSlobodnihMjesta() > 0)
            {
                trenutnaVozila.Add(vozilo);
                vozilo.state.Vrati(this, vrijeme, kilometri);
            }
            else
            {
                cw.Write("Trenutno nema slobodnih mjesta na lokaciji " + lokacija.naziv + "!");
            }
        }

        private Vozilo dajVoziloZaIznajmiti()
        {
            Vozilo retVal = null;            
            foreach (Vozilo v in dajSlobodnaVozila())
            {
                if (retVal == null) retVal = v;
                if(retVal.brojUnajmljivanja > v.brojUnajmljivanja)
                {
                    retVal = v;
                } else if(retVal.brojUnajmljivanja == v.brojUnajmljivanja)
                {
                    if(retVal.kilometri > v.kilometri)
                    {
                        retVal = v;
                    } else if(retVal.kilometri == v.kilometri)
                    {
                        if(retVal.idVozila > v.idVozila)
                        {
                            retVal = v;
                        }
                    }
                }
            }
            return retVal;
        }

        private List<Vozilo> dajSlobodnaVozila()
        {
            List<Vozilo> retVal = new List<Vozilo>();
            foreach (Vozilo v in trenutnaVozila)
            {
                if (v.state.GetType() == new SlobodnoState().GetType())
                {
                    retVal.Add(v);
                }
            }
            return retVal;
        }
    }
}
