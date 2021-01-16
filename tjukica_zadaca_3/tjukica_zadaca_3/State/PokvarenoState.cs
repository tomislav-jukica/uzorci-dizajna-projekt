using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1.State
{
    class PokvarenoState : VoziloState
    {
        public override bool Iznajmi(Korisnik korisnik, LokacijaKapacitet lokacija)
        {
            cw.Write("Nije moguće iznajmiti pokvareno vozilo.");
            return false;
        }

        public override void Napuni()
        {
            cw.Write("Nije moguće puniti pokvareno vozilo.");
        }

        public override void Vrati(Aktivnost.Builder builder)
        {
            cw.Write("Nije moguće vratiti vozilo koje nije iznajmljeno.");
        }

        public override void VratiPokvareno()
        {
            cw.Write("Nije moguće vratiti vozilo koje nije iznajmljeno.");
        }
    }
}
