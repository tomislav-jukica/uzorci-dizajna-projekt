using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    class Cjenik
    {
        public Vozilo vozilo { get; private set; }
        int najam;
        int cijenaSat;
        int cijenaKm;

        public static List<Cjenik> cjenik = new List<Cjenik>();

        public Cjenik(Vozilo vozilo, int najam, int cijenaSat, int cijenaKm)
        {
            this.vozilo = vozilo;
            this.najam = najam;
            this.cijenaSat = cijenaSat;
            this.cijenaKm = cijenaKm;
        }
    }
}
