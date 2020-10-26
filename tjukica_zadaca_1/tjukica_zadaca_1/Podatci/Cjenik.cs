using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    public class Cjenik
    {
        public Vozilo vozilo { get; private set; }
        public int najam { get; private set; }
        public int cijenaSat { get; private set; }
        public int cijenaKm { get; private set; }

        public Cjenik(Vozilo vozilo, int najam, int cijenaSat, int cijenaKm)
        {
            this.vozilo = vozilo;
            this.najam = najam;
            this.cijenaSat = cijenaSat;
            this.cijenaKm = cijenaKm;
        }
    }
}
