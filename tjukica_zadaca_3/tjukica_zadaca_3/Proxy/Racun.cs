using System;
using System.Collections.Generic;
using System.Text;

namespace tjukica_zadaca_1.Proxy
{
    public class Racun
    {
        public int id { get; private set; }
        public int idLokacijeNajma { get; private set; }
        public int idLokacijeVracanja { get; private set; }
        public int idVoziloTip { get; private set; }
        public double brojSati { get; private set; }
        public int brojKm { get; private set; }
        public int idKorisnik { get; private set; }
        public double ukupno { get; private set; }
        public DateTime datumIzdavanja { get; private set; }

        public Racun(int id, int lokacijaIdNajam, int lokacijaIdVracanje, int voziloId, double brojSati, int brojKm, int korisnikId, DateTime vrijeme, double ukupno)
        {
            this.id = id;
            this.idLokacijeNajma = lokacijaIdNajam;
            this.idLokacijeVracanja = lokacijaIdVracanje;
            this.idVoziloTip = voziloId;
            this.brojSati = brojSati;
            this.brojKm = brojKm;
            this.ukupno = ukupno;
            this.datumIzdavanja = vrijeme;
            this.idKorisnik = korisnikId;
        } 
    }
}
