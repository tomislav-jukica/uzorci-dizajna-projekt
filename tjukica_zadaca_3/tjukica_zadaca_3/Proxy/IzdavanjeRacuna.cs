using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1.Proxy
{
    interface IzdavanjeRacuna
    {
        public void IzdajRacun(int lokacijaIdNajam, int lokacijaIdVracanje, int voziloId, double brojSati, int brojKm, DateTime vrijeme, int korisnikId);
    }
}
