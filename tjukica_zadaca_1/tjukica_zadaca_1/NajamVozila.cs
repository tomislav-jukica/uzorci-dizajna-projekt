using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace tjukica_zadaca_1
{
    class NajamVozila : Vozilo
    {
        int idNajamVozila = 1;
        string tip;
        int vrijemePunjenja;
        int domet;

        float baterija;
        int kilometri;

        public NajamVozila(int id, string naziv, int punjenje, int domet) 
            : base(id, naziv, punjenje, domet)
        {
            this.idNajamVozila += 1;
            this.tip = naziv;
            this.vrijemePunjenja = punjenje;
            this.domet = domet;
            this.baterija = 1;
            this.kilometri = 0;
        }

    }
}
