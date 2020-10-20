using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    class Vozilo
    {
        public int id { get; private set; }
        private string naziv;
        private int vrijemePunjenja;
        private int domet;

        public static List<Vozilo> vozila = new List<Vozilo>();

        public Vozilo(int id, string naziv, int vrijemePunjenja, int domet)
        {
            this.id = id;
            this.naziv = naziv;
            this.vrijemePunjenja = vrijemePunjenja;
            this.domet = domet;
        }
    }
}
