using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1.State
{
    class SlobodnoState : VoziloState
    {
        public override void Iznajmi()
        {
            this.vozilo.brojUnajmljivanja++;
            this.vozilo.TransitionTo(new IznajmljenoState());            
        }

        public override void Napuni()
        {
            cw.Write("Nije moguće puniti slobodno vozilo.");
        }

        public override void Vrati(LokacijaKapacitet lokacija, DateTime vrijeme, int prijedeniKilometri)
        {
            cw.Write("Nije moguće vratiti slobodno vozilo.");
        }

        public override void VratiPokvareno()
        {
            cw.Write("Nije moguće vratiti slobodno vozilo.");
        }
    }
}
