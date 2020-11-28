using System;
using System.Collections.Generic;
using System.Text;
using tjukica_zadaca_1.Helpers;

namespace tjukica_zadaca_1.State
{
    public abstract class VoziloState
    {
        public Vozilo vozilo;
        protected ConsoleWriter cw = ConsoleWriter.getInstance();
        public void SetVozilo(Vozilo vozilo)
        {
            this.vozilo = vozilo;
        }

        public abstract void Iznajmi();
        public abstract void Vrati(LokacijaKapacitet lokacija, DateTime vrijeme, int prijedeniKilometri);
        public abstract void VratiPokvareno();
        public abstract void Napuni();
    }
}
