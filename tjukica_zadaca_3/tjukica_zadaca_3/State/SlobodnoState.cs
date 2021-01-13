using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1.State
{
    class SlobodnoState : VoziloState
    {
        public override void Iznajmi(Korisnik korisnik, LokacijaKapacitet lokacija)
        {
            this.vozilo.brojUnajmljivanja++;
            korisnik.IznajmiVozilo(this.vozilo);
            Baza.getInstance().getIznajmljenaVozila().Add(this.vozilo);
            cw.Write("Korisnik " + korisnik.ime + " unajmio je vozilo tipa " + this.vozilo.naziv + " na lokaciji " + lokacija.lokacija.naziv + ".", false);
            this.vozilo.TransitionTo(new IznajmljenoState());            
        }

        public override void Napuni()
        {
            cw.Write("Nije moguće puniti slobodno vozilo.");
        }

        public override void Vrati(Aktivnost.Builder builder)
        {
            cw.Write("Nije moguće vratiti slobodno vozilo.");
        }

        public override void VratiPokvareno()
        {
            cw.Write("Nije moguće vratiti slobodno vozilo.");
        }
    }
}
