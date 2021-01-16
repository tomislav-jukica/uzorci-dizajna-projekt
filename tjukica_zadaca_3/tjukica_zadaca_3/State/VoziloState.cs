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
        protected Baza baza = Baza.getInstance();
        public void SetVozilo(Vozilo vozilo)
        {
            this.vozilo = vozilo;
        }

        public abstract bool Iznajmi(Korisnik korisnik, LokacijaKapacitet lokacija);
        public abstract void Vrati(Aktivnost.Builder builder);
        public abstract void VratiPokvareno();
        public abstract void Napuni();
    }
}
