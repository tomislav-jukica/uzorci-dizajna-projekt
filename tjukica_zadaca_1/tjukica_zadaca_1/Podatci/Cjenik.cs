using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1
{
    public class Cjenik
    {
        public TipVozila tipVozila { get; private set; }
        public float najam { get; private set; }
        public float cijenaSat { get; private set; }
        public float cijenaKm { get; private set; }

        public Cjenik(TipVozila vozilo, float najam, float cijenaSat, float cijenaKm)
        {
            this.tipVozila = vozilo;
            this.najam = najam;
            this.cijenaSat = cijenaSat;
            this.cijenaKm = cijenaKm;
        }
    }
}
