using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    
    class LokacijaKapacitet
    {
        public Lokacija lokacija { get; private set; }
        public  Vozilo vozilo { get; private set; }
        private int brojMjesta;
        private int brojVozila;

        public static List<LokacijaKapacitet> kapacitetiLokacija = new List<LokacijaKapacitet>();
        public LokacijaKapacitet(Lokacija lokacija, Vozilo vozilo, int brojMjesta, int brojVozila)
        {
            this.lokacija = lokacija;
            this.vozilo = vozilo;
            this.brojMjesta = brojMjesta;
            this.brojVozila = brojVozila;
        }
    }
}
