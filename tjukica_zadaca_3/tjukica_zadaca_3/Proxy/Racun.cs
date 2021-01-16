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
        public decimal ukupno { get; private set; }
        public DateTime datumIzdavanja { get; private set; }
        public bool placen { get; set; } = false;

        public Racun(int id, int lokacijaIdNajam, int lokacijaIdVracanje, int voziloId, double brojSati, int brojKm, int korisnikId, DateTime vrijeme, decimal ukupno)
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
            this.placen = !ProvjeriUgovorKorisnika(korisnikId);
        }

        private bool ProvjeriUgovorKorisnika(int id)
        {
            return Baza.getInstance().getKorisnik(id).ugovor;
        }

    }
}
