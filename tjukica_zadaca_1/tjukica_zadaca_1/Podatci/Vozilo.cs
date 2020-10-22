﻿using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    public class Vozilo
    {
        public int id { get; private set; }
        public string naziv { get; private set; }
        private int vrijemePunjenja;
        private int domet;

        public Vozilo(int id, string naziv, int vrijemePunjenja, int domet)
        {
            this.id = id;
            this.naziv = naziv;
            this.vrijemePunjenja = vrijemePunjenja;
            this.domet = domet;
        }
    }
}