using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1.State
{
    class PokvarenoState : VoziloState
    {
        public override void Iznajmi(Korisnik korisnik, LokacijaKapacitet lokacija)
        {
            cw.Write("Nije moguće iznajmiti pokvareno vozilo.");
        }

        public override void Napuni()
        {
            cw.Write("Nije moguće puniti pokvareno vozilo.");
        }

        public override void Vrati(LokacijaKapacitet lokacija, DateTime vrijeme, int prijedeniKilometri)
        {
            cw.Write("Nije moguće vratiti vozilo koje nije iznajmljeno.");
        }

        public override void VratiPokvareno()
        {
            cw.Write("Nije moguće vratiti vozilo koje nije iznajmljeno.");
        }
    }
}
