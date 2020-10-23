using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace tjukica_zadaca_1
{
    public class NajamVozila : Vozilo
    {
        int idNajamVozila = 1;
        public float baterija { get; set; }
        public int kilometri { get; set; }
        public bool iznajmljen { get; set; }

        public NajamVozila(int id, string naziv, int punjenje, int domet) 
            : base(id, naziv, punjenje, domet)
        {
            this.idNajamVozila += 1;
            this.baterija = 1;
            this.kilometri = 0;
            iznajmljen = false;
        }

    }
}
