using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    class Korisnik
    {

        private int id { get; set; }
        private string ime { get; set; }

        public static List<Korisnik> korisnici = new List<Korisnik>();

        public Korisnik(int id, string ime)
        {
            this.id = id;
            this.ime = ime;

        }
    }
}
