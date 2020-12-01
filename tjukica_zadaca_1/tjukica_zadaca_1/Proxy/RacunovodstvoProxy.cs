using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1.Proxy
{
    class RacunovodstvoProxy : IzdavanjeRacuna
    {
        private Racunovodstvo racunovodstvo;

        public RacunovodstvoProxy(Racunovodstvo racunovodstvo) //protection proxy
        {
            this.racunovodstvo = racunovodstvo;
        }

        public void IzdajRacun(int lokacijaIdNajam, int lokacijaIdVracanje, int voziloId, double brojSati, int brojKm, DateTime vrijeme, int korisnikId)
        {
            racunovodstvo.IzdajRacun(lokacijaIdNajam, lokacijaIdVracanje, voziloId, brojSati, brojKm, vrijeme, korisnikId);
        }

        public List<Racun> GetRacuni()
        {
            return racunovodstvo.GetRacuni();
        }
    }
}
