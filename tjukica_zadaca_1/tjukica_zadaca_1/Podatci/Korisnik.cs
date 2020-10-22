using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    public class Korisnik
    {

        public int id { get; private set; }
        private string ime { get; set; }

        //public static List<Korisnik> korisnici = new List<Korisnik>();

        public Korisnik(int id, string ime)
        {
            this.id = id;
            this.ime = ime;

        }
    }
}
