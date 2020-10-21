using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    
    class LokacijaKapacitet
    {
        private int idLokacije;
        private int idVozila;
        private int brojMjesta;
        private int brojVozila;

        public static List<LokacijaKapacitet> kapacitetiLokacija = new List<LokacijaKapacitet>();
        public LokacijaKapacitet(int idLokacije, int idVozila, int brojMjesta, int brojVozila)
        {
            this.idLokacije = idLokacije;
            this.idVozila = idVozila;
            this.brojMjesta = brojMjesta;
            this.brojVozila = brojVozila;
        }
    }
}
