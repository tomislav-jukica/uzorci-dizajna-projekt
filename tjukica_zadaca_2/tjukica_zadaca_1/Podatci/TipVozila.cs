using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    public class TipVozila
    {
        public int id { get; private set; }
        public string naziv { get; private set; }
        public int vrijemePunjenja { get; private set; }
        public int domet { get; private set; }

        public TipVozila(int id, string naziv, int vrijemePunjenja, int domet)
        {
            this.id = id;
            this.naziv = naziv;
            this.vrijemePunjenja = vrijemePunjenja;
            this.domet = domet;
        }
    }
}
