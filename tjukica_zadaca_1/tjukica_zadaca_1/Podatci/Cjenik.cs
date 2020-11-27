using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    public class Cjenik
    {
        public TipVozila tipVozila { get; private set; }
        public int najam { get; private set; }
        public int cijenaSat { get; private set; }
        public int cijenaKm { get; private set; }

        public Cjenik(TipVozila vozilo, int najam, int cijenaSat, int cijenaKm)
        {
            this.tipVozila = vozilo;
            this.najam = najam;
            this.cijenaSat = cijenaSat;
            this.cijenaKm = cijenaKm;
        }
    }
}
