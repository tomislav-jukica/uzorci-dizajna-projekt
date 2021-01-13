using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1.State
{
    class IznajmljenoState : VoziloState
    {
        public override void Iznajmi(Korisnik korisnik, LokacijaKapacitet lokacija)
        {
            cw.Write("Nije moguće iznajmiti već iznajmljeno vozilo.");
        }

        public override void Napuni()
        {
            cw.Write("Nije moguće puniti bateriju dok je vozilo iznajmljeno.");
        }

        public override void Vrati(LokacijaKapacitet lokacija, DateTime vrijeme, int prijedeniKilometri)
        {
            NaPunjenjuState nps = new NaPunjenjuState(this.vozilo, lokacija, vrijeme, prijedeniKilometri);
            nps.IzracunajVrijemePunjenja(prijedeniKilometri);
            Baza.getInstance().getVozilaNaPunjenju().Add(nps);
            this.vozilo.TransitionTo(nps);

        }

        public override void VratiPokvareno()
        {
            this.vozilo.TransitionTo(new PokvarenoState());
        }
    }
}
