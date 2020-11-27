using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace tjukica_zadaca_1
{
    public class Vozilo : TipVozila
    {
        public int idVozila { get; set; }
        public float baterija { get; set; }
        public int kilometri { get; set; }
        public bool iznajmljen { get; set; }
        public int brojUnajmljivanja { get; set; } = 0;


        //TODO Vozilo state


        public Vozilo(int id, string naziv, int punjenje, int domet) 
            : base(id, naziv, punjenje, domet)
        {
            this.idVozila = Baza.getInstance().getVozilaZaNajam().Count + 1;
            this.baterija = 1;
            this.kilometri = 0;
            iznajmljen = false;
        }

    }
}
