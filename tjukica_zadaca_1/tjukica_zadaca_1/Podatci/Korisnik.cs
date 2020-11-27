using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    public class Korisnik
    {

        public int id { get; private set; }
        public string ime { get; private set; }
        public int brojNeispravnihVracanja { get; set; } = 0;

        public Vozilo najamVozila { get; set; }

        public List<Vozilo> najmovi = new List<Vozilo>();
        //TODO postaviti najmove u listu najmova
        public Korisnik(int id, string ime)
        {
            this.id = id;
            this.ime = ime;
            this.najamVozila = null;
        }


    }
}
