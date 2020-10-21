using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    class Cjenik
    {
        int idVozila;
        int najam;
        int cijenaSat;
        int cijenaKm;

        public static List<Cjenik> cjenik = new List<Cjenik>();

        public Cjenik(int idVozila, int najam, int cijenaSat, int cijenaKm)
        {
            this.idVozila = idVozila;
            this.najam = najam;
            this.cijenaSat = cijenaSat;
            this.cijenaKm = cijenaKm;
        }
    }
}
