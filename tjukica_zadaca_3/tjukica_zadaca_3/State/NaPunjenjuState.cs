using System;
using System.Collections.Generic;
using System.Text;
using tjukica_zadaca_1.State;

namespace tjukica_zadaca_1
{
    public class NaPunjenjuState : VoziloState
    {
        //public Vozilo vozilo { get; private set; }
        public LokacijaKapacitet lokacija { get; private set; }
        private DateTime vrijemeAktivnosti {  get;  set; }
        public DateTime gotovoPunjenje { get; private set; }

        public NaPunjenjuState(Vozilo vozilo, LokacijaKapacitet lokacija, DateTime vrijeme, int prijedeniKilometri)
        {
            this.vozilo = vozilo;
            this.lokacija = lokacija;
            this.vrijemeAktivnosti = vrijeme;            
        }

        public void IzracunajVrijemePunjenja(int napravljeniKilometri)
        {
            int vrijemePunjenja = vozilo.vrijemePunjenja; // npr. 4 sata
            int domet = vozilo.domet;
            int postotakBaterije = IzracunajPostotakBaterije(domet, napravljeniKilometri);
            if(postotakBaterije == -1)
            {
                cw.Write("Postotak baterije ne može biti manji od 0%!");
            }
            int zaNapuniti = 100 - postotakBaterije;
            double jedanPosto = (double)vrijemePunjenja / (double)100;
            double satiPunjenja = zaNapuniti * jedanPosto;
            gotovoPunjenje = vrijemeAktivnosti.AddHours(satiPunjenja);

        }

        private int IzracunajPostotakBaterije(int domet, int kilometri)
        {
            if(kilometri > domet)
            {
                cw.Write("Prijedeni kilometri su veci od dometa vozila.");
                return -1;
            } else
            {
                double baterija = (double)(domet - kilometri) / (double)domet;
                baterija *= 100;
                return (int)baterija;
            }
        }

        public override void Iznajmi()
        {
            cw.Write("Nije moguće iznajmiti vozilo koje je na punjenju.");
        }

        public override void Napuni()
        {
            this.vozilo.TransitionTo(new SlobodnoState());
        }

        public override void Vrati(LokacijaKapacitet lokacija, DateTime vrijeme, int prijedeniKilometri)
        {
            cw.Write("Nije moguće vratiti vozilo koje nije iznajmljeno.");
        }

        public override void VratiPokvareno()
        {
            baza.getVozilaNaPunjenju().Remove(this);            
            this.vozilo.TransitionTo(new PokvarenoState());
        }
    }
}
