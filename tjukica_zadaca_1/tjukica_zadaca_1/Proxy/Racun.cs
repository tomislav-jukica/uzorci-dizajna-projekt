using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1.Proxy
{
    class Racun
    {
        public int id { get; private set; }
        int idLokacijeNajma;
        int idLokacijeVracanja;
        int idVoziloTip;
        double brojSati;
        int brojKm;
        double ukupno;

        public Racun(int id, int lokacijaIdNajam, int lokacijaIdVracanje, int voziloId, double brojSati, int brojKm, int korisnikId, DateTime vrijeme, double ukupno)
        {
            this.id = id;
            this.idLokacijeNajma = lokacijaIdNajam;
            this.idLokacijeVracanja = lokacijaIdVracanje;
            this.idVoziloTip = voziloId;
            this.brojSati = brojSati;
            this.brojKm = brojKm;
            this.ukupno = ukupno;            
        } 
    }
}
