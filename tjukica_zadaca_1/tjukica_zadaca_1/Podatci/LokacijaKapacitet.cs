using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    
    public class LokacijaKapacitet
    {
        public Lokacija lokacija { get; private set; }
        public  Vozilo vozilo { get; private set; }
        public int brojMjesta { get; private set; }
        public int brojVozila { get; set; }

        public LokacijaKapacitet(Lokacija lokacija, Vozilo vozilo, int brojMjesta, int brojVozila)
        {
            this.lokacija = lokacija;
            this.vozilo = vozilo;
            this.brojMjesta = brojMjesta;
            this.brojVozila = brojVozila;
        }
    }
}
