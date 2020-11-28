using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1.State
{
    public abstract class VoziloState
    {
        protected Vozilo vozilo;
        public void SetVozilo(Vozilo vozilo)
        {
            this.vozilo = vozilo;
        }

        public abstract void Iznajmi();
        public abstract void Vrati();
        public abstract void VratiPokvareno();
        public abstract void Napuni();
    }
}
